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
    public class ReceiptVoucherAttachmentService : Repository<Receiptvoucherattachment>, IReceiptVoucherAttachment
    {
        private readonly IAttachment _attachment;
        public ReceiptVoucherAttachmentService(ErpDbContext dbContext, IAttachment attachment) : base(dbContext)
        {
            this._attachment = attachment;
        }
        public async Task<int> CreateAttachment(ReceiptVoucherAttachmentModel attachmentModel)
        {
            int associationId = 0;

            // assign values.
            Receiptvoucherattachment receiptVoucherAttachment = new Receiptvoucherattachment();

            receiptVoucherAttachment.ReceiptVoucherId = attachmentModel.ReceiptVoucherId;
            receiptVoucherAttachment.AttachmentId = attachmentModel.AttachmentId;

            await Create(receiptVoucherAttachment);

            associationId = receiptVoucherAttachment.AssociationId;

            return associationId; // returns.
        }

        public async Task<bool> UpdateAttachment(ReceiptVoucherAttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Receiptvoucherattachment receiptVoucherAttachment = await GetByIdAsync(w => w.AssociationId == attachmentModel.AssociationId);

            if (null != receiptVoucherAttachment)
            {
                // assign values.
                receiptVoucherAttachment.AttachmentId = attachmentModel.AttachmentId;

                isUpdated = await Update(receiptVoucherAttachment);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAttachment(int associationId)
        {
            bool isDeleted = false;

            // get record.
            Receiptvoucherattachment receiptVoucherAttachment = await GetByIdAsync(w => w.AssociationId == associationId);

            if (null != receiptVoucherAttachment)
            {
                isDeleted = await Delete(receiptVoucherAttachment);
            }

            return isDeleted; // returns.
        }

        public async Task<ReceiptVoucherAttachmentModel> GetAttachmentById(int associationId)
        {
            ReceiptVoucherAttachmentModel attachmentModel = null;

            IList<ReceiptVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(associationId, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        public async Task<DataTableResultModel<ReceiptVoucherAttachmentModel>> GetAttachmentByReceiptVoucherId(int receiptVoucherId)
        {
            DataTableResultModel<ReceiptVoucherAttachmentModel> resultModel = new DataTableResultModel<ReceiptVoucherAttachmentModel>();

            IList<ReceiptVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(0, receiptVoucherId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<ReceiptVoucherAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<ReceiptVoucherAttachmentModel>();
                resultModel.ResultList = new List<ReceiptVoucherAttachmentModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<ReceiptVoucherAttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<ReceiptVoucherAttachmentModel> resultModel = new DataTableResultModel<ReceiptVoucherAttachmentModel>();

            IList<ReceiptVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(0, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<ReceiptVoucherAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<ReceiptVoucherAttachmentModel>> GetAttachmentList(int associationId, int receiptVoucherId)
        {
            IList<ReceiptVoucherAttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Receiptvoucherattachment> query = GetQueryByCondition(w => w.AssociationId != 0)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.Category)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.StorageAccount);

            // apply filters.
            if (0 != associationId)
                query = query.Where(w => w.AssociationId == associationId);

            // apply filters.
            if (0 != receiptVoucherId)
                query = query.Where(w => w.ReceiptVoucherId == receiptVoucherId);

            // get records by query.
            List<Receiptvoucherattachment> receiptVoucherAttachmentList = await query.ToListAsync();

            if (null != receiptVoucherAttachmentList && receiptVoucherAttachmentList.Count > 0)
            {
                attachmentModelList = new List<ReceiptVoucherAttachmentModel>();

                foreach (Receiptvoucherattachment receiptVoucherAttachment in receiptVoucherAttachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(receiptVoucherAttachment));
                }

                return attachmentModelList.OrderBy(o => o.CategoryName).ThenBy(o => o.Description).ToList();
            }

            return attachmentModelList; // returns.
        }

        private async Task<ReceiptVoucherAttachmentModel> AssignValueToModel(Receiptvoucherattachment receiptVoucherAttachment)
        {
            //return await Task.Run(() =>
            //{
            ReceiptVoucherAttachmentModel attachmentModel = new ReceiptVoucherAttachmentModel();

            attachmentModel.AssociationId = receiptVoucherAttachment.AssociationId;
            attachmentModel.ReceiptVoucherId = receiptVoucherAttachment.ReceiptVoucherId;
            attachmentModel.AttachmentId = receiptVoucherAttachment.AttachmentId;

            if (receiptVoucherAttachment.Attachment!=null)
            {
                attachmentModel.CategoryId = receiptVoucherAttachment.Attachment.CategoryId;
                attachmentModel.Description = receiptVoucherAttachment.Attachment.Description;
                attachmentModel.ContainerName = receiptVoucherAttachment.Attachment.ContainerName;
                attachmentModel.ServerFileName = receiptVoucherAttachment.Attachment.ServerFileName;
                attachmentModel.UserFileName = receiptVoucherAttachment.Attachment.UserFileName;
                attachmentModel.FileExtension = receiptVoucherAttachment.Attachment.FileExtension;
                attachmentModel.ContentType = receiptVoucherAttachment.Attachment.ContentType;
                attachmentModel.ContentLength = receiptVoucherAttachment.Attachment.ContentLength;
                attachmentModel.StorageAccountId = receiptVoucherAttachment.Attachment.StorageAccountId;
                attachmentModel.AccountName = receiptVoucherAttachment.Attachment.StorageAccount.AccountName;
                attachmentModel.AccountKey = receiptVoucherAttachment.Attachment.StorageAccount.AccountKey;

                if (receiptVoucherAttachment.Attachment.Category != null)
                {
                    attachmentModel.CategoryName = receiptVoucherAttachment.Attachment.Category.CategoryName;
                }
                else
                {
                    attachmentModel.CategoryName = "";
                }

                if (receiptVoucherAttachment.Attachment.StorageAccount!=null)
                {
                    attachmentModel.AccountName = receiptVoucherAttachment.Attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = receiptVoucherAttachment.Attachment.StorageAccount.AccountKey;
                    attachmentModel.StorageType = receiptVoucherAttachment.Attachment.StorageAccount.StorageType;
                }
                else
                {
                    attachmentModel.AccountName = "";
                    attachmentModel.AccountKey = "";
                }

                attachmentModel.Url = await _attachment.GetUrl(receiptVoucherAttachment.AttachmentId);

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

        public async Task<AttachmentModel> SaveReceiptVoucherAttachment(AttachmentModel attachmentModel)
        {
            attachmentModel = await _attachment.SaveAttachment(attachmentModel);

            return attachmentModel;
        }
    }
}
