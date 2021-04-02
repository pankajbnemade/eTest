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
        Percentage = 1,

        [Description("Deduct")]
        Amount = 2,
    }
}
