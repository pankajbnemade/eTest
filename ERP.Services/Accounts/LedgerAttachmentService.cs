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
    public class LedgerAttachmentService : Repository<Ledgerattachment>, ILedgerAttachment
    {
        private readonly IAttachment _attachment;
        public LedgerAttachmentService(ErpDbContext dbContext, IAttachment attachment) : base(dbContext)
        {
            this._attachment = attachment;
        }
        public async Task<int> CreateAttachment(LedgerAttachmentModel attachmentModel)
        {
            int associationId = 0;

            // assign values.
            Ledgerattachment ledgerAttachment = new Ledgerattachment();

            ledgerAttachment.LedgerId = attachmentModel.LedgerId;
            ledgerAttachment.AttachmentId = attachmentModel.AttachmentId;

            await Create(ledgerAttachment);

            associationId = ledgerAttachment.AssociationId;

            return associationId; // returns.
        }

        public async Task<bool> UpdateAttachment(LedgerAttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Ledgerattachment ledgerAttachment = await GetByIdAsync(w => w.AssociationId == attachmentModel.AssociationId);

            if (null != ledgerAttachment)
            {
                // assign values.
                ledgerAttachment.AttachmentId = attachmentModel.AttachmentId;

                isUpdated = await Update(ledgerAttachment);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAttachment(int associationId)
        {
            bool isDeleted = false;

            // get record.
            Ledgerattachment ledgerAttachment = await GetByIdAsync(w => w.AssociationId == associationId);

            if (null != ledgerAttachment)
            {
                isDeleted = await Delete(ledgerAttachment);
            }

            return isDeleted; // returns.
        }

        public async Task<LedgerAttachmentModel> GetAttachmentById(int associationId)
        {
            LedgerAttachmentModel attachmentModel = null;

            IList<LedgerAttachmentModel> attachmentModelList = await GetAttachmentList(associationId, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        public async Task<DataTableResultModel<LedgerAttachmentModel>> GetAttachmentByLedgerId(int ledgerId)
        {
            DataTableResultModel<LedgerAttachmentModel> resultModel = new DataTableResultModel<LedgerAttachmentModel>();

            IList<LedgerAttachmentModel> attachmentModelList = await GetAttachmentList(0, ledgerId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<LedgerAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<LedgerAttachmentModel>();
                resultModel.ResultList = new List<LedgerAttachmentModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<LedgerAttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<LedgerAttachmentModel> resultModel = new DataTableResultModel<LedgerAttachmentModel>();

            IList<LedgerAttachmentModel> attachmentModelList = await GetAttachmentList(0, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<LedgerAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<LedgerAttachmentModel>> GetAttachmentList(int associationId, int ledgerId)
        {
            IList<LedgerAttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Ledgerattachment> query = GetQueryByCondition(w => w.AssociationId != 0)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.Category)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.StorageAccount);

            // apply filters.
            if (0 != associationId)
                query = query.Where(w => w.AssociationId == associationId);

            // apply filters.
            if (0 != ledgerId)
                query = query.Where(w => w.LedgerId == ledgerId);

            // get records by query.
            List<Ledgerattachment> ledgerAttachmentList = await query.ToListAsync();

            if (null != ledgerAttachmentList && ledgerAttachmentList.Count > 0)
            {
                attachmentModelList = new List<LedgerAttachmentModel>();

                foreach (Ledgerattachment ledgerAttachment in ledgerAttachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(ledgerAttachment));
                }

                return attachmentModelList.OrderBy(o => o.CategoryName).ThenBy(o => o.Description).ToList();
            }

            return attachmentModelList; // returns.
        }

        private async Task<LedgerAttachmentModel> AssignValueToModel(Ledgerattachment ledgerAttachment)
        {
            //return await Task.Run(() =>
            //{
            LedgerAttachmentModel attachmentModel = new LedgerAttachmentModel();

            attachmentModel.AssociationId = ledgerAttachment.AssociationId;
            attachmentModel.LedgerId = ledgerAttachment.LedgerId;
            attachmentModel.AttachmentId = ledgerAttachment.AttachmentId;

            if (ledgerAttachment.Attachment!=null)
            {
                attachmentModel.CategoryId = ledgerAttachment.Attachment.CategoryId;
                attachmentModel.Description = ledgerAttachment.Attachment.Description;
                attachmentModel.ContainerName = ledgerAttachment.Attachment.ContainerName;
                attachmentModel.ServerFileName = ledgerAttachment.Attachment.ServerFileName;
                attachmentModel.UserFileName = ledgerAttachment.Attachment.UserFileName;
                attachmentModel.FileExtension = ledgerAttachment.Attachment.FileExtension;
                attachmentModel.ContentType = ledgerAttachment.Attachment.ContentType;
                attachmentModel.ContentLength = ledgerAttachment.Attachment.ContentLength;
                attachmentModel.StorageAccountId = ledgerAttachment.Attachment.StorageAccountId;
                attachmentModel.AccountName = ledgerAttachment.Attachment.StorageAccount.AccountName;
                attachmentModel.AccountKey = ledgerAttachment.Attachment.StorageAccount.AccountKey;

                if (ledgerAttachment.Attachment.Category != null)
                {
                    attachmentModel.CategoryName = ledgerAttachment.Attachment.Category.CategoryName;
                }
                else
                {
                    attachmentModel.CategoryName = "";
                }

                if (ledgerAttachment.Attachment.StorageAccount!=null)
                {
                    attachmentModel.AccountName = ledgerAttachment.Attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = ledgerAttachment.Attachment.StorageAccount.AccountKey;
                    attachmentModel.StorageType = ledgerAttachment.Attachment.StorageAccount.StorageType;
                }
                else
                {
                    attachmentModel.AccountName = "";
                    attachmentModel.AccountKey = "";
                }

                attachmentModel.Url = await _attachment.GetUrl(ledgerAttachment.AttachmentId);

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

        public async Task<AttachmentModel> SaveLedgerAttachment(AttachmentModel attachmentModel)
        {
            attachmentModel = await _attachment.SaveAttachment(attachmentModel);

            return attachmentModel;
        }
    }
}
