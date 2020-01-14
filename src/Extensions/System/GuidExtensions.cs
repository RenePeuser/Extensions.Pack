using System;

namespace Extensions
{
    /// <summary>Class for extensions of <see cref="Guid" />.</summary>
    public static class TcGuidExtensions
    {
        /// <summary>Determines whether this <see cref="Guid" /> is empty.</summary>
        /// <param name="guid">The <see cref="Guid" /> which have to be checked.</param>
        /// <returns><c>True</c>if the <see cref="Guid" /> is empty; otherwise <c>false</c>.</returns>
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>Determines whether this <see cref="Guid" /> is empty.</summary>
        /// <param name="guid">The <see cref="Guid" /> which have to be checked.</param>
        /// <returns><c>True</c>if the <see cref="Guid" /> is not empty; otherwise <c>false</c>.</returns>
        public static bool IsNotEmpty(this Guid guid)
        {
            return guid.IsEmpty().Negate();
        }
    }
}
