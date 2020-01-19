using System;
using System.IO;

namespace Extensions.Pack
{
    public static class DriveInfoExtensions
    {
        public static double AvailableFreeSpaceGb(this DriveInfo driveInfo, int decimals = 1)
        {
            Throw.IfNull(() => driveInfo);

            var divisor = Math.Pow(1024, 3);
            var result = driveInfo.TotalFreeSpace / divisor;
            return Math.Round(result, 1);
        }
    }
}
