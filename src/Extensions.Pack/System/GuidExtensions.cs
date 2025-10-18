namespace Extensions.Pack
{
    /// <summary>Class for extensions of <see cref="Guid" />.</summary>
    public static class GuidExtensions
    {
        /// <summary>Determines whether this <see cref="Guid" /> is empty.</summary>
        /// <param name="guidValue">The <see cref="Guid" /> which have to be checked.</param>
        /// <returns><c>True</c>if the <see cref="Guid" /> is empty; otherwise <c>false</c>.</returns>
        public static bool IsEmpty(this Guid guidValue)
        {
            return guidValue.EqualsTo(Guid.Empty);
        }

        /// <summary>Determines whether this <see cref="Guid" /> is empty.</summary>
        /// <param name="guidValue">The <see cref="Guid" /> which have to be checked.</param>
        /// <returns><c>True</c>if the <see cref="Guid" /> is not empty; otherwise <c>false</c>.</returns>
        public static bool IsNotEmpty(this Guid guidValue)
        {
            return guidValue.IsEmpty().Negate();
        }
    }
}
