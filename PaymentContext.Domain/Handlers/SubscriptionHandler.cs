using System;
using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable, IHandler<CreateBoletoSubscriptionCommand>, IHandler<CreatePayPalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            command.Validate();

            if (command.Invalid)
            {
                AddNotifications(command);

                return new CommandResult(false, "Cannot create subscription");
            }

            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "CPF already in use");

            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Email already in use");

            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode,
                command.BoletoNumber,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument,
                command.PayerDocumentType),
                address,
                email
            );

            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            AddNotifications(name, document, address, student, subscription, payment);

            if (Invalid) return new CommandResult(false, "Cannot create subscription");

            _repository.CreateSubscription(student);

            _emailService.Send(student.ToString(), student.Email.Address, "Welcome", "Subscription created");

            return new CommandResult(true, "Subscription created successfully");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "CPF already in use");

            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Email already in use");

            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument,
                command.PayerDocumentType),
                address,
                email
            );

            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            AddNotifications(name, document, address, student, subscription, payment);

            _repository.CreateSubscription(student);

            _emailService.Send(student.ToString(), student.Email.Address, "Welcome", "Subscription created");

            return new CommandResult(true, "Subscription created successfully");
        }
    }
}