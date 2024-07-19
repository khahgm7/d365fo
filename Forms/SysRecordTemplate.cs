/// <summary>
/// Reference: SysRecordTemplateLookup.insertData()
/// Reference: SysRecordTemplate.lookupRecordTemplateValues()
/// </summary>
[Form]
public class BTZ_ManageProductTemplates extends FormRun
{
    container companyData;
    container userData;
    SysRecordTemplateTable toModifyTemplateTable;
    
    #define.BlankTemplateDescription("Blank")

    public SysRecordTmpTemplate createTmpData()
    {
        boolean allowBlank;
        SysRecordTmpTemplate tmp;

        TableId tableId = tableNum(InventTable);

        [companyData, allowBlank, userData] = this.getTemplateData();

        if (allowBlank)
        {
            tmp.Description = "@SYS24248";
            tmp.Type = SysRecordTemplateType::Blank;
            tmp.DefaultRecord = NoYes::No;
            tmp.insert();
        }
    
        SysRecordTmpTemplate::insertContainer(
            tableId
            , tmp
            , userData
            , SysRecordTemplateType::User
        );

        SysRecordTmpTemplate::insertContainer(
            tableId
            , tmp
            , condel(companyData, 1, 1)
            , SysRecordTemplateType::Company
        );
    
        sysRecordTmpTemplate.checkRecord(false);

        return tmp;
    }

    private container getTemplateData()
    {
        InventTable bufferInventTable;
        
        SysRecordTemplateStorageUser storageUser =
            SysRecordTemplateStorage::newCommon(
                bufferInventTable
                , SysRecordTemplateType::User
        ) as SysRecordTemplateStorageUser;

        SysRecordTemplateStorageCompany storageCompany =
            SysRecordTemplateStorage::newCommon(
                bufferInventTable
                , SysRecordTemplateType::Company
        ) as SysRecordTemplateStorageCompany;

        return [
            storageCompany.get()
            , storageCompany.allowBlank()
            , appl.ttsLevel() ? conNull() : storageUser.get()
        ];
    }

    private void closeButtonDelete()
    {
        toModifyTemplateTable = SysRecordTemplateTable::find(tableNum(InventTable), true);
        companyData = toModifyTemplateTable.data;

        List toDetele = new List(Types::String);

        for(
            SysRecordTmpTemplate common = 
                SysRecordTmpTemplate_ds.getFirst(true) ?
                SysRecordTmpTemplate_ds.getFirst(true) :
                SysRecordTmpTemplate_ds.cursor()
            ; common
            ; common = SysRecordTmpTemplate_ds.getNext()
        )
        {
            if(common.Description == #BlankTemplateDescription)
            {
                Info("Blank template is retained.");
                continue;
            }
            
            toDetele.addEnd(common.Description);
        }

        if(!this.confirmToDelete(toDetele.elements()))
            return;

        // We use Set here because elements in SetIterator will be sorted ascending automatically
        Set conToRemovePos = this.getPositionsOfContainerToBeRemoved(toDetele);

        this.deleteTemplates(conToRemovePos);
    }

    private boolean confirmToDelete(int _numberOfTemplates)
    {
        return Box::yesNo(strFmt("@BTZ:ManageProductTemplateBoxWarning", _numberOfTemplates), DialogButton::No) == DialogButton::Yes;
    }

    private Set getPositionsOfContainerToBeRemoved(List _list)
    {
        ListEnumerator le = _list.getEnumerator();
        Set ret = new Set(Types::Integer);

        while(le.moveNext())
        {
            int pos = this.findTemplateIdx(le.current());

            if(pos)
                ret.add(pos);
        }

        return ret;
    }

    protected int findTemplateIdx(SysRecordTemplateDescription _sysRecordTemplateDescription)
    {
        int i;
        SysRecordTemplateDescription tempDescription;
        boolean   defaultRecord;
        SysRecordTemplateDetails det;
        container result;

        for (i=conlen(companyData); i > 1; i--)
        {
            [tempDescription, defaultRecord, result, det] = conpeek(companyData, i);
            if (strLRTrim(tempDescription) == _sysRecordTemplateDescription)
            {
                return i;
            }
        }

        return 0;
    }

    private void deleteTemplates(Set _set)
    {
        container idxToDelete = this.getIndexToDelete(_set);

        for(Counter i = 1; i <= conLen(idxToDelete); i++)
        {
            companyData = conDel(companyData, conPeek(idxToDelete, i), 1);
        }

        ttsbegin;
        toModifyTemplateTable.data = companyData;
        toModifyTemplateTable.update();
        ttscommit;

        info(strFmt("@BTZ:manageProductTemplateSucceeded", conLen(idxToDelete)));
    }

    private container getIndexToDelete(Set _set)
    {
        SetIterator si = new SetIterator(_set);

        container idxAsc;

        while(si.more())
        {
            idxAsc += si.value();
            si.next();
        }

        // Reverse position set so the conDel will delete from the last to the first elements
        // This is to avoid messing up the order of the elements
        return conReverse(idxAsc);
    }

    [DataSource]
    class SysRecordTmpTemplate
    {
        public void init()
        {
            super();

            SysRecordTmpTemplate tmpData = element.createTmpData();
            SysRecordTmpTemplate.setTmpData(tmpData);
        }

    }

    [Control("CommandButton")]
    class ButtonDelete
    {
        public void clicked()
        {
            super();

            element.closeButtonDelete();
        }

    }

}