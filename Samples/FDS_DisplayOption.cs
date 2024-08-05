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

[ExtensionOf(formDataSourceStr(ProdCalcTrans, ProdCalcTrans))]
internal final class BTZ_ProdCalcTrans_FormDS_ProdCalcTrans_Extension
{
    public void displayOption(Common _record, FormRowDisplayOption _options)
    {
        next displayOption(_record, _options);

        int highLight = WinAPI::RGB2int(255, 160, 122); // Lightsalmon

        ProdCalcTrans currentRecord = _record as ProdCalcTrans;

        if (!currentRecord.costPriceSum() || !currentRecord.salesPriceSum())
            _options.backColor(highLight);
    }

    private void setBackground()
    {
        int colour;

        colour = WinAPI::RGB2int(175, 238, 238); //Pale Turquoise
        {
            ProdHist_SalesAll.colorScheme(FormColorScheme::RGB);
            ProdHist_SalesPast.colorScheme(FormColorScheme::RGB);
            ProdHist_SalesRange1.colorScheme(FormColorScheme::RGB);
            ProdHist_SalesRange2.colorScheme(FormColorScheme::RGB);
            ProdHist_SalesRange3.colorScheme(FormColorScheme::RGB);
            ProdHist_SalesRange4.colorScheme(FormColorScheme::RGB);

            ProdHist_SalesAll.backgroundColor(colour);
            ProdHist_SalesPast.backgroundColor(colour);
            ProdHist_SalesRange1.backgroundColor(colour);
            ProdHist_SalesRange2.backgroundColor(colour);
            ProdHist_SalesRange3.backgroundColor(colour);
            ProdHist_SalesRange4.backgroundColor(colour);
        }
    }
}