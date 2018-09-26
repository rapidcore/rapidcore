using System;
using RapidCore.Audit;
using RapidCore.DependencyInjection;
using Xunit;

namespace RapidCore.UnitTests.Audit
{
    public class AuditDifferTests
    {
        private readonly AuditDiffer auditDiffer;

        public AuditDifferTests()
        {
            auditDiffer = new AuditDiffer(new TheContainer());
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

        #region Victims
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
                return Activator.CreateInstance(type);
            }
        }
        #endregion
    }
}