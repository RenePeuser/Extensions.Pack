using System.Globalization;
using Argument.Check;

namespace Extensions.Pack
{
    public static class CultureInfoExtensions
    {
        public static IEnumerable<string> GetDesignators(this CultureInfo cultureInfo)
        {
            Throw.IfNull(cultureInfo);

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;

            if (dateTimeFormat.AMDesignator.IsNotNullOrEmpty())
            {
                yield return dateTimeFormat.AMDesignator;
            }

            if (dateTimeFormat.PMDesignator.IsNotNullOrEmpty())
            {
                yield return dateTimeFormat.PMDesignator;
            }
        }
    }
}
