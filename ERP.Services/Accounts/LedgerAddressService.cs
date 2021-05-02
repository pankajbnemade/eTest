using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class LedgerAddressService : Repository<Ledgeraddress>, ILedgerAddress
    {
        public LedgerAddressService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateLedgerAddress(LedgerAddressModel ledgerAddressModel)
        {
            int ledgerAddressId = 0;

            // assign values.
            Ledgeraddress ledgerAddress = new Ledgeraddress();
            ledgerAddress.LedgerId = ledgerAddressModel.LedgerId;
            ledgerAddress.AddressDescription = ledgerAddressModel.AddressDescription;
            ledgerAddress.CountryId = ledgerAddressModel.CountryId;
            ledgerAddress.StateId = ledgerAddressModel.StateId;
            ledgerAddress.CityId = ledgerAddressModel.CityId;
            ledgerAddress.EmailAddress = ledgerAddressModel.EmailAddress;
            ledgerAddress.PhoneNo = ledgerAddressModel.PhoneNo;
            ledgerAddress.PostalCode = ledgerAddressModel.PostalCode;
            ledgerAddress.FaxNo = ledgerAddressModel.FaxNo;
            await Create(ledgerAddress);
            ledgerAddressId = ledgerAddress.AddressId;

            return ledgerAddressId; // returns.
        }


        public async Task<bool> UpdateLedgerAddress(LedgerAddressModel ledgerAddressModel)
        {
            bool isUpdated = false;

            // get record.
            Ledgeraddress ledgerAddress = await GetByIdAsync(w => w.AddressId == ledgerAddressModel.AddressId);
            if (null != ledgerAddress)
            {
                // assign values.
                ledgerAddress.AddressDescription = ledgerAddressModel.AddressDescription;
                ledgerAddress.CountryId = ledgerAddressModel.CountryId;
                ledgerAddress.StateId = ledgerAddressModel.StateId;
                ledgerAddress.CityId = ledgerAddressModel.CityId;
                ledgerAddress.EmailAddress = ledgerAddressModel.EmailAddress;
                ledgerAddress.PhoneNo = ledgerAddressModel.PhoneNo;
                ledgerAddress.PostalCode = ledgerAddressModel.PostalCode;
                ledgerAddress.FaxNo = ledgerAddressModel.FaxNo;

                isUpdated = await Update(ledgerAddress);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteLedgerAddress(int ledgerAddressId)
        {
            bool isDeleted = false;

            // get record.
            Ledgeraddress ledgerAddress = await GetByIdAsync(w => w.AddressId == ledgerAddressId);
            if (null != ledgerAddress)
            {
                isDeleted = await Delete(ledgerAddress);
            }

            return isDeleted; // returns.
        }


        public async Task<LedgerAddressModel> GetLedgerAddressById(int ledgerAddressId)
        {
            LedgerAddressModel ledgerAddressModel = null;

            IList<LedgerAddressModel> ledgerAddressModelList = await GetLedgerAddressList(ledgerAddressId);
            if (null != ledgerAddressModelList && ledgerAddressModelList.Any())
            {
                ledgerAddressModel = ledgerAddressModelList.FirstOrDefault();
            }

            return ledgerAddressModel; // returns.
        }


        public async Task<DataTableResultModel<LedgerAddressModel>> GetLedgerAddressList()
        {
            DataTableResultModel<LedgerAddressModel> resultModel = new DataTableResultModel<LedgerAddressModel>();

            IList<LedgerAddressModel> ledgerAddressModelList = await GetLedgerAddressList(0);
            if (null != ledgerAddressModelList && ledgerAddressModelList.Any())
            {
                resultModel = new DataTableResultModel<LedgerAddressModel>();
                resultModel.ResultList = ledgerAddressModelList;
                resultModel.TotalResultCount = ledgerAddressModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<LedgerAddressModel>> GetLedgerAddressList(int ledgerAddressId)
        {
            IList<LedgerAddressModel> ledgerAddressModelList = null;

            // create query.
            IQueryable<Ledgeraddress> query = GetQueryByCondition(w => w.AddressId != 0)
                                                .Include(w => w.Ledger).Include(w => w.Country)
                                                .Include(w => w.State).Include(w => w.City);

            // apply filters.
            if (0 != ledgerAddressId)
                query = query.Where(w => w.AddressId == ledgerAddressId);

            // get records by query.
            List<Ledgeraddress> ledgerAddressList = await query.ToListAsync();
            if (null != ledgerAddressList && ledgerAddressList.Count > 0)
            {
                ledgerAddressModelList = new List<LedgerAddressModel>();

                foreach (Ledgeraddress ledgerAddress in ledgerAddressList)
                {
                    ledgerAddressModelList.Add(await AssignValueToModel(ledgerAddress));
                }
            }

            return ledgerAddressModelList; // returns.
        }

        private async Task<LedgerAddressModel> AssignValueToModel(Ledgeraddress ledgerAddress)
        {
            return await Task.Run(() =>
            {
                LedgerAddressModel ledgerAddressModel = new LedgerAddressModel();
                ledgerAddressModel.AddressId = ledgerAddress.AddressId;
                ledgerAddressModel.LedgerId = ledgerAddress.LedgerId;
                ledgerAddressModel.AddressDescription = ledgerAddress.AddressDescription;
                ledgerAddressModel.CountryId = ledgerAddress.CountryId;
                ledgerAddressModel.StateId = ledgerAddress.StateId;
                ledgerAddressModel.CityId = ledgerAddress.CityId;
                ledgerAddressModel.EmailAddress = ledgerAddress.EmailAddress;
                ledgerAddressModel.PhoneNo = ledgerAddress.PhoneNo;
                ledgerAddressModel.PostalCode = ledgerAddress.PostalCode;
                ledgerAddressModel.FaxNo = ledgerAddress.FaxNo;
                //####
                ledgerAddressModel.LedgerName = ledgerAddress.Ledger.LedgerName;
                ledgerAddressModel.CountryName = ledgerAddress.Country.CountryName;
                ledgerAddressModel.StateIName = ledgerAddress.State.StateName;
                ledgerAddressModel.CityName = ledgerAddress.City.CityName;

                return ledgerAddressModel;
            });
        }

        public async Task<IList<SelectListModel>> GetLedgerAddressSelectList(int ledgerId)
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.AddressId != 0))
            {
                IQueryable<Ledgeraddress> query = GetQueryByCondition(w => w.AddressId != 0);

                // apply filters.
                if (0 != ledgerId)
                    query = query.Where(w => w.LedgerId == ledgerId);

                resultModel = await query.Select(s => new SelectListModel
                {
                    DisplayText = s.AddressDescription,
                    Value = s.AddressId.ToString()
                }).ToListAsync();
            }

            return resultModel; // returns.
        }


    }
}
