# Issue

When creating a new Sales quotation from All customers form, the Tax group is missing on the Sales quotation create form.

# Cause

A new flight is introduced.

```
ProjSalesQuotationInitializationFlight
```

This leads the code to below call stacks where the SalesQuotationTable.TaxGroup is initialised from ProjParameters instead even when it has been copied from CustTable.

```cs
>	Dynamics.AX.ApplicationSuite.159.netmodule!Dynamics.AX.Application.SalesQuotationTableType.`initFromProjTable(Dynamics.AX.Application.ProjTable _projTable) Line 229	X++
 	Dynamics.AX.ApplicationSuite.159.netmodule!Dynamics.AX.Application.SalesQuotationTable.`initProjQuotationFromProject(string _fromProjId, bool @_fromProjId_IsDefaultSet) Line 1284	X++
 	Dynamics.AX.ApplicationSuite.Forms.20.netmodule!Dynamics.AX.Application.Forms.SalesCreateQuotation.`run() Line 356	X++
 	Dynamics.AX.ApplicationSuite.159.netmodule!Dynamics.AX.Application.SalesQuotationTableForm.`create() Line 148	X++
 	Dynamics.AX.BitzerDevModel.Class.BTZ_SalesQuotationTableForm_Class_Extension.netmodule!Dynamics.AX.Application.BTZ_SalesQuotationTableForm_Class_Extension.create(Dynamics.AX.Application.SalesQuotationTableForm this) Line 13	X++
 	Dynamics.AX.ApplicationSuite.155.netmodule!Dynamics.AX.Application.SalesCreateQuotation.`create(Dynamics.AX.Application.QuotationType _quotationType, Microsoft.Dynamics.Ax.Xpp.Common _callerRecord, bool _initFromCustTable, bool @_initFromCustTable_IsDefaultSet) Line 28	X++
 	Dynamics.AX.ApplicationSuite.155.netmodule!Dynamics.AX.Application.SalesCreateQuotation.`main(Dynamics.AX.Application.Args _args) Line 81	X++
```

# Solution

## Solution 1: Disable the mentioned flight.

## Solution 2: Extension

```cs
internal final class BTZ_SalesQuotationTable_PrePostHandler
{
    private static boolean              isProjSalesQuotationInitializationFlightEnabled = true;
    private static boolean              resetAllFields = true;
    private static str                  fromProjIdArgsName = "_fromProjId";
    private static SalesQuotationTable  curCommon;
    private static SalesQuotationTable  origCommon;

    private static boolean shouldInvoke(XppPrePostArgs args)
    {
        SalesQuotationTable common = args.getThis() as SalesQuotationTable;

        boolean ret =
            smmParametersTable::find().BTZ_ResetFieldsSalesQuotationWhenCreatingFromCustomer
            && common.QuotationType != QuotationType::Project
            && isProjSalesQuotationInitializationFlightEnabled
            && strLen(args.getArg(fromProjIdArgsName)) == 0
        ;

        if(ret)
            curCommon = common;

        return ret;
    }

    [PreHandlerFor(tableStr(SalesQuotationTable), tableMethodStr(SalesQuotationTable, initProjQuotationFromProject))]
    public static void SalesQuotationTable_Pre_initProjQuotationFromProject(XppPrePostArgs args)
    {
        if(BTZ_SalesQuotationTable_PrePostHandler::shouldInvoke(args))
            BTZ_SalesQuotationTable_PrePostHandler::saveOrig();
    }

    [PostHandlerFor(tableStr(SalesQuotationTable), tableMethodStr(SalesQuotationTable, initProjQuotationFromProject))]
    public static void SalesQuotationTable_Post_initProjQuotationFromProject(XppPrePostArgs args)
    {
        if(BTZ_SalesQuotationTable_PrePostHandler::shouldInvoke(args))
            BTZ_SalesQuotationTable_PrePostHandler::fetchOrig();
    }

    private static void saveOrig()
    {
        buf2Buf(curCommon, origCommon);
    }

    private static void fetchOrig()
    {
        buf2Buf(origCommon, curCommon);
    }

}
```