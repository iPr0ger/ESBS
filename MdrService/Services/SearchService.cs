using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MdrService.Contracts.Requests.v1;
using MdrService.Interfaces;
using MdrService.Models.DbConnection;
using Microsoft.EntityFrameworkCore;


namespace MdrService.Services
{
    public class SearchService : ISearchService
    {
        private readonly MdrDbConnection _dbConnection;

        public SearchService(MdrDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        private static int CalculateSkip(int page, int size)
        {
            var skip = 0;
            if (page > 1)
            {
                skip = (page - 1) * size;
            }

            return skip;
        }


        public async Task<ICollection<int>> GetSpecificStudy(SpecificStudyRequest specificStudyRequest)
        {
            var skip = CalculateSkip(page:specificStudyRequest.Page, size:specificStudyRequest.Size);

            var searchQuery = _dbConnection.StudyIdentifiers.Where(
                    studyIdentifier => studyIdentifier.IdentifierTypeId.Equals(specificStudyRequest.SearchType) &&
                                       studyIdentifier.IdentifierValue.ToUpper().Equals(specificStudyRequest.SearchValue.ToUpper()))
                .Select(identifier => identifier.StudyId);
            
            var filtersRequest = specificStudyRequest.Filters;

            var studyFilterQuery = _dbConnection.Studies
                .Where(study => !filtersRequest.StudyTypes.Contains(study.StudyTypeId)
                                && !filtersRequest.StudyStatuses.Contains(study.StudyStatusId)
                                && !filtersRequest.StudyGenderEligibility.Contains(study.StudyGenderEligId)).Select(study => study.Id);

            var studyFeatureFilterQuery = _dbConnection.StudyFeatures
                .Where(sf => !filtersRequest.StudyFeatureValues.Contains(sf.FeatureValueId)).Select(sf => sf.StudyId);

            var dataObjectFilterQuery = _dbConnection.DataObjects
                .Where(dataObject => !filtersRequest.ObjectTypes.Contains(dataObject.ObjectTypeId)
                                     && !filtersRequest.ObjectAccessTypes.Contains(dataObject.AccessTypeId)).Select(o => o.Id);
            
            var query = _dbConnection.StudyObjectLinks
                .Where(link => searchQuery.Contains(link.StudyId)
                               && studyFilterQuery.Contains(link.StudyId)
                               && studyFeatureFilterQuery.Contains(link.StudyId) 
                               && dataObjectFilterQuery.Contains(link.ObjectId));

            var orderedQuery = query.OrderBy(arg => arg.StudyId);
            
            var selectRes = orderedQuery
                .Select(t => t.StudyId)
                .Distinct()
                .Skip(skip).Take(specificStudyRequest.Size);
            
            return await selectRes.ToArrayAsync();
        }

        public async Task<ICollection<int>> GetByStudyCharacteristics(StudyCharacteristicsRequest studyCharacteristicsRequest)
        {
            // Get skip
            var skip = CalculateSkip(page:studyCharacteristicsRequest.Page, size:studyCharacteristicsRequest.Size);

            var joinQuery = _dbConnection.StudyTitles.Join(
                _dbConnection.StudyTopics,
                title => title.StudyId,
                topic => topic.StudyId,
                (title, topic) => new
                {
                    title.StudyId,
                    title.TitleText,
                    topic.OriginalValue
                });

            if (!string.IsNullOrEmpty(studyCharacteristicsRequest.TitleContains))
            {
                joinQuery = joinQuery.Where(arg => arg.TitleText.ToLower().Contains(studyCharacteristicsRequest.TitleContains.ToLower()));
            }
            
            if (!string.IsNullOrEmpty(studyCharacteristicsRequest.TopicsInclude))
            {
                joinQuery = joinQuery.Where(arg => arg.OriginalValue.ToLower().Contains(studyCharacteristicsRequest.TopicsInclude.ToLower()));
            }

            var joinQueryStudyIds = joinQuery.Select(args => args.StudyId);

            // Get filters
            var filtersRequest = studyCharacteristicsRequest.Filters;
            
            var studyFilterQuery = _dbConnection.Studies
                .Where(study => !filtersRequest.StudyTypes.Contains(study.StudyTypeId)
                                && !filtersRequest.StudyStatuses.Contains(study.StudyStatusId)
                                && !filtersRequest.StudyGenderEligibility.Contains(study.StudyGenderEligId)).Select(study => study.Id);

            var studyFeatureFilterQuery = _dbConnection.StudyFeatures
                .Where(sf => !filtersRequest.StudyFeatureValues.Contains(sf.FeatureValueId)).Select(sf => sf.StudyId);

            var dataObjectFilterQuery = _dbConnection.DataObjects
                .Where(dataObject => !filtersRequest.ObjectTypes.Contains(dataObject.ObjectTypeId)
                && !filtersRequest.ObjectAccessTypes.Contains(dataObject.AccessTypeId)).Select(o => o.Id);

            var query = _dbConnection.StudyObjectLinks
                            .Where(link => joinQueryStudyIds.Contains(link.StudyId)
                                           && studyFilterQuery.Contains(link.StudyId)
                                           && studyFeatureFilterQuery.Contains(link.StudyId) 
                                           && dataObjectFilterQuery.Contains(link.ObjectId));

            var orderedQuery = query.OrderBy(arg => arg.StudyId);
            
            var selectRes = orderedQuery
                .Select(t => t.StudyId)
                .Distinct()
                .Skip(skip).Take(studyCharacteristicsRequest.Size);
            
            return await selectRes.ToArrayAsync();

        }

        public async Task<ICollection<int>> GetViaPublishedPaper(ViaPublishedPaperRequest viaPublishedPaperRequest)
        {

            var skip = CalculateSkip(page:viaPublishedPaperRequest.Page, size:viaPublishedPaperRequest.Size);

            // Get filters
            var filtersRequest = viaPublishedPaperRequest.Filters;
            
            var studyFilterQuery = _dbConnection.Studies
                .Where(study => !filtersRequest.StudyTypes.Contains(study.StudyTypeId)
                                && !filtersRequest.StudyStatuses.Contains(study.StudyStatusId)
                                && !filtersRequest.StudyGenderEligibility.Contains(study.StudyGenderEligId)).Select(study => study.Id);

            var studyFeatureFilterQuery = _dbConnection.StudyFeatures
                .Where(sf => !filtersRequest.StudyFeatureValues.Contains(sf.FeatureValueId)).Select(sf => sf.StudyId);

            var dataObjectFilterQuery = _dbConnection.DataObjects
                .Where(dataObject => !filtersRequest.ObjectTypes.Contains(dataObject.ObjectTypeId)
                                     && !filtersRequest.ObjectAccessTypes.Contains(dataObject.AccessTypeId)).Select(o => o.Id);
            
            
            if (viaPublishedPaperRequest.SearchType == "doi")
            {
                var query = _dbConnection.StudyObjectLinks.Where(p =>
                    _dbConnection.DataObjects
                        .Where(dataObject => dataObject.Doi.Contains(viaPublishedPaperRequest.SearchValue))
                        .Select(o => o.Id).Contains(p.ObjectId)
                                             && studyFilterQuery.Contains(p.StudyId)
                                             && studyFeatureFilterQuery.Contains(p.StudyId) 
                                             && dataObjectFilterQuery.Contains(p.ObjectId));
                
                var orderedQuery = query.OrderBy(p => p.Id);
                var selectRes = orderedQuery.Select(p => p.StudyId)
                    .Distinct()
                    .Skip(skip)
                    .Take(viaPublishedPaperRequest.Size);
                
                return await selectRes.ToArrayAsync();
            }
            else
            {
                var query = _dbConnection.StudyObjectLinks.Where(
                    link => _dbConnection.ObjectTitles.Where(ot => ot.TitleText.ToLower()
                                .Contains(viaPublishedPaperRequest.SearchValue.ToLower()))
                        .Select(title => title.ObjectId).Contains(link.ObjectId)
                    && studyFilterQuery.Contains(link.StudyId)
                       && studyFeatureFilterQuery.Contains(link.StudyId) 
                       && dataObjectFilterQuery.Contains(link.ObjectId));
                
                var orderedQuery = query.OrderBy(p => p.StudyId);
                var selectRes = orderedQuery.Select(p => p.StudyId)
                    .Distinct()
                    .Skip(skip)
                    .Take(viaPublishedPaperRequest.Size);

                return await selectRes.ToArrayAsync();
            }
        }

        public async Task<int?> GetByStudyId(StudyIdRequest studyIdRequest)
        {
            var res = await _dbConnection.Studies
                .FirstOrDefaultAsync(p => p.Id.Equals(studyIdRequest.StudyId));
            return res?.Id;
        }
    }
}