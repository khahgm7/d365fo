public void displayOption(Common _record, FormRowDisplayOption _options)
{
    WHSParameters parms = element.whsParms();

    if (parms.BTZ_CycleCountUseColour == NoYes::Yes)
    {
        int thresholdCheck = parms.BTZ_CycleCountThreshold;
        int highLight = WinAPI::RGB2int(255, 160, 122); // Lightsalmon
        int zero = WinAPI::RGB2int(224, 255, 255); // Light cyan

# View record = _record;

        if (abs(record.BTZ_CostAmount) >= thresholdCheck)
        {
            _options.backColor(highLight);
        }

        if (record.BTZ_CostAmount == 0)
        {
            _options.backColor(zero);
        }

    }

    super(_record, _options);
}