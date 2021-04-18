using NUnit.Framework;
using System;
using System.Globalization;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class LevelOfParallelismAttribute : PropertyAttribute
    {
        /// <inheritdoc />
        /// <summary>
        /// Construct a LevelOfParallelismAttribute.
        /// </summary>
        /// <param name="level">The number of worker threads to be created by the framework.</param>
        public LevelOfParallelismAttribute(string level) : base(int.Parse(level, CultureInfo.CurrentCulture))
        {
        }
    }
}