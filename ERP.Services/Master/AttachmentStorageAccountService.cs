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
    public class AttachmentStorageAccountService : Repository<Attachmentstorageaccount>, IAttachmentStorageAccount
    {
        public AttachmentStorageAccountService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateStorageAccount(AttachmentStorageAccountModel storageAccountModel)
        {
            int storageAccountId = 0;

            // assign values.
            Attachmentstorageaccount storageAccount = new Attachmentstorageaccount();

            storageAccount.AccountName = storageAccountModel.AccountName;
            await Create(storageAccount);
            storageAccountId = storageAccount.StorageAccountId;

            return storageAccountId; // returns.
        }

        /// <summary>
        /// update storageAccount.
        /// </summary>
        /// <param name="storageAccountModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateStorageAccount(AttachmentStorageAccountModel storageAccountModel)
        {
            bool isUpdated = false;

            // get record.
            Attachmentstorageaccount storageAccount = await GetByIdAsync(w => w.StorageAccountId == storageAccountModel.StorageAccountId);
            if (null != storageAccount)
            {
                // assign values.
                storageAccount.AccountName = storageAccountModel.AccountName;
                isUpdated = await Update(storageAccount);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// update storageAccount.
        /// </summary>
        /// <param name="storageAccountModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteStorageAccount(int storageAccountId)
        {
            bool isDeleted = false;

            // get record.
            Attachmentstorageaccount storageAccount = await GetByIdAsync(w => w.StorageAccountId == storageAccountId);
            if (null != storageAccount)
            {
                isDeleted = await Delete(storageAccount);
            }

            return isDeleted; // returns.
        }

        /// <summary>
        /// get storageAccount based on storageAccountId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<AttachmentStorageAccountModel> GetStorageAccountById(int storageAccountId)
        {
            AttachmentStorageAccountModel storageAccountModel = null;

            IList<AttachmentStorageAccountModel> storageAccountModelList = await GetStorageAccountList(storageAccountId);

            if (null != storageAccountModelList && storageAccountModelList.Any())
            {
                storageAccountModel = storageAccountModelList.FirstOrDefault();
            }

            return storageAccountModel; // returns.
        }

        /// <summary>
        /// get all storageAccount list
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<AttachmentStorageAccountModel>> GetStorageAccountList()
        {
            DataTableResultModel<AttachmentStorageAccountModel> resultModel = new DataTableResultModel<AttachmentStorageAccountModel>();

            IList<AttachmentStorageAccountModel> storageAccountModelList = await GetStorageAccountList(0);

            if (null != storageAccountModelList && storageAccountModelList.Any())
            {
                resultModel = new DataTableResultModel<AttachmentStorageAccountModel>();
                resultModel.ResultList = storageAccountModelList;
                resultModel.TotalResultCount = storageAccountModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<AttachmentStorageAccountModel>> GetStorageAccountList(int storageAccountId)
        {
            IList<AttachmentStorageAccountModel> storageAccountModelList = null;

            // create query.
            IQueryable<Attachmentstorageaccount> query = GetQueryByCondition(w => w.StorageAccountId != 0).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != storageAccountId)
                query = query.Where(w => w.StorageAccountId == storageAccountId);

            // get records by query.
            List<Attachmentstorageaccount> storageAccountList = await query.ToListAsync();

            if (null != storageAccountList && storageAccountList.Count > 0)
            {
                storageAccountModelList = new List<AttachmentStorageAccountModel>();
                foreach (Attachmentstorageaccount storageAccount in storageAccountList)
                {
                    storageAccountModelList.Add(await AssignValueToModel(storageAccount));
                }
            }

            return storageAccountModelList; // returns.
        }

        private async Task<AttachmentStorageAccountModel> AssignValueToModel(Attachmentstorageaccount storageAccount)
        {
            return await Task.Run(() =>
            {
                // assign values.
                AttachmentStorageAccountModel storageAccountModel = new AttachmentStorageAccountModel();

                storageAccountModel.StorageAccountId = storageAccount.StorageAccountId;
                storageAccountModel.StorageType = storageAccount.StorageType;
                storageAccountModel.AccountName = storageAccount.AccountName;
                storageAccountModel.ContainerName = storageAccount.ContainerName;
                storageAccountModel.AllowedFileExtension = storageAccount.AllowedFileExtension;
                storageAccountModel.AllowedContentLength = storageAccount.AllowedContentLength;
                storageAccountModel.PreparedByName = null != storageAccount.PreparedByUser ? storageAccount.PreparedByUser.UserName : null;

                return storageAccountModel;
            });
        }

        public async Task<IList<SelectListModel>> GetStorageAccountSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.StorageAccountId != 0))
            {
                IQueryable<Attachmentstorageaccount> query = GetQueryByCondition(w => w.StorageAccountId != 0);

                resultModel = await query.Select(s => new SelectListModel
                {
                    DisplayText = s.AccountName,
                    Value = s.StorageAccountId.ToString()
                }).OrderBy(w => w.DisplayText).ToListAsync();
            }

            return resultModel; // returns.
        }

    }
}
