
# InventSysSetup

## Error code

```
InventSysSetup failed to execute successfully, and the exception is System.AggregateException: One or more errors occurred. ---> Microsoft.Dynamics.Ax.Xpp.ClrErrorException: NullReferenceException ---> System.NullReferenceException: Object reference not set to an instance of an object.
```

## Solution

1. Log in to the machine
2. Open SSMS and connect to the SQL server
3. Target database: AxDB
4. Execute below script:
	
	``` sql
	declare
	  @Flightname nvarchar(200) = 'WHSReserveAvailQuantitiesStoredProceduresFlight_KillSwitch'
	;

	if not exists (select top(1) 'x' from SYSFLIGHTING where FLIGHTNAME = @Flightname)
		begin
			insert into SYSFLIGHTING(FLIGHTNAME, ENABLED, PARTITION, RECVERSION)
			values(@Flightname, 1, 5637144576, 1)
		end
	else	
		update SYSFLIGHTING
		set ENABLED = 1
		where FLIGHTNAME = @Flightname
		and PARTITION = 5637144576
	```

5. Open cmd with Administrator and run iisreset
6. Resume from LCS

## Error

```
08/14/2024 00:35:33: 08/14/2024 00:35:33: SysSetupInstaller: Script: InventSysSetup failed to execute successfully, and the exception is System.AggregateException: One or more errors occurred. ---> Microsoft.Dynamics.Ax.Xpp.ClrErrorException: NullReferenceException ---> System.NullReferenceException: Object reference not set to an instance of an object.
   at Dynamics.AX.Application.WhsOnHandSPHelper.sqlTypeNameForField(Int32 _tableId, Int32 _dimFieldId)
   at Dynamics.AX.Application.WhsOnHandSPHelper.buildCreateInventReserveMinValuesForDimIdSPStmt(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedure(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`syncDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.syncDBStoredProcedures()
   at Dynamics.AX.Application.InventSysSetup.`loadData()
   at Dynamics.AX.Application.InventSysSetup.loadData()
   at InventSysSetup::loadData(Object , Object[] , Boolean& )
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2001
   --- End of inner exception stack trace ---
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2056
   at Microsoft.Dynamics.AX.Framework.Syssetup.SysSetupTaskHelper.<>c__DisplayClass1_0.<ExecuteMethodInNewTask>b__0()
   at System.Threading.Tasks.Task`1.InnerInvoke()
   at System.Threading.Tasks.Task.Execute()
   --- End of inner exception stack trace ---
   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task.Wait(Int32 millisecondsTimeout, CancellationToken cancellationToken)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.ExecuteScriptWithTimeout(Int32 scriptId, DictClass syssetupDictClass, String className, Int32 timeout)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.<>c__DisplayClass25_0.<WriteAllData>b__0()
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupRetryHelper.Execute(Action executeMethod, String className)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.WriteAllData()
---> (Inner Exception #0) Microsoft.Dynamics.Ax.Xpp.ClrErrorException: NullReferenceException ---> System.NullReferenceException: Object reference not set to an instance of an object.
   at Dynamics.AX.Application.WhsOnHandSPHelper.sqlTypeNameForField(Int32 _tableId, Int32 _dimFieldId)
   at Dynamics.AX.Application.WhsOnHandSPHelper.buildCreateInventReserveMinValuesForDimIdSPStmt(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedure(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`syncDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.syncDBStoredProcedures()
   at Dynamics.AX.Application.InventSysSetup.`loadData()
   at Dynamics.AX.Application.InventSysSetup.loadData()
   at InventSysSetup::loadData(Object , Object[] , Boolean& )
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2001
   --- End of inner exception stack trace ---
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2056
   at Microsoft.Dynamics.AX.Framework.Syssetup.SysSetupTaskHelper.<>c__DisplayClass1_0.<ExecuteMethodInNewTask>b__0()
   at System.Threading.Tasks.Task`1.InnerInvoke()
   at System.Threading.Tasks.Task.Execute()<---
```

```
08/14/2024 00:35:33: 08/14/2024 00:35:33: SysSetupInstaller: ContinueOnError is Set to False for Script: InventSysSetup. Time elapsed: 0:00:00:16.1029438
08/14/2024 00:35:33: 08/14/2024 00:35:33: SysSetupInstaller: Executed 67 Scripts Successfully, so far failed at InventSysSetup script.
08/14/2024 00:35:33: New SyssetupInstaller exception System.AggregateException: One or more errors occurred. ---> Microsoft.Dynamics.Ax.Xpp.ClrErrorException: NullReferenceException ---> System.NullReferenceException: Object reference not set to an instance of an object.
   at Dynamics.AX.Application.WhsOnHandSPHelper.sqlTypeNameForField(Int32 _tableId, Int32 _dimFieldId)
   at Dynamics.AX.Application.WhsOnHandSPHelper.buildCreateInventReserveMinValuesForDimIdSPStmt(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedure(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`syncDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.syncDBStoredProcedures()
   at Dynamics.AX.Application.InventSysSetup.`loadData()
   at Dynamics.AX.Application.InventSysSetup.loadData()
   at InventSysSetup::loadData(Object , Object[] , Boolean& )
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2001
   --- End of inner exception stack trace ---
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2056
   at Microsoft.Dynamics.AX.Framework.Syssetup.SysSetupTaskHelper.<>c__DisplayClass1_0.<ExecuteMethodInNewTask>b__0()
   at System.Threading.Tasks.Task`1.InnerInvoke()
   at System.Threading.Tasks.Task.Execute()
   --- End of inner exception stack trace ---
   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task.Wait(Int32 millisecondsTimeout, CancellationToken cancellationToken)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.ExecuteScriptWithTimeout(Int32 scriptId, DictClass syssetupDictClass, String className, Int32 timeout)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.<>c__DisplayClass25_0.<WriteAllData>b__0()
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupRetryHelper.Execute(Action executeMethod, String className)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.WriteAllData()
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.loadAllData()
   at Microsoft.Dynamics.AX.Deployment.Setup.AppOperations.ExecuteSyssetupScripts(Action`2 logCallback, Predicate`1 scriptFilterCondition)
---> (Inner Exception #0) Microsoft.Dynamics.Ax.Xpp.ClrErrorException: NullReferenceException ---> System.NullReferenceException: Object reference not set to an instance of an object.
   at Dynamics.AX.Application.WhsOnHandSPHelper.sqlTypeNameForField(Int32 _tableId, Int32 _dimFieldId)
   at Dynamics.AX.Application.WhsOnHandSPHelper.buildCreateInventReserveMinValuesForDimIdSPStmt(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedure(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`syncDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.syncDBStoredProcedures()
   at Dynamics.AX.Application.InventSysSetup.`loadData()
   at Dynamics.AX.Application.InventSysSetup.loadData()
   at InventSysSetup::loadData(Object , Object[] , Boolean& )
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2001
   --- End of inner exception stack trace ---
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2056
   at Microsoft.Dynamics.AX.Framework.Syssetup.SysSetupTaskHelper.<>c__DisplayClass1_0.<ExecuteMethodInNewTask>b__0()
   at System.Threading.Tasks.Task`1.InnerInvoke()
   at System.Threading.Tasks.Task.Execute()<---
```

```
08/14/2024 00:35:35: 2024-08-14T00:35:35.5064125-06:00 post-sync custom action: '<RunFullSync>g__RaiseDbSyncEvent|24_8' finished. Time elapsed: 00:04:04.9593879.
08/14/2024 00:35:35: 2024-08-14T00:35:35.5689152-06:00 PostTableViewSyncActions finished. Time elapsed: 00:04:32.0643728.
08/14/2024 00:35:35: [DbSync: 7.0.30673] [Platform: 7.0.7279.51] [SourcePlatform: 7.0.7198.66] [DatabaseVersion: 7.0.7198.66]: Stopped DBSync monitoring
08/14/2024 00:35:35: Application configuration sync failed.	Microsoft.Dynamics.AX.Framework.Database.TableSyncException: Custom action threw exception(s), please investigate before synchronizing again: 'AggregateException:One or more errors occurred.
   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task.Wait(Int32 millisecondsTimeout, CancellationToken cancellationToken)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.ExecuteScriptWithTimeout(Int32 scriptId, DictClass syssetupDictClass, String className, Int32 timeout)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.<>c__DisplayClass25_0.<WriteAllData>b__0()
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupRetryHelper.Execute(Action executeMethod, String className)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.WriteAllData()
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.loadAllData()
   at Microsoft.Dynamics.AX.Deployment.Setup.AppOperations.ExecuteSyssetupScripts(Action`2 logCallback, Predicate`1 scriptFilterCondition)
   at Microsoft.Dynamics.AX.Deployment.Setup.AppOperations.RaiseOnDbsyncSyncApplEvent(Action`2 logCallback)
   at Microsoft.Dynamics.AX.Deployment.Setup.Program.<RunFullSync>g__RaiseDbSyncEvent|24_8(IMetadataProvider _)
   at Microsoft.Dynamics.AX.Framework.Database.Tools.LegacyCodepath.<>c__DisplayClass36_0.<NOTE_LeavingSynchronizer_CallStackAboveThisLineIsCustomCode>b__0()
   at Microsoft.Dynamics.AX.Framework.Database.Tools.LegacyCodepath.ExecuteWithinAOS(SyncOptions syncOptions, String sqlConnectionString, IMetadataProvider metadataProvider, Func`1 func, Action`1 errorHandler)
Inner exceptions:
ClrErrorException:NullReferenceException
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2056
   at Microsoft.Dynamics.AX.Framework.Syssetup.SysSetupTaskHelper.<>c__DisplayClass1_0.<ExecuteMethodInNewTask>b__0()
   at System.Threading.Tasks.Task`1.InnerInvoke()
   at System.Threading.Tasks.Task.Execute()
Inner exception:
NullReferenceException:Object reference not set to an instance of an object.
   at Dynamics.AX.Application.WhsOnHandSPHelper.sqlTypeNameForField(Int32 _tableId, Int32 _dimFieldId)
   at Dynamics.AX.Application.WhsOnHandSPHelper.buildCreateInventReserveMinValuesForDimIdSPStmt(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedure(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`syncDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.syncDBStoredProcedures()
   at Dynamics.AX.Application.InventSysSetup.`loadData()
   at Dynamics.AX.Application.InventSysSetup.loadData()
   at InventSysSetup::loadData(Object , Object[] , Boolean& )
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2001' ---> System.AggregateException: One or more errors occurred. ---> Microsoft.Dynamics.Ax.Xpp.ClrErrorException: NullReferenceException ---> System.NullReferenceException: Object reference not set to an instance of an object.
   at Dynamics.AX.Application.WhsOnHandSPHelper.sqlTypeNameForField(Int32 _tableId, Int32 _dimFieldId)
   at Dynamics.AX.Application.WhsOnHandSPHelper.buildCreateInventReserveMinValuesForDimIdSPStmt(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedure(Boolean _includeDelta, Boolean _includeCW)
   at Dynamics.AX.Application.WhsOnHandSPHelper.createInventReserveMinValuesForDimIdStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.createDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.`syncDBStoredProcedures()
   at Dynamics.AX.Application.WhsOnHandSPHelper.syncDBStoredProcedures()
   at Dynamics.AX.Application.InventSysSetup.`loadData()
   at Dynamics.AX.Application.InventSysSetup.loadData()
   at InventSysSetup::loadData(Object , Object[] , Boolean& )
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2001
   --- End of inner exception stack trace ---
   at Microsoft.Dynamics.Ax.Xpp.ReflectionCallHelper.MakeInstanceCall(Object instance, String MethodName, Object[] parameters) in D:\dbs\sh\l23t\0515_191343\cmd\d\Source\Kernel\xppil\XppSupport\ReflectionCallHelper.cs:line 2056
   at Microsoft.Dynamics.AX.Framework.Syssetup.SysSetupTaskHelper.<>c__DisplayClass1_0.<ExecuteMethodInNewTask>b__0()
   at System.Threading.Tasks.Task`1.InnerInvoke()
   at System.Threading.Tasks.Task.Execute()
   --- End of inner exception stack trace ---
   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task.Wait(Int32 millisecondsTimeout, CancellationToken cancellationToken)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.ExecuteScriptWithTimeout(Int32 scriptId, DictClass syssetupDictClass, String className, Int32 timeout)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.<>c__DisplayClass25_0.<WriteAllData>b__0()
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupRetryHelper.Execute(Action executeMethod, String className)
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.WriteAllData()
   at Microsoft.Dynamics.AX.Framework.Syssetup.SyssetupInstaller.loadAllData()
   at Microsoft.Dynamics.AX.Deployment.Setup.AppOperations.ExecuteSyssetupScripts(Action`2 logCallback, Predicate`1 scriptFilterCondition)
   at Microsoft.Dynamics.AX.Deployment.Setup.AppOperations.RaiseOnDbsyncSyncApplEvent(Action`2 logCallback)
   at Microsoft.Dynamics.AX.Deployment.Setup.Program.<RunFullSync>g__RaiseDbSyncEvent|24_8(IMetadataProvider _)
   at Microsoft.Dynamics.AX.Framework.Database.Tools.LegacyCodepath.<>c__DisplayClass36_0.<NOTE_LeavingSynchronizer_CallStackAboveThisLineIsCustomCode>b__0()
   at Microsoft.Dynamics.AX.Framework.Database.Tools.LegacyCodepath.ExecuteWithinAOS(SyncOptions syncOptions, String sqlConnectionString, IMetadataProvider metadataProvider, Func`1 func, Action`1 errorHandler)
   --- End of inner exception stack trace ---
   at Microsoft.Dynamics.AX.Framework.Database.Tools.LegacyCodepath.<>c.<NOTE_LeavingSynchronizer_CallStackAboveThisLineIsCustomCode>b__36_1(Tuple`2 result)
   at Microsoft.Dynamics.AX.Framework.Database.Tools.LegacyCodepath.ExecuteWithinAOS(SyncOptions syncOptions, String sqlConnectionString, IMetadataProvider metadataProvider, Func`1 func, Action`1 errorHandler)
   at Microsoft.Dynamics.AX.Framework.Database.Tools.LegacyCodepath.NOTE_LeavingSynchronizer_CallStackAboveThisLineIsCustomCode(SyncOptions syncOptions, String sqlConnectionString, IMetadataProvider metadataProvider, Action`1 a)
   at Microsoft.Dynamics.AX.Framework.Database.Tools.LegacyCodepath.RunCustomAction(SyncOptions syncOptions, String sqlConnectionString, IMetadataProvider metadataProvider, Action`1 a)
   at Microsoft.Dynamics.AX.Framework.Database.Tools.SyncEngine.PostTableSync()
   at Microsoft.Dynamics.AX.Framework.Database.Tools.SyncEngine.FullSync()
   at Microsoft.Dynamics.AX.Framework.Database.Tools.SyncEngine.RunSync()
   at Microsoft.Dynamics.AX.Framework.Database.Tools.SyncEngine.Run(String metadataDirectory, String sqlConnectionString, SyncOptions options)
08/14/2024 00:35:35: Sync step name: FullAll
08/14/2024 00:35:35: Start datetime: 2024-08-14 06:13:08
08/14/2024 00:35:35: End datetime: 2024-08-14 06:35:35
08/14/2024 00:35:35: Success: no
08/14/2024 00:35:35: PITR required for Rollback?: yes
08/14/2024 00:35:35: DBSync required for Rollback?: no
08/14/2024 00:35:35: Should retry?: yes
08/14/2024 00:35:35: Is custom action failure?: yes
08/14/2024 00:35:35: First-time sync evaluation in RunServicingSync: found FirstSyncSuccess value = 1.
08/14/2024 00:35:35: The operation failed.
```