using CorrespondenceCore.Application.DTO.Enum;

namespace CorrespondenceCore.Api.DTO
{
    public class getNextNumberDTO
    {
        public TypeDTO type { get; set; }
        public int year { get; set; }
        public int? deptID { get; set; }
    }
}
