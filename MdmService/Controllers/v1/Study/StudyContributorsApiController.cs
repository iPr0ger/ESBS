using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MdmService.Contracts.Responses;
using MdmService.DTO.Study;
using MdmService.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MdmService.Controllers.v1.Study
{
    public class StudyContributorsApiController : BaseApiController
    {
        private readonly IStudyRepository _studyRepository;

        public StudyContributorsApiController(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
        }
    
        [HttpGet("studies/{sdSid}/contributors")]
        [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
        public async Task<IActionResult> GetStudyContributors(string sdSid)
        {
            var study = await _studyRepository.GetStudyById(sdSid);
            if (study == null) return NotFound(new ApiResponse<StudyContributorDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyContribs = await _studyRepository.GetStudyContributors(sdSid);
            if (studyContribs == null)
                return NotFound(new ApiResponse<StudyContributorDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No study contributors have been found." },
                    Data = null
                });
            return Ok(new ApiResponse<StudyContributorDto>()
            {
                Total = studyContribs.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyContribs
            });
        }

        [HttpGet("studies/{sdSid}/contributors/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
        public async Task<IActionResult> GetStudyContributor(string sdSid, int id)
        {
            var study = await _studyRepository.GetStudyById(sdSid);
            if (study == null) return NotFound(new ApiResponse<StudyContributorDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            
            var studyContributor = await _studyRepository.GetStudyContributor(id);
            if (studyContributor == null) return NotFound(new ApiResponse<StudyContributorDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study contributors have been found." },
                Data = null
            });

            var studyContribList = new List<StudyContributorDto>() { studyContributor };
            return Ok(new ApiResponse<StudyContributorDto>()
            {
                Total = studyContribList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyContribList
            });
        }

        [HttpPost("studies/{sdSid}/contributors")]
        [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
        public async Task<IActionResult> CreateStudyContributor(string sdSid, [FromBody] StudyContributorDto studyContributorDto)
        {
            var study = await _studyRepository.GetStudyById(sdSid);
            if (study == null) return NotFound(new ApiResponse<StudyContributorDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyContrib = await _studyRepository.CreateStudyContributor(sdSid, studyContributorDto);
            if (studyContrib == null)
                return BadRequest(new ApiResponse<StudyContributorDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study contributor creation." },
                    Data = null
                });


            var studyContribList = new List<StudyContributorDto>() { studyContrib };
            return Ok(new ApiResponse<StudyContributorDto>()
            {
                Total = studyContribList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyContribList
            });
        }

        [HttpPut("studies/{sdSid}/contributors")]
        [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
        public async Task<IActionResult> UpdateStudyContributor(string sdSid, [FromBody] StudyContributorDto studyContributorDto)
        {
            var study = await _studyRepository.GetStudyById(sdSid);
            if (study == null)
                return NotFound(new ApiResponse<StudyContributorDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No studies have been found." },
                    Data = null
                });
            
            var studyContributor = await _studyRepository.GetStudyContributor(studyContributorDto.Id);
            if (studyContributor == null) return NotFound(new ApiResponse<StudyContributorDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study contributors have been found." },
                Data = null
            });

            var updatedStudyContrib = await _studyRepository.UpdateStudyContributor(studyContributorDto);
            if (updatedStudyContrib == null)
                return BadRequest(new ApiResponse<StudyContributorDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study contributor update." },
                    Data = null
                });

            var studyContribList = new List<StudyContributorDto>() { updatedStudyContrib };
            return Ok(new ApiResponse<StudyContributorDto>()
            {
                Total = studyContribList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyContribList
            });
        }

        [HttpDelete("studies/{sdSid}/contributors/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
        public async Task<IActionResult> DeleteStudyContributor(string sdSid, int id)
        {
            var study = await _studyRepository.GetStudyById(sdSid);
            if (study == null) return NotFound(new ApiResponse<StudyContributorDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            
            var studyContrib = await _studyRepository.GetStudyContributor(id);
            if (studyContrib == null) return NotFound(new ApiResponse<StudyContributorDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study contributors have been found." },
                Data = null
            });
            var count = await _studyRepository.DeleteStudyContributor(id);
            return Ok(new ApiResponse<StudyContributorDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Study contributor has been removed." },
                Data = null
            });
        }

        [HttpDelete("studies/{sdSid}/contributors")]
        [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
        public async Task<IActionResult> DeleteAllStudyContributors(string sdSid)
        {
            var study = await _studyRepository.GetStudyById(sdSid);
            if (study == null) return NotFound(new ApiResponse<StudyContributorDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            var count = await _studyRepository.DeleteAllStudyContributors(sdSid);
            return Ok(new ApiResponse<StudyContributorDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All study contributors have been removed." },
                Data = null
            });
        }
    
    }
}