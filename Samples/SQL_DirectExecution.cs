internal final class MiscFunc_Helper
{
    public const static SecurityPrivilegeName sqlAccess = privilegeStr(MiscFunc_SQL_Maintain);
    public const static SecurityPrivilegeName aotAccess = privilegeStr(MiscFunc_AOT_Maintain);
    public const static SecurityPrivilegeName userAccess = privilegeStr(MiscFunc_Users_Maintain);

    public static int timeNow()
    {
        return timeNow();
    }

    public static str timeConsumed(int _startTime, int _endTime = MiscFunc_Helper::timeNow())
    {
        str timeConsumed = timeConsumed(_startTime, _endTime);
        return timeConsumed ? timeConsumed : "Less than a second";
    }

    public static void showTimeConsumed(int _startTime, int _endTime = MiscFunc_Helper::timeNow())
    {
        Info(strFmt("Execution time: %1.", MiscFunc_Helper::timeConsumed(_startTime, _endTime)));
    }

    public static boolean canAccess(
        str _objectName
        , SecurityObjectType _type = SecurityObjectType::Privilege
        , UserId _userId = curUserId()
        )
    {
        return BTZ_Global_Security::canAccess(_objectName, _type, _userId);
    }

    public static str userHasNoAccess(UserId _userId = curUserId())
    {
        throw Error(BTZ_Global_Security::userHasNoAccess());
    }

    public static anytype sql(str _sql, MiscFunc_SQLType _type = MiscFunc_SQLType::Query)
    {
        anytype results;

        new SqlStatementExecutePermission(_sql).assert();
        Statement statement = new Connection().createStatement();

        try
        {
            if(_type == MiscFunc_SQLType::Query)
                results = statement.executeQuery(_sql);
            else
            {
                ttsbegin;

                results = statement.executeUpdate(_sql);

                if(_type == MiscFunc_SQLType::Execute)
                    ttscommit;
                else
                    ttsabort;
            }
        }
        catch(Exception::Error)
        {
            throw 
                error(strFmt(
                    "Error code: %1 | Error: %2."
                    , statement.getLastError()
                    , statement.getLastErrorText()
                ));
        }

        CodeAccessPermission::revertAssert();

        return results;
    }

    public static Map aot(str _object)
    {
        /* How this map looks like
            [
                {
                    "key": objectType - "Table"
                    , "value" : container - [content]
                }
                , {
                    "key": objectType - "Class"
                    , "value" : container - [content]
                }
                ,...
            ]
        */

        Map map = new Map(Types::String, Types::Container);

        TableId tableId = tableName2Id(_object);

        if(tableId)
        {
            map.insert("Table", MiscFunc_Helper::aotTable(tableId));
        }

        return map;
    }

    private static container aotTable(TableId _tableId)
    {
        /* How this contain looks like
            [
                isSystemTable
                , [
                    [isSystemField1, fieldName1]
                    , [isSystemField2, fieldName2]
                    , ...
                    , [isSystemFieldN, fieldNameN]
                ]
            ]
        */

        container con;
        
        SysDictTable dt = new SysDictTable(_tableId);

        // Is system table - 0/1
        con += dt.isSystemTable();

        // Add fields' details
        container fieldCon;
        {
            for(Counter i = 1; i <= dt.fieldCnt(); i++)
            {
                DictField df = dt.fieldObject(dt.fieldCnt2Id(i));
                
                container singleFieldCon;
                
                singleFieldCon += df.isSystem(); // Is system field - true/false
                singleFieldCon += df.name();

                fieldCon = conIns(fieldCon, conLen(fieldCon) + 1, singleFieldCon);
            }
        }

        con = conIns(con, 2, fieldCon);

        return con;
    }

    public static boolean confirm(str _message = "")
    {
        return Box::yesNo(_message, DialogButton::No) == DialogButton::Yes;
    }

}