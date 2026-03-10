
using webapi.Model.CorrespondenceModel.Enum;

namespace webapi.Model.CorrespondenceModel
{
    public class getNextNumberDTO
    {
        public TypeDTO type { get; set; }
        public int year { get; set; }
        public int? deptID { get; set; }
    }
}
