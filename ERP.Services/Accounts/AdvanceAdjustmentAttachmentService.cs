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
    public class AdvanceAdjustmentAttachmentService : Repository<Advanceadjustmentattachment>, IAdvanceAdjustmentAttachment
    {
        private readonly IAttachment _attachment;
        public AdvanceAdjustmentAttachmentService(ErpDbContext dbContext, IAttachment attachment) : base(dbContext)
        {
            this._attachment = attachment;
        }
        public async Task<int> CreateAttachment(AdvanceAdjustmentAttachmentModel attachmentModel)
        {
            int associationId = 0;

            // assign values.
            Advanceadjustmentattachment advanceAdjustmentAttachment = new Advanceadjustmentattachment();

            advanceAdjustmentAttachment.AdvanceAdjustmentId = attachmentModel.AdvanceAdjustmentId;
            advanceAdjustmentAttachment.AttachmentId = attachmentModel.AttachmentId;

            await Create(advanceAdjustmentAttachment);

            associationId = advanceAdjustmentAttachment.AssociationId;

            return associationId; // returns.
        }

        public async Task<bool> UpdateAttachment(AdvanceAdjustmentAttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Advanceadjustmentattachment advanceAdjustmentAttachment = await GetByIdAsync(w => w.AssociationId == attachmentModel.AssociationId);

            if (null != advanceAdjustmentAttachment)
            {
                // assign values.
                advanceAdjustmentAttachment.AttachmentId = attachmentModel.AttachmentId;

                isUpdated = await Update(advanceAdjustmentAttachment);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAttachment(int associationId)
        {
            bool isDeleted = false;

            // get record.
            Advanceadjustmentattachment advanceAdjustmentAttachment = await GetByIdAsync(w => w.AssociationId == associationId);

            if (null != advanceAdjustmentAttachment)
            {
                isDeleted = await Delete(advanceAdjustmentAttachment);
            }

            return isDeleted; // returns.
        }

        public async Task<AdvanceAdjustmentAttachmentModel> GetAttachmentById(int associationId)
        {
            AdvanceAdjustmentAttachmentModel attachmentModel = null;

            IList<AdvanceAdjustmentAttachmentModel> attachmentModelList = await GetAttachmentList(associationId, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        public async Task<DataTableResultModel<AdvanceAdjustmentAttachmentModel>> GetAttachmentByAdvanceAdjustmentId(int advanceAdjustmentId)
        {
            DataTableResultModel<AdvanceAdjustmentAttachmentModel> resultModel = new DataTableResultModel<AdvanceAdjustmentAttachmentModel>();

            IList<AdvanceAdjustmentAttachmentModel> attachmentModelList = await GetAttachmentList(0, advanceAdjustmentId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<AdvanceAdjustmentAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<AdvanceAdjustmentAttachmentModel>();
                resultModel.ResultList = new List<AdvanceAdjustmentAttachmentModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<AdvanceAdjustmentAttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<AdvanceAdjustmentAttachmentModel> resultModel = new DataTableResultModel<AdvanceAdjustmentAttachmentModel>();

            IList<AdvanceAdjustmentAttachmentModel> attachmentModelList = await GetAttachmentList(0, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<AdvanceAdjustmentAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<AdvanceAdjustmentAttachmentModel>> GetAttachmentList(int associationId, int advanceAdjustmentId)
        {
            IList<AdvanceAdjustmentAttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Advanceadjustmentattachment> query = GetQueryByCondition(w => w.AssociationId != 0)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.Category)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.StorageAccount);

            // apply filters.
            if (0 != associationId)
                query = query.Where(w => w.AssociationId == associationId);

            // apply filters.
            if (0 != advanceAdjustmentId)
                query = query.Where(w => w.AdvanceAdjustmentId == advanceAdjustmentId);

            // get records by query.
            List<Advanceadjustmentattachment> advanceAdjustmentAttachmentList = await query.ToListAsync();

            if (null != advanceAdjustmentAttachmentList && advanceAdjustmentAttachmentList.Count > 0)
            {
                attachmentModelList = new List<AdvanceAdjustmentAttachmentModel>();

                foreach (Advanceadjustmentattachment advanceAdjustmentAttachment in advanceAdjustmentAttachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(advanceAdjustmentAttachment));
                }

                return attachmentModelList.OrderBy(o => o.CategoryName).ThenBy(o => o.Description).ToList();
            }

            return attachmentModelList; // returns.
        }

        private async Task<AdvanceAdjustmentAttachmentModel> AssignValueToModel(Advanceadjustmentattachment advanceAdjustmentAttachment)
        {
            //return await Task.Run(() =>
            //{
            AdvanceAdjustmentAttachmentModel attachmentModel = new AdvanceAdjustmentAttachmentModel();

            attachmentModel.AssociationId = advanceAdjustmentAttachment.AssociationId;
            attachmentModel.AdvanceAdjustmentId = advanceAdjustmentAttachment.AdvanceAdjustmentId;
            attachmentModel.AttachmentId = advanceAdjustmentAttachment.AttachmentId;

            if (advanceAdjustmentAttachment.Attachment!=null)
            {
                attachmentModel.CategoryId = advanceAdjustmentAttachment.Attachment.CategoryId;
                attachmentModel.Description = advanceAdjustmentAttachment.Attachment.Description;
                attachmentModel.ContainerName = advanceAdjustmentAttachment.Attachment.ContainerName;
                attachmentModel.ServerFileName = advanceAdjustmentAttachment.Attachment.ServerFileName;
                attachmentModel.UserFileName = advanceAdjustmentAttachment.Attachment.UserFileName;
                attachmentModel.FileExtension = advanceAdjustmentAttachment.Attachment.FileExtension;
                attachmentModel.ContentType = advanceAdjustmentAttachment.Attachment.ContentType;
                attachmentModel.ContentLength = advanceAdjustmentAttachment.Attachment.ContentLength;
                attachmentModel.StorageAccountId = advanceAdjustmentAttachment.Attachment.StorageAccountId;
                attachmentModel.AccountName = advanceAdjustmentAttachment.Attachment.StorageAccount.AccountName;
                attachmentModel.AccountKey = advanceAdjustmentAttachment.Attachment.StorageAccount.AccountKey;

                if (advanceAdjustmentAttachment.Attachment.Category != null)
                {
                    attachmentModel.CategoryName = advanceAdjustmentAttachment.Attachment.Category.CategoryName;
                }
                else
                {
                    attachmentModel.CategoryName = "";
                }

                if (advanceAdjustmentAttachment.Attachment.StorageAccount!=null)
                {
                    attachmentModel.AccountName = advanceAdjustmentAttachment.Attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = advanceAdjustmentAttachment.Attachment.StorageAccount.AccountKey;
                    attachmentModel.StorageType = advanceAdjustmentAttachment.Attachment.StorageAccount.StorageType;
                }
                else
                {
                    attachmentModel.AccountName = "";
                    attachmentModel.AccountKey = "";
                }

                attachmentModel.Url = await _attachment.GetUrl(advanceAdjustmentAttachment.AttachmentId);

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

        public async Task<AttachmentModel> SaveAdvanceAdjustmentAttachment(AttachmentModel attachmentModel)
        {
            attachmentModel = await _attachment.SaveAttachment(attachmentModel);

            return attachmentModel;
        }
    }
}
