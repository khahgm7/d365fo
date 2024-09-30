internal static void invokeBundleCPQPricing(Common _common)
{
    new ExecutePermission().assert();

    str methodName = "invokeBundleCPQPricing";
    DictTable dt = new DictTable(_common.tableId);

    if (tableHasMethod(dt, methodName))
        dt.callObject(methodName, _common);

    CodeAccessPermission::revertAssert();
}