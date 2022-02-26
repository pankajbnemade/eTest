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
    public class PaymentVoucherAttachmentService : Repository<Paymentvoucherattachment>, IPaymentVoucherAttachment
    {
        private readonly IAttachment _attachment;
        public PaymentVoucherAttachmentService(ErpDbContext dbContext, IAttachment attachment) : base(dbContext)
        {
            this._attachment = attachment;
        }
        public async Task<int> CreateAttachment(PaymentVoucherAttachmentModel attachmentModel)
        {
            int associationId = 0;

            // assign values.
            Paymentvoucherattachment paymentVoucherAttachment = new Paymentvoucherattachment();

            paymentVoucherAttachment.PaymentVoucherId = attachmentModel.PaymentVoucherId;
            paymentVoucherAttachment.AttachmentId = attachmentModel.AttachmentId;

            await Create(paymentVoucherAttachment);

            associationId = paymentVoucherAttachment.AssociationId;

            return associationId; // returns.
        }

        public async Task<bool> UpdateAttachment(PaymentVoucherAttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Paymentvoucherattachment paymentVoucherAttachment = await GetByIdAsync(w => w.AssociationId == attachmentModel.AssociationId);

            if (null != paymentVoucherAttachment)
            {
                // assign values.
                paymentVoucherAttachment.AttachmentId = attachmentModel.AttachmentId;

                isUpdated = await Update(paymentVoucherAttachment);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAttachment(int associationId)
        {
            bool isDeleted = false;

            // get record.
            Paymentvoucherattachment paymentVoucherAttachment = await GetByIdAsync(w => w.AssociationId == associationId);

            if (null != paymentVoucherAttachment)
            {
                isDeleted = await Delete(paymentVoucherAttachment);
            }

            return isDeleted; // returns.
        }

        public async Task<PaymentVoucherAttachmentModel> GetAttachmentById(int associationId)
        {
            PaymentVoucherAttachmentModel attachmentModel = null;

            IList<PaymentVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(associationId, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        public async Task<DataTableResultModel<PaymentVoucherAttachmentModel>> GetAttachmentByPaymentVoucherId(int paymentVoucherId)
        {
            DataTableResultModel<PaymentVoucherAttachmentModel> resultModel = new DataTableResultModel<PaymentVoucherAttachmentModel>();

            IList<PaymentVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(0, paymentVoucherId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<PaymentVoucherAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<PaymentVoucherAttachmentModel>();
                resultModel.ResultList = new List<PaymentVoucherAttachmentModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<PaymentVoucherAttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<PaymentVoucherAttachmentModel> resultModel = new DataTableResultModel<PaymentVoucherAttachmentModel>();

            IList<PaymentVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(0, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<PaymentVoucherAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<PaymentVoucherAttachmentModel>> GetAttachmentList(int associationId, int paymentVoucherId)
        {
            IList<PaymentVoucherAttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Paymentvoucherattachment> query = GetQueryByCondition(w => w.AssociationId != 0)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.Category)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.StorageAccount);

            // apply filters.
            if (0 != associationId)
                query = query.Where(w => w.AssociationId == associationId);

            // apply filters.
            if (0 != paymentVoucherId)
                query = query.Where(w => w.PaymentVoucherId == paymentVoucherId);

            // get records by query.
            List<Paymentvoucherattachment> paymentVoucherAttachmentList = await query.ToListAsync();

            if (null != paymentVoucherAttachmentList && paymentVoucherAttachmentList.Count > 0)
            {
                attachmentModelList = new List<PaymentVoucherAttachmentModel>();

                foreach (Paymentvoucherattachment paymentVoucherAttachment in paymentVoucherAttachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(paymentVoucherAttachment));
                }

                return attachmentModelList.OrderBy(o => o.CategoryName).ThenBy(o => o.Description).ToList();
            }

            return attachmentModelList; // returns.
        }

        private async Task<PaymentVoucherAttachmentModel> AssignValueToModel(Paymentvoucherattachment paymentVoucherAttachment)
        {
            //return await Task.Run(() =>
            //{
            PaymentVoucherAttachmentModel attachmentModel = new PaymentVoucherAttachmentModel();

            attachmentModel.AssociationId = paymentVoucherAttachment.AssociationId;
            attachmentModel.PaymentVoucherId = paymentVoucherAttachment.PaymentVoucherId;
            attachmentModel.AttachmentId = paymentVoucherAttachment.AttachmentId;

            if (paymentVoucherAttachment.Attachment!=null)
            {
                attachmentModel.CategoryId = paymentVoucherAttachment.Attachment.CategoryId;
                attachmentModel.Description = paymentVoucherAttachment.Attachment.Description;
                attachmentModel.ContainerName = paymentVoucherAttachment.Attachment.ContainerName;
                attachmentModel.ServerFileName = paymentVoucherAttachment.Attachment.ServerFileName;
                attachmentModel.UserFileName = paymentVoucherAttachment.Attachment.UserFileName;
                attachmentModel.FileExtension = paymentVoucherAttachment.Attachment.FileExtension;
                attachmentModel.ContentType = paymentVoucherAttachment.Attachment.ContentType;
                attachmentModel.ContentLength = paymentVoucherAttachment.Attachment.ContentLength;
                attachmentModel.StorageAccountId = paymentVoucherAttachment.Attachment.StorageAccountId;
                attachmentModel.AccountName = paymentVoucherAttachment.Attachment.StorageAccount.AccountName;
                attachmentModel.AccountKey = paymentVoucherAttachment.Attachment.StorageAccount.AccountKey;

                if (paymentVoucherAttachment.Attachment.Category != null)
                {
                    attachmentModel.CategoryName = paymentVoucherAttachment.Attachment.Category.CategoryName;
                }
                else
                {
                    attachmentModel.CategoryName = "";
                }

                if (paymentVoucherAttachment.Attachment.StorageAccount!=null)
                {
                    attachmentModel.AccountName = paymentVoucherAttachment.Attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = paymentVoucherAttachment.Attachment.StorageAccount.AccountKey;
                    attachmentModel.StorageType = paymentVoucherAttachment.Attachment.StorageAccount.StorageType;
                }
                else
                {
                    attachmentModel.AccountName = "";
                    attachmentModel.AccountKey = "";
                }

                attachmentModel.Url = await _attachment.GetUrl(paymentVoucherAttachment.AttachmentId);

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

        public async Task<AttachmentModel> SavePaymentVoucherAttachment(AttachmentModel attachmentModel)
        {
            attachmentModel = await _attachment.SaveAttachment(attachmentModel);

            return attachmentModel;
        }
    }
}
