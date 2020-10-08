using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class StudentAttachment
    {
        public string Ssn { get; set; }
        public int AttachmentId { get; set; }
        public string Attachemnt { get; set; }
        public string Note { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public int Id { get; set; }

        public Attatchments Attachment { get; set; }
        public Students SsnNavigation { get; set; }
    }
}
