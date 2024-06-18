using System.Diagnostics;
using System.Reflection;

namespace PulseCore.Contracts.Extensions
{
    public static class AssemblyHelper
    {
        public static bool IsCompiledInDebug(this Assembly assembly)
        {
            return assembly.GetCustomAttribute<DebuggableAttribute>()?.IsJITTrackingEnabled ?? false;
        }
    }
}
