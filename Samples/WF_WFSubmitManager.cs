/// <summary>
/// The BTZ_SalesPricing_WFSubmitManager menu item action event handler.
/// </summary>
public class BTZ_SalesPricing_WFSubmitManager
{
    public static void main(Args _args)
    {
        if (!_args.record() || !_args.caller())
        {
            throw error(Error::wrongUseOfFunction(funcName()));
        }

        // Opens the submit to workflow dialog.
        WorkflowSubmitDialog workflowSubmitDialog = WorkflowSubmitDialog::construct(_args.caller().getActiveWorkflowConfiguration());

        workflowSubmitDialog.run();

        SalesTable localSalesTable = _args.record() as SalesTable;

        if (workflowSubmitDialog.parmIsClosedOK())
        {
            try
            {
                RecId salesTableRecId = localSalesTable.RecId;

                BTZ_SalesPricing_Helper::updateSalesTableWFStatus(
                    salesTableRecId
                    , BTZ_SalesPricing_WFStatus::InReview
                );

                ttsbegin;

                Workflow::activateFromWorkflowType(
                    workFlowTypeStr(BTZ_SalesPricing_WF)
                    , salesTableRecId
                    , workflowSubmitDialog.parmWorkflowComment()
                    , NoYes::No
                );

                info(strFmt("@BTZ:SalesPricingApprovalSubmitInfo", localSalesTable.SalesId));

                ttscommit;
            }
            catch (exception::Error)
            {
                info(strFmt("@BTZ:SalesPricingApprovalSubmitFailure", localSalesTable.SalesId));
            }

            BTZ_SalesPricing_Helper::callerRefresh(_args.record().dataSource());
        }
    }

}