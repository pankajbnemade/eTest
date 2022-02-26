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
    public class DebitNoteAttachmentService : Repository<Debitnoteattachment>, IDebitNoteAttachment
    {
        private readonly IAttachment _attachment;
        public DebitNoteAttachmentService(ErpDbContext dbContext, IAttachment attachment) : base(dbContext)
        {
            this._attachment = attachment;
        }
        public async Task<int> CreateAttachment(DebitNoteAttachmentModel attachmentModel)
        {
            int associationId = 0;

            // assign values.
            Debitnoteattachment debitNoteAttachment = new Debitnoteattachment();

            debitNoteAttachment.DebitNoteId = attachmentModel.DebitNoteId;
            debitNoteAttachment.AttachmentId = attachmentModel.AttachmentId;

            await Create(debitNoteAttachment);

            associationId = debitNoteAttachment.AssociationId;

            return associationId; // returns.
        }

        public async Task<bool> UpdateAttachment(DebitNoteAttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Debitnoteattachment debitNoteAttachment = await GetByIdAsync(w => w.AssociationId == attachmentModel.AssociationId);

            if (null != debitNoteAttachment)
            {
                // assign values.
                debitNoteAttachment.AttachmentId = attachmentModel.AttachmentId;

                isUpdated = await Update(debitNoteAttachment);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAttachment(int associationId)
        {
            bool isDeleted = false;

            // get record.
            Debitnoteattachment debitNoteAttachment = await GetByIdAsync(w => w.AssociationId == associationId);

            if (null != debitNoteAttachment)
            {
                isDeleted = await Delete(debitNoteAttachment);
            }

            return isDeleted; // returns.
        }

        public async Task<DebitNoteAttachmentModel> GetAttachmentById(int associationId)
        {
            DebitNoteAttachmentModel attachmentModel = null;

            IList<DebitNoteAttachmentModel> attachmentModelList = await GetAttachmentList(associationId, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        public async Task<DataTableResultModel<DebitNoteAttachmentModel>> GetAttachmentByDebitNoteId(int debitNoteId)
        {
            DataTableResultModel<DebitNoteAttachmentModel> resultModel = new DataTableResultModel<DebitNoteAttachmentModel>();

            IList<DebitNoteAttachmentModel> attachmentModelList = await GetAttachmentList(0, debitNoteId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<DebitNoteAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<DebitNoteAttachmentModel>();
                resultModel.ResultList = new List<DebitNoteAttachmentModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<DebitNoteAttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<DebitNoteAttachmentModel> resultModel = new DataTableResultModel<DebitNoteAttachmentModel>();

            IList<DebitNoteAttachmentModel> attachmentModelList = await GetAttachmentList(0, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<DebitNoteAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<DebitNoteAttachmentModel>> GetAttachmentList(int associationId, int debitNoteId)
        {
            IList<DebitNoteAttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Debitnoteattachment> query = GetQueryByCondition(w => w.AssociationId != 0)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.Category)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.StorageAccount);

            // apply filters.
            if (0 != associationId)
                query = query.Where(w => w.AssociationId == associationId);

            // apply filters.
            if (0 != debitNoteId)
                query = query.Where(w => w.DebitNoteId == debitNoteId);

            // get records by query.
            List<Debitnoteattachment> debitNoteAttachmentList = await query.ToListAsync();

            if (null != debitNoteAttachmentList && debitNoteAttachmentList.Count > 0)
            {
                attachmentModelList = new List<DebitNoteAttachmentModel>();

                foreach (Debitnoteattachment debitNoteAttachment in debitNoteAttachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(debitNoteAttachment));
                }

                return attachmentModelList.OrderBy(o => o.CategoryName).ThenBy(o => o.Description).ToList();
            }

            return attachmentModelList; // returns.
        }

        private async Task<DebitNoteAttachmentModel> AssignValueToModel(Debitnoteattachment debitNoteAttachment)
        {
            //return await Task.Run(() =>
            //{
            DebitNoteAttachmentModel attachmentModel = new DebitNoteAttachmentModel();

            attachmentModel.AssociationId = debitNoteAttachment.AssociationId;
            attachmentModel.DebitNoteId = debitNoteAttachment.DebitNoteId;
            attachmentModel.AttachmentId = debitNoteAttachment.AttachmentId;

            if (debitNoteAttachment.Attachment!=null)
            {
                attachmentModel.CategoryId = debitNoteAttachment.Attachment.CategoryId;
                attachmentModel.Description = debitNoteAttachment.Attachment.Description;
                attachmentModel.ContainerName = debitNoteAttachment.Attachment.ContainerName;
                attachmentModel.ServerFileName = debitNoteAttachment.Attachment.ServerFileName;
                attachmentModel.UserFileName = debitNoteAttachment.Attachment.UserFileName;
                attachmentModel.FileExtension = debitNoteAttachment.Attachment.FileExtension;
                attachmentModel.ContentType = debitNoteAttachment.Attachment.ContentType;
                attachmentModel.ContentLength = debitNoteAttachment.Attachment.ContentLength;
                attachmentModel.StorageAccountId = debitNoteAttachment.Attachment.StorageAccountId;
                attachmentModel.AccountName = debitNoteAttachment.Attachment.StorageAccount.AccountName;
                attachmentModel.AccountKey = debitNoteAttachment.Attachment.StorageAccount.AccountKey;

                if (debitNoteAttachment.Attachment.Category != null)
                {
                    attachmentModel.CategoryName = debitNoteAttachment.Attachment.Category.CategoryName;
                }
                else
                {
                    attachmentModel.CategoryName = "";
                }

                if (debitNoteAttachment.Attachment.StorageAccount!=null)
                {
                    attachmentModel.AccountName = debitNoteAttachment.Attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = debitNoteAttachment.Attachment.StorageAccount.AccountKey;
                    attachmentModel.StorageType = debitNoteAttachment.Attachment.StorageAccount.StorageType;
                }
                else
                {
                    attachmentModel.AccountName = "";
                    attachmentModel.AccountKey = "";
                }

                attachmentModel.Url = await _attachment.GetUrl(debitNoteAttachment.AttachmentId);

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

        public async Task<AttachmentModel> SaveDebitNoteAttachment(AttachmentModel attachmentModel)
        {
            attachmentModel = await _attachment.SaveAttachment(attachmentModel);

            return attachmentModel;
        }
    }
}
