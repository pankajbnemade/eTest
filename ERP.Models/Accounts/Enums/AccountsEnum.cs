using System.ComponentModel;

namespace ERP.Models.Accounts.Enums
{

    public enum DocumentType
    {
        [Description("Ledger")]
        Ledger = 1,

        [Description("Sales Invoice")]
        SalesInvoice = 2,

        [Description("Purchase Invoice")]
        PurchaseInvoice = 3,

        [Description("Credit Note")]
        CreditNote = 4,

        [Description("Debit Note")]
        DebitNote = 5,

        [Description("Receipt Voucher")]
        ReceiptVoucher = 6,

        [Description("Payment Voucher")]
        PaymentVoucher = 7,

        [Description("Journal Voucher")]
        JournalVoucher = 8,

        [Description("Contra Voucher")]
        ContraVoucher = 9,

        [Description("Advance Adjustment")]
        AdvanceAdjustment = 10,

        [Description("Tax Register")]
        TaxRegister = 11,

        [Description("Currency Conversion")]
        CurrencyConversion = 12,

        [Description("Employee Master")]
        EmployeeMaster = 13,
    }


    public enum DocumentStatus
    {
        [Description("Inprocess")]
        Inprocess = 1,

        [Description("ApprovalRequested")]
        ApprovalRequested = 2,

        [Description("ApprovalRejected")]
        ApprovalRejected = 3,

        [Description("Approved")]
        Approved = 4,
        
        [Description("Closed")]
        Closed = 5,

        [Description("Posted")]
        Posted = 6,
        
        [Description("Cancelled")]
        Cancelled = 7,
    }

    /// <summary>
    /// tax total type.
    /// </summary>
    public enum TaxModelType
    {
        [Description("Sub Total")]
        SubTotal = 1,

        [Description("Line Wise")]
        LineWise = 2,
    }

    /// <summary>
    /// tax total type.
    /// </summary>
    public enum DiscountType
    {
        [Description("Percentage")]
        Percentage = 1,

        [Description("Amount")]
        Amount = 2,
    }

    /// <summary>
    /// tax add or deduct
    /// </summary>
    public enum TaxAddOrDeduct
    {
        [Description("Add")]
        Add = 1,

        [Description("Deduct")]
        Deduct = 2,
    }



    /// <summary>
    /// tax add or deduct
    /// </summary>
    public enum LedgerName
    {
        [Description("Duties and Taxes")]
        DutiesAndTaxes = 29,

        [Description("Sundry Debtor")]
        SundryDebtor = 33,

        [Description("Sundry Creditor")]
        SundryCreditor = 28,

        [Description("Bank A/C")]
        BankAccount = 34,
        
        [Description("Cash A/C")]
        CashAccount = 35,
    }

    public enum TypeCorB
    {
        [Description("Cash")]
        C = 1,

        [Description("Bank")]
        B = 2,
    }

    public enum PaymentType
    {
        //[Description("LC")]
        //LC = 1,

        [Description("PDC")]
        PDC = 2,
    }

    /// <summary>
    /// tax total type.
    /// </summary>
    public enum TransactionType
    {
        [Description("Advance")]
        Advance = 1,

        [Description("Outstanding")]
        Outstanding = 2,
    }

    /// <summary>
    /// tax total type.
    /// </summary>
    public enum TransactionType_JV
    {
        [Description("Other")]
        Other = 1,

        [Description("Outstanding")]
        Outstanding = 2,
    }

}
