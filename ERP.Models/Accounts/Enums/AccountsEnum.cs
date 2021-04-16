using System.ComponentModel;

namespace ERP.Models.Accounts.Enums
{
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
        DutiesAndTaxes = 17,

        [Description("Sundry Debtor")]
        SundryDebtor = 26,

        [Description("Bank A/C")]
        BankAccount = 27,
    }
}
