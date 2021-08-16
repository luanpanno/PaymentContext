using Flunt.Validations;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            AddNotifications(
                new Contract<Name>()
                    .Requires()
                    .IsGreaterOrEqualsThan(FirstName, 1, "Name.FirstName", "Name must have at least 3 characters")
                    .IsGreaterOrEqualsThan(LastName, 1, "Name.LastName", "Last Name must have at least 3 characters")
                    .IsLowerOrEqualsThan(FirstName, 40, "Name.FirstName", "Name must have at most 40 characters")
            );
        }
    }
}