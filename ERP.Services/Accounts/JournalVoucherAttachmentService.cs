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
    public class JournalVoucherAttachmentService : Repository<Journalvoucherattachment>, IJournalVoucherAttachment
    {
        private readonly IAttachment _attachment;
        public JournalVoucherAttachmentService(ErpDbContext dbContext, IAttachment attachment) : base(dbContext)
        {
            this._attachment = attachment;
        }
        public async Task<int> CreateAttachment(JournalVoucherAttachmentModel attachmentModel)
        {
            int associationId = 0;

            // assign values.
            Journalvoucherattachment journalVoucherAttachment = new Journalvoucherattachment();

            journalVoucherAttachment.JournalVoucherId = attachmentModel.JournalVoucherId;
            journalVoucherAttachment.AttachmentId = attachmentModel.AttachmentId;

            await Create(journalVoucherAttachment);

            associationId = journalVoucherAttachment.AssociationId;

            return associationId; // returns.
        }

        public async Task<bool> UpdateAttachment(JournalVoucherAttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Journalvoucherattachment journalVoucherAttachment = await GetByIdAsync(w => w.AssociationId == attachmentModel.AssociationId);

            if (null != journalVoucherAttachment)
            {
                // assign values.
                journalVoucherAttachment.AttachmentId = attachmentModel.AttachmentId;

                isUpdated = await Update(journalVoucherAttachment);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAttachment(int associationId)
        {
            bool isDeleted = false;

            // get record.
            Journalvoucherattachment journalVoucherAttachment = await GetByIdAsync(w => w.AssociationId == associationId);

            if (null != journalVoucherAttachment)
            {
                isDeleted = await Delete(journalVoucherAttachment);
            }

            return isDeleted; // returns.
        }

        public async Task<JournalVoucherAttachmentModel> GetAttachmentById(int associationId)
        {
            JournalVoucherAttachmentModel attachmentModel = null;

            IList<JournalVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(associationId, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        public async Task<DataTableResultModel<JournalVoucherAttachmentModel>> GetAttachmentByJournalVoucherId(int journalVoucherId)
        {
            DataTableResultModel<JournalVoucherAttachmentModel> resultModel = new DataTableResultModel<JournalVoucherAttachmentModel>();

            IList<JournalVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(0, journalVoucherId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<JournalVoucherAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<JournalVoucherAttachmentModel>();
                resultModel.ResultList = new List<JournalVoucherAttachmentModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<JournalVoucherAttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<JournalVoucherAttachmentModel> resultModel = new DataTableResultModel<JournalVoucherAttachmentModel>();

            IList<JournalVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(0, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<JournalVoucherAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<JournalVoucherAttachmentModel>> GetAttachmentList(int associationId, int journalVoucherId)
        {
            IList<JournalVoucherAttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Journalvoucherattachment> query = GetQueryByCondition(w => w.AssociationId != 0)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.Category)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.StorageAccount);

            // apply filters.
            if (0 != associationId)
                query = query.Where(w => w.AssociationId == associationId);

            // apply filters.
            if (0 != journalVoucherId)
                query = query.Where(w => w.JournalVoucherId == journalVoucherId);

            // get records by query.
            List<Journalvoucherattachment> journalVoucherAttachmentList = await query.ToListAsync();

            if (null != journalVoucherAttachmentList && journalVoucherAttachmentList.Count > 0)
            {
                attachmentModelList = new List<JournalVoucherAttachmentModel>();

                foreach (Journalvoucherattachment journalVoucherAttachment in journalVoucherAttachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(journalVoucherAttachment));
                }

                return attachmentModelList.OrderBy(o => o.CategoryName).ThenBy(o => o.Description).ToList();
            }

            return attachmentModelList; // returns.
        }

        private async Task<JournalVoucherAttachmentModel> AssignValueToModel(Journalvoucherattachment journalVoucherAttachment)
        {
            //return await Task.Run(() =>
            //{
            JournalVoucherAttachmentModel attachmentModel = new JournalVoucherAttachmentModel();

            attachmentModel.AssociationId = journalVoucherAttachment.AssociationId;
            attachmentModel.JournalVoucherId = journalVoucherAttachment.JournalVoucherId;
            attachmentModel.AttachmentId = journalVoucherAttachment.AttachmentId;

            if (journalVoucherAttachment.Attachment!=null)
            {
                attachmentModel.CategoryId = journalVoucherAttachment.Attachment.CategoryId;
                attachmentModel.Description = journalVoucherAttachment.Attachment.Description;
                attachmentModel.ContainerName = journalVoucherAttachment.Attachment.ContainerName;
                attachmentModel.ServerFileName = journalVoucherAttachment.Attachment.ServerFileName;
                attachmentModel.UserFileName = journalVoucherAttachment.Attachment.UserFileName;
                attachmentModel.FileExtension = journalVoucherAttachment.Attachment.FileExtension;
                attachmentModel.ContentType = journalVoucherAttachment.Attachment.ContentType;
                attachmentModel.ContentLength = journalVoucherAttachment.Attachment.ContentLength;
                attachmentModel.StorageAccountId = journalVoucherAttachment.Attachment.StorageAccountId;
                attachmentModel.AccountName = journalVoucherAttachment.Attachment.StorageAccount.AccountName;
                attachmentModel.AccountKey = journalVoucherAttachment.Attachment.StorageAccount.AccountKey;

                if (journalVoucherAttachment.Attachment.Category != null)
                {
                    attachmentModel.CategoryName = journalVoucherAttachment.Attachment.Category.CategoryName;
                }
                else
                {
                    attachmentModel.CategoryName = "";
                }

                if (journalVoucherAttachment.Attachment.StorageAccount!=null)
                {
                    attachmentModel.AccountName = journalVoucherAttachment.Attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = journalVoucherAttachment.Attachment.StorageAccount.AccountKey;
                    attachmentModel.StorageType = journalVoucherAttachment.Attachment.StorageAccount.StorageType;
                }
                else
                {
                    attachmentModel.AccountName = "";
                    attachmentModel.AccountKey = "";
                }

                attachmentModel.Url = await _attachment.GetUrl(journalVoucherAttachment.AttachmentId);

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

        public async Task<AttachmentModel> SaveJournalVoucherAttachment(AttachmentModel attachmentModel)
        {
            attachmentModel = await _attachment.SaveAttachment(attachmentModel);

            return attachmentModel;
        }
    }
}
