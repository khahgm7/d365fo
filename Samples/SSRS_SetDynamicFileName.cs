/// <summary>
/// This is to extend the SSRSReportRunController instance.
/// Filename is set at exportViewerReport() - For example SrsReportPdfViewerControl::exportViewerReport()
/// str fileName = controller.parmDialogCaption() ? controller.parmDialogCaption() : controller.parmReportName();
/// </summary>
/// <param name = "_contract"></param>
private void setExportFileName(CSG_ProjectInvoiceReportRDPContract _contract)
{
    ProjInvoiceID projInvoiceID = _contract.getValue(#parameterProjInvoiceID);
    
    ProjInvoiceJour projInvoiceJour;

    select firstonly projInvoiceJour
        where projInvoiceJour.ProjInvoiceId == projInvoiceID;

    DirPartyName invoiceAccountName = CustTable::find(projInvoiceJour.InvoiceAccount).name();

    this.parmDialogCaption(
        strFmt(
            "%1-%2"
            , projInvoiceID
            , invoiceAccountName
        )
    );
}