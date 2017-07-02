using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DotDiff.Tests
{
    [TestFixture]
    public class AttributesAuditingTests
    {
        [Test]
        public void Audit_Adds_Annotated_Properties_To_Collection()
        {
            var profile1 = new UserProfile
            {
                Email = "profile1@gmail.com",UserName = "User1"
            };
            var profile2 = new UserProfile
            {
                Email = "profile2@gmail.com",UserName = "User2"
            };

            var auditBuilder = new AuditBuilderMock<UserProfile>();
            auditBuilder.Audit(profile1, profile2);
            var pairs = auditBuilder.GetPairs();

            Assert.That(pairs.Count(), Is.EqualTo(1));//one property have [Audit] attribute
            Assert.AreEqual(pairs.First().Key, nameof(UserProfile.Email));
            Assert.AreEqual(pairs.First().OldValue, profile1.Email);
            Assert.AreEqual(pairs.First().NewValue, profile2.Email);
        }
    }

    public class AuditBuilderMock<T> : AuditBuilderBase<T> where T : class
    {
        public IEnumerable<AuditPair> GetPairs()
        {
            return AuditPairs;
        }
    }

    public class UserProfile
    {
        [Audit]
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
