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
    public class VoucherStyleService : Repository<Voucherstyle>, IVoucherStyle
    {
        public VoucherStyleService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateVoucherStyle(VoucherStyleModel voucherStyleModel)
        {
            int voucherStyleId = 0;

            // assign values.
            Voucherstyle voucherStyle = new Voucherstyle();
            voucherStyle.VoucherStyleName = voucherStyleModel.VoucherStyleName;
            await Create(voucherStyle);
            voucherStyleId = voucherStyle.VoucherStyleId;

            return voucherStyleId; // returns.
        }

        public async Task<bool> UpdateVoucherStyle(VoucherStyleModel voucherStyleModel)
        {
            bool isUpdated = false;

            // get record.
            Voucherstyle voucherStyle = await GetByIdAsync(w => w.VoucherStyleId == voucherStyleModel.VoucherStyleId);
            if (null != voucherStyle)
            {
                // assign values.
                voucherStyle.VoucherStyleName = voucherStyleModel.VoucherStyleName;

                isUpdated = await Update(voucherStyle);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteVoucherStyle(int voucherStyleId)
        {
            bool isDeleted = false;

            // get record.
            Voucherstyle voucherStyle = await GetByIdAsync(w => w.VoucherStyleId == voucherStyleId);
            if (null != voucherStyle)
            {
                isDeleted = await Delete(voucherStyle);
            }

            return isDeleted; // returns.
        }

        public async Task<VoucherStyleModel> GetVoucherStyleById(int voucherStyleId)
        {
            VoucherStyleModel voucherStyleModel = null;

            IList<VoucherStyleModel> voucherStyleModelList = await GetVoucherStyleList(voucherStyleId);
            if (null != voucherStyleModelList && voucherStyleModelList.Any())
            {
                voucherStyleModel = voucherStyleModelList.FirstOrDefault();
            }

            return voucherStyleModel; // returns.
        }

        public async Task<DataTableResultModel<VoucherStyleModel>> GetVoucherStyleList()
        {
            DataTableResultModel<VoucherStyleModel> resultModel = new DataTableResultModel<VoucherStyleModel>();

            IList<VoucherStyleModel> voucherStyleModelList = await GetVoucherStyleList(0);
            if (null != voucherStyleModelList && voucherStyleModelList.Any())
            {
                resultModel = new DataTableResultModel<VoucherStyleModel>();
                resultModel.ResultList = voucherStyleModelList;
                resultModel.TotalResultCount = voucherStyleModelList.Count();
            }

            return resultModel; // returns.
        }

        /// <summary>
        /// get all voucherStyle list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<VoucherStyleModel>> GetVoucherStyleList(int voucherStyleId)
        {
            IList<VoucherStyleModel> voucherStyleModelList = null;

            // get records by query.

            IQueryable<Voucherstyle> query = GetQueryByCondition(w => w.VoucherStyleId != 0);
            if (0 != voucherStyleId)
                query = query.Where(w => w.VoucherStyleId == voucherStyleId);

            IList<Voucherstyle> voucherStyleList = await query.ToListAsync();

            if (null != voucherStyleList && voucherStyleList.Count > 0)
            {
                voucherStyleModelList = new List<VoucherStyleModel>();
                foreach (Voucherstyle voucherStyle in voucherStyleList)
                {
                    voucherStyleModelList.Add(await AssignValueToModel(voucherStyle));
                }
            }

            return voucherStyleModelList; // returns.
        }

        private async Task<VoucherStyleModel> AssignValueToModel(Voucherstyle voucherStyle)
        {
            return await Task.Run(() =>
            {
                VoucherStyleModel voucherStyleModel = new VoucherStyleModel();
                voucherStyleModel.VoucherStyleId = voucherStyle.VoucherStyleId;
                voucherStyleModel.VoucherStyleName = voucherStyle.VoucherStyleName;

                if (null != voucherStyle.PreparedByUser)
                {
                    voucherStyleModel.PreparedByName = voucherStyle.PreparedByUser.UserName;
                }

                return voucherStyleModel;
            });
        }

        public async Task<IList<SelectListModel>> GetVoucherStyleSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.VoucherStyleId != 0))
            {
                IQueryable<Voucherstyle> query = GetQueryByCondition(w => w.VoucherStyleId != 0);
                resultModel = await query
                                    .Select(s => new SelectListModel
                                    {
                                        DisplayText = s.VoucherStyleName,
                                        Value = s.VoucherStyleId.ToString()
                                    })
                                    .ToListAsync();
            }

            return resultModel; // returns.
        }
    }
}
