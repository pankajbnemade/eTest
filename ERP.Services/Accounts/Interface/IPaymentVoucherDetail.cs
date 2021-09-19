﻿using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPaymentVoucherDetail : IRepository<Paymentvoucherdetail>
    {
        Task<int> CreatePaymentVoucherDetail(PaymentVoucherDetailModel paymentVoucherDetailModel);

        Task<bool> UpdatePaymentVoucherDetail(PaymentVoucherDetailModel paymentVoucherDetailModel);

        Task<bool> UpdatePaymentVoucherDetailAmount(int? paymentVoucherDetailId);

        Task<bool> DeletePaymentVoucherDetail(int paymentVoucherDetailId);

        Task<PaymentVoucherDetailModel> GetPaymentVoucherDetailById(int paymentVoucherDetailId);

        Task<IList<PaymentVoucherDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId);

        Task<DataTableResultModel<PaymentVoucherDetailModel>> GetPaymentVoucherDetailByPaymentVoucherId(int paymentVoucherId, int addRow);

        Task<DataTableResultModel<PaymentVoucherDetailModel>> GetPaymentVoucherDetailList();

    }
}
