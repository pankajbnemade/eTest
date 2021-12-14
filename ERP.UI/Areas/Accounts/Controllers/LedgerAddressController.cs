using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class LedgerAddressController : Controller
    {
        private readonly ILedger _ledger;
        private readonly ILedgerAddress _ledgerAddress;

        private readonly ICountry _country;
        private readonly IState _state;
        private readonly ICity _city;

        /// <summary>
        /// constractor.
        /// </summary>
        public LedgerAddressController(ILedger ledger, ILedgerAddress ledgerAddress, ICountry country, IState state, ICity city)
        {
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
            this._country = country;
            this._state = state;
            this._city = city;
        }

        /// <summary>
        /// ledger address.
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <returns></returns>
        /// 
        public async Task<IActionResult> LedgerAddress(int ledgerId)
        {
            ViewBag.LedgerId = ledgerId;

            return await Task.Run(() =>
            {
                return PartialView("_LedgerAddress");
            });
        }

        /// <summary>
        /// get  ledger addresss list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetLedgerAddressList(int ledgerId)
        {
            DataTableResultModel<LedgerAddressModel> resultModel = await _ledgerAddress.GetLedgerAddressByLedgerId(ledgerId);

            return await Task.Run(() =>
            {
                return Json(new
                {
                    draw = "1",
                    recordsTotal = resultModel.TotalResultCount,
                    data = resultModel.ResultList
                });
            });
        }

        /// <summary>
        /// add ledger address.
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddLedgerAddress(int ledgerId)
        {
            ViewBag.CountryList = await _country.GetCountrySelectList();

            LedgerAddressModel ledgerAddressModel = new LedgerAddressModel();
            ledgerAddressModel.LedgerId = ledgerId;

            return await Task.Run(() =>
            {
                return PartialView("_AddLedgerAddress", ledgerAddressModel);
            });
        }

        /// <summary>
        /// edit ledger address.
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditLedgerAddress(int addressId)
        {
            ViewBag.CountryList = await _country.GetCountrySelectList();

            LedgerAddressModel ledgerAddressModel = await _ledgerAddress.GetLedgerAddressById(addressId);

            return await Task.Run(() =>
            {
                return PartialView("_AddLedgerAddress", ledgerAddressModel);
            });
        }

        /// <summary>
        /// save  ledger address.
        /// </summary>
        /// <param name="ledgerAddressModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveLedgerAddress(LedgerAddressModel ledgerAddressModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (ledgerAddressModel.AddressId > 0)
                {
                    // update record.
                    if (true == await _ledgerAddress.UpdateLedgerAddress(ledgerAddressModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    ledgerAddressModel.AddressId = await _ledgerAddress.CreateLedgerAddress(ledgerAddressModel);

                    // add new record.
                    if (ledgerAddressModel.AddressId > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> GetStateByCountryId(int countryId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            IList<SelectListModel> selectList = await _state.GetStateSelectListByCountryId(countryId);

            if (null != selectList)
            {
                data.Result.Status = true;
                data.Result.Data = selectList;
            }
            else
            {
                data.Result.Message = "NoItems";
            }

            return Json(data); // returns.
        }

        [HttpPost]
        public async Task<JsonResult> GetCityByStateId(int stateId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            IList<SelectListModel> selectList = await _city.GetCitySelectListByStateId(stateId);

            if (null != selectList)
            {
                data.Result.Status = true;
                data.Result.Data = selectList;
            }
            else
            {
                data.Result.Message = "NoItems";
            }

            return Json(data); // returns.
        }

        /// <summary>
        /// delete ledger address.
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteLedgerAddress(int addressId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            LedgerAddressModel ledgerAddressModel = await _ledgerAddress.GetLedgerAddressById(addressId);

            if (true == await _ledgerAddress.DeleteLedgerAddress(addressId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
