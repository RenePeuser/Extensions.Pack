using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Pack.Exceptions
{
    public class MissingSettingsException<TSettings> : Exception where TSettings : class
    {
        public MissingSettingsException() : base($"The setting: '{typeof(TSettings).Name}' is missing. Please check your specific appsettings.json or your environment variables.")
        {
        }
    }
}
