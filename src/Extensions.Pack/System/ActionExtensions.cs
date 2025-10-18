using Argument.Check;

namespace Extensions.Pack
{
    public static class ActionExtensions
    {
        public static void NullableInvoke(this Action? action)
        {
            action?.Invoke();
        }

        public static void NullableInvoke<T>(this Action<T>? action,
                                             T arg0)
        {
            action?.Invoke(arg0);
        }

        public static void NullableInvoke<T1, T2>(this Action<T1, T2>? action,
                                                  T1 arg1,
                                                  T2 arg2)
        {
            action?.Invoke(arg1, arg2);
        }
    }
}
