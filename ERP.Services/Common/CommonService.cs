using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Common.Interface;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Common
{
    public class CommonService : ICommon
    {
        IVoucherSetupDetail _voucherSetupDetail;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommonService(IVoucherSetupDetail voucherSetupDetail, IHttpContextAccessor httpContextAccessor)
        {
            _voucherSetupDetail = voucherSetupDetail;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GenerateNoModel> GenerateVoucherNo(int maxNo, int voucherSetupId, int companyId, int financialYearId)
        {
            VoucherSetupDetailModel voucherSetupDetailModel = null;

            voucherSetupDetailModel = await _voucherSetupDetail.GetVoucherSetupDetailForNoGeneration(voucherSetupId, companyId, financialYearId);

            return await Task.Run(() =>
            {
                string voucherNo = string.Empty;
                int voucherStyleId = (int)voucherSetupDetailModel.VoucherStyleId;
                maxNo = maxNo + 1;

                voucherNo = $"{voucherSetupDetailModel.NoPreString}{voucherSetupDetailModel.NoSeparator}{Convert.ToString(maxNo).PadLeft(5, char.Parse(voucherSetupDetailModel.NoPad))}{voucherSetupDetailModel.NoPostString}";

                return new GenerateNoModel { MaxNo = maxNo, VoucherStyleId = voucherStyleId, VoucherNo = voucherNo };
            });
        }


        //public async Task<Boolean> CheckSession()
        //{
        //    Boolean isSession = true;

        //    UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(_httpContextAccessor.HttpContext.Session, "UserSession");

        //    return await Task.Run(() =>
        //    {
        //        if (userSession == null)
        //        {
        //            isSession = false;
        //        }

        //        if (isSession == true)
        //        {
        //            if (userSession.UserId == 0 || userSession.CompanyId == 0 || userSession.FinancialYearId == 0)
        //            {
        //                isSession = false;
        //            }
        //        }

        //        return isSession;
        //    });

        //}


        #region "Amount To Word Million"

        public async Task<string> AmountInWord_Million(string amount, string currencyCode, string denomination)
        {
            return await Task.Run(() =>
            {
                string isNegative = "";
                string amountInWord = "";

                amount = Convert.ToDouble(amount).ToString();

                if (amount.Contains("-"))
                {
                    isNegative = "Minus ";
                    amount = amount.Substring(1, amount.Length - 1);
                }

                if (amount == "0")
                {
                    amountInWord = "Zero Only";
                }
                else
                {
                    amountInWord = currencyCode + " " + isNegative + ConvertToWords(amount, denomination);
                }

                return amountInWord;
            });
        }
        private static String ConvertToWords(String numb, string denomination)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents  
                        endStr = denomination + " " + endStr;//Cents  
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }
        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://ones' range

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros
                        //if (beginsZero) word = " and " + word.Trim();
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }

        #endregion
    }
}
