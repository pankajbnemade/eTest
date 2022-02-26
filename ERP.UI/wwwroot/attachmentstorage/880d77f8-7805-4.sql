ALTER TABLE tbl_Acct_Invoice_PurchaseInvoice
ALTER COLUMN AdditionalCharges DECIMAL(18, 4);


GO

ALTER PROCEDURE [dbo].[Usp_Acct_Invoice_Purchase_Options_List] 
	--DECLARE
	@InvoiceNo varchar(25),                  
	@FromDate varchar(20),                  
	@ToDate varchar(20),                  
	@SupplierID int,             
	@PONO varchar(25),               
	@BranchID varchar(25),      
	@YearId varchar(25)       
	--@Type AS varchar(10)    
AS    
BEGIN

	--SET @InvoiceNo=''
	--SET @FromDate=''
	--SET @ToDate=''
	--SET @SupplierID=0
	--SET @PONO=''
	--SET @BranchID='14'
	--SET @YearId='6'
              
  
	if @FromDate='' set @FromDate='01-Jan-1900'                                                        
	if @ToDate='' set @ToDate='01-Jan-1900'   

	select fk_Voucherid,VoucherType,isnull(sum(DebitAmt_FC),0) AS DebitAmt_FC,isnull(sum(CreditAmt_FC),0) AS CreditAmt_FC into #temp_acct_CostCentreTrans 
	from tbl_acct_CostCentreTrans
	where VoucherType='PI'
	GROUP BY fk_Voucherid,VoucherType


	SELECT DISTINCT tbl_Acct_Invoice_PurchaseInvoice.pk_Invoice_ID AS Pk_InvoiceId, 
	tbl_Acct_Invoice_PurchaseInvoice.SupplierRef, tbl_Master_PartyMaster.PartyName, 
	tbl_Acct_Invoice_PurchaseInvoice.InvoiceNo, 
	tbl_POP_POrder.POOrderNo, 
	CONVERT(varchar(50), tbl_Acct_Invoice_PurchaseInvoice.InvoiceDatedOn, 106) AS InvoiceDate, 
	CONVERT(decimal(18, 4),tbl_Acct_Invoice_PurchaseInvoice.GrossAmount_FC) AS GrossAmount, 
	convert(decimal(18, 4),dbo.tbl_Acct_Invoice_PurchaseInvoice.TotalGrossAmount_FC) as TotalGrossAmount, 
	CONVERT(decimal(18, 4),tbl_Acct_Invoice_PurchaseInvoice.NetAmount_FC) AS NetAmount,
	tbl_Acct_Invoice_PurchaseInvoice.IsCancel,tbl_Acct_Invoice_PurchaseInvoice.fk_ApprovedBy ,
			isnull(#temp_acct_CostCentreTrans.DebitAmt_FC,0)  AS Cost_DebitAmt_FC,isnull(#temp_acct_CostCentreTrans.CreditAmt_FC,0)  AS Cost_CreditAmt_FC,
			CASE WHEN isnull(tbl_Acct_Invoice_PurchaseInvoice.IsCancel,0) = 1 THEN 'Cancel'
				ELSE (CASE WHEN isnull(tbl_Acct_Invoice_PurchaseInvoice.fk_ApprovedBy,0) = 0 THEN 'Approval Pending' 
				ELSE 'Approved' END) END AS StatusName,
		tbl_Acct_Invoice_PurchaseInvoice.Fk_YearId,
		CONVERT(VARCHAR(MAX),'') AS RelatedDoc
	INTO #temp_PI
	FROM  tbl_Acct_Invoice_PurchaseInvoice 
		left OUTER JOIN tbl_Acct_Invoice_PurchaseInvoice_Details
		ON tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID = tbl_Acct_Invoice_PurchaseInvoice.pk_Invoice_ID
		LEFT OUTER JOIN tbl_Master_PartyMaster 
		ON tbl_Acct_Invoice_PurchaseInvoice.fk_SupplierID = tbl_Master_PartyMaster.pk_PartyId 
		LEFT OUTER JOIN tbl_Store_GRNMaster 
		on tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID=tbl_Store_GRNMaster.pk_GRNId
		LEFT OUTER JOIN tbl_POP_POrder 
		ON tbl_Store_GRNMaster.POID = tbl_POP_POrder.pk_POOrderId  
		LEFT OUTER JOIN #temp_acct_CostCentreTrans 
		ON #temp_acct_CostCentreTrans.fk_Voucherid=tbl_Acct_Invoice_PurchaseInvoice.pk_Invoice_ID

	WHERE tbl_Acct_Invoice_PurchaseInvoice.FK_Branch_BranchID= @BranchID 
	AND tbl_Acct_Invoice_PurchaseInvoice.InvoiceForType IN ('Purchase Order', 'Other')
	AND tbl_Acct_Invoice_PurchaseInvoice.FK_YearID = @yearId 
	AND tbl_Acct_Invoice_PurchaseInvoice.IsCommonPurchase = 0
	AND (@SupplierID=0 or tbl_Acct_Invoice_PurchaseInvoice.fk_SupplierID=@SupplierID)
	AND (@PONO='' or tbl_POP_POrder.POOrderNo like '%' + @PONO + '%' )
	AND (@InvoiceNo='' or tbl_Acct_Invoice_PurchaseInvoice.InvoiceNo like '%' + @InvoiceNo + '%' )
	and (@FromDate='01-Jan-1900' or (datediff(dd,tbl_Acct_Invoice_PurchaseInvoice.InvoiceDatedOn , convert(datetime,@FromDate))<=0))                                                            
	and (@ToDate='01-Jan-1900' or (datediff(dd,tbl_Acct_Invoice_PurchaseInvoice.InvoiceDatedOn , convert(datetime,@ToDate))>=0)) 
	ORDER BY tbl_Acct_Invoice_PurchaseInvoice.pk_Invoice_ID DESC

	
	insert INTO #temp_PI
	SELECT DISTINCT tbl_Acct_Invoice_PurchaseInvoice.pk_Invoice_ID AS Pk_InvoiceId, 
	tbl_Acct_Invoice_PurchaseInvoice.SupplierRef, tbl_Master_PartyMaster.PartyName, 
	tbl_Acct_Invoice_PurchaseInvoice.InvoiceNo, 
	tbl_Sales_SalesOrder_JOB_Mst.JOBNO, 
	CONVERT(varchar(50), tbl_Acct_Invoice_PurchaseInvoice.InvoiceDatedOn, 106) AS InvoiceDate, 
	CONVERT(decimal(18, 4),tbl_Acct_Invoice_PurchaseInvoice.GrossAmount_FC) AS GrossAmount, 
	convert(decimal(18, 4),dbo.tbl_Acct_Invoice_PurchaseInvoice.TotalGrossAmount_FC) as TotalGrossAmount, 
	CONVERT(decimal(18, 4),tbl_Acct_Invoice_PurchaseInvoice.NetAmount_FC) AS NetAmount,
	tbl_Acct_Invoice_PurchaseInvoice.IsCancel,tbl_Acct_Invoice_PurchaseInvoice.fk_ApprovedBy ,
			isnull(#temp_acct_CostCentreTrans.DebitAmt_FC,0)  AS Cost_DebitAmt_FC,isnull(#temp_acct_CostCentreTrans.CreditAmt_FC,0)  AS Cost_CreditAmt_FC,
			CASE WHEN isnull(tbl_Acct_Invoice_PurchaseInvoice.IsCancel,0) = 1 THEN 'Cancel'
				ELSE (CASE WHEN isnull(tbl_Acct_Invoice_PurchaseInvoice.fk_ApprovedBy,0) = 0 THEN 'Approval Pending' 
				ELSE 'Approved' END) END AS StatusName,
		tbl_Acct_Invoice_PurchaseInvoice.Fk_YearId,
		CONVERT(VARCHAR(MAX),'') AS RelatedDoc
	FROM  tbl_Acct_Invoice_PurchaseInvoice 
		left OUTER JOIN tbl_Acct_Invoice_PurchaseInvoice_Details
		ON tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID = tbl_Acct_Invoice_PurchaseInvoice.pk_Invoice_ID
		LEFT OUTER JOIN tbl_Master_PartyMaster 
		ON tbl_Acct_Invoice_PurchaseInvoice.fk_SupplierID = tbl_Master_PartyMaster.pk_PartyId 
		left outer join Tbl_Sales_Job_DeliveryOrder_Master 
		on Tbl_Sales_Job_DeliveryOrder_Master.Pk_JOBDeliveryOrderId=tbl_Acct_Invoice_PurchaseInvoice_Details.fk_JobOrder_GRNId
		LEFT OUTER JOIN tbl_Sales_SalesOrder_JOB_Mst 
		ON tbl_Sales_SalesOrder_JOB_Mst.pk_JOBMstId=Tbl_Sales_Job_DeliveryOrder_Master.fk_JobId
		LEFT OUTER JOIN #temp_acct_CostCentreTrans 
		ON #temp_acct_CostCentreTrans.fk_Voucherid=tbl_Acct_Invoice_PurchaseInvoice.pk_Invoice_ID

	WHERE tbl_Acct_Invoice_PurchaseInvoice.FK_Branch_BranchID= @BranchID 
	AND tbl_Acct_Invoice_PurchaseInvoice.InvoiceForType = 'Job Order'
	AND tbl_Acct_Invoice_PurchaseInvoice.FK_YearID = @yearId 
	AND tbl_Acct_Invoice_PurchaseInvoice.IsCommonPurchase = 0
	AND (@SupplierID=0 or tbl_Acct_Invoice_PurchaseInvoice.fk_SupplierID=@SupplierID)
	AND (@PONO='' or tbl_Sales_SalesOrder_JOB_Mst.JOBNO like '%' + @PONO + '%' )
	AND (@InvoiceNo='' or tbl_Acct_Invoice_PurchaseInvoice.InvoiceNo like '%' + @InvoiceNo + '%' )
	and (@FromDate='01-Jan-1900' or (datediff(dd,tbl_Acct_Invoice_PurchaseInvoice.InvoiceDatedOn , convert(datetime,@FromDate))<=0))                                                            
	and (@ToDate='01-Jan-1900' or (datediff(dd,tbl_Acct_Invoice_PurchaseInvoice.InvoiceDatedOn , convert(datetime,@ToDate))>=0)) 
	ORDER BY tbl_Acct_Invoice_PurchaseInvoice.pk_Invoice_ID DESC



	UPDATE	#temp_PI
	SET		RelatedDoc = ISNULL('PV:' + STUFF((SELECT DISTINCT ', ' + VoucherNo
								FROM tbl_Acct_Voucher_PaymentReceipt 
								INNER JOIN tbl_Acct_Voucher_PaymentReceipt_Details ON pk_Voucher_PR_Id = fk_Voucher_PR_Id    
								WHERE TypePorR = 'P'  
									AND tbl_Acct_Voucher_PaymentReceipt_Details.BillNo = #temp_PI.InvoiceNo
									AND	tbl_Acct_Voucher_PaymentReceipt_Details.InvYearId = #temp_PI.Fk_YearId
								ORDER BY ', ' + VoucherNo
							FOR XML PATH(''), TYPE).value('.[1]', 'nvarchar(max)'), 1, 1, ''),'')


	UPDATE	#temp_PI
	SET		RelatedDoc = RelatedDoc 
						+ ISNULL(' JV:' + STUFF((SELECT DISTINCT ', ' + VoucherNo
								FROM tbl_Acct_Voucher_SPCD 
								INNER JOIN tbl_Acct_Voucher_SPDC_Details ON pk_Voucher_SPCD_Id = fk_Voucher_SPCDId    
								WHERE TypePorR = 'J'  
									AND tbl_Acct_Voucher_SPDC_Details.BillNo = #temp_PI.InvoiceNo
									AND	tbl_Acct_Voucher_SPDC_Details.InvYearId = #temp_PI.Fk_YearId
								ORDER BY ', ' + VoucherNo
						FOR XML PATH(''), TYPE).value('.[1]', 'nvarchar(max)'), 1, 1, ''),'')

	
	UPDATE	#temp_PI
	SET		RelatedDoc = RelatedDoc 
						+ ISNULL(' AD:' + STUFF((SELECT DISTINCT ', ' + CONVERT(VARCHAR(500),maxno)
								FROM Tbl_Acct_AdvancedDtls 
								WHERE BillType = 'P'  
									AND Tbl_Acct_AdvancedDtls.BillNo = #temp_PI.InvoiceNo
									AND	Tbl_Acct_AdvancedDtls.Fk_YearId = #temp_PI.Fk_YearId
								ORDER BY ', ' + CONVERT(VARCHAR(500),maxno)
						FOR XML PATH(''), TYPE).value('.[1]', 'nvarchar(max)'), 1, 1, ''),'')
	
	UPDATE	#temp_PI
	SET		RelatedDoc = RelatedDoc 
						+ ISNULL(' DN:' + STUFF((SELECT DISTINCT ', ' + DebitNoteNo
								FROM tbl_Accounts_Invoice_Debit 
								WHERE InvType = 'P'  
									AND tbl_Accounts_Invoice_Debit.InvId = #temp_PI.Pk_InvoiceId
								ORDER BY ', ' + DebitNoteNo
						FOR XML PATH(''), TYPE).value('.[1]', 'nvarchar(max)'), 1, 1, ''),'')
	
	UPDATE	#temp_PI
	SET		RelatedDoc = RelatedDoc 
						+ ISNULL(' CN:' + STUFF((SELECT DISTINCT ', ' + CreditNoteNo
								FROM tbl_Accounts_Invoice_Credit
								WHERE InvType = 'P'  
									AND tbl_Accounts_Invoice_Credit.InvId = #temp_PI.Pk_InvoiceId
								ORDER BY ', ' + CreditNoteNo
						FOR XML PATH(''), TYPE).value('.[1]', 'nvarchar(max)'), 1, 1, ''),'')


	SELECT * 
	FROM #temp_PI
	ORDER BY Pk_InvoiceId DESC 



	DROP TABLE #temp_acct_CostCentreTrans
	DROP TABLE #temp_PI



END







GO
/****** Object:  StoredProcedure [dbo].[Usp_Acct_Purchase_Invoice_ChargeBreakup_Add]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROC [dbo].[Usp_Acct_Purchase_Invoice_ChargeBreakup_Add]
	@AdditionalChgId int,
	@PurchaseInvId int,
	@ChargesId int,
	@ChargeValue decimal(18,4),
	@LoginEmpId int
AS
BEGIN
declare @POID int=0


if (ISNULL((select count(*) from (select tbl_Store_GRNMaster.POID 
					from  tbl_Acct_Invoice_PurchaseInvoice_Details 
					inner join tbl_Store_GRNMaster on  tbl_Store_GRNMaster.pk_GRNId=tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID
					where tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID=@PurchaseInvId
					group by tbl_Store_GRNMaster.POID) as A),0)=1)
begin
	set @POID = ISNULL((select distinct tbl_Store_GRNMaster.POID 
						from  tbl_Acct_Invoice_PurchaseInvoice_Details 
						inner join tbl_Store_GRNMaster on  tbl_Store_GRNMaster.pk_GRNId=tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID
						where tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID = @PurchaseInvId), 0)


end

	IF NOT EXISTS(SELECT pk_AdditionalChgId FROM tbl_Acct_PI_AdditionalCharges WHERE ChargesID=@ChargesId AND fk_Invoice_ID = @PurchaseInvId)
	BEGIN
		
		DECLARE @CurrencyId int    
		DECLARE @RoundOff_FC AS INT    
    
		SELECT @CurrencyId = fk_CurrencyId    
		FROM tbl_Acct_Invoice_PurchaseInvoice 
		WHERE pk_Invoice_ID = @PurchaseInvId    
    
		SET @RoundOff_FC = ISNULL((SELECT TOP 1 CurrencyDecimal FROM tbl_Master_Currency_Master WHERE pk_CurrencyId = @CurrencyId), 2)   

		INSERT INTO tbl_Acct_PI_AdditionalCharges
		(
			fk_Invoice_ID,
			fk_POId,
			ChargesID,
			InvCharges
		)
		VALUES
		(
			@PurchaseInvId,
			@POID,
			@ChargesId,
			ISNULL(ROUND(@ChargeValue, @RoundOff_FC), 0)  
		)


		SET @AdditionalChgId=@@identity

		UPDATE	tbl_Acct_Invoice_PurchaseInvoice
		SET		IsChargesUpdated = 0,
				fk_ApprovedBy = 0,
				fk_Emp_ChekedById = 0,
				AdditionalCharges = ISNULL((SELECT ROUND(SUM(InvCharges), @RoundOff_FC) AS InvCharges FROM tbl_Acct_PI_AdditionalCharges WHERE fk_Invoice_ID = @PurchaseInvId), 0)
		WHERE	pk_Invoice_ID = @PurchaseInvId

	END
	ELSE
	BEGIN
		SET @AdditionalChgId = 0
	END

	SELECT @AdditionalChgId
END







GO
/****** Object:  StoredProcedure [dbo].[Usp_Acct_Purchase_Invoice_ChargeBreakup_Delete]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROC [dbo].[Usp_Acct_Purchase_Invoice_ChargeBreakup_Delete]
	@pk_AdditionalChgId int,
	@PurchaseInvId int,
	@LoginEmpId int
AS
BEGIN

	DECLARE @CurrencyId int    
	DECLARE @RoundOff_FC AS INT    
    
	SELECT @CurrencyId = fk_CurrencyId    
	FROM tbl_Acct_Invoice_PurchaseInvoice 
	WHERE pk_Invoice_ID = @PurchaseInvId    
    
	SET @RoundOff_FC = ISNULL((SELECT TOP 1 CurrencyDecimal FROM tbl_Master_Currency_Master WHERE pk_CurrencyId = @CurrencyId), 2)   


	DELETE FROM tbl_Acct_PI_AdditionalCharges
	WHERE pk_AdditionalChgId = @pk_AdditionalChgId and fk_Invoice_ID = @PurchaseInvId

	UPDATE	tbl_Acct_Invoice_PurchaseInvoice
	SET		IsChargesUpdated = 0,
			fk_ApprovedBy = 0,
			fk_Emp_ChekedById = 0,
			AdditionalCharges = ISNULL((SELECT ROUND(SUM(InvCharges), @RoundOff_FC) AS InvCharges FROM tbl_Acct_PI_AdditionalCharges WHERE fk_Invoice_ID = @PurchaseInvId), 0)
	WHERE	pk_Invoice_ID = @PurchaseInvId

	SELECT @pk_AdditionalChgId

END






GO
/****** Object:  StoredProcedure [dbo].[Usp_Acct_Purchase_Invoice_ChargeBreakup_Get]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[Usp_Acct_Purchase_Invoice_ChargeBreakup_Get] --4289
--declare 
	@PurchaseInvId INT,
	@LoginEmpId int
--set @PurchaseInvId=4354

AS
BEGIN

	CREATE TABLE #temp
	(
		ChargeTypeId INT,
		ChargeTypeName VARCHAR(MAX),
		TotalCharge DECIMAL(18, 4),
		UsedCharge DECIMAL(18, 4),
		PendingCharge DECIMAL(18, 4),
		UsedInvoices VARCHAR(MAX),
		ChargeType VARCHAR(MAX)
	)

	IF EXISTS(SELECT pk_Invoice_ID FROM tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @PurchaseInvId AND InvoiceForType = 'Purchase Order')
	BEGIN

		declare @POID int=0
	
		if (ISNULL((select count(*) from (select tbl_Store_GRNMaster.POID 
						from  tbl_Acct_Invoice_PurchaseInvoice_Details 
						inner join tbl_Store_GRNMaster on  tbl_Store_GRNMaster.pk_GRNId=tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID
						where tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID=@PurchaseInvId
						group by tbl_Store_GRNMaster.POID) as A),0)=1)
		begin
			set @POID = ISNULL((select distinct tbl_Store_GRNMaster.POID 
						from  tbl_Acct_Invoice_PurchaseInvoice_Details 
						inner join tbl_Store_GRNMaster on  tbl_Store_GRNMaster.pk_GRNId=tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID
						where tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID=@PurchaseInvId),0)
		end

		INSERT INTO #temp
		SELECT ChargeID AS ChargeTypeId,Description1 AS ChargeTypeName,
			sum(TotalPrice) AS TotalCharge,convert(decimal(18,4),0) AS UsedCharge,convert(decimal(18,4),0) AS PendingCharge,
			convert(varchar(max),'') as UsedInvoices,
			convert(varchar(max),'') as ChargeType
		FROM tbl_POP_PurchaseOrder_HeaderCost_Mst
			INNER JOIN Userlist on pk_UserListId=ChargeID
		WHERE fk_POId=@POID 
		GROUP BY ChargeID,Description1

		UPDATE #temp  
		SET UsedCharge = isnull((SELECT sum(InvCharges) FROM tbl_Acct_PI_AdditionalCharges  
		INNER JOIN tbl_Acct_Invoice_SalesInvoice on pk_Invoice_ID=fk_Invoice_ID
		WHERE fk_POId=@POID and ChargesID=ChargeTypeId and isnull(IsCancel,0)=0) ,0)  

		UPDATE #temp
		SET PendingCharge=TotalCharge-UsedCharge

		DELETE FROM #temp where PendingCharge=0 and TotalCharge=0 and UsedCharge=0

		UPDATE #temp  
		SET #temp.UsedInvoices = replace(t3.SONo_Comma,' ',',<BR/>')  
		FROM (SELECT t1.ChargeTypeId ,
		(select  InvoiceNo+'('+convert(varchar(10),convert(decimal(18,4),InvCharges))+')' as "data()"  
		from tbl_Acct_PI_AdditionalCharges 
		inner join tbl_Acct_Invoice_PurchaseInvoice on pk_Invoice_ID=fk_Invoice_ID
		where tbl_Acct_Invoice_PurchaseInvoice.fk_POId=@POID and t1.ChargeTypeId=ChargesID and isnull(IsCancel,0)=0
		FOR XML PATH('')  
		) as SONo_Comma  
		FROM #temp AS t1) AS t3  
		WHERE #temp.ChargeTypeId = t3.ChargeTypeId 


		-- To add additional charges other than PO

		INSERT INTO #temp
		SELECT	pk_UserListId AS ChargeTypeId,
				Description1 AS ChargeTypeName,
				CONVERT(DECIMAL(18,4), 0) AS TotalCharge,
				CONVERT(DECIMAL(18,4), 0) AS UsedCharge,
				CONVERT(DECIMAL(18,4), 0) AS PendingCharge,
				CONVERT(VARCHAR(MAX), '') as UsedInvoices,
				CONVERT(VARCHAR(MAX), '') as ChargeType
		FROM	Userlist
		WHERE	fk_CategoryId = 48 AND pk_UserListId NOT IN (SELECT A.ChargeTypeId FROM #temp AS A)
		ORDER BY Description1

	END
	ELSE --JOB ORDER / OTHER
	BEGIN

		INSERT INTO #temp
		SELECT	pk_UserListId AS ChargeTypeId,
				Description1 AS ChargeTypeName,
				CONVERT(DECIMAL(18,4), 0) AS TotalCharge,
				CONVERT(DECIMAL(18,4), 0) AS UsedCharge,
				CONVERT(DECIMAL(18,4), 0) AS PendingCharge,
				CONVERT(VARCHAR(MAX), '') as UsedInvoices,
				CONVERT(VARCHAR(MAX), '') as ChargeType
		FROM	Userlist
		WHERE	fk_CategoryId = 48
		ORDER BY Description1
		
	END

	
	----- table 0--------
	SELECT * FROM #temp ORDER BY TotalCharge DESC,ChargeTypeName

	----- table 1--------
	SELECT  pk_AdditionalChgId,
			fk_Invoice_ID,
			fk_POId,
			ChargesID,
			Description1 AS ChargesName,
			InvCharges 
	FROM	tbl_Acct_PI_AdditionalCharges
	INNER JOIN  Userlist on pk_UserListId=ChargesID
	WHERE fk_Invoice_ID = @PurchaseInvId
	ORDER BY ChargesName

	----- table 2--------
	SELECT	SUM(InvCharges) AS InvCharges,
			(SELECT InvoiceForType FROM tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @PurchaseInvId) AS InvoiceForType
	FROM	tbl_Acct_PI_AdditionalCharges
	WHERE fk_Invoice_ID = @PurchaseInvId

	DROP TABLE #temp 

END






GO
/****** Object:  StoredProcedure [dbo].[USP_Acct_PurchaseInvoice_AddEditItem]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[USP_Acct_PurchaseInvoice_AddEditItem]
	@InvoiceDtlId INT, 
	@PurchaseInvoiceId INT, 
	@SrNo INT, 
	@ItemDesc VARCHAR(3000), 
	@UOM INT, 
	@AcceptedQty DECIMAL(18, 2), 
	@UnitRate DECIMAL(18, 4), 
	@LoginEmpId INT
AS
BEGIN

	IF EXISTS(SELECT pk_Invoice_ID FROM tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @PurchaseInvoiceId AND fk_ApprovedBy = 0 AND IsCancel = 0 AND InvoiceForType = 'Other' )
	BEGIN

		DECLARE @CurrencyId int    
		DECLARE @RoundOff_FC AS INT    
    
		SELECT	@CurrencyId = fk_CurrencyId    
		FROM	tbl_Acct_Invoice_PurchaseInvoice
		WHERE	pk_Invoice_ID = @PurchaseInvoiceId    
    
		SET @RoundOff_FC = ISNULL((SELECT TOP 1 CurrencyDecimal FROM tbl_Master_Currency_Master WHERE pk_CurrencyId = @CurrencyId), 2)   
  
		SET @UnitRate = ISNULL(ROUND(@UnitRate, @RoundOff_FC), 0)  

		IF @InvoiceDtlId = 0
		BEGIN

			INSERT INTO  tbl_Acct_Invoice_PurchaseInvoice_Details  
			(
				fk_Invoice_ID, fk_JobOrder_GRNId, fk_ProductID, HeatNos, POQty, AcceptedQty,
				Per,RatePerUOM, TotalAmount, fk_JobOrder_GRNDetailsId, ItemDesc, UOM
			)  
			SELECT	@PurchaseInvoiceId, 0 AS fk_JobOrder_GRNId, 0 AS fk_ProductID, '' AS HeatNos, 0 AS POQty, @AcceptedQty,
					1 AS Per, @UnitRate, ISNULL(ROUND(@UnitRate * @AcceptedQty, @RoundOff_FC), 0), 0 AS fk_JobOrder_GRNDetailsId, @ItemDesc, @UOM

			SET @InvoiceDtlId = ISNULL(@@IDENTITY, 0)

		END
		ELSE
		BEGIN

			UPDATE	tbl_Acct_Invoice_PurchaseInvoice_Details
			SET		AcceptedQty = @AcceptedQty,
					RatePerUOM = @UnitRate, 
					TotalAmount = ISNULL(ROUND(@UnitRate * @AcceptedQty, @RoundOff_FC), 0), 
					ItemDesc = @ItemDesc,
					UOM = @UOM
			WHERE	pk_Invoice_Details_ID = @InvoiceDtlId

		END

		UPDATE	tbl_Acct_Invoice_PurchaseInvoice
		SET		IsChargesUpdated = 0,
				fk_ApprovedBy = 0,
				fk_Emp_ChekedById = 0
		WHERE	pk_Invoice_ID = @PurchaseInvoiceId

		SELECT @InvoiceDtlId

	END

END



GO
/****** Object:  StoredProcedure [dbo].[USP_Acct_PurchaseInvoice_FillItems]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[USP_Acct_PurchaseInvoice_FillItems]-- 1    
	--DECLARE
	@InvoiceDtlId AS INT, 
	@InvId AS INT, 
	@TableName VARCHAR(50), 
	@LoginEmpId AS INT  
	--SET @InvId=6679
AS          
BEGIN
  
	DECLARE @IsEditVisible AS INT = 0
	
	IF EXISTS(SELECT pk_Invoice_ID FROM	tbl_Acct_Invoice_PurchaseInvoice WHERE	pk_Invoice_ID = @InvId AND InvoiceForType = 'Purchase Order')
	BEGIN

		SELECT  tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID AS fk_GRNMst_GRNId, 
				tbl_Store_GRNMaster.GRNNo, 
				isnull(Product_Master.ReferenceCode, tbl_POP_POrderDetails.fk_Item_ItemId) AS ReferenceCode, 
				isnull(Product_Master.LongDesc, tbl_POP_POrderDetails.description) AS LongDesc, 
				isnull(UserList.Description1,'') AS UOM, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.POQty, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.AcceptedQty, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.Per AS RatePer, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.RatePerUOM AS Rate, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.TotalAmount AS Amount, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.HeatNos, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.pk_Invoice_Details_ID, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.fk_ProductID AS fk_Items_ItemId, 
				tbl_POP_POrder.POOrderNo, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.UOM AS UOMId, 
				ISNULL(tbl_Acct_Invoice_PurchaseInvoice_Details.pk_GRNDetailsId, 0) AS pk_GRNDetailsId,
				@IsEditVisible AS IsEditVisible
		FROM    UserList RIGHT OUTER JOIN
				tbl_POP_POrder RIGHT OUTER JOIN
				tbl_POP_Shipping RIGHT OUTER JOIN
				tbl_Store_GRNMaster ON tbl_POP_Shipping.Pk_shippingid = tbl_Store_GRNMaster.fk_shippingId ON 
				tbl_POP_POrder.pk_POOrderId = tbl_Store_GRNMaster.POID RIGHT OUTER JOIN
				tbl_Acct_Invoice_PurchaseInvoice_Details LEFT OUTER JOIN
				Product_Master ON tbl_Acct_Invoice_PurchaseInvoice_Details.fk_ProductID = Product_Master.pk_ProductId LEFT OUTER JOIN
				tbl_POP_POrderDetails RIGHT OUTER JOIN
				tbl_Store_GRNDetails ON tbl_POP_POrderDetails.pk_POrderDetailsId = tbl_Store_GRNDetails.fk_PODetsID ON 
				tbl_Acct_Invoice_PurchaseInvoice_Details.pk_GRNDetailsId = tbl_Store_GRNDetails.pk_GRNDetailsId ON 
				tbl_Store_GRNMaster.pk_GRNId = tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID ON 
				UserList.pk_UserListId = Product_Master.fk_UOMId1
		WHERE   (tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID = @InvId)  
				AND (@InvoiceDtlId = 0 OR @InvoiceDtlId = tbl_Acct_Invoice_PurchaseInvoice_Details.pk_Invoice_Details_ID)
		ORDER BY tbl_Store_GRNMaster.GRNNo, tbl_Acct_Invoice_PurchaseInvoice_Details.pk_Invoice_Details_ID 

    END
	ELSE IF EXISTS(SELECT pk_Invoice_ID FROM	tbl_Acct_Invoice_PurchaseInvoice WHERE	pk_Invoice_ID = @InvId AND InvoiceForType = 'Job Order')
	BEGIN

		SELECT  tbl_Acct_Invoice_PurchaseInvoice_Details.fk_JobOrder_GRNId AS fk_GRNMst_GRNId, 
				Tbl_Sales_Job_DeliveryOrder_Master.JOBDONo AS GRNNo, 
				isnull(Product_Master.ReferenceCode, tbl_Sales_SalesOrder_JOB_Dtl.ItemId) AS ReferenceCode, 
				isnull(Product_Master.LongDesc, tbl_Sales_SalesOrder_JOB_Dtl.ProductName) AS LongDesc, 
				isnull(UserList.Description1,'') AS UOM, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.POQty, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.AcceptedQty, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.Per AS RatePer, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.RatePerUOM AS Rate, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.TotalAmount AS Amount, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.HeatNos, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.pk_Invoice_Details_ID, 
				tbl_Acct_Invoice_PurchaseInvoice_Details.fk_ProductID AS fk_Items_ItemId, 
				tbl_Sales_SalesOrder_JOB_Mst.JOBNO AS POOrderNo, 
				ISNULL(tbl_Acct_Invoice_PurchaseInvoice_Details.UOM, UserList.Description1) AS UOMId, 
				ISNULL(tbl_Acct_Invoice_PurchaseInvoice_Details.fk_JobOrder_GRNDetailsId, 0) AS pk_GRNDetailsId,
				@IsEditVisible AS IsEditVisible
		FROM    UserList RIGHT OUTER JOIN
				tbl_Sales_SalesOrder_JOB_Mst RIGHT OUTER JOIN
				Tbl_Sales_Job_DeliveryOrder_Master ON 
				tbl_Sales_SalesOrder_JOB_Mst.pk_JOBMstId = Tbl_Sales_Job_DeliveryOrder_Master.fk_JobId RIGHT OUTER JOIN
				tbl_Acct_Invoice_PurchaseInvoice_Details LEFT OUTER JOIN
				Product_Master ON tbl_Acct_Invoice_PurchaseInvoice_Details.fk_ProductID = Product_Master.pk_ProductId 
				LEFT OUTER JOIN
				tbl_Sales_SalesOrder_JOB_Dtl RIGHT OUTER JOIN
				Tbl_Sales_Job_DeliveryOrder_Details ON tbl_Sales_SalesOrder_JOB_Dtl.pk_JOBDtlId = Tbl_Sales_Job_DeliveryOrder_Details.fk_JOBDtlId ON 
				tbl_Acct_Invoice_PurchaseInvoice_Details.fk_JobOrder_GRNDetailsId = Tbl_Sales_Job_DeliveryOrder_Details.pk_JOBDeliveryOrderDetId ON 
				Tbl_Sales_Job_DeliveryOrder_Master.Pk_JOBDeliveryOrderId = tbl_Acct_Invoice_PurchaseInvoice_Details.fk_JobOrder_GRNId ON 
				UserList.pk_UserListId = Product_Master.fk_UOMId1
		WHERE   (tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID = @InvId)  
				AND (@InvoiceDtlId = 0 OR @InvoiceDtlId = tbl_Acct_Invoice_PurchaseInvoice_Details.pk_Invoice_Details_ID)
		ORDER BY Tbl_Sales_Job_DeliveryOrder_Master.JOBDONo, tbl_Acct_Invoice_PurchaseInvoice_Details.pk_Invoice_Details_ID

	END
	ELSE
	BEGIN
		
		SET @IsEditVisible = ISNULL((SELECT 1 FROM tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvId AND fk_ApprovedBy = 0 AND IsCancel = 0 AND InvoiceForType = 'Other'), 0)

		SELECT  ISNULL(fk_GRN_ID, 0) AS fk_GRNMst_GRNId, 
				'' AS GRNNo, 
				'' AS ReferenceCode, 
				ItemDesc AS LongDesc, 
				ISNULL(Description1, '') AS UOM, 
				ISNULL(POQty, 0) AS POQty, 
				ISNULL(AcceptedQty, 0) AS AcceptedQty, 
				ISNULL(Per, 0) AS RatePer, 
				ISNULL(RatePerUOM, 0) AS Rate, 
				ISNULL(TotalAmount, 0) AS Amount, 
				ISNULL(HeatNos, '') AS HeatNos, 
				fk_Invoice_ID, 
				pk_Invoice_Details_ID, 
				ISNULL(fk_ProductID, 0)  AS fk_Items_ItemId, 
				'' AS POOrderNo, 
				ISNULL(UOM, 0) AS UOMId, 
				ISNULL(pk_GRNDetailsId, 0) AS pk_GRNDetailsId,
				@IsEditVisible AS IsEditVisible
		FROM    tbl_Acct_Invoice_PurchaseInvoice_Details
		LEFT OUTER JOIN UserList ON UserList.pk_UserListId = tbl_Acct_Invoice_PurchaseInvoice_Details.UOM
		WHERE   (tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID = @InvId)  
				AND (@InvoiceDtlId = 0 OR @InvoiceDtlId = tbl_Acct_Invoice_PurchaseInvoice_Details.pk_Invoice_Details_ID)
		ORDER BY tbl_Acct_Invoice_PurchaseInvoice_Details.pk_Invoice_Details_ID

	END 
   
END





GO
/****** Object:  StoredProcedure [dbo].[Usp_Acct_PurchaseInvoice_Get_EditMode]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--	[Usp_Acct_PurchaseInvoice_Get_EditMode] 34525,'tbl_Acct_FYGOP1818'   

ALTER PROC [dbo].[Usp_Acct_PurchaseInvoice_Get_EditMode] --34525,'tbl_Acct_FYGOP1818'    
	--declare
	@InvId AS int,
	@TableName varchar(50)    
	--set @InvId=6679
AS          
BEGIN

	DECLARE @POID INT = 0
	DECLARE @POCurr int = 0
	DECLARE @BlockPOChange int = 0
	DECLARE @BlockGRNChange int = 0
	DECLARE @BlockIsCancel int = 0

	IF EXISTS(SELECT pk_Invoice_Details_ID FROM tbl_Acct_Invoice_PurchaseInvoice_Details WHERE fk_Invoice_ID = @InvId)
		OR EXISTS(SELECT pk_AdditionalChgId FROM tbl_Acct_PI_AdditionalCharges WHERE fk_Invoice_ID = @InvId)
	BEGIN
		SET @BlockPOChange = 1

		IF EXISTS(SELECT DISTINCT fk_POId FROM  tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvId AND ISNULL(IsCancel, 0) = 0)
		BEGIN
			SET @BlockIsCancel = 1
		END
	END


	IF (ISNULL((SELECT COUNT(*) FROM (SELECT tbl_Store_GRNMaster.POID 
		FROM  tbl_Acct_Invoice_PurchaseInvoice_Details 
		INNER JOIN tbl_Store_GRNMaster on  tbl_Store_GRNMaster.pk_GRNId=tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID
		WHERE tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID=@InvId
		GROUP BY tbl_Store_GRNMaster.POID) as A),0)=1)
	BEGIN
		SET @POID = ISNULL((SELECT DISTINCT tbl_Store_GRNMaster.POID 
				FROM  tbl_Acct_Invoice_PurchaseInvoice_Details 
				INNER JOIN tbl_Store_GRNMaster ON  tbl_Store_GRNMaster.pk_GRNId=tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID
				WHERE tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID=@InvId),0)

	END

	SET @POID = CASE WHEN ISNULL(@POID, 0) <> 0 THEN ISNULL(@POID, 0) ELSE ISNULL((SELECT DISTINCT fk_POId FROM  tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvId), 0) END

	SET @POCurr = ISNULL((SELECT CurrencyId FROM tbl_POP_POrder WHERE pk_POOrderId = @POID), 0)

	
	SELECT	pk_Invoice_ID, InvoiceNo, CONVERT(VARCHAR(50), InvoiceDatedOn,106) AS InvoiceDate,   
			fk_SupplierID, fk_SupAddressID, 
			@POID AS fk_POId,
			ISNULL(@POCurr, 0) AS POIdCurrId, 
			DefaultACLegID,GrossAmount, GrossAmount_FC, TotalGrossAmount, TotalGrossAmount_FC,    
			AmtAfterCharge, AmtAfterCharge_FC, DiscountAmt, DiscountAmt_FC, DiscountType, AmtAfterDisc,   
			AmtAfterDisc_FC, fk_CurrencyId, CurrencyRate,NetAmount, NetAmount_FC,   
			NetAmtInWord AS AmtInWords, CreditLimit, PayTerms, DeliveryPeriod, DeliveryTerms, OtherTerms,  
			Remark, fk_PreparedBy,fk_ApprovedBy, Fk_YearId, VoucherStyle,  Fk_Branch_BranchID,   
			InvoiceType, IsCancel, IsLock, SupplierRef, fk_schedulingId, fk_RegSetup_ID,     
			IsCommonPurchase, isAcctPosting,OtherRef,
			fk_BranchCurrencyId,
			ExchangeRate_BC,
			TaxAmount_FC,
			TaxAmount,
			TaxAmount_BC,
			GrossAmount_BC,
			NetAmount_BC,
			InvoiceForType,
			fk_JobOrderId,
			fk_VATAddId,
			SupplyDate,
			fk_ShipToAddId,
			(SELECT COUNT(Billno)  
			FROM	tbl_Acct_Voucher_PaymentReceipt_Details 
			INNER JOIN tbl_Acct_Voucher_PaymentReceipt   ON fk_Voucher_PR_Id=pk_Voucher_PR_Id WHERE billno=InvoiceNo AND TypePorR='P'   
			AND  tbl_Acct_Voucher_PaymentReceipt.Fk_Branch_BranchID=tbl_Acct_Invoice_PurchaseInvoice.Fk_Branch_BranchID 
			AND InvYearId=tbl_Acct_Invoice_PurchaseInvoice.Fk_YearId) AS PREnt,    

			(SELECT COUNT(tbl_Acct_Voucher_SPDC_Details.Billno) FROM tbl_Acct_Voucher_SPDC_Details   
			INNER JOIN tbl_Acct_Voucher_SPCD ON fk_Voucher_SPCDId=pk_Voucher_SPCD_Id   
			WHERE tbl_Acct_Voucher_SPDC_Details.Billno=InvoiceNo AND TypePorR='P'   
			AND  tbl_Acct_Voucher_SPCD.Fk_Branch_BranchID=tbl_Acct_Invoice_PurchaseInvoice.Fk_Branch_BranchID   
			AND InvYearId=tbl_Acct_Invoice_PurchaseInvoice.Fk_YearId) AS JVEnt,

			ISNULL(AdditionalCharges,0) as AdditionalCharges,
			ISNULL(IsChargesUpdated,0) as IsChargesUpdated,
			@BlockPOChange as BlockPOChange,
			@BlockGRNChange as BlockGRNChange,
			case when fk_ApprovedBy = 0 then 0 else 1 end as BlockGO,
			@BlockIsCancel AS BlockIsCancel
	FROM	tbl_Acct_Invoice_PurchaseInvoice 
	WHERE	pk_Invoice_ID = @InvId       
  
	
	------------------------------------- table 1------------------------------------------------

	

	SELECT	pk_ChargesId,
			fk_Invoice_ID,
			fk_ChargesID,
			CONVERT(INT, TypePer0Amt1) AS TypePer0Amt1,
			Rate,
			(CASE Add0OrLess1 WHEN 0 THEN 1 ELSE -1 END) * ChargesValue_FC AS ChargesValue_FC,
			(CASE Add0OrLess1 WHEN 0 THEN 1 ELSE -1 END) * ChargesValue_BC AS ChargesValue_BC,
			(CASE Add0OrLess1 WHEN 0 THEN 1 ELSE -1 END) * ChargesValue AS ChargesValue,
			CONVERT(VARCHAR(3000), '') AS LedgerName
	INTO	#temp_TaxDetails
	FROM	tbl_Acct_Invoice_PurchaseInvoice_Charges
	WHERE	fk_Invoice_ID = @InvId

	EXEC('UPDATE	#temp_TaxDetails
		SET		LedgerName = ISNULL((SELECT Name FROM ' + @TableName + ' WHERE pk_AcctGrpId = fk_ChargesID), '''')')

	SELECT * FROM #temp_TaxDetails ORDER BY pk_ChargesId
   
END



GO
/****** Object:  StoredProcedure [dbo].[USP_Acct_PurchaseInvoice_GetPO]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  
ALTER PROC [dbo].[USP_Acct_PurchaseInvoice_GetPO] 
	@SuppId  INT ,  
	@BranchId INT,  
	@InvoiceId INT         
AS     
BEGIN
      
	CREATE TABLE #temp_POList
	(
		POOrderNo VARCHAR(500),
		pk_POOrderId INT
	)

	INSERT INTO #temp_POList (POOrderNo, pk_POOrderId)
	SELECT DISTINCT tbl_POP_POrder.POOrderNo, 
					tbl_POP_POrder.pk_POOrderId
	FROM	tbl_POP_POrder 
	RIGHT OUTER JOIN tbl_Store_GRNMaster ON tbl_POP_POrder.pk_POOrderId = tbl_Store_GRNMaster.POID
	WHERE	tbl_POP_POrder.fk_Party_SupplierId = @SuppId 
		AND ISNULL(tbl_Store_GRNMaster.IsAcctPurchaseDone, 0) <> 1
		AND tbl_Store_GRNMaster.status = 2
		AND tbl_POP_POrder.Isversion = 1
		
	INSERT INTO #temp_POList (POOrderNo, pk_POOrderId)
	SELECT DISTINCT tbl_POP_POrder.POOrderNo, 
					tbl_POP_POrder.pk_POOrderId
	FROM tbl_POP_POrder 
	WHERE  tbl_POP_POrder.pk_POOrderId IN (SELECT fk_POId FROM tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvoiceId)
		OR tbl_POP_POrder.pk_POOrderId IN (SELECT POId FROM tbl_Acct_Invoice_PurchaseInvoice_Details
									INNER JOIN tbl_Store_GRNMaster ON tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID = tbl_Store_GRNMaster.pk_GRNId
									 WHERE Fk_Invoice_ID = @InvoiceId AND tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID <> 0)
	

	SELECT DISTINCT * FROM #temp_POList ORDER BY POOrderNo

	DROP TABLE #temp_POList


END
GO
/****** Object:  StoredProcedure [dbo].[USP_Acct_PurchaseInvoice_GetTotalAdditionalCharges]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[USP_Acct_PurchaseInvoice_GetTotalAdditionalCharges]
	--DECLARE  
	@POId int ,        
	@InvId int ,        
	@InvoiceForType VARCHAR(200)           
AS      
BEGIN

	--SET @fk_soId = 3567  
	--SET @InvId = 4322  

	CREATE TABLE #tempCharges  
	(  
		OrderCharges decimal(18, 4),  
		InvoiceCharges decimal(18, 4),  
	)  

	IF @POId <> 0
	BEGIN

		IF @InvoiceForType = 'Purchase Order'
		BEGIN

			INSERT INTO #tempCharges  
			SELECT	SUM(TotalPrice), 0
			FROM	tbl_POP_PurchaseOrder_HeaderCost_Mst
			WHERE	fk_POId = @POId 

			INSERT INTO #tempCharges  
			SELECT	0, SUM(InvCharges) AS InvCharges  
			FROM	tbl_Acct_PI_AdditionalCharges 
				INNER JOIN tbl_Acct_Invoice_PurchaseInvoice ON fk_Invoice_ID = pk_Invoice_ID  
			WHERE	tbl_Acct_Invoice_PurchaseInvoice.fk_POId = @POId AND IsCancel <> 1  AND pk_Invoice_ID <> @InvId

		END
		ELSE
		BEGIN

			INSERT INTO #tempCharges  
			SELECT	0,0

			INSERT INTO #tempCharges  
			SELECT	0, SUM(InvCharges) AS InvCharges  
			FROM	tbl_Acct_PI_AdditionalCharges 
				INNER JOIN tbl_Acct_Invoice_PurchaseInvoice ON fk_Invoice_ID = pk_Invoice_ID  
			WHERE	tbl_Acct_Invoice_PurchaseInvoice.fk_JobOrderId = @POId AND IsCancel <> 1  AND pk_Invoice_ID <> @InvId

		END
	END

	SELECT ISNULL(SUM(OrderCharges) - SUM(InvoiceCharges), 0) AS TotalAdditionalCharges   
	FROM	#tempCharges 

	DROP TABLE #tempCharges  

END














GO
/****** Object:  StoredProcedure [dbo].[Usp_Get_PurchaseInvoice_ItemDetails]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--	Usp_Get_PurchaseInvoice_ItemDetails   '1','Job Order',0

ALTER PROC [dbo].[Usp_Get_PurchaseInvoice_ItemDetails]   -- '16158','Purchase Order',0
	--DECLARE  
	@GRNIds varchar(100) , 
	@InvoiceForType varchar(100) , 
	@InvoiceId varchar(100)        
AS        
    
--SET @Grn_Ids = '23,24'  
  
BEGIN        

	DECLARE @CurrencyId int    
	DECLARE @RoundOff_FC AS INT    
    
	SELECT	@CurrencyId = fk_CurrencyId    
	FROM	tbl_Acct_Invoice_PurchaseInvoice
	WHERE	pk_Invoice_ID = @InvoiceId    
    
	SET @RoundOff_FC = ISNULL((SELECT TOP 1 CurrencyDecimal FROM tbl_Master_Currency_Master WHERE pk_CurrencyId = @CurrencyId), 2)   
  
	----- @Rate
	DECLARE @Str AS VARCHAR(MAX) = ''    


    IF @InvoiceForType = 'Purchase Order'
	BEGIN
		PRINT 1

		SET @Str = 
			'SELECT     tbl_Store_GRNDetails.fk_GRNMst_GRNId, tbl_Store_GRNDetails.fk_Items_ItemId, tbl_POP_POrder.POOrderNo, tbl_Store_GRNMaster.GRNNo, 
						ISNULL(Product_Master.ReferenceCode, tbl_POP_POrderDetails.fk_Item_ItemId) AS ReferenceCode, ISNULL(Product_Master.ProductName, 
						tbl_POP_POrderDetails.description) AS LongDesc, tbl_Store_GRNDetails.HeatNos, tbl_Store_GRNDetails.POQty, tbl_Store_GRNDetails.AcceptedQty, 
						Userlist.Description1 AS UOM, 
						tbl_Store_GRNDetails.RatePer, 
						ROUND(tbl_POP_POrderDetails.unitprice, ' + CONVERT(VARCHAR(10), @RoundOff_FC) + ') AS Rate, 
						ROUND(tbl_POP_POrderDetails.unitprice * tbl_Store_GRNDetails.AcceptedQty / tbl_Store_GRNDetails.RatePer, ' + CONVERT(VARCHAR(10), @RoundOff_FC) + ') AS Amount, 
						tbl_Store_GRNDetails.pk_GRNDetailsId,
						0 AS pk_Invoice_Details_ID,
						0 AS IsEditVisible
			FROM        Product_Master RIGHT OUTER JOIN
						tbl_Store_GRNDetails INNER JOIN
						tbl_Store_GRNMaster ON tbl_Store_GRNDetails.fk_GRNMst_GRNId = tbl_Store_GRNMaster.pk_GRNId INNER JOIN
						tbl_POP_POrderDetails INNER JOIN
						tbl_POP_POrder ON tbl_POP_POrderDetails.fk_POrder_POOrderId = tbl_POP_POrder.pk_POOrderId ON 
						tbl_Store_GRNDetails.fk_PODetsID = tbl_POP_POrderDetails.pk_POrderDetailsId ON 
						Product_Master.pk_ProductId = tbl_Store_GRNDetails.fk_Items_ItemId
						LEFT OUTER JOIN UserList ON UserList.pk_UserListId = Product_Master.fk_UOMId1
			WHERE		tbl_Store_GRNDetails.fk_GRNMst_GRNId in ('+ @GRNIds +')'
		
		EXEC(@Str)  
    
	END
	ELSE
	BEGIN

		PRINT 2

		SET @Str = 
			'SELECT	Tbl_Sales_Job_DeliveryOrder_Details.fk_JOBDeliveryOrderId AS fk_GRNMst_GRNId, 
						Tbl_Sales_Job_DeliveryOrder_Details.fk_itemid AS fk_Items_ItemId, tbl_Sales_SalesOrder_JOB_Mst.JOBNO AS POOrderNo, 
						Tbl_Sales_Job_DeliveryOrder_Master.JOBDONo AS GRNNo, 
						ISNULL(Product_Master.ReferenceCode, tbl_Sales_SalesOrder_JOB_Dtl.ItemId) AS ReferenceCode, 
						ISNULL(Product_Master.ProductName, tbl_Sales_SalesOrder_JOB_Dtl.ProductName) AS LongDesc, 
						'''' AS HeatNos, Tbl_Sales_Job_DeliveryOrder_Details.JOBSOQty AS POQty, Tbl_Sales_Job_DeliveryOrder_Details.Quantity AS AcceptedQty, 
						Userlist.Description1 AS UOM, 
						1 AS RatePer, 
						ROUND(tbl_Sales_SalesOrder_JOB_Dtl.UnitCost, ' + CONVERT(VARCHAR(10), @RoundOff_FC) + ') AS Rate, 
						ROUND(tbl_Sales_SalesOrder_JOB_Dtl.UnitCost * Tbl_Sales_Job_DeliveryOrder_Details.Quantity / 1, ' + CONVERT(VARCHAR(10), @RoundOff_FC) + ') AS Amount, 
						Tbl_Sales_Job_DeliveryOrder_Details.pk_JOBDeliveryOrderDetId AS pk_GRNDetailsId,
						0 AS pk_Invoice_Details_ID,
						0 AS IsEditVisible
			FROM		Product_Master 
						RIGHT OUTER JOIN Tbl_Sales_Job_DeliveryOrder_Details 
						INNER JOIN Tbl_Sales_Job_DeliveryOrder_Master ON Tbl_Sales_Job_DeliveryOrder_Details.fk_JOBDeliveryOrderId = Tbl_Sales_Job_DeliveryOrder_Master.Pk_JOBDeliveryOrderId 
						INNER JOIN tbl_Sales_SalesOrder_JOB_Dtl 
						INNER JOIN tbl_Sales_SalesOrder_JOB_Mst ON tbl_Sales_SalesOrder_JOB_Dtl.fk_JOBMstId = tbl_Sales_SalesOrder_JOB_Mst.pk_JOBMstId ON 
						Tbl_Sales_Job_DeliveryOrder_Details.fk_JOBDtlId = tbl_Sales_SalesOrder_JOB_Dtl.pk_JOBDtlId ON 
						Product_Master.pk_ProductId = Tbl_Sales_Job_DeliveryOrder_Details.fk_itemid 
						INNER JOIN Userlist ON Userlist.pk_UserListId = Tbl_Sales_Job_DeliveryOrder_Details.fk_UomId
			WHERE		Tbl_Sales_Job_DeliveryOrder_Details.fk_JOBDeliveryOrderId in (' + @GRNIds + ')' 

		EXEC(@Str)  
	END

END        
















GO
/****** Object:  StoredProcedure [dbo].[USP_PurchaseInvoice_Breakup_AddEditDet]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROC [dbo].[USP_PurchaseInvoice_Breakup_AddEditDet]
	@PurchaseInvoiceId INT,
	@BreakupChargeID INT,
	@Percentage DECIMAL(18, 2),
	@Remark nvarchar(2000),
	@LoginEmpId INT

AS   
BEGIN --PROC

	IF @BreakupChargeID = 0
	BEGIN

		INSERT INTO tbl_Acct_Purchase_Invoice_Amt_Breakup
		(  
			fk_mstId,
			Percentage,
			Remarks,
			Amount,
			fk_UpdatedBy_EmpId,
			UpdatedDate
		)  
		VALUES  
		(  
			@PurchaseInvoiceId,
			@Percentage,
			@Remark,
			0,
			@LoginEmpId,
			GETDATE()
		)
		
		SET @BreakupChargeID = ISNULL(@@IDENTITY,0)

	END
	ELSE
	BEGIN

		UPDATE	tbl_Acct_Purchase_Invoice_Amt_Breakup
		SET		Percentage = @Percentage,
				Remarks = @Remark,
				fk_UpdatedBy_EmpId = @LoginEmpId,
				UpdatedDate = GETDATE()
		WHERE	pk_ChargeID = @BreakupChargeID

	END
		
	IF @PurchaseInvoiceId <> 0
	BEGIN
		UPDATE	tbl_Acct_Invoice_PurchaseInvoice
		SET		fk_ApprovedBy = 0,
				fk_Emp_ChekedById = 0
		WHERE	pk_Invoice_ID = @PurchaseInvoiceId

		EXEC USP_PurchaseInvoice_Master_UpdateAmount @PurchaseInvoiceId, @LoginEmpId  
	END

	SELECT ISNULL(@BreakupChargeID,0)

END --PROC

GO
/****** Object:  StoredProcedure [dbo].[USP_PurchaseInvoice_Breakup_DeleteDet]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROC [dbo].[USP_PurchaseInvoice_Breakup_DeleteDet]
	@PurchaseInvoiceId INT,
	@BreakupChargeID INT,
	@LoginEmpId INT
AS
BEGIN --PROC

	IF (@BreakupChargeID <> 0)
	BEGIN   
		DELETE FROM	tbl_Acct_Purchase_Invoice_Amt_Breakup
		WHERE	pk_ChargeID = @BreakupChargeID
		
		UPDATE	tbl_Acct_Invoice_PurchaseInvoice
		SET		fk_ApprovedBy = 0,
				fk_Emp_ChekedById = 0
		WHERE	pk_Invoice_ID = @PurchaseInvoiceId

		EXEC USP_PurchaseInvoice_Master_UpdateAmount @PurchaseInvoiceId, @LoginEmpId  

		SELECT ''
	END  
	ELSE  
	BEGIN  
		SELECT 'Record Not Deleted'
	END

END --PROC

GO
/****** Object:  StoredProcedure [dbo].[Usp_PurchaseInvoice_Charges_InsertUpDate]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROC [dbo].[Usp_PurchaseInvoice_Charges_InsertUpDate] 
	--DECLARE
	@ChargesId INT, 
	@InvoiceId INT, 
	@TypePer0Amt1 INT, 
	@Rate DECIMAL(18, 4),
	@LoginEmpId INT
As    
BEGIN        

	DECLARE @CurrencyId int    
	DECLARE @RoundOff_FC AS INT    
    
	SELECT	@CurrencyId = fk_CurrencyId    
	FROM	tbl_Acct_Invoice_PurchaseInvoice
	WHERE	pk_Invoice_ID = @InvoiceId    
    
	SET @RoundOff_FC = ISNULL((SELECT TOP 1 CurrencyDecimal FROM tbl_Master_Currency_Master WHERE pk_CurrencyId = @CurrencyId), 2)   
  
	SET @Rate = ISNULL(ROUND(@Rate, @RoundOff_FC), 0)

	----- @Rate

	UPDATE	tbl_Acct_Invoice_PurchaseInvoice_Charges
	SET		TypePer0Amt1 = @TypePer0Amt1,
			Rate = @Rate
	WHERE	pk_ChargesId = @ChargesId

	UPDATE	tbl_Acct_Invoice_PurchaseInvoice
	SET		fk_ApprovedBy = 0,
			IsChargesUpdated = 0
	WHERE	pk_Invoice_ID = @InvoiceId

END



GO
/****** Object:  StoredProcedure [dbo].[Usp_PurchaseInvoice_Details_InsertUpDate]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROC [dbo].[Usp_PurchaseInvoice_Details_InsertUpDate]
	--DECLARE
	@InvoiceId INT,
	@GRNId INT,
	@Action VARCHAR(100),
	@LoginEmpId INT
AS
BEGIN

	DECLARE @IsCancel INT = 0
	DECLARE @ApprovedBy INT = 0
	DECLARE @InvoiceForType VARCHAR(500) = ''
	DECLARE @RetVal VARCHAR(2000) = ''
	DECLARE @CurrencyId int    
	DECLARE @RoundOff_FC AS INT   

	SELECT	@IsCancel = ISNULL(IsCancel, 0),
			@ApprovedBy = ISNULL(fk_ApprovedBy, 0),
			@InvoiceForType = ISNULL(InvoiceForType, ''),
			@CurrencyId = fk_CurrencyId  
	FROM	tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvoiceId

	SET @RoundOff_FC = ISNULL((SELECT TOP 1 CurrencyDecimal FROM tbl_Master_Currency_Master WHERE pk_CurrencyId = @CurrencyId), 2)   
  

	----- @Rate

	
	IF @ApprovedBy <> 0
	BEGIN
		SET @RetVal = 'You can not add or delete Item. Invoice is allready approved.' 
	END

	ELSE IF @IsCancel = 1
	BEGIN
		SET @RetVal = 'You can not add or delete Item. Invoice is allready cancelled.' 
	END

	ELSE
	BEGIN ---ADD / DELETE
		IF @Action = 'DELETE'
		BEGIN ---DELETE

			IF @InvoiceForType = 'Purchase Order'
			BEGIN

				DELETE FROM tbl_Acct_Invoice_PurchaseInvoice_Details WHERE fk_Invoice_Id = @InvoiceId AND fk_GRN_ID = @GRNId
	 
				UPDATE	tbl_Store_GRNMaster 
				SET		IsAcctPurchaseDone = 0 
				WHERE	pk_GRNId = @GRNId

			END
			ELSE IF @InvoiceForType = 'Job Order'
			BEGIN
				
				DELETE FROM tbl_Acct_Invoice_PurchaseInvoice_Details WHERE fk_Invoice_ID = @InvoiceId AND fk_JobOrder_GRNId = @GRNId

			END

		END  ---DELETE
		ELSE IF @Action = 'ADD'
		BEGIN ---ADD

			CREATE TABLE #temp_GRNDtl
			(
				GRNId INT, 
				fk_ProductID INT,
				HeatNos VARCHAR(3000),
				POQty DECIMAL(18, 2), 
				AcceptedQty DECIMAL(18, 2), 
				Per INT, 
				RatePerUOM DECIMAL(18, 4), 
				TotalAmount DECIMAL(18, 4), 
				pk_GRNDetailsId INT,
				ItemDesc VARCHAR(3000), 
			)
			
			IF @InvoiceForType = 'Purchase Order'
			BEGIN

				IF EXISTS(SELECT pk_Invoice_Details_ID FROM tbl_Acct_Invoice_PurchaseInvoice_Details WHERE fk_Invoice_ID = @InvoiceId AND fk_GRN_ID = @GRNId)
				BEGIN
					SET @RetVal = 'Selected GRN allready added in selected purchase invoice'
				END
				ELSE IF EXISTS(SELECT pk_Invoice_Details_ID FROM tbl_Acct_Invoice_PurchaseInvoice_Details WHERE fk_Invoice_ID <> @InvoiceId AND fk_GRN_ID = @GRNId)
				BEGIN
					SET @RetVal = 'Selected GRN allready added in other purchase invoice'
				END
				ELSE
				BEGIN

					INSERT INTO #temp_GRNDtl
					(
						GRNId, fk_ProductID, HeatNos, POQty, AcceptedQty,
						Per, RatePerUOM, TotalAmount, pk_GRNDetailsId, ItemDesc
					)

					SELECT	tbl_Store_GRNDetails.fk_GRNMst_GRNId AS GRNId,
							tbl_Store_GRNDetails.fk_Items_ItemId AS fk_ProductID, 
							tbl_Store_GRNDetails.HeatNos AS HeatNos, 
							tbl_Store_GRNDetails.POQty AS POQty, 
							tbl_Store_GRNDetails.AcceptedQty AS AcceptedQty,
							tbl_Store_GRNDetails.RatePer AS Per,
							ROUND(tbl_POP_POrderDetails.unitprice, @RoundOff_FC) AS RatePerUOM, 
							ROUND(ROUND(tbl_POP_POrderDetails.unitprice, @RoundOff_FC) * tbl_Store_GRNDetails.AcceptedQty / tbl_Store_GRNDetails.RatePer, @RoundOff_FC) AS TotalAmount, 
							tbl_Store_GRNDetails.pk_GRNDetailsId, 
							'' AS ItemDesc
					FROM    tbl_Store_GRNDetails INNER JOIN
							tbl_Store_GRNMaster ON tbl_Store_GRNDetails.fk_GRNMst_GRNId = tbl_Store_GRNMaster.pk_GRNId INNER JOIN
							tbl_POP_POrderDetails INNER JOIN
							tbl_POP_POrder ON tbl_POP_POrderDetails.fk_POrder_POOrderId = tbl_POP_POrder.pk_POOrderId ON 
							tbl_Store_GRNDetails.fk_PODetsID = tbl_POP_POrderDetails.pk_POrderDetailsId 
					WHERE	tbl_Store_GRNDetails.fk_GRNMst_GRNId = @GRNId
							AND	tbl_Store_GRNDetails.AcceptedQty > 0
							AND tbl_Store_GRNDetails.pk_GRNDetailsId 
								NOT IN (SELECT PD.pk_GRNDetailsId FROM tbl_Acct_Invoice_PurchaseInvoice_Details AS PD)
				
					INSERT INTO  tbl_Acct_Invoice_PurchaseInvoice_Details
					(
						fk_Invoice_ID, fk_GRN_ID, fk_ProductID, HeatNos, POQty, AcceptedQty,
						Per, RatePerUOM, TotalAmount, pk_GRNDetailsId, ItemDesc
					)
					SELECT	@InvoiceId,	GRNId, fk_ProductID, HeatNos, POQty, AcceptedQty,
							Per, RatePerUOM, TotalAmount, pk_GRNDetailsId, ItemDesc     
					FROM	#temp_GRNDtl 
					WHERE	GRNId = @GRNId 

					IF EXISTS(SELECT pk_GRNDetailsId FROM tbl_Acct_Invoice_PurchaseInvoice_Details WHERE fk_GRN_ID = @GRNId AND fk_Invoice_ID = @InvoiceId)
					BEGIN
						UPDATE	tbl_Store_GRNMaster 
						SET		IsAcctPurchaseDone = 1
						WHERE	pk_GRNId = @GRNId
					END
					
				END

			END
			ELSE IF @InvoiceForType = 'Job Order'
			BEGIN

				IF EXISTS(SELECT pk_GRNDetailsId FROM tbl_Acct_Invoice_PurchaseInvoice_Details WHERE fk_Invoice_Id = @InvoiceId AND fk_JobOrder_GRNId = @GRNId)
				BEGIN
					SET @RetVal = 'Selected Job GRN allready added in selected purchase invoice'
				END
				ELSE IF EXISTS(SELECT pk_GRNDetailsId FROM tbl_Acct_Invoice_PurchaseInvoice_Details WHERE fk_Invoice_Id <> @InvoiceId AND fk_JobOrder_GRNId = @GRNId)
				BEGIN
					SET @RetVal = 'Selected Job GRN allready added in other purchase invoice'
				END
				ELSE
				BEGIN

					INSERT INTO #temp_GRNDtl
					(
						GRNId, fk_ProductID, HeatNos, POQty, AcceptedQty,
						Per, RatePerUOM, TotalAmount, pk_GRNDetailsId, ItemDesc
					)

					SELECT	Tbl_Sales_Job_DeliveryOrder_Details.fk_JOBDeliveryOrderId AS GRNId,
							Tbl_Sales_Job_DeliveryOrder_Details.fk_itemid AS fk_ProductID, 
							'' AS HeatNos, 
							Tbl_Sales_Job_DeliveryOrder_Details.JOBSOQty AS POQty, 
							Tbl_Sales_Job_DeliveryOrder_Details.Quantity AS AcceptedQty,
							1 AS Per,
							ROUND(tbl_Sales_SalesOrder_JOB_Dtl.UnitCost, @RoundOff_FC) AS RatePerUOM,
							ROUND(ROUND(tbl_Sales_SalesOrder_JOB_Dtl.UnitCost, @RoundOff_FC) * Tbl_Sales_Job_DeliveryOrder_Details.Quantity / 1, @RoundOff_FC) AS TotalAmount, 
							Tbl_Sales_Job_DeliveryOrder_Details.pk_JOBDeliveryOrderDetId AS pk_GRNDetailsId, 
							'' AS ItemDesc
					FROM	Tbl_Sales_Job_DeliveryOrder_Details 
							INNER JOIN Tbl_Sales_Job_DeliveryOrder_Master ON Tbl_Sales_Job_DeliveryOrder_Details.fk_JOBDeliveryOrderId = Tbl_Sales_Job_DeliveryOrder_Master.Pk_JOBDeliveryOrderId 
							INNER JOIN tbl_Sales_SalesOrder_JOB_Dtl 
							INNER JOIN tbl_Sales_SalesOrder_JOB_Mst ON tbl_Sales_SalesOrder_JOB_Dtl.fk_JOBMstId = tbl_Sales_SalesOrder_JOB_Mst.pk_JOBMstId ON 
							Tbl_Sales_Job_DeliveryOrder_Details.fk_JOBDtlId = tbl_Sales_SalesOrder_JOB_Dtl.pk_JOBDtlId
					WHERE	Tbl_Sales_Job_DeliveryOrder_Details.fk_JOBDeliveryOrderId = @GRNId
							AND	Tbl_Sales_Job_DeliveryOrder_Details.Quantity > 0
							AND Tbl_Sales_Job_DeliveryOrder_Details.pk_JOBDeliveryOrderDetId 
								NOT IN (SELECT PD.fk_JobOrder_GRNDetailsId FROM tbl_Acct_Invoice_PurchaseInvoice_Details AS PD)


					INSERT INTO  tbl_Acct_Invoice_PurchaseInvoice_Details
					(
						fk_Invoice_ID, fk_JobOrder_GRNId, fk_ProductID, HeatNos, POQty, AcceptedQty,
						Per, RatePerUOM, TotalAmount, fk_JobOrder_GRNDetailsId, ItemDesc
					)
					SELECT	@InvoiceId,	GRNId, fk_ProductID, HeatNos, POQty, AcceptedQty,
							Per, RatePerUOM, TotalAmount, pk_GRNDetailsId, ItemDesc  
					FROM	#temp_GRNDtl 
					WHERE	GRNId = @GRNId 

				END

			END

			DROP TABLE #temp_GRNDtl

		END  ---ADD

	END ---ADD / DELETE

	IF @InvoiceId <> 0 AND @RetVal = ''
	BEGIN
		EXEC USP_PurchaseInvoice_Master_UpdateAmount @InvoiceId, @LoginEmpId  
	END

	SELECT @RetVal AS RetVal

END




GO
/****** Object:  StoredProcedure [dbo].[Usp_PurchaseInvoice_Get_EditMode]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- [Usp_PurchaseInvoice_Get_EditMode] 33721, 'tbl_Acct_FYGOP1818'

ALTER PROC [dbo].[Usp_PurchaseInvoice_Get_EditMode] -- 33721, 'tbl_Acct_FYGOP1818'
	--declare
	@InvId as int,
	@TableName varchar(50)  
	--set @InvId=4978   
AS      
BEGIN ----

	SELECT	tbl_Acct_Invoice_PurchaseInvoice.*,
			(SELECT COUNT(Billno) 
			from tbl_Acct_Voucher_PaymentReceipt_Details 
			inner join tbl_Acct_Voucher_PaymentReceipt 
			on fk_Voucher_PR_Id=pk_Voucher_PR_Id where billno=InvoiceNo and TypePorR='P' 
			and  tbl_Acct_Voucher_PaymentReceipt.Fk_Branch_BranchID=tbl_Acct_Invoice_PurchaseInvoice.Fk_Branch_BranchID 
			and InvYearId=tbl_Acct_Invoice_PurchaseInvoice.Fk_YearId) as PREnt,

			(SELECT COUNT(tbl_Acct_Voucher_SPDC_Details.Billno) 
			from tbl_Acct_Voucher_SPDC_Details 
			inner join tbl_Acct_Voucher_SPCD on fk_Voucher_SPCDId=pk_Voucher_SPCD_Id 
			where tbl_Acct_Voucher_SPDC_Details.Billno=InvoiceNo and TypePorR='P' 
			and  tbl_Acct_Voucher_SPCD.Fk_Branch_BranchID=tbl_Acct_Invoice_PurchaseInvoice.Fk_Branch_BranchID 
			and InvYearId=tbl_Acct_Invoice_PurchaseInvoice.Fk_YearId) as JVEnt
	FROM	tbl_Acct_Invoice_PurchaseInvoice 
	WHERE	pk_Invoice_ID = @InvId 


	SELECT tbl_Store_GRNMaster.GRNNo + ' - ' + '' AS GRNNo,  
		CONVERT(varchar(50),dbo.tbl_Store_GRNMaster.GRNDate, 106) AS GRNDate, 
		dbo.tbl_POP_POrder.POOrderNo, CONVERT(varchar(50), tbl_POP_POrder.PODate , 106) AS PODate,
		dbo.tbl_Store_GRNMaster.pk_GRNId,1 as sel
	FROM  dbo.tbl_POP_POrder RIGHT OUTER JOIN dbo.tbl_Store_GRNMaster 
		ON dbo.tbl_POP_POrder.pk_POOrderId = dbo.tbl_POP_POrder.pk_POOrderId
		RIGHT OUTER JOIN dbo.tbl_Acct_Invoice_PurchaseInvoice_Details 
		ON dbo.tbl_Store_GRNMaster.pk_GRNId = dbo.tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID   
	WHERE fk_Invoice_ID=@InvId

	UNION

	SELECT GRN.GRNNo + ' - ' + '' AS GRNNo, CONVERT(varchar(50), GRN.GRNDate, 106) AS GRNDate, tbl_POP_POrder.POOrderNo, 
			CONVERT(varchar(50), tbl_POP_POrder.PODate , 106) AS PODate,GRN.pk_GRNId,0 as sel
	FROM tbl_Store_GRNMaster AS GRN LEFT OUTER JOIN tbl_POP_POrder 
		ON tbl_POP_POrder.pk_POOrderId = tbl_POP_POrder.pk_POOrderId
	WHERE ( GRN.fk_Party_SupplierId=(select fk_SupplierID from tbl_Acct_Invoice_PurchaseInvoice 
		where pk_Invoice_ID = @InvId) and GRN.IsAcctPurchaseDone=0) 
		and  (dbo.tbl_POP_POrder.IsPOCancelled <> 1)       
		and GRN.fk_branch_Branchid=(select Fk_Branch_BranchID from tbl_Acct_Invoice_PurchaseInvoice where pk_Invoice_ID = @InvId) 

      
	SELECT tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID AS fk_GRNMst_GRNId, 
		tbl_Store_GRNMaster.GRNNo, Product_Master.ReferenceCode,Product_Master.LongDesc, 
		UserList.Description1, tbl_Acct_Invoice_PurchaseInvoice_Details.POQty, 
		tbl_Acct_Invoice_PurchaseInvoice_Details.AcceptedQty,
		tbl_Acct_Invoice_PurchaseInvoice_Details.Per,
		tbl_Acct_Invoice_PurchaseInvoice_Details.RatePerUOM AS Rate, 
		tbl_Acct_Invoice_PurchaseInvoice_Details.TotalAmount AS Amount, 
		tbl_Acct_Invoice_PurchaseInvoice_Details.HeatNos, 
		tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID, 
		tbl_Acct_Invoice_PurchaseInvoice_Details.pk_Invoice_Details_ID, 
		tbl_Acct_Invoice_PurchaseInvoice_Details.fk_ProductID AS pk_ProductId, 
		tbl_POP_POrder.POOrderNo
	FROM tbl_POP_POrder RIGHT OUTER JOIN tbl_Store_GRNMaster 
		ON tbl_POP_POrder.pk_POOrderId = tbl_POP_POrder.pk_POOrderId
		RIGHT OUTER JOIN
		tbl_Acct_Invoice_PurchaseInvoice_Details INNER JOIN Product_Master 
		ON tbl_Acct_Invoice_PurchaseInvoice_Details.fk_ProductID = Product_Master.pk_ProductId 
		INNER JOIN UserList ON Product_Master.fk_UOMId1 = UserList.pk_UserListId ON 
		tbl_Store_GRNMaster.pk_GRNId = tbl_Acct_Invoice_PurchaseInvoice_Details.fk_GRN_ID   
	WHERE fk_Invoice_ID = @InvId      

  
------------------------------------- table 2 ------------------------------------------------

	DECLARE @TaxDetails AS VARCHAR(MAX)
	SET @TaxDetails = ''
	
	SELECT	CONVERT(VARCHAR(1000), '') AS LedgerName,
			fk_ChargesID,
			Rate,
			Add0OrLess1,
			(CASE Add0OrLess1 WHEN 0 THEN 1 ELSE -1 END) * ChargesValue_FC AS ChargesValue_FC,
			(CASE Add0OrLess1 WHEN 0 THEN 1 ELSE -1 END) * ChargesValue_BC AS ChargesValue_BC,
			(CASE Add0OrLess1 WHEN 0 THEN 1 ELSE -1 END) * ChargesValue AS ChargesValue
	INTO	#temp_TaxDetails
	FROM	tbl_Acct_Invoice_PurchaseInvoice_Charges
	WHERE	fk_Invoice_ID = @InvId

	EXEC('UPDATE	#temp_TaxDetails
		SET		LedgerName = ISNULL((SELECT Name FROM ' + @TableName + ' WHERE pk_AcctGrpId = fk_ChargesID), '''')')

	SET @TaxDetails = @TaxDetails 
					+ '<div class="DivStyle" id="Space" align="center" style="height: 100%;">VAT details'
					+ '<table  class="LabelNormal" border="1" width="100%" style="border-collapse: collapse;" cellpadding="4" >'
					+ '<tr style="text-align : center; font-weight:bold;vertical-align:top;background-color:#1e1e1e;color:white;">'
					+ '<td>' + 'Ledger Name' + '</td>'
					+ '<td>' + 'Rate %' + '</td>' 
					+ '<td>' + 'Tax Amount FC' + '</td>'
					+ '<td>' + 'Tax Amount BC' + '</td>'
					+ '</tr>'
	
	SET @TaxDetails = @TaxDetails 
					+ ISNULL(STUFF((SELECT '<tr style="vertical-align:top;">'
					+ '<td>' + ISNULL(CONVERT(VARCHAR(100), #temp_TaxDetails.LedgerName),'') + '</td>'
					+ '<td>' + ISNULL(CONVERT(VARCHAR(25), #temp_TaxDetails.Rate), '') + '</td>' 
					+ '<td>' + ISNULL(CONVERT(VARCHAR(25), #temp_TaxDetails.ChargesValue_FC), '') + '</td>'
					+ '<td>' + ISNULL(CONVERT(VARCHAR(25), #temp_TaxDetails.ChargesValue_BC), '') + '</td>'
					+ '</tr>'
				FROM    #temp_TaxDetails
				ORDER BY LedgerName
			FOR XML PATH(''), TYPE).value('.[1]', 'nvarchar(max)'), 1, 0, ''),'')
			
	SET @TaxDetails = @TaxDetails + '</table></div>'

	SELECT @TaxDetails AS TaxDetails


END ---






GO
/****** Object:  StoredProcedure [dbo].[USP_PurchaseInvoice_GetSuppWiseJobOrder]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--- USP_SalesInvoice_GetCustWiseJobOrder 3058,21,0

ALTER PROC [dbo].[USP_PurchaseInvoice_GetSuppWiseJobOrder]
	@SuppId INT,
	@BranchId INT,
	@PurchaseInvoiceId INT
AS
BEGIN --PROC

SELECT distinct fk_JobOrder_GRNId 
into #TempJobOrder
FROM tbl_Acct_Invoice_PurchaseInvoice_Details
inner join tbl_Acct_Invoice_PurchaseInvoice 
on tbl_Acct_Invoice_PurchaseInvoice.pk_Invoice_ID=tbl_Acct_Invoice_PurchaseInvoice_Details.fk_Invoice_ID
where isnull(tbl_Acct_Invoice_PurchaseInvoice.fk_JobOrderId,0)<>0 
and isnull(tbl_Acct_Invoice_PurchaseInvoice.IsCancel,0)=0
(
SELECT DISTINCT	tbl_Sales_SalesOrder_JOB_Mst.JOBNO AS JobOrderNO, 
			tbl_Sales_SalesOrder_JOB_Mst.pk_JOBMstId AS pk_JobOrderId
FROM	tbl_Sales_SalesOrder_JOB_Mst 
inner join Tbl_Sales_Job_DeliveryOrder_Master on Tbl_Sales_Job_DeliveryOrder_Master.fk_JobId=tbl_Sales_SalesOrder_JOB_Mst.pk_JOBMstId
WHERE	tbl_Sales_SalesOrder_JOB_Mst.Supplier = @SuppId 
		AND tbl_Sales_SalesOrder_JOB_Mst.Fk_Branch_BranchID = @BranchID
		AND Tbl_Sales_Job_DeliveryOrder_Master.Status IN (1)
		AND Tbl_Sales_Job_DeliveryOrder_Master.Pk_JOBDeliveryOrderId 
			NOT IN (SELECT fk_JobOrder_GRNId FROM #TempJobOrder)  

UNION 

SELECT DISTINCT	tbl_Sales_SalesOrder_JOB_Mst.JOBNO AS JobOrderNO, 
		tbl_Sales_SalesOrder_JOB_Mst.pk_JOBMstId AS pk_JobOrderId
FROM	tbl_Sales_SalesOrder_JOB_Mst 
WHERE	tbl_Sales_SalesOrder_JOB_Mst.pk_JOBMstId IN (SELECT fk_JobOrderId FROM tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @PurchaseInvoiceId)
)
ORDER BY tbl_Sales_SalesOrder_JOB_Mst.JOBNO

drop table #TempJobOrder


END --PROC





GO
/****** Object:  StoredProcedure [dbo].[Usp_PurchaseInvoice_Master_InsertUpDate]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[Usp_PurchaseInvoice_Master_InsertUpDate]        
	@InvoiceID int,        
	@InvoiceNo nvarchar(100),        
	@InvoiceDatedOn datetime,        
	@fk_SupplierID int,
	@fk_SupAddressID int,       
	@fk_POId  int,        
	@DefaultACLegID varchar(100),        
	@GrossAmount decimal(18, 4),        
	@AmtAfterCharge decimal(18, 4),        
	@DiscountAmt decimal(18, 4),        
	@DiscountType int,        
	@AmtAfterDisc decimal(18, 4),        
	@fk_CurrencyId int,        
	@CurrencyRate decimal(18, 4),        
	@NetAmount decimal(18, 4),        
	@AmtInWords nvarchar(200),        
	@Remark varchar (5000),        
	@fk_PreparedBy  int,        
	@fk_ApprovedBy  int,        
	@Fk_YearId  int,        
	@VoucherStyle  int,        
	@MaxPINVId  int,        
	@Fk_Branch_BranchID  int,        
	@DetailsXml varchar(max),        
	@ChargesXml varchar(max),
	@SupplierRef varchar(200),	
	@IsCancel  int,
	@CreditLimit int,
	@POID varchar(1000), 
	@txtTotalInvAddChg decimal(18, 4), 
	@PayTerms varchar(5000), 
	@DeliveryPeriod int, 
	@DeliveryTerms varchar(5000), 
	@OtherTerms varchar(5000),
	@RegSetupId INT,
	@ExchangeRate_BC Decimal(18, 4),
	@InvoiceForType VARCHAR(500), 
	@JobOrderId INT, 
	@VATAddId INT, 
	@SupplyDate VARCHAR(500),
	@ShipToAddId INT,
	@LoginEmpId int
AS         
BEGIN      
    DECLARE @BranchCurrencyId AS INT
	  
	DECLARE @RoundOff_FC AS INT    
    
	SET @RoundOff_FC = ISNULL((SELECT TOP 1 CurrencyDecimal FROM tbl_Master_Currency_Master WHERE pk_CurrencyId = @fk_CurrencyId), 2)

	IF	@InvoiceID = 0        
	BEGIN      
	
		INSERT INTO tbl_Acct_Invoice_PurchaseInvoice         
		(
			InvoiceNo,
			InvoiceDatedOn,
			fk_SupplierID,
			fk_SupAddressID,
			fk_POId,
			DefaultACLegID,
			GrossAmount,
			GrossAmount_FC,
			AmtAfterCharge,
			AmtAfterCharge_FC,        
			DiscountAmt,
			DiscountAmt_FC,
			DiscountType,
			AmtAfterDisc,
			AmtAfterDisc_FC,
			fk_CurrencyId,
			CurrencyRate,
			NetAmount,
			NetAmount_FC,
			NetAmtInWord,   
			Remark,
			IsCommonPurchase,
			IsCancel, 
			fk_PreparedBy,
			fk_ApprovedBy,
			Fk_YearId,
			VoucherStyle,
			MaxPIVNo,
			Fk_Branch_BranchID,
			SupplierRef,
			CreditLimit,     
			TotalGrossAmount,
			TotalGrossAmount_FC,
			AdditionalCharges,
			PayTerms,
			DeliveryPeriod,
			DeliveryTerms,
			OtherTerms,
			fk_RegSetup_ID,
			ExchangeRate_BC,
			InvoiceForType, 
			fk_JobOrderId, fk_VATAddId, SupplyDate, fk_ShipToAddId
		)      
		VALUES 
		(
			@InvoiceNo,
			@InvoiceDatedOn,
			@fk_SupplierID,
			@fk_SupAddressID,
			@fk_POId,
			@DefaultACLegID,
			CONVERT(DECIMAL(18, 4), (@AmtAfterDisc*@CurrencyRate)),  --For discount added on 8th mar 2012
			ROUND(@AmtAfterDisc, @RoundOff_FC),  --For discount added on 8th mar 2012
			CONVERT(DECIMAL(18, 4), (@AmtAfterCharge*@CurrencyRate)),
			ROUND(@AmtAfterCharge, @RoundOff_FC),        
			CONVERT(DECIMAL(18, 4), (@DiscountAmt*@CurrencyRate)),
			ROUND(@DiscountAmt, @RoundOff_FC),
			@DiscountType,
			CONVERT(DECIMAL(18, 4), (@AmtAfterDisc*@CurrencyRate)),
			ROUND(@AmtAfterDisc, @RoundOff_FC),
			@fk_CurrencyId,
			@CurrencyRate,
			CONVERT(DECIMAL(18, 4), (@AmtAfterDisc*@CurrencyRate)),
			ROUND(@AmtAfterDisc, @RoundOff_FC),
			@AmtInWords,    
			@Remark,
			0,
			@IsCancel, --0,
			@fk_PreparedBy,
			0,
			@Fk_YearId,
			@VoucherStyle,
			@MaxPINVId,
			@Fk_Branch_BranchID,
			@SupplierRef,
			@CreditLimit,    
			CONVERT(DECIMAL(18, 4),(@GrossAmount*@CurrencyRate)),	--For discount added on 8th mar 2012
			ROUND(@GrossAmount, @RoundOff_FC),
			0,
			@PayTerms,
			@DeliveryPeriod,
			@DeliveryTerms,
			@OtherTerms,
			@RegSetupId,
			@ExchangeRate_BC,
			@InvoiceForType, 
			@JobOrderId, @VATAddId, @SupplyDate,@ShipToAddId
		)   --For discount added on 8th mar 2012
     
		SET @InvoiceID = @@IDENTITY   

		INSERT INTO tbl_Acct_Invoice_PurchaseInvoice_Charges
		(
			fk_Invoice_ID,
			fk_ChargesID,
			TypePer0Amt1,
			Rate,
			OnWhich,
			RoundOff,
			Add0OrLess1,
			IsSeperatePosting,
			ChargesValue,
			NetAmount,
			Remarks,
			ChargesValue_FC,
			ChargesValue_BC
		)
		SELECT	@InvoiceID AS fk_Invoice_ID,
				fk_ChargesID,
				TypePer0Amt1,
				ROUND(Rate, @RoundOff_FC),
				OnWhich,
				RoundOff,
				Add0OrLess1,
				IsSeperatePosting,
				0 AS ChargesValue,
				0 AS NetAmount,
				0 AS Remarks,
				0 AS ChargesValue_FC,
				0 AS ChargesValue_BC
		FROM	tbl_Account_RegisterSetupCharges
		WHERE	fk_RegSetupId = @RegSetupId

		IF @InvoiceID <> 0
		BEGIN

			SELECT	@BranchCurrencyId = ISNULL(fk_BranchCurrencyId, 0)
			FROM	tbl_Master_Branch 
			WHERE	pk_BranchId = @Fk_Branch_BranchID
		
			UPDATE	tbl_Acct_Invoice_PurchaseInvoice
			SET		fk_BranchCurrencyId = @BranchCurrencyId,
					ExchangeRate_BC = @ExchangeRate_BC
			WHERE	pk_Invoice_ID = @InvoiceID
	
		END

	END        
	ELSE       
	BEGIN

		DECLARE @RegSetupId_Old AS INT

		SET @RegSetupId_Old = ISNULL((SELECT fk_RegSetup_ID FROM tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvoiceID), 0)

		UPDATE	tbl_Acct_Invoice_PurchaseInvoice        
		SET		fk_SupplierID = @fk_SupplierID,
				fk_POId = @fk_POId,
				fk_SupAddressID = @fk_SupAddressID,
				DefaultACLegID=@DefaultACLegID,	      
				Remark=@Remark,        
				fk_ApprovedBy=@fk_ApprovedBy, 
				fk_Emp_ChekedById=0, 
				TotalGrossAmount_FC=ROUND(@GrossAmount, @RoundOff_FC),
				TotalGrossAmount=CONVERT(DECIMAL(18, 4),(@GrossAmount*@CurrencyRate)),
				AmtAfterCharge_FC=ROUND(@AmtAfterCharge, @RoundOff_FC),
				AmtAfterCharge=CONVERT(DECIMAL(18, 4),(@AmtAfterCharge*@CurrencyRate)),        
				DiscountAmt_FC=ROUND(@DiscountAmt, @RoundOff_FC),
				DiscountAmt=CONVERT(DECIMAL(18, 4),(@DiscountAmt*@CurrencyRate)),
				DiscountType=@DiscountType,    
				AmtAfterDisc_FC=ROUND(@AmtAfterDisc, @RoundOff_FC),  
				AmtAfterDisc=CONVERT(DECIMAL(18, 4),(@AmtAfterDisc*@CurrencyRate)),      
				GrossAmount_FC=ROUND(@AmtAfterDisc, @RoundOff_FC),  
				GrossAmount=CONVERT(DECIMAL(18, 4),(@AmtAfterDisc*@CurrencyRate)),      
				NetAmount_FC=ROUND(@AmtAfterDisc, @RoundOff_FC),
				NetAmount=CONVERT(DECIMAL(18, 4),(@AmtAfterDisc*@CurrencyRate)),
				NetAmtInWord=@AmtInWords,
				fk_CurrencyId=@fk_CurrencyId,
				CurrencyRate=@CurrencyRate,
				SupplierRef=@SupplierRef,
				CreditLimit=@CreditLimit,
				IsCancel=@IsCancel,
				PayTerms=@PayTerms,      
				DeliveryPeriod=@DeliveryPeriod,      
				DeliveryTerms=@DeliveryTerms,      
				OtherTerms=@OtherTerms,
				fk_RegSetup_ID = @RegSetupId,
				ExchangeRate_BC = @ExchangeRate_BC,
				InvoiceForType = @InvoiceForType,
				fk_JobOrderId = @JobOrderId,
				fk_VATAddId = @VATAddId,
				SupplyDate = @SupplyDate,
				fk_ShipToAddId = @ShipToAddId
		WHERE	pk_Invoice_ID = @InvoiceID

		IF EXISTS(SELECT pk_AssignRoleID FROM tbl_Admin_Assign_Role WHERE EmpId = @LoginEmpId AND (Role1 IN (39, 41, 64) OR ROLE2 IN (39, 41, 64)))
		BEGIN
			UPDATE	tbl_Acct_Invoice_PurchaseInvoice  
			SET		InvoiceDatedOn = @InvoiceDatedOn
			WHERE	pk_Invoice_ID = @InvoiceID  
		END

		IF @RegSetupId_Old <> @RegSetupId
		BEGIN

			DELETE FROM	tbl_Acct_Invoice_PurchaseInvoice_Charges WHERE fk_Invoice_ID = @InvoiceID

			INSERT INTO tbl_Acct_Invoice_PurchaseInvoice_Charges
			(
				fk_Invoice_ID,
				fk_ChargesID,
				TypePer0Amt1,
				Rate,
				OnWhich,
				RoundOff,
				Add0OrLess1,
				IsSeperatePosting,
				ChargesValue,
				NetAmount,
				Remarks,
				ChargesValue_FC,
				ChargesValue_BC
			)
			SELECT	@InvoiceID AS fk_Invoice_ID,
					fk_ChargesID,
					TypePer0Amt1,
					ROUND(Rate, @RoundOff_FC),
					OnWhich,
					RoundOff,
					Add0OrLess1,
					IsSeperatePosting,
					0 AS ChargesValue,
					0 AS NetAmount,
					0 AS Remarks,
					0 AS ChargesValue_FC,
					0 AS ChargesValue_BC
			FROM	tbl_Account_RegisterSetupCharges
			WHERE	fk_RegSetupId = @RegSetupId

		END

		IF EXISTS(SELECT pk_Invoice_ID FROM tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvoiceID AND ISNULL(fk_BranchCurrencyId, 0) = 0)
		BEGIN

			SELECT	@Fk_Branch_BranchID = ISNULL(Fk_Branch_BranchID, 0)
			FROM	tbl_Acct_Invoice_PurchaseInvoice 
			WHERE	pk_Invoice_ID = @InvoiceID

			SELECT	@BranchCurrencyId = ISNULL(fk_BranchCurrencyId, 0)
			FROM	tbl_Master_Branch 
			WHERE	pk_BranchId = @Fk_Branch_BranchID
		
			UPDATE	tbl_Acct_Invoice_PurchaseInvoice
			SET		fk_BranchCurrencyId = @BranchCurrencyId
			WHERE	pk_Invoice_ID = @InvoiceID
	
		END

	END      

	SELECT @InvoiceID    


	--IF @DetailsXml <> ''        
	--BEGIN        
	--	EXEC Usp_PurchaseInvoice_Details_InsertUpDate @InvoiceID, @DetailsXml        
	--END        
    
	IF	@InvoiceID <> 0
	BEGIN
		EXEC USP_PurchaseInvoice_Master_UpdateAmount @InvoiceID, @LoginEmpId 
		
		UPDATE	tbl_Acct_Invoice_PurchaseInvoice  
		SET		IsChargesUpdated = 1
		WHERE	pk_Invoice_ID = @InvoiceID   
	END

END


















GO
/****** Object:  StoredProcedure [dbo].[USP_PurchaseInvoice_Master_UpdateAmount]    Script Date: 2/22/2022 10:00:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[USP_PurchaseInvoice_Master_UpdateAmount] 
	@InvoiceId INT,
	@LoginEmpId INT
AS
BEGIN --PROC
	------------- UPDATE AMOUNT -------------

	DECLARE @TotalGrossAmount_FC DECIMAL(18, 4) = 0
	DECLARE @TotalGrossAmount DECIMAL(18, 4) = 0
	DECLARE @AmtAfterCharge_FC DECIMAL(18, 4) = 0
	DECLARE @AmtAfterCharge DECIMAL(18, 4) = 0
	DECLARE @DiscountAmt_FC DECIMAL(18, 4) = 0
	DECLARE @DiscountAmt DECIMAL(18, 4) = 0
	DECLARE @DiscountAmount_FC DECIMAL(18, 4) = 0
	DECLARE @DiscountAmount DECIMAL(18, 4) = 0
	DECLARE @AmtAfterDisc_FC DECIMAL(18, 4) = 0
	DECLARE @AmtAfterDisc DECIMAL(18, 4) = 0
	DECLARE @GrossAmount_FC DECIMAL(18, 4) = 0
	DECLARE @GrossAmount DECIMAL(18, 4) = 0
	DECLARE @NetAmount_FC DECIMAL(18, 4) = 0
	DECLARE @NetAmount DECIMAL(18, 4) = 0
	DECLARE @NetAmtInWord VARCHAR(5000) = ''
	DECLARE @RoundOff_FC AS INT = 0
	DECLARE @RoundOff_BC AS INT = 0 
	DECLARE @RoundOff AS INT = 0
	DECLARE @AdditionalCharges as decimal(18, 4)=0
	DECLARE @TaxAmount_FC AS DECIMAL(18, 4)
	DECLARE	@ExchangeRate_BC AS DECIMAL(18, 4)
	DECLARE @RegSetupId AS INT

	--SET @RoundOff_FC = 2
	--SET @RoundOff = 2

	DECLARE	@CurrencyCode	NVARCHAR(100) = '' 
	DECLARE	@Denomination	NVARCHAR(100) = ''
	DECLARE	@CurrencyId INT = 0
	DECLARE	@ExchangeRate FLOAT = 0


	SELECT	@CurrencyId = fk_CurrencyId,
			@ExchangeRate = CurrencyRate,
			@DiscountAmt_FC = DiscountAmt_FC,
			@ExchangeRate_BC = ExchangeRate_BC,
			@RegSetupId = ISNULL(fk_RegSetup_ID, 0)
	FROM	tbl_Acct_Invoice_PurchaseInvoice 
	WHERE	pk_Invoice_ID = @InvoiceId

	---------------------------------------------------------------------------
	---------Auto Update: Branch Currency Exchange Rate------------------------
	---------------------------------------------------------------------------
	declare @DocumentDate date
	declare @DocumentCurrencyId int,@BranchId int,@BranchCurrencyId int

	set @DocumentDate=isnull((select InvoiceDatedOn from tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvoiceId),'')
	set @DocumentCurrencyId=isnull((select fk_CurrencyId from tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvoiceId),'')
	set @BranchId=isnull((select fk_Branch_BranchId from tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvoiceId),'')
	set @BranchCurrencyId=isnull((select fk_BranchCurrencyId from tbl_Acct_Invoice_PurchaseInvoice WHERE pk_Invoice_ID = @InvoiceId),'')

	if exists(select pkId from tbl_Master_Branch_Curr_Conversion where datediff(day,EffectiveDate,@DocumentDate)=0 and fk_CurrencyId=@DocumentCurrencyId 
							and BranchId=@BranchId and BranchCurrencyId=@BranchCurrencyId)
	begin
		set @ExchangeRate_BC=ISNULL((select Ex_Rate from tbl_Master_Branch_Curr_Conversion where datediff(day,EffectiveDate,@DocumentDate)=0 and fk_CurrencyId=@DocumentCurrencyId 
							and BranchId=@BranchId and BranchCurrencyId=@BranchCurrencyId),0)
		
		update tbl_Acct_Invoice_PurchaseInvoice 
		set ExchangeRate_BC=@ExchangeRate_BC
		WHERE pk_Invoice_ID = @InvoiceId

	end
	---------------------------------------------------------------------------
	---------------------------------------------------------------------------
	
	 SET @RoundOff_FC = ISNULL((SELECT TOP 1 CurrencyDecimal FROM tbl_Master_Currency_Master WHERE pk_CurrencyId = @CurrencyId), 2)  
	 SET @RoundOff_BC = ISNULL((SELECT TOP 1 CurrencyDecimal FROM tbl_Master_Currency_Master WHERE pk_CurrencyId = @BranchCurrencyId), 2)  
	 SET @RoundOff = ISNULL((SELECT TOP 1 CurrencyDecimal FROM tbl_Master_Currency_Master INNER JOIN tbl_Master_Branch ON pk_CurrencyId = fk_CurrencyId WHERE pk_BranchId = @BranchId), 2)  
  
	
	SET @AdditionalCharges = ROUND(ISNULL((SELECT SUM(InvCharges) FROM tbl_Acct_PI_AdditionalCharges WHERE fk_Invoice_ID = @InvoiceID), 0), @RoundOff_FC)
	SET @TotalGrossAmount_FC = ROUND(ISNULL((SELECT SUM(TotalAmount) FROM tbl_Acct_Invoice_PurchaseInvoice_Details WHERE fk_Invoice_ID = @InvoiceID), 0), @RoundOff_FC)

	SET @TotalGrossAmount_FC = ROUND(@TotalGrossAmount_FC, @RoundOff_FC)
	SET @AmtAfterCharge_FC = ROUND(@TotalGrossAmount_FC+ @AdditionalCharges, @RoundOff_FC)
	SET @DiscountAmt_FC = ROUND(@DiscountAmt_FC, @RoundOff_FC)
	SET @DiscountAmount_FC = ROUND(@DiscountAmt_FC, @RoundOff_FC)
	SET @AmtAfterDisc_FC = ROUND(@AmtAfterCharge_FC - @DiscountAmount_FC, @RoundOff_FC)
	SET @GrossAmount_FC = ROUND(@AmtAfterDisc_FC, @RoundOff_FC)
	

	UPDATE	tbl_Acct_Invoice_PurchaseInvoice_Charges
	SET		ChargesValue_FC = ROUND(Rate * @GrossAmount_FC / 100, @RoundOff_FC)
	WHERE	fk_Invoice_ID = @InvoiceID AND TypePer0Amt1 = 0

	UPDATE	tbl_Acct_Invoice_PurchaseInvoice_Charges
	SET		ChargesValue_FC = ROUND(Rate, @RoundOff_FC)
	WHERE	fk_Invoice_ID = @InvoiceID AND TypePer0Amt1 = 1

	UPDATE	tbl_Acct_Invoice_PurchaseInvoice_Charges
	SET		ChargesValue_BC = ROUND(ChargesValue_FC * @ExchangeRate_BC, @RoundOff_BC),
			ChargesValue = ROUND(ChargesValue_FC * @ExchangeRate, @RoundOff)
	WHERE	fk_Invoice_ID = @InvoiceID

	------------------------------------------ ADD TAX IN PURCHASE INVOICE AS PER MASTER --------------------------------------------

	SET @TaxAmount_FC = ISNULL((SELECT	SUM((CASE Add0OrLess1 WHEN 0 THEN ChargesValue_FC ELSE (-1) * ChargesValue_FC END))
								FROM	tbl_Acct_Invoice_PurchaseInvoice_Charges WHERE	fk_Invoice_ID = @InvoiceID), 0)

	SET @TaxAmount_FC = ROUND(@TaxAmount_FC, @RoundOff_FC)
	SET @NetAmount_FC = ROUND(@GrossAmount_FC + @TaxAmount_FC, @RoundOff_FC)

	-------------- AMOUNT IN FOREIGN CURRENCY ------------------------------
	
	UPDATE	tbl_Acct_Invoice_PurchaseInvoice
	SET		TotalGrossAmount_FC = ROUND(@TotalGrossAmount_FC, @RoundOff_FC),
			AdditionalCharges = ROUND(@AdditionalCharges, @RoundOff_FC), 
			AmtAfterCharge_FC = ROUND(@AmtAfterCharge_FC, @RoundOff_FC),
			DiscountAmt_FC = ROUND(@DiscountAmt_FC, @RoundOff_FC),
			DiscountAmount_FC = ROUND(@DiscountAmount_FC, @RoundOff_FC),
			AmtAfterDisc_FC = ROUND(@AmtAfterDisc_FC, @RoundOff_FC),
			GrossAmount_FC = ROUND(@GrossAmount_FC, @RoundOff_FC),
			TaxAmount_FC = ROUND(@TaxAmount_FC, @RoundOff_FC),
			NetAmount_FC = ROUND(@NetAmount_FC, @RoundOff_FC)
	WHERE	pk_Invoice_ID = @InvoiceId
	
	-------------- AMOUNT IN BRANCH CURRENCY ------------------------------

	UPDATE	tbl_Acct_Invoice_PurchaseInvoice  
	SET		GrossAmount_BC = ROUND((GrossAmount_FC * @ExchangeRate_BC), @RoundOff_BC), 
			TaxAmount_BC = ROUND((TaxAmount_FC * @ExchangeRate_BC), @RoundOff_BC), 
			NetAmount_BC = ROUND((NetAmount_FC * @ExchangeRate_BC), @RoundOff_BC)
	WHERE	pk_Invoice_ID = @InvoiceID
	
	-------------- AMOUNT IN WORDS ------------------------------

	SELECT	@CurrencyCode = CurrencyCode, 
			@Denomination = Denomination
	FROM	tbl_Master_Currency_Master 
	WHERE	pk_CurrencyId = @CurrencyId

	SELECT	@NetAmtInWord = DBO.UDF_AmountToWord(@NetAmount_FC, @CurrencyCode, @Denomination)
	
	-------------- AMOUNT IN AED ------------------------------

	UPDATE	tbl_Acct_Invoice_PurchaseInvoice
	SET		TotalGrossAmount = ROUND(TotalGrossAmount_FC * @ExchangeRate, @RoundOff),
			AmtAfterCharge = ROUND(AmtAfterCharge_FC * @ExchangeRate, @RoundOff),
			DiscountAmt = ROUND(DiscountAmt_FC * @ExchangeRate, @RoundOff),
			DiscountAmount = ROUND(DiscountAmount_FC * @ExchangeRate, @RoundOff),
			AmtAfterDisc = ROUND(AmtAfterDisc_FC * @ExchangeRate, @RoundOff),
			GrossAmount = ROUND(GrossAmount_FC * @ExchangeRate, @RoundOff),
			TaxAmount = ROUND(TaxAmount_FC * @ExchangeRate, @RoundOff), 
			NetAmount = ROUND(NetAmount_FC * @ExchangeRate, @RoundOff),
			NetAmtInWord = @NetAmtInWord,
			fk_ApprovedBy = 0,
			IsChargesUpdated = 0
	WHERE	pk_Invoice_ID = @InvoiceId


END --PROC







GO
