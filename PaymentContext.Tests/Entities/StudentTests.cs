using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;

namespace PaymentContext.Tests.Entities
{
    [TestClass]
    public class StudentTests
    {
        [TestMethod]
        public void AddSubscription()
        {
            var subscription = new Subscription(null);
            var student = new Student("Luan", "Panno", "12345678912", "luanpanno@gmail.com");
            student.AddSubscription(subscription);
        }
    }
}
