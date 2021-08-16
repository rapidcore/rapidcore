using System;
using System.Collections.Generic;
using RapidCore;
using RapidCore.Audit;
using RapidCore.DependencyInjection;
using RapidCore.Diffing;
using Xunit;

namespace UnitTests.Core.Audit
{
    public class AuditDifferTests
    {
        private readonly AuditDiffer auditDiffer;

        public AuditDifferTests()
        {
            auditDiffer = new AuditDiffer(new TheContainer(), new StateChangeFinder());
        }

        [Fact]
        public void GetAuditReadyDiff()
        {
            var oldState = new UserProfile
            {
                Email = "poor@victim.com",
                InternalThingy = "Debbie looks SO young",
                Password = "123456"
            };
            
            var newState = new UserProfile
            {
                Email = "ima@victim.com",
                InternalThingy = "Debbie looks SOOOOO young",
                Password = "987654321"
            };

            var actual = auditDiffer.GetAuditReadyDiff(oldState, newState);
            
            Assert.Equal(2, actual.Changes.Count);
            
            Assert.Equal("Email", actual.Changes[0].Breadcrumb);
            Assert.Equal("poor@victim.com", actual.Changes[0].OldValue);
            Assert.Equal("ima@victim.com", actual.Changes[0].NewValue);
            
            Assert.Equal("Password", actual.Changes[1].Breadcrumb);
            Assert.Equal("******", actual.Changes[1].OldValue);
            Assert.Equal("******", actual.Changes[1].NewValue);
        }
        
        [Fact]
        public void GetAuditReadyDiff_ignoresListWhenNotIncluded()
        {
            var oldState = new ThingWithIgnoredList
            {
                ThisShouldBeInTheLog = "Old string",
                TheseShouldNotBeInTheLog = new List<UnimportantThing>
                {
                    new UnimportantThing {SomeString = "Old unimportant string"}
                }
            };
            
            var newState = new ThingWithIgnoredList
            {
                ThisShouldBeInTheLog = "New string",
                TheseShouldNotBeInTheLog = new List<UnimportantThing>
                {
                    new UnimportantThing {SomeString = "New unimportant string"}
                }
            };
            
            var actual = auditDiffer.GetAuditReadyDiff(oldState, newState);
            
            Assert.Single(actual.Changes);
            
            Assert.Equal("ThisShouldBeInTheLog", actual.Changes[0].Breadcrumb);
            Assert.Equal("Old string", actual.Changes[0].OldValue);
            Assert.Equal("New string", actual.Changes[0].NewValue);
        }
        
        [Fact]
        public void GetAuditReadyDiff_ignoresComplexPropertyWhenNotIncluded()
        {
            var oldState = new ThingWithIgnoredComplex
            {
                ThisShouldBeInTheLog = "Old string",
                ThisShouldNotBeInTheLog = new UnimportantThing {SomeString = "Old unimportant string"}
            };
            
            var newState = new ThingWithIgnoredComplex
            {
                ThisShouldBeInTheLog = "New string",
                ThisShouldNotBeInTheLog = new UnimportantThing {SomeString = "New unimportant string"}
            };
            
            var actual = auditDiffer.GetAuditReadyDiff(oldState, newState);
            
            Assert.Single(actual.Changes);
            
            Assert.Equal("ThisShouldBeInTheLog", actual.Changes[0].Breadcrumb);
            Assert.Equal("Old string", actual.Changes[0].OldValue);
            Assert.Equal("New string", actual.Changes[0].NewValue);
        }

        [Fact]
        public void ThrowOnInvalidValueMasker()
        {
            var oldState = new InvalidMaskerVictim
            {
                Yo = "der"
            };

            var actual = Record.Exception(() => auditDiffer.GetAuditReadyDiff(oldState, null));

            Assert.IsType<ArgumentException>(actual);
            Assert.Equal("A ValueMasker must implement IAuditValueMasker, which UserProfile does not.", actual.Message);
        }
        
        [Fact]
        public void ThrowOnMissingValueMasker_containerThrows()
        {
            var oldState = new ThrowingMissingMaskerVictim
            {
                Yo = "der"
            };

            var actual = Record.Exception(() => auditDiffer.GetAuditReadyDiff(oldState, null));

            Assert.IsType<FailureToResolveException>(actual);
            Assert.Equal("The value masker \"ThrowInContainerValueMasker\" could not be resolved through the container adapter \"TheContainer\". Has it been registered in the container?", actual.Message);
        }
        
        [Fact]
        public void ThrowOnMissingValueMasker_containerReturnsNull()
        {
            var oldState = new NullingMissingMaskerVictim
            {
                Yo = "der"
            };

            var actual = Record.Exception(() => auditDiffer.GetAuditReadyDiff(oldState, null));

            Assert.IsType<FailureToResolveException>(actual);
            Assert.Equal("The value masker \"ReturnNullFromContainerValueMasker\" could not be resolved through the container adapter \"TheContainer\". Has it been registered in the container?", actual.Message);
        }

        #region Victims
        public class UnimportantThing
        {
            public string SomeString { get; set; }
        }
        
        public class ThingWithIgnoredComplex
        {
            public string ThisShouldBeInTheLog { get; set; }
            
            [Audit(Include = false)]
            public UnimportantThing ThisShouldNotBeInTheLog { get; set; }
        }
        
        public class ThingWithIgnoredList
        {
            public string ThisShouldBeInTheLog { get; set; }
            
            [Audit(Include = false)]
            public List<UnimportantThing> TheseShouldNotBeInTheLog { get; set; }
        }
        
        public class UserProfile
        {
            private string PrivateShouldBeIgnored { get; set; } = "ignore me";
            
            public string Email { get; set; }
            
            [Audit(ValueMasker = typeof(PasswordAuditValueMasker))]
            public string Password { get; set; }
            
            [Audit(Include = false)]
            public string InternalThingy { get; set; }
        }
        
        public class InvalidMaskerVictim
        {
            [Audit(ValueMasker = typeof(UserProfile))]
            public string Yo { get; set; } = "my man";
        }
        
        public class ThrowInContainerValueMasker : IAuditValueMasker
        {
            public string MaskValue(object value)
            {
                return "no";
            }
        }
        
        public class ThrowingMissingMaskerVictim
        {
            [Audit(ValueMasker = typeof(ThrowInContainerValueMasker))]
            public string Yo { get; set; } = "my man";
        }
        
        public class ReturnNullFromContainerValueMasker : IAuditValueMasker
        {
            public string MaskValue(object value)
            {
                return "no";
            }
        }
        
        public class NullingMissingMaskerVictim
        {
            [Audit(ValueMasker = typeof(ReturnNullFromContainerValueMasker))]
            public string Yo { get; set; } = "my man";
        }
        
        public class TheContainer : IRapidContainerAdapter
        {
            public T Resolve<T>()
            {
                throw new NotImplementedException();
            }

            public T Resolve<T>(string name)
            {
                throw new NotImplementedException();
            }

            public object Resolve(Type type)
            {
                if (type == typeof(ThrowInContainerValueMasker))
                {
                    throw new InvalidOperationException("just throw something");
                }

                if (type == typeof(ReturnNullFromContainerValueMasker))
                {
                    return null;
                }
                
                return Activator.CreateInstance(type);
            }
        }
        #endregion
    }
}