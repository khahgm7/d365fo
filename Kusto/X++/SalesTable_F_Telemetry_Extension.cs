[ExtensionOf(formStr(SalesTable))]
internal final class SalesTable_F_Telemetry_Extension
{
    public void init()
    {
        next init();

        this.logTelemetry();
    }

    private void logTelemetry()
    {
        if(SysIntParameters::find().CaptureCustomTraces)
        {
            SysApplicationInsightsEventTelemetry eventTelemetry = 
                SysApplicationInsightsEventTelemetry::newFromEventIdName(
                    TelemetryConst::newEventIdWithGuid(TelemetryConst::actionEnterForm)
                    , TelemetryConst::newNameByFormRun(this)
                );

            SysApplicationInsightsTelemetryLogger::instance().trackEvent(eventTelemetry);
        }
    }

}