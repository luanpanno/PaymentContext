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

            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(FirstName, 3, "Name.FirstName", "Name must contains at least 3 characters")
                .HasMinLen(LastName, 3, "Name.LastName", "Last Name must contains at least 3 characters")
                .HasMaxLen(FirstName, 40, "Name.FirstName", "Name must contains at most 3 characters")
            );
        }

        public override string ToString() => $"{FirstName} {LastName}";
    }
}