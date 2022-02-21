using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.Models.Master
{
    public class AttachmentModel
    {
        public int AttachmentId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string Guidno { get; set; }
        public string ContainerName { get; set; }
        public string ServerFileName { get; set; }
        public string UserFileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public long? ContentLength { get; set; }
        public string Url { get; set; }
        public int StorageAccountId { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
