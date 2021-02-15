using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RapidCore.Xunit.Assertions
{
    public static partial class RapidCoreAssert
    {
        /// <summary>
        /// Verify that 2 collections contain the _same_ elements
        /// </summary>
        /// <param name="a">The first collection</param>
        /// <param name="b">The second collection</param>
        /// <typeparam name="T">The type of element in the collections</typeparam>
        public static void ContainsTheSameElements<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            Assert.NotNull(a);
            Assert.NotNull(b);
            
            Assert.Equal(a.Count(), b.Count());

            // using a loop and Assert.Contains instead
            // of Assert.All as this method gives more
            // useful hint of what is wrong with the collections.
            foreach (var inA in a)
            {
                Assert.Contains(inA, b);
            }
        }
    }
}