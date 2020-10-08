


namespace System.DTO.SubjectDTO
{
    public class UpdateSubjectDto
    {
        public int Id { get; set; }
        public string SubjectCode { get; set; }
        public string Name { get; set; }
        public int? PracticalTime { get; set; }
        public int? TheoreticalTime { get; set; }
        public int SubjectTypeId { get; set; }
        public int StudySemesterId { get; set; }
    }
}