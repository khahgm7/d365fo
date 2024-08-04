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

    List toDeletePersonal = new List(Types::String);
    List toDeleteShared = new List(Types::String);

    for (
        SysRecordTmpTemplate common =
            SysRecordTmpTemplate_ds.getFirst(true) ?
            SysRecordTmpTemplate_ds.getFirst(true) :
            SysRecordTmpTemplate_ds.cursor()
        ; common
        ; common = SysRecordTmpTemplate_ds.getNext()
    )
    {
        switch (common.Type)
        {
            case SysRecordTemplateType::Blank:
                Info("Blank template is retained.");
                continue;
            case SysRecordTemplateType::User:
                toDeletePersonal.addEnd(common.Description);
                continue;
            case SysRecordTemplateType::Company:
                toDeleteShared.addEnd(common.Description);
                continue;
        }
    }

    if (!this.confirmToDelete(toDeletePersonal.elements() + toDeleteShared.elements()))
        return;

    this.deleteTemplates(toDeletePersonal, SysRecordTemplateType::User);
    this.deleteTemplates(toDeleteShared, SysRecordTemplateType::Company);

    info(
        strFmt(
            "@BTZ:manageProductTemplateSucceeded"
            , toDeletePersonal.elements()
            , toDeleteShared.elements()
        )
    );
}

private boolean confirmToDelete(int _numberOfTemplates)
{
    return Box::yesNo(strFmt("@BTZ:ManageProductTemplateBoxWarning", _numberOfTemplates), DialogButton::No) == DialogButton::Yes;
}

private void deleteTemplates(List _listToDete, SysRecordTemplateType _type)
{
    // We use Set here because elements in SetIterator will be sorted ascending automatically
    Set setToRemovePos = this.getPositionsOfContainerToBeRemoved(_listToDete, _type);
    this.doDeleteTemplates(setToRemovePos, _type);
}

private Set getPositionsOfContainerToBeRemoved(List _list, SysRecordTemplateType _type)
{
    ListEnumerator le = _list.getEnumerator();
    Set ret = new Set(Types::Integer);

    while (le.moveNext())
    {
        int pos = this.findTemplateIdx(le.current(), _type);

        if (pos)
            ret.add(pos);
    }

    return ret;
}

protected int findTemplateIdx(SysRecordTemplateDescription _sysRecordTemplateDescription, SysRecordTemplateType _type)
{
    int i;
    SysRecordTemplateDescription tempDescription;
    boolean defaultRecord;
    SysRecordTemplateDetails det;
    container result;

    container targetContainerData = _type == SysRecordTemplateType::User ? userData : companyData;
    int endOfContainerData = _type == SysRecordTemplateType::User ? 0 : 1;

    for (i = conlen(targetContainerData); i > endOfContainerData; i--)
    {
    [tempDescription, defaultRecord, result, det] = conpeek(targetContainerData, i);

        if (strLRTrim(tempDescription) == _sysRecordTemplateDescription)
        {
            return i;
        }
    }

    return 0;
}

private void doDeleteTemplates(Set _set, SysRecordTemplateType _type)
{
    container idxToDelete = this.getIndexToDelete(_set);

    for (Counter i = 1; i <= conLen(idxToDelete); i++)
    {
        this.removeTemplateFromData(_type, conPeek(idxToDelete, i));
    }

    if (_type == SysRecordTemplateType::User)
        this.doDeletePersonalTemplates();
    if (_type == SysRecordTemplateType::Company)
        this.doDeleteSharedTemplates();
}

private void removeTemplateFromData(SysRecordTemplateType _type, int _pos)
{
    if (_type == SysRecordTemplateType::User)
        userData = conDel(userData, _pos, 1);
    else if (_type == SysRecordTemplateType::Company)
        companyData = conDel(companyData, _pos, 1);
}

private container getIndexToDelete(Set _set)
{
    SetIterator si = new SetIterator(_set);

    container idxAsc;

    while (si.more())
    {
        idxAsc += si.value();
        si.next();
    }

    // Reverse position set so the conDel will delete from the last to the first elements
    // This is to avoid messing up the order of the elements
    return conReverse(idxAsc);
}

private void doDeletePersonalTemplates()
{
    InventTable bufferInventTable;

    SysRecordTemplateStorageUser storageUser =
            SysRecordTemplateStorage::newCommon(
                bufferInventTable
                , SysRecordTemplateType::User
            ) as SysRecordTemplateStorageUser;

    storageUser.parmValueForManageProductTemplate(userData);
    xSysLastValue::saveLast(storageUser);
}

private void doDeleteSharedTemplates()
{
    ttsbegin;
    toModifyTemplateTable.data = companyData;
    toModifyTemplateTable.update();
    ttscommit;
}

[SysObsolete("All personal templates will be saved in one record in SysLastValue. Hence, single deletion will not be supported", false, 04\08\2024)]
private void detelePersonalTemplatesObsolete(List _toDeletePersonal)
{
    if (!_toDeletePersonal.elements() || !curUserId())
        return;

    ListEnumerator le = _toDeletePersonal.getEnumerator();
    SysLastValue sysLastValue;

    sysLastValue.skipAosValidation(true);
    sysLastValue.skipEvents(true);
    sysLastValue.skipDeleteActions(true);
    sysLastValue.skipDataMethods(true);
    sysLastValue.skipDataSourceValidateDelete(true);

    ttsbegin;

    while (le.moveNext())
    {
        if (!le.current())
            continue;

        str partDocument =
            strFmt(
                "<element type=\"str\">%1</element>"
                , le.current()
            );

        while select forupdate sysLastValue
                where sysLastValue.userId == curUserId()
                    && sysLastValue.designName == tableStr(InventTable)
                {
            if (strContains(sysLastValue.document, partDocument))
            {
                sysLastValue.doDelete();
                break;
            }
        }
    }

    ttscommit;
}

[DataSource]
class SysRecordTmpTemplate
{
    public void init()
    {
        super();

        SysRecordTmpTemplate tmpData = element.createTmpData();
        SysRecordTmpTemplate.setTmpData(tmpData);

        this.cacheAddDisplayMethods();
    }

    private void cacheAddDisplayMethods()
    {
        this.cacheAddMethod(tableMethodStr(SysRecordTmpTemplate, templateType));
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