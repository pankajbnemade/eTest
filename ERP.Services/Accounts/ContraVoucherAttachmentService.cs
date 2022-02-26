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
    public class ContraVoucherAttachmentService : Repository<Contravoucherattachment>, IContraVoucherAttachment
    {
        private readonly IAttachment _attachment;
        public ContraVoucherAttachmentService(ErpDbContext dbContext, IAttachment attachment) : base(dbContext)
        {
            this._attachment = attachment;
        }
        public async Task<int> CreateAttachment(ContraVoucherAttachmentModel attachmentModel)
        {
            int associationId = 0;

            // assign values.
            Contravoucherattachment contraVoucherAttachment = new Contravoucherattachment();

            contraVoucherAttachment.ContraVoucherId = attachmentModel.ContraVoucherId;
            contraVoucherAttachment.AttachmentId = attachmentModel.AttachmentId;

            await Create(contraVoucherAttachment);

            associationId = contraVoucherAttachment.AssociationId;

            return associationId; // returns.
        }

        public async Task<bool> UpdateAttachment(ContraVoucherAttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Contravoucherattachment contraVoucherAttachment = await GetByIdAsync(w => w.AssociationId == attachmentModel.AssociationId);

            if (null != contraVoucherAttachment)
            {
                // assign values.
                contraVoucherAttachment.AttachmentId = attachmentModel.AttachmentId;

                isUpdated = await Update(contraVoucherAttachment);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAttachment(int associationId)
        {
            bool isDeleted = false;

            // get record.
            Contravoucherattachment contraVoucherAttachment = await GetByIdAsync(w => w.AssociationId == associationId);

            if (null != contraVoucherAttachment)
            {
                isDeleted = await Delete(contraVoucherAttachment);
            }

            return isDeleted; // returns.
        }

        public async Task<ContraVoucherAttachmentModel> GetAttachmentById(int associationId)
        {
            ContraVoucherAttachmentModel attachmentModel = null;

            IList<ContraVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(associationId, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        public async Task<DataTableResultModel<ContraVoucherAttachmentModel>> GetAttachmentByContraVoucherId(int contraVoucherId)
        {
            DataTableResultModel<ContraVoucherAttachmentModel> resultModel = new DataTableResultModel<ContraVoucherAttachmentModel>();

            IList<ContraVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(0, contraVoucherId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<ContraVoucherAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<ContraVoucherAttachmentModel>();
                resultModel.ResultList = new List<ContraVoucherAttachmentModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<ContraVoucherAttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<ContraVoucherAttachmentModel> resultModel = new DataTableResultModel<ContraVoucherAttachmentModel>();

            IList<ContraVoucherAttachmentModel> attachmentModelList = await GetAttachmentList(0, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<ContraVoucherAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<ContraVoucherAttachmentModel>> GetAttachmentList(int associationId, int contraVoucherId)
        {
            IList<ContraVoucherAttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Contravoucherattachment> query = GetQueryByCondition(w => w.AssociationId != 0)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.Category)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.StorageAccount);

            // apply filters.
            if (0 != associationId)
                query = query.Where(w => w.AssociationId == associationId);

            // apply filters.
            if (0 != contraVoucherId)
                query = query.Where(w => w.ContraVoucherId == contraVoucherId);

            // get records by query.
            List<Contravoucherattachment> contraVoucherAttachmentList = await query.ToListAsync();

            if (null != contraVoucherAttachmentList && contraVoucherAttachmentList.Count > 0)
            {
                attachmentModelList = new List<ContraVoucherAttachmentModel>();

                foreach (Contravoucherattachment contraVoucherAttachment in contraVoucherAttachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(contraVoucherAttachment));
                }

                return attachmentModelList.OrderBy(o => o.CategoryName).ThenBy(o => o.Description).ToList();
            }

            return attachmentModelList; // returns.
        }

        private async Task<ContraVoucherAttachmentModel> AssignValueToModel(Contravoucherattachment contraVoucherAttachment)
        {
            //return await Task.Run(() =>
            //{
            ContraVoucherAttachmentModel attachmentModel = new ContraVoucherAttachmentModel();

            attachmentModel.AssociationId = contraVoucherAttachment.AssociationId;
            attachmentModel.ContraVoucherId = contraVoucherAttachment.ContraVoucherId;
            attachmentModel.AttachmentId = contraVoucherAttachment.AttachmentId;

            if (contraVoucherAttachment.Attachment!=null)
            {
                attachmentModel.CategoryId = contraVoucherAttachment.Attachment.CategoryId;
                attachmentModel.Description = contraVoucherAttachment.Attachment.Description;
                attachmentModel.ContainerName = contraVoucherAttachment.Attachment.ContainerName;
                attachmentModel.ServerFileName = contraVoucherAttachment.Attachment.ServerFileName;
                attachmentModel.UserFileName = contraVoucherAttachment.Attachment.UserFileName;
                attachmentModel.FileExtension = contraVoucherAttachment.Attachment.FileExtension;
                attachmentModel.ContentType = contraVoucherAttachment.Attachment.ContentType;
                attachmentModel.ContentLength = contraVoucherAttachment.Attachment.ContentLength;
                attachmentModel.StorageAccountId = contraVoucherAttachment.Attachment.StorageAccountId;
                attachmentModel.AccountName = contraVoucherAttachment.Attachment.StorageAccount.AccountName;
                attachmentModel.AccountKey = contraVoucherAttachment.Attachment.StorageAccount.AccountKey;

                if (contraVoucherAttachment.Attachment.Category != null)
                {
                    attachmentModel.CategoryName = contraVoucherAttachment.Attachment.Category.CategoryName;
                }
                else
                {
                    attachmentModel.CategoryName = "";
                }

                if (contraVoucherAttachment.Attachment.StorageAccount!=null)
                {
                    attachmentModel.AccountName = contraVoucherAttachment.Attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = contraVoucherAttachment.Attachment.StorageAccount.AccountKey;
                    attachmentModel.StorageType = contraVoucherAttachment.Attachment.StorageAccount.StorageType;
                }
                else
                {
                    attachmentModel.AccountName = "";
                    attachmentModel.AccountKey = "";
                }

                attachmentModel.Url = await _attachment.GetUrl(contraVoucherAttachment.AttachmentId);

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

        public async Task<AttachmentModel> SaveContraVoucherAttachment(AttachmentModel attachmentModel)
        {
            attachmentModel = await _attachment.SaveAttachment(attachmentModel);

            return attachmentModel;
        }
    }
}
