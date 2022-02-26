using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Utility;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class SalesInvoiceAttachmentService : Repository<Salesinvoiceattachment>, ISalesInvoiceAttachment
    {
        public SalesInvoiceAttachmentService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateAttachment(SalesInvoiceAttachmentModel attachmentModel)
        {
            int associationId = 0;

            // assign values.
            Salesinvoiceattachment salesInvoiceAttachment = new Salesinvoiceattachment();

            salesInvoiceAttachment.SalesInvoiceId = attachmentModel.SalesInvoiceId;
            salesInvoiceAttachment.AttachmentId = attachmentModel.AttachmentId;

            await Create(salesInvoiceAttachment);

            associationId = salesInvoiceAttachment.AssociationId;

            return associationId; // returns.
        }

        public async Task<bool> UpdateAttachment(SalesInvoiceAttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoiceattachment salesInvoiceAttachment = await GetByIdAsync(w => w.AssociationId == attachmentModel.AssociationId);

            if (null != salesInvoiceAttachment)
            {
                // assign values.
                salesInvoiceAttachment.AttachmentId = attachmentModel.AttachmentId;

                isUpdated = await Update(salesInvoiceAttachment);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAttachment(int associationId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoiceattachment salesInvoiceAttachment = await GetByIdAsync(w => w.AssociationId == associationId);

            if (null != salesInvoiceAttachment)
            {
                isDeleted = await Delete(salesInvoiceAttachment);
            }

            return isDeleted; // returns.
        }

        public async Task<SalesInvoiceAttachmentModel> GetAttachmentById(int associationId)
        {
            SalesInvoiceAttachmentModel attachmentModel = null;

            IList<SalesInvoiceAttachmentModel> attachmentModelList = await GetAttachmentList(associationId, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        public async Task<DataTableResultModel<SalesInvoiceAttachmentModel>> GetAttachmentBySalesInvoiceId(int salesInvoiceId)
        {
            DataTableResultModel<SalesInvoiceAttachmentModel> resultModel = new DataTableResultModel<SalesInvoiceAttachmentModel>();

            IList<SalesInvoiceAttachmentModel> attachmentModelList = await GetAttachmentList(0, salesInvoiceId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<SalesInvoiceAttachmentModel>();
                resultModel.ResultList = new List<SalesInvoiceAttachmentModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<SalesInvoiceAttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<SalesInvoiceAttachmentModel> resultModel = new DataTableResultModel<SalesInvoiceAttachmentModel>();

            IList<SalesInvoiceAttachmentModel> attachmentModelList = await GetAttachmentList(0, 0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceAttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<SalesInvoiceAttachmentModel>> GetAttachmentList(int associationId, int salesInvoiceId)
        {
            IList<SalesInvoiceAttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Salesinvoiceattachment> query = GetQueryByCondition(w => w.AssociationId != 0)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.Category)
                                                       .Include(i => i.Attachment)
                                                       .ThenInclude(i => i.StorageAccount);

            // apply filters.
            if (0 != associationId)
                query = query.Where(w => w.AssociationId == associationId);

            // apply filters.
            if (0 != salesInvoiceId)
                query = query.Where(w => w.SalesInvoiceId == salesInvoiceId);

            // get records by query.
            List<Salesinvoiceattachment> salesInvoiceAttachmentList = await query.ToListAsync();

            if (null != salesInvoiceAttachmentList && salesInvoiceAttachmentList.Count > 0)
            {
                attachmentModelList = new List<SalesInvoiceAttachmentModel>();

                foreach (Salesinvoiceattachment salesInvoiceAttachment in salesInvoiceAttachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(salesInvoiceAttachment));
                }

                return attachmentModelList.OrderBy(o => o.CategoryName).ThenBy(o => o.Description).ToList();
            }

            return attachmentModelList; // returns.
        }

        private async Task<SalesInvoiceAttachmentModel> AssignValueToModel(Salesinvoiceattachment salesInvoiceAttachment)
        {
            return await Task.Run(() =>
            {
                SalesInvoiceAttachmentModel attachmentModel = new SalesInvoiceAttachmentModel();

                attachmentModel.AssociationId = salesInvoiceAttachment.AssociationId;
                attachmentModel.SalesInvoiceId = salesInvoiceAttachment.SalesInvoiceId;
                attachmentModel.AttachmentId = salesInvoiceAttachment.AttachmentId;

                if (salesInvoiceAttachment.Attachment!=null)
                {
                    attachmentModel.CategoryId = salesInvoiceAttachment.Attachment.CategoryId;
                    attachmentModel.Description = salesInvoiceAttachment.Attachment.Description;
                    attachmentModel.ContainerName = salesInvoiceAttachment.Attachment.ContainerName;
                    attachmentModel.ServerFileName = salesInvoiceAttachment.Attachment.ServerFileName;
                    attachmentModel.UserFileName = salesInvoiceAttachment.Attachment.UserFileName;
                    attachmentModel.FileExtension = salesInvoiceAttachment.Attachment.FileExtension;
                    attachmentModel.ContentType = salesInvoiceAttachment.Attachment.ContentType;
                    attachmentModel.ContentLength = salesInvoiceAttachment.Attachment.ContentLength;

                    attachmentModel.StorageAccountId = salesInvoiceAttachment.Attachment.StorageAccountId;
                    attachmentModel.AccountName = salesInvoiceAttachment.Attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = salesInvoiceAttachment.Attachment.StorageAccount.AccountKey;

                    if (salesInvoiceAttachment.Attachment.Category != null)
                    {
                        attachmentModel.CategoryName = salesInvoiceAttachment.Attachment.Category.CategoryName;
                    }
                    else
                    {
                        attachmentModel.CategoryName = "";
                    }

                    if (salesInvoiceAttachment.Attachment.StorageAccount!=null)
                    {
                        attachmentModel.AccountName = salesInvoiceAttachment.Attachment.StorageAccount.AccountName;
                        attachmentModel.AccountKey = salesInvoiceAttachment.Attachment.StorageAccount.AccountKey;
                        attachmentModel.StorageType = salesInvoiceAttachment.Attachment.StorageAccount.StorageType;
                    }
                    else
                    {
                        attachmentModel.AccountName = "";
                        attachmentModel.AccountKey = "";
                    }

                    if (attachmentModel.StorageType.ToLower()==EnumHelper.GetDescription(StorageType.File).ToLower())
                    {
                        attachmentModel.Url = Path.Combine(attachmentModel.ContainerName, attachmentModel.ServerFileName + attachmentModel.FileExtension);
                    }
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
            });
        }

    }
}
