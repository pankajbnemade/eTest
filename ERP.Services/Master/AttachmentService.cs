using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Models.Utility;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;


namespace ERP.Services.Master
{
    public class AttachmentService : Repository<Attachment>, IAttachment
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAttachmentStorageAccount _attachmentStorageAccount;

        public AttachmentService(ErpDbContext dbContext, IAttachmentStorageAccount attachmentStorageAccount, IWebHostEnvironment webHostEnvironment) : base(dbContext)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._attachmentStorageAccount = attachmentStorageAccount;
        }

        public async Task<AttachmentModel> SaveAttachment(AttachmentModel attachmentModel)
        {
            int attachmentId = 0;

            try
            {
                if (attachmentModel.FileUpload != null)
                {
                    attachmentModel.AttachmentId = 0;

                    AttachmentStorageAccountModel storageAccountModel;

                    storageAccountModel = await _attachmentStorageAccount.GetStorageAccountById(StaticData.StorageAccountId);

                    if (storageAccountModel == null)
                    {
                        
                        attachmentModel.ErrorMessage = "Storage account details not available.";
                        return attachmentModel;
                    }

                    attachmentModel.ContainerName = storageAccountModel.ContainerName;
                    attachmentModel.StorageAccountId = storageAccountModel.StorageAccountId;
                    attachmentModel.StorageType = storageAccountModel.StorageType;
                    attachmentModel.AccountName = storageAccountModel.AccountName;
                    attachmentModel.AccountKey = storageAccountModel.AccountKey;

                    IQueryable<Attachment> query = GetQueryByCondition(w => w.AttachmentId != 0)
                                            .Where(w => w.ServerFileName == attachmentModel.ServerFileName);

                    List<Attachment> attachmentList = await query.ToListAsync();

                    if (null != attachmentList && attachmentList.Any())
                    {
                        attachmentModel.ErrorMessage = "Attachment with same name exists, please try again";
                        return attachmentModel;
                    }

                    if (attachmentModel.StorageType.ToLower() == EnumHelper.GetDescription(StorageType.File).ToLower())
                    {
                        if (await UploadToFileStorage(attachmentModel) == false)
                        {
                            attachmentModel.ErrorMessage = "Attachment not uploaded to file storage, please try again";
                            return attachmentModel;
                        }
                    }
                    else if (attachmentModel.StorageType.ToLower() == EnumHelper.GetDescription(StorageType.Azure).ToLower())
                    {
                        if (await UploadToAzureStorage(attachmentModel) == false)
                        {
                            attachmentModel.ErrorMessage = "Attachment not uploaded to azure storage, please try again";
                            return attachmentModel;
                        }
                    }
                    else
                    {
                        attachmentModel.ErrorMessage = "Storage type not provided, please try again";
                        return attachmentModel;
                    }
                }

                if (attachmentModel.AttachmentId > 0)
                {
                    // update record.
                    if (false == await UpdateAttachment(attachmentModel))
                    {
                        attachmentModel.ErrorMessage = "Attachement details not updated, please try again";
                    }
                }
                else
                {
                    // add new record.
                    attachmentId = await CreateAttachment(attachmentModel);
                    attachmentModel.AttachmentId = attachmentId;

                    if (attachmentModel.AttachmentId == 0)
                    {
                        attachmentModel.ErrorMessage = "Attachment details not created. Please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return attachmentModel;
        }

        private async Task<bool> UploadToFileStorage(AttachmentModel attachmentModel)
        {
            //return await Task.Run(() =>
            //{
            var fileName = attachmentModel.ServerFileName + attachmentModel.FileExtension;

            var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, attachmentModel.ContainerName);

            var filePath = Path.Combine(uploadFolder, fileName);

            FileStream fileStream = new FileStream(filePath, FileMode.Create);

            await attachmentModel.FileUpload.CopyToAsync(fileStream);

            fileStream.Close();

            return true;
            //});
        }
        private async Task<bool> UploadToAzureStorage(AttachmentModel attachmentModel)
        {
            //try
            //{
                var fileName = attachmentModel.ServerFileName + attachmentModel.FileExtension;

                //var uploads = Path.Combine(_webHostEnvironment.WebRootPath, attachmentModel.ContainerName);

                //var filePath = Path.Combine(uploads, uniqueFileName);

                BlobServiceClient blobServiceClient = new BlobServiceClient(attachmentModel.AccountKey);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(attachmentModel.ContainerName);

                BlobClient blobClient = containerClient.GetBlobClient(fileName);

                using var fileStream = attachmentModel.FileUpload.OpenReadStream();

                await blobClient.UploadAsync(fileStream, true);

                fileStream.Close();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            return true;
        }

        public async Task<int> CreateAttachment(AttachmentModel attachmentModel)
        {
            int attachmentId = 0;

            // assign values.
            Attachment attachment = new Attachment();

            attachment.CategoryId = attachmentModel.CategoryId;
            attachment.Description = attachmentModel.Description;
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
                attachment.CategoryId = attachmentModel.CategoryId;
                attachment.Description = attachmentModel.Description;
                //attachment.ContainerName = attachmentModel.ContainerName;
                //attachment.ServerFileName = attachmentModel.ServerFileName;
                //attachment.UserFileName = attachmentModel.UserFileName;
                //attachment.FileExtension = attachmentModel.FileExtension;
                //attachment.ContentType = attachmentModel.ContentType;
                //attachment.ContentLength = attachmentModel.ContentLength;
                //attachment.StorageAccountId = attachmentModel.StorageAccountId;

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
            IQueryable<Attachment> query = GetQueryByCondition(w => w.AttachmentId != 0).Include(w => w.PreparedByUser).Include(w => w.StorageAccount);

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
                attachmentModel.ContainerName = attachment.ContainerName;
                attachmentModel.ServerFileName = attachment.ServerFileName;
                attachmentModel.UserFileName = attachment.UserFileName;
                attachmentModel.FileExtension = attachment.FileExtension;
                attachmentModel.ContentType = attachment.ContentType;
                attachmentModel.ContentLength = attachment.ContentLength;
                attachmentModel.StorageAccountId = attachment.StorageAccountId;

                if (null != attachment.StorageAccount)
                {
                    attachmentModel.AccountName = attachment.StorageAccount.AccountName;
                    attachmentModel.AccountKey = attachment.StorageAccount.AccountKey;
                    attachmentModel.StorageType = attachment.StorageAccount.StorageType;
                }

                attachmentModel.PreparedByName = null != attachment.PreparedByUser ? attachment.PreparedByUser.UserName : null;

                return attachmentModel;
            });
        }

    }
}
