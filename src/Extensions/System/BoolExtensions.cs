using System;

namespace Extensions
{
    /// <summary>Provides extension methods for the <see cref="bool" /> type.</summary>
    public static class BoolExtensions
    {
        /// <summary>Converts a nullable boolean value into a value of <see cref="bool" /> type.</summary>
        /// <param name="value">The source nullable bool.</param>
        /// <returns><c>False</c> if the given value is <c>null</c>; otherwise the current boolean value.</returns>
        public static bool ToBool(this bool? value)
        {
            return value.HasValue && value.Value;
        }

        /// <summary>Converts a nullable <see cref="bool" /> to an <see cref="int" />.</summary>
        /// <param name="value">The source.</param>
        /// <returns><c>1</c> if <c>True</c>; otherwise <c>0</c>.</returns>
        public static int ToInt(this bool? value)
        {
            return value.ToBool().ToInt();
        }

        /// <summary>Invokes the specified action if <paramref name="value>" /> is <c>true</c>.</summary>
        /// <param name="value">The boolean value that decides whether the specified action will be invoked.</param>
        /// <param name="action">The action.</param>
        /// <returns><c>true</c> if the specified action is true; otherwise, <c>false</c>.</returns>
        public static bool IfTrueThen(this bool value, Action action)
        {
            Throw.IfNull(() => action);

            if (value)
            {
                action();
            }

            return value;
        }

        /// <summary>Invokes the specific action, if source is <c>false</c>.</summary>
        /// <param name="value">The source, which is checked for true.</param>
        /// <param name="action">The action.</param>
        /// <returns><c>true</c> if the specified action is false; otherwise, <c>false</c>.</returns>
        public static bool IfFalseThen(this bool value, Action action)
        {
            Throw.IfNull(() => action);

            if (!value)
            {
                action();
            }

            return value;
        }

        /// <summary>Negates the specified source.</summary>
        /// <param name="source">If set to <c>true</c> <c>false</c> will be returned and otherwise.</param>
        /// <returns>The negated value of the given bool.</returns>
        public static bool IsFalse(this bool source)
        {
            return !source;
        }

        /// <summary>Negates the specified source.</summary>
        /// <param name="source">If set to <c>true</c> <c>false</c> will be returned and otherwise.</param>
        /// <returns>The negated value of the given bool.</returns>
        public static bool Negate(this bool source)
        {
            return !source;
        }

        public static bool IsNot(this bool source)
        {
            return !source;
        }
    }
}
