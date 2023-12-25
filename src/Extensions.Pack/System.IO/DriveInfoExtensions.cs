using Argument.Check;

namespace Extensions.Pack
{
    public static class DriveInfoExtensions
    {
        public static double AvailableFreeSpaceGb(this DriveInfo driveInfo)
        {
            Throw.IfNull(driveInfo);

            var divisor = Math.Pow(1024, 3);
            var result = driveInfo.TotalFreeSpace / divisor;
            return Math.Round(result, 1);
        }
    }
}
