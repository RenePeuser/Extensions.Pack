using System;

namespace Extensions
{
    // Hint: With .Net 4.6 you dont need such extensions because
    // the null propagator operator ?. will handle excactly the same
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
