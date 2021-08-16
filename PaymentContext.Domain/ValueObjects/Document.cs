using Flunt.Validations;
using PaymentContext.Domain.Enums;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Document : ValueObject
    {
        public string Number { get; private set; }
        public EDocumentType Type { get; private set; }

        public Document(string number, EDocumentType type)
        {
            Number = number;
            Type = type;

            AddNotifications(
                new Contract<Document>()
                    .Requires()
                    .IsTrue(Validate(), "Document.Number", "Invalid document")
            );
        }

        private bool Validate()
        {
            return
                (Type == EDocumentType.CNPJ && Number.Length == 14) ||
                (Type == EDocumentType.CPF && Number.Length == 11);
        }
    }
}