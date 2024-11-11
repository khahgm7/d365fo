[ExtensionOf(classStr(SalesTableType))]
final class SalesTableTYpe_Extension
{
    public void updating()
    {
        next updating();

        this.refreshSalesTableSystemFields();
    }

    private void refreshSalesTableSystemFields()
    {
        SalesTable localCommon = salesTable as SalesTable;
        SalesTable tmpBuffer;

        buf2buf(localCommon, tmpBuffer);

        localCommon.reread();

        buf2buf(tmpBuffer, localCommon);
    }
}