using System;

namespace Extensions
{
    public static class ActionExtensions
    {
        public static void NullableInvoke(this Action action)
        {
            if (action.IsNotNull())
            {
                action();
            }
        }

        public static void NullableInvoke<T>(this Action<T> action, T arg0)
        {
            if (action.IsNotNull())
            {
                action(arg0);
            }
        }

        public static void NullableInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (action.IsNotNull())
            {
                action(arg1, arg2);
            }
        }
    }
}
