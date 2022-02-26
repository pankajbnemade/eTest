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
    public class CreditNoteAttachmentService : Repository<Creditnoteattachment>, ICreditNoteAttachment
    {
        private readonly IAttachment _attachment;
        public CreditNoteAttachmentService(ErpDbContext dbContext, IAttachment attachment) : base(dbContext)
        {
            this._attachment = attachment;
        }
        public async Task<int> CreateAttachment(CreditNoteAttachmentModel attachmentModel)
        {
            int associationId = 0;

            // assign values.
            Creditnoteattachment creditNoteAttachment = new Creditnoteattachment();

            creditNoteAttachment.CreditNoteId = attachmentModel.CreditNoteId;
            creditNoteAttachment.AttachmentId = attachmentModel.AttachmentId;

            await Create(creditNoteAttachment);

            associationId = creditNoteAttachment.AssociationId;

            return associationId; // returns.
        }

        public async Task<bool> UpdateAttachment(CreditNoteAttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Creditnoteattachment creditNoteAttachment = await GetByIdAsync(w => w.AssociationId == attachmentModel.AssociationId);

            if (null != creditNoteAttachment)
            {
                // assign values.
                creditNoteAttachment.AttachmentId = attachmentModel.AttachmentId;

                isUpdated = await Update(creditNoteAttachment);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAttachment(int associationId)
        {
            bool isDeleted = false;

            // get record.
            Creditnoteattachment creditNoteAttachment = await GetByIdAsync(w => w.AssociationId == associationId);

            if (null != creditNoteAttachment)
            {
                isDeleted = await Delete(creditNoteAttachment);
            }

            return isDeleted; // returns.
        }

        public async Task<CreditNoteAttachmentModel> GetAttachmentById(int associationId)
        {
            CreditNoteAttachmentModel attachmentModel = null;

            IList<CreditNoteAttachmentModel> attachmentModelList = await GetAttachmentList(associationId, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        public async Task<DataTableResultModel<CreditNoteAttachmentModel>> GetAttachmentByCreditNoteId(int creditNoteId)
        {
            DataTableResultModel<CreditNoteAttachmentModel> resultModel = new DataTableResultModel<CreditNoteAttachmentModel>();

            IList<CreditNoteAttachmentModel> attachmentModelList = await GetAttachmentList(0, creditNoteId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<CreditNoteAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<CreditNoteAttachmentModel>();
                resultModel.ResultList = new List<CreditNoteAttachmentModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<CreditNoteAttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<CreditNoteAttachmentModel> resultModel = new DataTableResultModel<CreditNoteAttachmentModel>();

            IList<CreditNoteAttachmentModel> attachmentModelList = await GetAttachmentList(0, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<CreditNoteAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<CreditNoteAttachmentModel>> GetAttachmentList(int associationId, int creditNoteId)
        {
            IList<CreditNoteAttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Creditnoteattachment> query = GetQueryByCondition(w => w.AssociationId != 0)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.Category)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.StorageAccount);

            // apply filters.
            if (0 != associationId)
                query = query.Where(w => w.AssociationId == associationId);

            // apply filters.
            if (0 != creditNoteId)
                query = query.Where(w => w.CreditNoteId == creditNoteId);

            // get records by query.
            List<Creditnoteattachment> creditNoteAttachmentList = await query.ToListAsync();

            if (null != creditNoteAttachmentList && creditNoteAttachmentList.Count > 0)
            {
                attachmentModelList = new List<CreditNoteAttachmentModel>();

                foreach (Creditnoteattachment creditNoteAttachment in creditNoteAttachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(creditNoteAttachment));
                }

                return attachmentModelList.OrderBy(o => o.CategoryName).ThenBy(o => o.Description).ToList();
            }

            return attachmentModelList; // returns.
        }

        private async Task<CreditNoteAttachmentModel> AssignValueToModel(Creditnoteattachment creditNoteAttachment)
        {
            //return await Task.Run(() =>
            //{
            CreditNoteAttachmentModel attachmentModel = new CreditNoteAttachmentModel();

            attachmentModel.AssociationId = creditNoteAttachment.AssociationId;
            attachmentModel.CreditNoteId = creditNoteAttachment.CreditNoteId;
            attachmentModel.AttachmentId = creditNoteAttachment.AttachmentId;

            if (creditNoteAttachment.Attachment!=null)
            {
                attachmentModel.CategoryId = creditNoteAttachment.Attachment.CategoryId;
                attachmentModel.Description = creditNoteAttachment.Attachment.Description;
                attachmentModel.ContainerName = creditNoteAttachment.Attachment.ContainerName;
                attachmentModel.ServerFileName = creditNoteAttachment.Attachment.ServerFileName;
                attachmentModel.UserFileName = creditNoteAttachment.Attachment.UserFileName;
                attachmentModel.FileExtension = creditNoteAttachment.Attachment.FileExtension;
                attachmentModel.ContentType = creditNoteAttachment.Attachment.ContentType;
                attachmentModel.ContentLength = creditNoteAttachment.Attachment.ContentLength;
                attachmentModel.StorageAccountId = creditNoteAttachment.Attachment.StorageAccountId;
                attachmentModel.AccountName = creditNoteAttachment.Attachment.StorageAccount.AccountName;
                attachmentModel.AccountKey = creditNoteAttachment.Attachment.StorageAccount.AccountKey;

                if (creditNoteAttachment.Attachment.Category != null)
                {
                    attachmentModel.CategoryName = creditNoteAttachment.Attachment.Category.CategoryName;
                }
                else
                {
                    attachmentModel.CategoryName = "";
                }

                if (creditNoteAttachment.Attachment.StorageAccount!=null)
                {
                    attachmentModel.AccountName = creditNoteAttachment.Attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = creditNoteAttachment.Attachment.StorageAccount.AccountKey;
                    attachmentModel.StorageType = creditNoteAttachment.Attachment.StorageAccount.StorageType;
                }
                else
                {
                    attachmentModel.AccountName = "";
                    attachmentModel.AccountKey = "";
                }

                attachmentModel.Url = await _attachment.GetUrl(creditNoteAttachment.AttachmentId);

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

        public async Task<AttachmentModel> SaveCreditNoteAttachment(AttachmentModel attachmentModel)
        {
            attachmentModel = await _attachment.SaveAttachment(attachmentModel);

            return attachmentModel;
        }
    }
}
