using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class PurchaseInvoiceAttachmentService : Repository<Purchaseinvoiceattachment>, IPurchaseInvoiceAttachment
    {
        private readonly IAttachment _attachment;
        public PurchaseInvoiceAttachmentService(ErpDbContext dbContext, IAttachment attachment) : base(dbContext)
        {
            this._attachment = attachment;
        }
        public async Task<int> CreateAttachment(PurchaseInvoiceAttachmentModel attachmentModel)
        {
            int associationId = 0;

            // assign values.
            Purchaseinvoiceattachment purchaseInvoiceAttachment = new Purchaseinvoiceattachment();

            purchaseInvoiceAttachment.PurchaseInvoiceId = attachmentModel.PurchaseInvoiceId;
            purchaseInvoiceAttachment.AttachmentId = attachmentModel.AttachmentId;

            await Create(purchaseInvoiceAttachment);

            associationId = purchaseInvoiceAttachment.AssociationId;

            return associationId; // returns.
        }

        public async Task<bool> UpdateAttachment(PurchaseInvoiceAttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoiceattachment purchaseInvoiceAttachment = await GetByIdAsync(w => w.AssociationId == attachmentModel.AssociationId);

            if (null != purchaseInvoiceAttachment)
            {
                // assign values.
                purchaseInvoiceAttachment.AttachmentId = attachmentModel.AttachmentId;

                isUpdated = await Update(purchaseInvoiceAttachment);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAttachment(int associationId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoiceattachment purchaseInvoiceAttachment = await GetByIdAsync(w => w.AssociationId == associationId);

            if (null != purchaseInvoiceAttachment)
            {
                isDeleted = await Delete(purchaseInvoiceAttachment);
            }

            return isDeleted; // returns.
        }

        public async Task<PurchaseInvoiceAttachmentModel> GetAttachmentById(int associationId)
        {
            PurchaseInvoiceAttachmentModel attachmentModel = null;

            IList<PurchaseInvoiceAttachmentModel> attachmentModelList = await GetAttachmentList(associationId, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        public async Task<DataTableResultModel<PurchaseInvoiceAttachmentModel>> GetAttachmentByPurchaseInvoiceId(int purchaseInvoiceId)
        {
            DataTableResultModel<PurchaseInvoiceAttachmentModel> resultModel = new DataTableResultModel<PurchaseInvoiceAttachmentModel>();

            IList<PurchaseInvoiceAttachmentModel> attachmentModelList = await GetAttachmentList(0, purchaseInvoiceId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceAttachmentModel>();
                resultModel.ResultList = new List<PurchaseInvoiceAttachmentModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<PurchaseInvoiceAttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<PurchaseInvoiceAttachmentModel> resultModel = new DataTableResultModel<PurchaseInvoiceAttachmentModel>();

            IList<PurchaseInvoiceAttachmentModel> attachmentModelList = await GetAttachmentList(0, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<PurchaseInvoiceAttachmentModel>> GetAttachmentList(int associationId, int purchaseInvoiceId)
        {
            IList<PurchaseInvoiceAttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Purchaseinvoiceattachment> query = GetQueryByCondition(w => w.AssociationId != 0)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.Category)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.StorageAccount);

            // apply filters.
            if (0 != associationId)
                query = query.Where(w => w.AssociationId == associationId);

            // apply filters.
            if (0 != purchaseInvoiceId)
                query = query.Where(w => w.PurchaseInvoiceId == purchaseInvoiceId);

            // get records by query.
            List<Purchaseinvoiceattachment> purchaseInvoiceAttachmentList = await query.ToListAsync();

            if (null != purchaseInvoiceAttachmentList && purchaseInvoiceAttachmentList.Count > 0)
            {
                attachmentModelList = new List<PurchaseInvoiceAttachmentModel>();

                foreach (Purchaseinvoiceattachment purchaseInvoiceAttachment in purchaseInvoiceAttachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(purchaseInvoiceAttachment));
                }

                return attachmentModelList.OrderBy(o => o.CategoryName).ThenBy(o => o.Description).ToList();
            }

            return attachmentModelList; // returns.
        }

        private async Task<PurchaseInvoiceAttachmentModel> AssignValueToModel(Purchaseinvoiceattachment purchaseInvoiceAttachment)
        {
            //return await Task.Run(() =>
            //{
            PurchaseInvoiceAttachmentModel attachmentModel = new PurchaseInvoiceAttachmentModel();

            attachmentModel.AssociationId = purchaseInvoiceAttachment.AssociationId;
            attachmentModel.PurchaseInvoiceId = purchaseInvoiceAttachment.PurchaseInvoiceId;
            attachmentModel.AttachmentId = purchaseInvoiceAttachment.AttachmentId;

            if (purchaseInvoiceAttachment.Attachment!=null)
            {
                attachmentModel.CategoryId = purchaseInvoiceAttachment.Attachment.CategoryId;
                attachmentModel.Description = purchaseInvoiceAttachment.Attachment.Description;
                attachmentModel.ContainerName = purchaseInvoiceAttachment.Attachment.ContainerName;
                attachmentModel.ServerFileName = purchaseInvoiceAttachment.Attachment.ServerFileName;
                attachmentModel.UserFileName = purchaseInvoiceAttachment.Attachment.UserFileName;
                attachmentModel.FileExtension = purchaseInvoiceAttachment.Attachment.FileExtension;
                attachmentModel.ContentType = purchaseInvoiceAttachment.Attachment.ContentType;
                attachmentModel.ContentLength = purchaseInvoiceAttachment.Attachment.ContentLength;
                attachmentModel.StorageAccountId = purchaseInvoiceAttachment.Attachment.StorageAccountId;
                attachmentModel.AccountName = purchaseInvoiceAttachment.Attachment.StorageAccount.AccountName;
                attachmentModel.AccountKey = purchaseInvoiceAttachment.Attachment.StorageAccount.AccountKey;

                if (purchaseInvoiceAttachment.Attachment.Category != null)
                {
                    attachmentModel.CategoryName = purchaseInvoiceAttachment.Attachment.Category.CategoryName;
                }
                else
                {
                    attachmentModel.CategoryName = "";
                }

                if (purchaseInvoiceAttachment.Attachment.StorageAccount!=null)
                {
                    attachmentModel.AccountName = purchaseInvoiceAttachment.Attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = purchaseInvoiceAttachment.Attachment.StorageAccount.AccountKey;
                    attachmentModel.StorageType = purchaseInvoiceAttachment.Attachment.StorageAccount.StorageType;
                }
                else
                {
                    attachmentModel.AccountName = "";
                    attachmentModel.AccountKey = "";
                }

                attachmentModel.Url = await _attachment.GetUrl(purchaseInvoiceAttachment.AttachmentId);

            }
            else
            {
                attachmentModel.CategoryId = 0;
                attachmentModel.CategoryName = "";
                attachmentModel.Description = "";
                attachmentModel.ContainerName = "";
                attachmentModel.ServerFileName = "";
                attachmentModel.UserFileName = "";
                attachmentModel.FileExtension = "";
                attachmentModel.ContentType = "";
                attachmentModel.ContentLength =0;
                attachmentModel.Url = "";
                attachmentModel.StorageAccountId = 0;
                attachmentModel.StorageType = "";
                attachmentModel.AccountName = "";
                attachmentModel.AccountKey = "";
            }

            return attachmentModel;
            //});
        }

        public async Task<AttachmentModel> SaveInvoiceAttachment(AttachmentModel attachmentModel)
        {
            attachmentModel = await _attachment.SaveAttachment(attachmentModel);

            return attachmentModel;
        }
    }
}
