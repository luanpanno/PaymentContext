using Flunt.Validations;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Address { get; set; }

        public Email(string address)
        {
            Address = address;

            AddNotifications(new Contract<Email>().Requires().IsEmail(Address, "Email.Address", "Invalid email"));
        }
    }
}