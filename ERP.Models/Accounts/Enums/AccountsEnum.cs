﻿using System.ComponentModel;

namespace ERP.Models.Accounts.Enums
{
    /// <summary>
    /// tax add or deduct
    /// </summary>
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
        DutiesAndTaxes = 17,

        [Description("Sundry Debtor")]
        SundryDebtor = 26,

        [Description("Sundry Creditor")]
        SundryCreditor = 16,

        [Description("Bank A/C")]
        BankAccount = 27,
        
        [Description("Cash A/C")]
        CashAccount = 28,
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
   
}
