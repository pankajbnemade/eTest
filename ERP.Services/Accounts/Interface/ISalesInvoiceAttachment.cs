using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceAttachment : IRepository<Salesinvoiceattachment>
    {
        Task<int> CreateAttachment(SalesInvoiceAttachmentModel currencyModel);
       
        Task<bool> UpdateAttachment(SalesInvoiceAttachmentModel currencyModel);
      
        Task<bool> DeleteAttachment(int associationId);
        
        Task<SalesInvoiceAttachmentModel> GetAttachmentById(int associationId);

        Task<DataTableResultModel<SalesInvoiceAttachmentModel>> GetAttachmentBySalesInvoiceId(int salesInvoiceId);

        Task<DataTableResultModel<SalesInvoiceAttachmentModel>> GetAttachmentList();

        Task<AttachmentModel> SaveInvoiceAttachment(AttachmentModel attachmentModel);

    }
}
