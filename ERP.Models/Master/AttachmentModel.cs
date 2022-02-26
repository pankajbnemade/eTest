using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class AttachmentModel
    {
        public int AttachmentId { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Container Name is required.")]
        public string ContainerName { get; set; }

        [Required(ErrorMessage = "Server File Name is required.")]
        public string ServerFileName { get; set; }

        [Required(ErrorMessage = "User File Name is required.")]
        public string UserFileName { get; set; }

        [Required(ErrorMessage = "File Extension is required.")]
        public string FileExtension { get; set; }

        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public string Url { get; set; }

        [Required(ErrorMessage = "StorageAccountId is required.")]
        public int StorageAccountId { get; set; }

        [Display(Name = "Account Name")]
        public string StorageType { get; set; }

        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Display(Name = "Account Key")]
        public string AccountKey { get; set; }
        public string AllowedFileExtension { get; set; }
        public long AllowedContentLength { get; set; }
        public IFormFile FileUpload { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
