using System.Collections.Generic;
using RapidCore.Xunit.Assertions;
using Xunit;
using Xunit.Sdk;

namespace UnitTests.Xunit.Assertions
{
    public class ContainsTheSameElementsTests
    {
        [Fact]
        public void ContainsTheSameElements_complains_if_a_is_null()
        {
            try
            {
                RapidCoreAssert.ContainsTheSameElements(null, new int[0]);
                RapidCoreAssert.Fail("we should have got a complaint about 'a' being null");
            }
            catch (NotNullException)
            {
                // yay
            }
        }


        [Fact]
        public void ContainsTheSameElements_complains_if_b_is_null()
        {
            try
            {
                RapidCoreAssert.ContainsTheSameElements(new int[0], null);
                RapidCoreAssert.Fail("we should have got a complaint about 'b' being null");
            }
            catch (NotNullException)
            {
                // yay
            }
        }


        [Fact]
        public void ContainsTheSameElements_failsIfCollectionsHaveDifferentNumberOfElements()
        {
            try
            {
                RapidCoreAssert.ContainsTheSameElements(new[] {1, 2}, new[] {1, 2, 3});
                RapidCoreAssert.Fail("collections have different number of elements");
            }
            catch (EqualException)
            {
                // yay
            }
        }

        [Fact]
        public void ContainsTheSameElements_failsIfCollectionsHaveDifferentElements()
        {
            try
            {
                var item1 = new Victim {Id = "1"};
                var item2 = new Victim {Id = "2"};
                var item3 = new Victim {Id = "3"};

                var a = new List<Victim> {item1, item2};
                var b = new List<Victim> {item1, item3};

                RapidCoreAssert.ContainsTheSameElements(a, b);
                RapidCoreAssert.Fail("collections have different elements");
            }
            catch (ContainsException)
            {
                // yay
            }
        }


        [Fact]
        public void ContainsTheSameElements_works_whenCollectionsHaveTheSameElements_inTheSameOrder()
        {
            var item1 = new Victim {Id = "1"};
            var item2 = new Victim {Id = "2"};

            var a = new List<Victim> {item1, item2};
            var b = new List<Victim> {item1, item2};

            RapidCoreAssert.ContainsTheSameElements(a, b);
        }


        [Fact]
        public void ContainsTheSameElements_works_whenCollectionsHaveTheSameElements_inDifferentOrder()
        {
            var item1 = new Victim {Id = "1"};
            var item2 = new Victim {Id = "2"};

            var a = new List<Victim> {item1, item2};
            var b = new List<Victim> {item2, item1};

            RapidCoreAssert.ContainsTheSameElements(a, b);
        }


        #region Victims

        private class Victim
        {
            public string Id { get; set; }
        }

        #endregion
    }
}