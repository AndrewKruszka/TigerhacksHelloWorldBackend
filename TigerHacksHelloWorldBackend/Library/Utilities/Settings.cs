using System;
using Library.Utilities.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Library.Utilities
{
    public class Settings : ISettings
    {
        public string DB { get; private set; }
        public int GridPrecision { get; private set; }

        public Settings(IConfiguration configuration)
        {
            DB = ConnectionStringBuilder.Build(configuration);
            GridPrecision = int.Parse(configuration.GetSection("AppSettings")["GridPrecision"]);
        }
    }
}

