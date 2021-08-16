using System;

namespace PaymentContext.Domain.Entities
{
    public class PayPalPayment : Payment
    {
        public string TransactionCode { get; private set; }

        public PayPalPayment(
            string transactionCode,
            DateTime paidDate,
            DateTime expireDate,
            decimal total,
            decimal totalPaid,
            string payer,
            string document,
            string address,
            string email
        ) : base(paidDate, expireDate, total, totalPaid, payer, document, address, email)
        {
            TransactionCode = transactionCode;
        }
    }
}