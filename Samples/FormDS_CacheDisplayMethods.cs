[ExtensionOf(formDataSourceStr(PurchTable, PurchTable))]
internal final class BTZ_PurchTable_PurchTableDS_Perf_Extension
{
    public void init()
    {
        next init();

        if (!BTZ_Global::shouldDisableCustomCacheAddMethod())
        {
            this.cacheAddMethod(tableMethodStr(PurchTable, displayTotalAmount));
            this.cacheAddMethod(tableStaticMethodStr(PurchTable, displayChckForAttachment));
        }
    }

}