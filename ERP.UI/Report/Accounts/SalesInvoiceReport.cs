using ERP.Models.Accounts;
using ERP.Services.Accounts.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace ERP.UI.Report.Accounts
{
    public class SalesInvoiceReport
    {
        //IWebHostEnvironment _webHostEnvironment;

        //public SalesInvoiceReport(IWebHostEnvironment webHostEnvironment)
        //{
        //    _webHostEnvironment = webHostEnvironment;
        //}

        Document _document;
        int _totalColumnItem = 8;
        PdfPTable _pdfPTableItem = new PdfPTable(8);
        PdfPCell _pdfPCellItem;

        Font _fontStyleReportTitle;
        Font _fontStyleTH;
        Font _fontStyleTR;
        Font _fontStyleLabel;
        Font _fontStyleText;
        MemoryStream _memoryStream = new MemoryStream();

        string _reportTitle = "Sales Invoice";
        string _webRootPath ;
        SalesInvoiceModel _salesInvoiceModel = new SalesInvoiceModel();
        IList<SalesInvoiceDetailModel> _salesInvoiceDetailModelList = new List<SalesInvoiceDetailModel>();
        IList<SalesInvoiceTaxModel> _salesInvoiceTaxModelList = new List<SalesInvoiceTaxModel>();
        IList<SalesInvoiceDetailTaxModel> _salesInvoiceDetailTaxModelList = new List<SalesInvoiceDetailTaxModel>();

        public byte[] PrepareReport(SalesInvoiceModel salesInvoiceModel, IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList,
                                    IList<SalesInvoiceTaxModel> salesInvoiceTaxModelList, 
                                    IList<SalesInvoiceDetailTaxModel> salesInvoiceDetailTaxModelList, string webRootPath)
        {
            _webRootPath = webRootPath;
            _salesInvoiceModel = salesInvoiceModel;
            _salesInvoiceDetailModelList = salesInvoiceDetailModelList;
            _salesInvoiceTaxModelList = salesInvoiceTaxModelList;
            _salesInvoiceDetailTaxModelList = salesInvoiceDetailTaxModelList;

            _document=new Document(PageSize.A4, 0f, 0f, 0f, 0f);

            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfPTableItem.WidthPercentage=100;
            _pdfPTableItem.HorizontalAlignment=Element.ALIGN_LEFT;

            _fontStyleReportTitle=FontFactory.GetFont("Arial", 14f, 1);

            _fontStyleLabel =FontFactory.GetFont("Arial", 8f, 1);
            _fontStyleText =FontFactory.GetFont("Arial", 8f, 0);
            _fontStyleTH=FontFactory.GetFont("Arial", 8f, 1);
            _fontStyleTR=FontFactory.GetFont("Arial", 8f, 0);

            PdfWriter.GetInstance(_document, _memoryStream);

            _document.Open();

            _pdfPTableItem.SetWidths(new float[] { 20f, 100f, 40f, 30f, 30f, 40f, 40f, 40f });

            this.ReportHeader();
            this.ReportBody();

            _pdfPTableItem.HeaderRows = 2;

            _document.Add(_pdfPTableItem);
            _document.Close();
            return _memoryStream.ToArray();
        }


        private PdfPTable CompanyDetails()
        {
            PdfPTable _pdfPTable = new PdfPTable(1);
            _pdfPTable.WidthPercentage=100;
            _pdfPTable.HorizontalAlignment=Element.ALIGN_LEFT;

            PdfPCell _pdfPCell;

            _pdfPCell = new PdfPCell(new Phrase(_salesInvoiceModel.CustomerLedgerName.ToUpper(), _fontStyleReportTitle));
            _pdfPCell.Colspan=1;
            _pdfPCell.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;

            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            string compnayAddress = _salesInvoiceModel.BillToAddress
                            + ", " +  _salesInvoiceModel.BillToAddressCountryName
                            + ", " +  _salesInvoiceModel.BillToAddressStateName
                            + ", " +  _salesInvoiceModel.BillToAddressCityName
                            + ", " +  _salesInvoiceModel.BillToAddressPhoneNo;

            _pdfPCell = new PdfPCell(new Phrase(HttpUtility.HtmlDecode(compnayAddress), _fontStyleText));
            _pdfPCell.Colspan=1;
            _pdfPCell.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;

            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            return _pdfPTable;
        }

        private PdfPTable PageHeaderDetails()
        {
            PdfPTable _pdfPTable = new PdfPTable(3);
            _pdfPTable.WidthPercentage=100;
            _pdfPTable.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPTable.SetWidths(new float[] { 10f, 10f, 10f});

            PdfPCell _pdfPCell;

            _pdfPCell = new PdfPCell(CompanyDetails());
            _pdfPCell.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_reportTitle.ToUpper(), _fontStyleReportTitle));
            _pdfPCell.HorizontalAlignment=Element.ALIGN_CENTER;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            string path = _webRootPath + "/images" + "/company1logo.png";

            Image logo = Image.GetInstance(path);
            logo.ScaleAbsolute(100f, 45f);

            _pdfPCell = new PdfPCell(logo);
            _pdfPCell.HorizontalAlignment=Element.ALIGN_RIGHT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();

            return _pdfPTable;
        }


        private PdfPTable ReportMaster()
        {
            PdfPTable _pdfPTable = new PdfPTable(4);
            _pdfPTable.WidthPercentage=100;
            _pdfPTable.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPTable.SetWidths(new float[] { 10f, 10f, 10f, 10f });

            PdfPCell _pdfPCell;

            _pdfPCell = new PdfPCell(new Phrase("Invoice No :", _fontStyleLabel));
            _pdfPCell.HorizontalAlignment=Element.ALIGN_RIGHT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_salesInvoiceModel.InvoiceNo, _fontStyleText));
            _pdfPCell.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Invoice Date :", _fontStyleLabel));
            _pdfPCell.HorizontalAlignment=Element.ALIGN_RIGHT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(((DateTime)_salesInvoiceModel.InvoiceDate).ToString("dd/MM/yyyyy"), _fontStyleText));
            _pdfPCell.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("Cust Ref No :", _fontStyleLabel));
            _pdfPCell.HorizontalAlignment=Element.ALIGN_RIGHT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_salesInvoiceModel.CustomerReferenceNo, _fontStyleText));
            _pdfPCell.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Cust Ref Date :", _fontStyleLabel));
            _pdfPCell.HorizontalAlignment=Element.ALIGN_RIGHT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(((DateTime)_salesInvoiceModel.CustomerReferenceDate).ToString("dd/MM/yyyyy"), _fontStyleText));
            _pdfPCell.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPCell.Border=0;
            _pdfPCell.ExtraParagraphSpace=0;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();

            return _pdfPTable;
        }


        private void ReportHeader()
        {
            //_pdfPCellItem = new PdfPCell(CompanyDetails());
            //_pdfPCellItem.Colspan=3;
            //_pdfPCellItem.HorizontalAlignment=Element.ALIGN_LEFT;
            //_pdfPCellItem.Border=0;
            //_pdfPCellItem.ExtraParagraphSpace=0;
            //_pdfPTableItem.AddCell(_pdfPCellItem);


            //_pdfPCellItem=new PdfPCell(new Phrase("Sales Invoice", _fontStyleReportTitle));
            //_pdfPCellItem.Colspan=3;
            //_pdfPCellItem.HorizontalAlignment=Element.ALIGN_CENTER;
            //_pdfPCellItem.Border=0;
            //_pdfPCellItem.ExtraParagraphSpace=0;
            //_pdfPTableItem.AddCell(_pdfPCellItem);


            //string path = _webRootPath + "/images" + "/company1logo.png";

            //Image logo = Image.GetInstance(path);
            //logo.ScaleAbsolute(100f, 45f);

            //_pdfPCellItem = new PdfPCell(logo);
            //_pdfPCellItem.Colspan=3;
            //_pdfPCellItem.HorizontalAlignment=Element.ALIGN_RIGHT;
            //_pdfPCellItem.Border=0;
            //_pdfPCellItem.ExtraParagraphSpace=0;
            //_pdfPTableItem.AddCell(_pdfPCellItem);
            //_pdfPTableItem.CompleteRow();


            _pdfPCellItem = new PdfPCell(PageHeaderDetails());
            _pdfPCellItem.Colspan=_totalColumnItem;
            _pdfPCellItem.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPCellItem.Border=0;
            _pdfPCellItem.ExtraParagraphSpace=0;
            _pdfPTableItem.AddCell(_pdfPCellItem);
            _pdfPTableItem.CompleteRow();


            _pdfPCellItem = new PdfPCell(ReportMaster());
            _pdfPCellItem.Colspan=_totalColumnItem;
            _pdfPCellItem.HorizontalAlignment=Element.ALIGN_LEFT;
            _pdfPCellItem.Border=0;
            _pdfPCellItem.ExtraParagraphSpace=0;
            _pdfPTableItem.AddCell(_pdfPCellItem);
            _pdfPTableItem.CompleteRow();


        }

        private void ReportBody()
        {
            _pdfPCellItem=new PdfPCell(new Phrase("Sr No", _fontStyleTH));
            _pdfPCellItem.VerticalAlignment=Element.ALIGN_CENTER;
            _pdfPCellItem.HorizontalAlignment=Element.ALIGN_CENTER;
            _pdfPCellItem.BackgroundColor=BaseColor.LIGHT_GRAY;
            _pdfPCellItem.ExtraParagraphSpace=0;
            _pdfPTableItem.AddCell(_pdfPCellItem);

            _pdfPCellItem=new PdfPCell(new Phrase("Description", _fontStyleTH));
            _pdfPCellItem.VerticalAlignment=Element.ALIGN_CENTER;
            _pdfPCellItem.HorizontalAlignment=Element.ALIGN_CENTER;
            _pdfPCellItem.BackgroundColor=BaseColor.LIGHT_GRAY;
            _pdfPCellItem.ExtraParagraphSpace=0;
            _pdfPTableItem.AddCell(_pdfPCellItem);

            _pdfPCellItem=new PdfPCell(new Phrase("Quantity", _fontStyleTH));
            _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
            _pdfPCellItem.HorizontalAlignment=Element.ALIGN_CENTER;
            _pdfPCellItem.BackgroundColor=BaseColor.LIGHT_GRAY;
            _pdfPCellItem.ExtraParagraphSpace=0;
            _pdfPTableItem.AddCell(_pdfPCellItem);

            _pdfPCellItem=new PdfPCell(new Phrase("UOM", _fontStyleTH));
            _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
            _pdfPCellItem.HorizontalAlignment=Element.ALIGN_CENTER;
            _pdfPCellItem.BackgroundColor=BaseColor.LIGHT_GRAY;
            _pdfPCellItem.ExtraParagraphSpace=0;
            _pdfPTableItem.AddCell(_pdfPCellItem);

            _pdfPCellItem=new PdfPCell(new Phrase("Per Unit", _fontStyleTH));
            _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
            _pdfPCellItem.HorizontalAlignment=Element.ALIGN_CENTER;
            _pdfPCellItem.BackgroundColor=BaseColor.LIGHT_GRAY;
            _pdfPCellItem.ExtraParagraphSpace=0;
            _pdfPTableItem.AddCell(_pdfPCellItem);

            _pdfPCellItem=new PdfPCell(new Phrase("Unit Price", _fontStyleTH));
            _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
            _pdfPCellItem.HorizontalAlignment=Element.ALIGN_CENTER;
            _pdfPCellItem.BackgroundColor=BaseColor.LIGHT_GRAY;
            _pdfPCellItem.ExtraParagraphSpace=0;
            _pdfPTableItem.AddCell(_pdfPCellItem);

            _pdfPCellItem=new PdfPCell(new Phrase("Gross Amount FC", _fontStyleTH));
            _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
            _pdfPCellItem.HorizontalAlignment=Element.ALIGN_CENTER;
            _pdfPCellItem.BackgroundColor=BaseColor.LIGHT_GRAY;
            _pdfPCellItem.ExtraParagraphSpace=0;
            _pdfPTableItem.AddCell(_pdfPCellItem);

            _pdfPCellItem=new PdfPCell(new Phrase("Gross Amount", _fontStyleTH));
            _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
            _pdfPCellItem.HorizontalAlignment=Element.ALIGN_CENTER;
            _pdfPCellItem.BackgroundColor=BaseColor.LIGHT_GRAY;
            _pdfPCellItem.ExtraParagraphSpace=0;
            _pdfPTableItem.AddCell(_pdfPCellItem);

            _pdfPTableItem.CompleteRow();

            //table body


            foreach (SalesInvoiceDetailModel _salesInvoiceDetailModel in _salesInvoiceDetailModelList)
            {
                _pdfPCellItem=new PdfPCell(new Phrase(_salesInvoiceDetailModel.SrNo.ToString(), _fontStyleTR));
                _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
                _pdfPCellItem.HorizontalAlignment=Element.ALIGN_RIGHT;
                _pdfPTableItem.AddCell(_pdfPCellItem);

                _pdfPCellItem=new PdfPCell(new Phrase(_salesInvoiceDetailModel.Description, _fontStyleTR));
                _pdfPCellItem.VerticalAlignment=Element.ALIGN_CENTER;
                _pdfPCellItem.HorizontalAlignment=Element.ALIGN_JUSTIFIED;
                _pdfPTableItem.AddCell(_pdfPCellItem);

                _pdfPCellItem=new PdfPCell(new Phrase(_salesInvoiceDetailModel.Quantity.ToString(), _fontStyleTR));
                _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
                _pdfPCellItem.HorizontalAlignment=Element.ALIGN_RIGHT;
                _pdfPTableItem.AddCell(_pdfPCellItem);

                _pdfPCellItem=new PdfPCell(new Phrase(_salesInvoiceDetailModel.UnitOfMeasurementName, _fontStyleTR));
                _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
                _pdfPCellItem.HorizontalAlignment=Element.ALIGN_CENTER;
                _pdfPTableItem.AddCell(_pdfPCellItem);

                _pdfPCellItem=new PdfPCell(new Phrase(_salesInvoiceDetailModel.PerUnit.ToString(), _fontStyleTR));
                _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
                _pdfPCellItem.HorizontalAlignment=Element.ALIGN_RIGHT;
                _pdfPTableItem.AddCell(_pdfPCellItem);

                _pdfPCellItem=new PdfPCell(new Phrase(_salesInvoiceDetailModel.UnitPrice.ToString(), _fontStyleTR));
                _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
                _pdfPCellItem.HorizontalAlignment=Element.ALIGN_RIGHT;
                _pdfPTableItem.AddCell(_pdfPCellItem);

                _pdfPCellItem=new PdfPCell(new Phrase(_salesInvoiceDetailModel.GrossAmountFc.ToString(), _fontStyleTR));
                _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
                _pdfPCellItem.HorizontalAlignment=Element.ALIGN_RIGHT;
                _pdfPTableItem.AddCell(_pdfPCellItem);

                _pdfPCellItem=new PdfPCell(new Phrase(_salesInvoiceDetailModel.GrossAmount.ToString(), _fontStyleTR));
                _pdfPCellItem.VerticalAlignment=Element.ALIGN_TOP;
                _pdfPCellItem.HorizontalAlignment=Element.ALIGN_RIGHT;
                _pdfPTableItem.AddCell(_pdfPCellItem);

                _pdfPTableItem.CompleteRow();
            }

        }

    }
}
