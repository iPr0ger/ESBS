using System.Collections.Generic;

namespace MdrService.Contracts.Responses.v1
{
    public class BaseResponse
    {
        public int Total { get; set; } 
        public ICollection<StudyListResponse.StudyListResponse> Data { get; set; }
    }
}