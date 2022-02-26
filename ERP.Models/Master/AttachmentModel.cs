using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class AttachmentModel
    {
        public int AttachmentId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string ContainerName { get; set; }
        public string ServerFileName { get; set; }
        public string UserFileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public long? ContentLength { get; set; }
        public string Url { get; set; }
        public int StorageAccountId { get; set; }

        [Display(Name = "Account Name")]
        public string StorageType { get; set; }

        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Display(Name = "Account Key")]
        public string AccountKey { get; set; }

        public IFormFile FileUpload { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
