using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SalesInvoiceAttachmentModel
    {
        public int AssociationId { get; set; }
        public int SalesInvoiceId { get; set; }
        public int AttachmentId { get; set; }
        public string PreparedByName { get; set; }

    }
}
