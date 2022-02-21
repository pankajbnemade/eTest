using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class AttachmentService : Repository<Attachment>, IAttachment
    {
        public AttachmentService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateAttachment(AttachmentModel attachmentModel)
        {
            int attachmentId = 0;

            // assign values.
            Attachment attachment = new Attachment();

            attachment.CategoryId = attachmentModel.CategoryId;
            attachment.Description = attachmentModel.Description;
            attachment.Guidno = attachmentModel.Guidno;
            attachment.ContainerName = attachmentModel.ContainerName;
            attachment.ServerFileName = attachmentModel.ServerFileName;
            attachment.UserFileName = attachmentModel.UserFileName;
            attachment.FileExtension = attachmentModel.FileExtension;
            attachment.ContentType = attachmentModel.ContentType;
            attachment.ContentLength = attachmentModel.ContentLength;
            attachment.StorageAccountId = attachmentModel.StorageAccountId;

            await Create(attachment);

            attachmentId = attachment.AttachmentId;

            return attachmentId; // returns.
        }

        /// <summary>
        /// update attachment.
        /// </summary>
        /// <param name="attachmentModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateAttachment(AttachmentModel attachmentModel)
        {
            bool isUpdated = false;

            // get record.
            Attachment attachment = await GetByIdAsync(w => w.AttachmentId == attachmentModel.AttachmentId);
            if (null != attachment)
            {
                // assign values.
                attachment.Description = attachmentModel.Description;
                attachment.Guidno = attachmentModel.Guidno;
                attachment.ContainerName = attachmentModel.ContainerName;
                attachment.ServerFileName = attachmentModel.ServerFileName;
                attachment.UserFileName = attachmentModel.UserFileName;
                attachment.FileExtension = attachmentModel.FileExtension;
                attachment.ContentType = attachmentModel.ContentType;
                attachment.ContentLength = attachmentModel.ContentLength;
                attachment.StorageAccountId = attachmentModel.StorageAccountId;

                isUpdated = await Update(attachment);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// update attachment.
        /// </summary>
        /// <param name="attachmentModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteAttachment(int attachmentId)
        {
            bool isDeleted = false;

            // get record.
            Attachment attachment = await GetByIdAsync(w => w.AttachmentId == attachmentId);

            if (null != attachment)
            {
                isDeleted = await Delete(attachment);
            }

            return isDeleted; // returns.
        }

        /// <summary>
        /// get attachment based on attachmentId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<AttachmentModel> GetAttachmentById(int attachmentId)
        {
            AttachmentModel attachmentModel = null;

            IList<AttachmentModel> attachmentModelList = await GetAttachmentList(attachmentId);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                attachmentModel = attachmentModelList.FirstOrDefault();
            }

            return attachmentModel; // returns.
        }

        /// <summary>
        /// get all attachment list
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<AttachmentModel>> GetAttachmentList()
        {
            DataTableResultModel<AttachmentModel> resultModel = new DataTableResultModel<AttachmentModel>();

            IList<AttachmentModel> attachmentModelList = await GetAttachmentList(0);

            if (null != attachmentModelList && attachmentModelList.Any())
            {
                resultModel = new DataTableResultModel<AttachmentModel>();
                resultModel.ResultList = attachmentModelList;
                resultModel.TotalResultCount = attachmentModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<AttachmentModel>> GetAttachmentList(int attachmentId)
        {
            IList<AttachmentModel> attachmentModelList = null;

            // create query.
            IQueryable<Attachment> query = GetQueryByCondition(w => w.AttachmentId != 0).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != attachmentId)
                query = query.Where(w => w.AttachmentId == attachmentId);

            // get records by query.
            List<Attachment> attachmentList = await query.ToListAsync();

            if (null != attachmentList && attachmentList.Count > 0)
            {
                attachmentModelList = new List<AttachmentModel>();

                foreach (Attachment attachment in attachmentList)
                {
                    attachmentModelList.Add(await AssignValueToModel(attachment));
                }
            }

            return attachmentModelList; // returns.
        }

        private async Task<AttachmentModel> AssignValueToModel(Attachment attachment)
        {
            return await Task.Run(() =>
            {
                // assign values.
                AttachmentModel attachmentModel = new AttachmentModel();

                attachmentModel.AttachmentId = attachment.AttachmentId;
                attachmentModel.CategoryId = attachment.CategoryId;
                attachmentModel.Description = attachment.Description;
                attachmentModel.Guidno = attachment.Guidno;
                attachmentModel.ContainerName = attachment.ContainerName;
                attachmentModel.ServerFileName = attachment.ServerFileName;
                attachmentModel.UserFileName = attachment.UserFileName;
                attachmentModel.FileExtension = attachment.FileExtension;
                attachmentModel.ContentType = attachment.ContentType;
                attachmentModel.ContentLength = attachment.ContentLength;
                attachmentModel.StorageAccountId = attachment.StorageAccountId;

                attachmentModel.PreparedByName = null != attachment.PreparedByUser ? attachment.PreparedByUser.UserName : null;

                return attachmentModel;
            });
        }

    }
}
