using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class VoucherSetupDetailService : Repository<Vouchersetupdetail>, IVoucherSetupDetail
    {
        public VoucherSetupDetailService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateVoucherSetupDetail(VoucherSetupDetailModel voucherSetupDetailModel)
        {
            int voucherSetupDetailId = 0;

            // assign values.
            Vouchersetupdetail voucherSetupDetail = new Vouchersetupdetail();

            voucherSetupDetail.VoucherSetupId = voucherSetupDetailModel.VoucherSetupId;
            voucherSetupDetail.NoPad = voucherSetupDetailModel.NoPad;
            voucherSetupDetail.NoPreString = voucherSetupDetailModel.NoPreString;
            voucherSetupDetail.NoPostString = voucherSetupDetailModel.NoPostString;
            voucherSetupDetail.NoSeperator = voucherSetupDetailModel.NoSeperator;
            voucherSetupDetail.FormatText = voucherSetupDetailModel.FormatText;
            voucherSetupDetail.VoucherStyleId = voucherSetupDetailModel.VoucherStyleId;
            voucherSetupDetail.CompanyId = voucherSetupDetailModel.CompanyId;
            voucherSetupDetail.FinancialYearId = voucherSetupDetailModel.FinancialYearId;

            voucherSetupDetailId = await Create(voucherSetupDetail);

            return voucherSetupDetailId; // returns.
        }

        public async Task<bool> UpdateVoucherSetupDetail(VoucherSetupDetailModel voucherSetupDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Vouchersetupdetail voucherSetupDetail = await GetByIdAsync(w => w.VoucherSetupDetId == voucherSetupDetailModel.VoucherSetupDetId);
            if (null != voucherSetupDetail)
            {
                // assign values.
                voucherSetupDetail.NoPad = voucherSetupDetailModel.NoPad;
                voucherSetupDetail.NoPreString = voucherSetupDetailModel.NoPreString;
                voucherSetupDetail.NoPostString = voucherSetupDetailModel.NoPostString;
                voucherSetupDetail.NoSeperator = voucherSetupDetailModel.NoSeperator;
                voucherSetupDetail.FormatText = voucherSetupDetailModel.FormatText;
                voucherSetupDetail.VoucherStyleId = voucherSetupDetailModel.VoucherStyleId;
                voucherSetupDetail.CompanyId = voucherSetupDetailModel.CompanyId;
                voucherSetupDetail.FinancialYearId = voucherSetupDetailModel.FinancialYearId;

                isUpdated = await Update(voucherSetupDetail);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteVoucherSetupDetail(int voucherSetupDetailId)
        {
            bool isDeleted = false;

            // get record.
            Vouchersetupdetail voucherSetupDetail = await GetByIdAsync(w => w.VoucherSetupDetId == voucherSetupDetailId);
            if (null != voucherSetupDetail)
            {
                isDeleted = await Delete(voucherSetupDetail);
            }

            return isDeleted; // returns.
        }


        public async Task<VoucherSetupDetailModel> GetVoucherSetupDetailById(int voucherSetupDetailId)
        {
            VoucherSetupDetailModel voucherSetupDetailModel = null;

            IList<VoucherSetupDetailModel> voucherSetupDetailModelList = await GetVoucherSetupDetailList(voucherSetupDetailId);
            if (null != voucherSetupDetailModelList && voucherSetupDetailModelList.Any())
            {
                voucherSetupDetailModel = voucherSetupDetailModelList.FirstOrDefault();
            }

            return voucherSetupDetailModel; // returns.
        }

        public async Task<DataTableResultModel<VoucherSetupDetailModel>> GetVoucherSetupDetailList()
        {
            DataTableResultModel<VoucherSetupDetailModel> resultModel = new DataTableResultModel<VoucherSetupDetailModel>();

            IList<VoucherSetupDetailModel> voucherSetupDetailModelList = await GetVoucherSetupDetailList(0);
            if (null != voucherSetupDetailModelList && voucherSetupDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<VoucherSetupDetailModel>();
                resultModel.ResultList = voucherSetupDetailModelList;
                resultModel.TotalResultCount = voucherSetupDetailModelList.Count();
            }

            return resultModel; // returns.
        }


        /// <summary>
        /// get all voucherSetupDetail list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<VoucherSetupDetailModel>> GetVoucherSetupDetailList(int voucherSetupDetailId)
        {
            IList<VoucherSetupDetailModel> voucherSetupDetailModelList = null;

            // get records by query.

            IQueryable<Vouchersetupdetail> query = GetQueryByCondition(w => w.VoucherSetupDetId != 0).Include(w => w.PreparedByUser).Include(w => w.VoucherStyle);

            if (0 != voucherSetupDetailId)
                query = query.Where(w => w.VoucherSetupDetId == voucherSetupDetailId);

            IList<Vouchersetupdetail> voucherSetupDetailList = await query.ToListAsync();

            if (null != voucherSetupDetailList && voucherSetupDetailList.Count > 0)
            {
                voucherSetupDetailModelList = new List<VoucherSetupDetailModel>();
                foreach (Vouchersetupdetail voucherSetupDetail in voucherSetupDetailList)
                {
                    voucherSetupDetailModelList.Add(await AssignValueToModel(voucherSetupDetail));
                }
            }

            return voucherSetupDetailModelList; // returns.
        }

        private async Task<VoucherSetupDetailModel> AssignValueToModel(Vouchersetupdetail voucherSetupDetail)
        {
            return await Task.Run(() =>
            {
                VoucherSetupDetailModel voucherSetupDetailModel = new VoucherSetupDetailModel();

                voucherSetupDetailModel.VoucherSetupDetId = voucherSetupDetail.VoucherSetupDetId;
                voucherSetupDetailModel.VoucherSetupId = voucherSetupDetail.VoucherSetupId;
                voucherSetupDetailModel.NoPad = voucherSetupDetail.NoPad;
                voucherSetupDetailModel.NoPreString = voucherSetupDetail.NoPreString;
                voucherSetupDetailModel.NoPostString = voucherSetupDetail.NoPostString;
                voucherSetupDetailModel.NoSeperator = voucherSetupDetail.NoSeperator;
                voucherSetupDetailModel.FormatText = voucherSetupDetail.FormatText;
                voucherSetupDetailModel.VoucherStyleId = voucherSetupDetail.VoucherStyleId;
                voucherSetupDetailModel.CompanyId = voucherSetupDetail.CompanyId;
                voucherSetupDetailModel.FinancialYearId = voucherSetupDetail.FinancialYearId;

                voucherSetupDetailModel.VoucherStyleName = voucherSetupDetail.VoucherStyle.VoucherStyleName;
                voucherSetupDetailModel.PreparedByName = voucherSetupDetail.PreparedByUser.UserName;

                return voucherSetupDetailModel;
            });
        }

    }
}
