using Microsoft.Dynamics.AX.Framework.Utilities;
using Microsoft.Dynamics.ApplicationPlatform.Environment;
using System.Web;

internal final class BTZ_DeepLink_Service extends SysOperationServiceBase
{
    MenuItemName name;
    boolean regenerate;

    System.String hostURL;
    
    RecordInsertList insertList;
    TableId tableId;
    FieldId fieldId;
    str dsName;
    str fieldName;

    int noOfRecords = 0;

    const str noLink = "N/A";

    public void execute(BTZ_DeepLink_Contract _contract)
    {
        this.setContract(_contract);
        this.processMulti();
        //this.process();
    }

    private void processMulti()
    {
        ListEnumerator le = con2List(str2con(strReplace(name, " ", ""), ";")).getEnumerator();

        while(le.moveNext())
        {
            this.setCurrentName(le.current());
            this.process();
        }
    }

    private void setCurrentName(MenuItemName _name)
    {
        name = _name;
    }

    private void setContract(BTZ_DeepLink_Contract _contract)
    {
        name = _contract.parmName();
        regenerate = _contract.parmRegenerate();
    }

    private void process()
    {
        ttsbegin;

        if(regenerate)
            this.removeOldLinks();

        this.initRecordInsertList();
        this.populateNewLinks();

        ttscommit;

        this.showResult();
    }

    private void removeOldLinks()
    {
        BTZ_DeepLink common;

        delete_from common
            where common.RefMenuItemName == name;
    }

    private void initRecordInsertList()
    {
        insertList = new RecordInsertList(tableNum(BTZ_DeepLink), true, true, true, true, true);
    }

    private void populateNewLinks()
    {
        switch(name)
        {
            case menuItemDisplayStr(ProdTableListPage):
                {
                    tableId = tableNum(ProdTable);
                    fieldId = fieldNum(ProdTable, ProdId);
                    dsName = formDataSourceStr(ProdTableListPage, ProdTable);
                    fieldName = fieldStr(ProdTable, ProdId);
                }
                break;
            case menuItemDisplayStr(SalesTableListPage):
                {
                    tableId = tableNum(SalesTable);
                    fieldId = fieldNum(SalesTable, SalesId);
                    dsName = formDataSourceStr(SalesTableListPage, SalesTable);
                    fieldName = fieldStr(SalesTable, SalesId);
                }
                break;
            default:
                throw Error(Error::wrongUseOfFunction(funcName()));
        }

        this.addAndInsertNewLinks();
    }

    private void addAndInsertNewLinks()
    {
        Common sourceCommon = new DictTable(tableId).makeRecord();
        BTZ_DeepLink deepLinkTable;

        deepLinkTable.reread();
        
        while select sourceCommon
            notexists join deepLinkTable
                where deepLinkTable.RefMenuItemName == name
                && deepLinkTable.IdentifierNumber == sourceCommon.(fieldId)
        {
            UrlHelper.UrlGenerator generator = this.initURLGenerator();

            UrlHelper.RequestQueryParameterCollection requestQueryParameterCollection = 
                generator.RequestQueryParameterCollection;

            requestQueryParameterCollection.UpdateOrAddEntry(
                dsName
                , fieldName
                , sourceCommon.(fieldId)
            );

            BTZ_DeepLink common;
            {
                common.IdentifierNumber = sourceCommon.(fieldId);
                common.RefMenuItemName = name;
                common.URL = generator.GenerateFullUrl().AbsoluteUri;
                //common.URLDecoded = HttpUtility::UrlDecode(common.URL);
                common.URLDecoded = noLink;
            }
            
            insertList.add(common);
            noOfRecords++;
        }

        insertList.insertDatabase();
    }

    private UrlHelper.UrlGenerator initURLGenerator()
    {
        UrlHelper.UrlGenerator generator = new Microsoft.Dynamics.AX.Framework.Utilities.UrlHelper.UrlGenerator();

        generator.HostUrl = this.getHostURL();
        generator.Company = curext();
        generator.MenuItemName = name;
        generator.Partition = getCurrentPartition();

        generator.EncryptRequestQuery = true;

        return generator;
    }

    private System.String getHostURL()
    {
        if(hostURL)
            return hostURL;

        IApplicationEnvironment env = EnvironmentFactory::GetApplicationEnvironment();
        str currentUrl = env.Infrastructure.HostUrl;

        hostURL = new System.Uri(currentUrl).GetLeftPart(System.UriPartial::Authority);
 
        return hostURL;
    }

    private void showResult()
    {
        Info(
            strFmt(
                "@BTZ:DeepLinkPopulateResult"
                , noOfRecords
                , name
            )
        );
    }

}