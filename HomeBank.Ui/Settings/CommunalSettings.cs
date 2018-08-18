using System;
using System.Configuration;
using System.Globalization;
using HomeBank.Domain.DomainModels.CommunalModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Presentation.Infrastructure;

namespace HomeBank.Ui.Settings
{
    internal sealed class CommunalSettings : ICommunalSettings
    {
        public CommunalTariffs CommunalTariffs
        {
            get
            {
                var electricalSupply = double.Parse(ConfigurationManager.AppSettings["ElectricalSupplyInRublesPerKilowatt"]);
                var couldWaterSupply = double.Parse(ConfigurationManager.AppSettings["CouldWaterSupplyInRublesPerCubicMeters"]);
                var hotWaterSupply = double.Parse(ConfigurationManager.AppSettings["HotWaterSupplyInRublesPerCubicMeters"]);
                
                return new CommunalTariffs(electricalSupply, couldWaterSupply, hotWaterSupply);
            }
        }
        
        public void Save(CommunalTariffs tariffs)
        {
            if (tariffs == null) throw new ArgumentNullException(nameof(tariffs));

            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            configuration.AppSettings.Settings["ElectricalSupplyInRublesPerKilowatt"].Value =
                tariffs.ElectricalSupplyInRublesPerKilowatt.ToString(CultureInfo.InvariantCulture);
            
            configuration.AppSettings.Settings["CouldWaterSupplyInRublesPerCubicMeters"].Value =
                tariffs.CouldWaterSupplyInRublesPerCubicMeters.ToString(CultureInfo.InvariantCulture);

            configuration.AppSettings.Settings["HotWaterSupplyInRublesPerCubicMeters"].Value =
                tariffs.HotWaterSupplyInRublesPerCubicMeters.ToString(CultureInfo.InvariantCulture);
            
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}