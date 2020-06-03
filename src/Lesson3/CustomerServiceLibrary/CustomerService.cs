using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CustomerServiceLibrary
{
    public interface IWorkstationSettings
    {
        int? WorkstationId { get; set; }
    }
    public interface ICustomerRepository
    {
        void Save(Customer customer);
        void SaveExtended(Customer customer);
        string LocalTimeZone { get; set; }

        event EventHandler<NotifyEventArgs> Notify;
    }
    public class NotifyEventArgs : EventArgs
    {
        public NotifyEventArgs(string customerName)
        {
            CustomerName = customerName;
        }

        public string CustomerName { get; set; }

    }
    public interface INameBuilder
    {
        string From(string firstName, string lastName);
    }
    public interface ICustomerStatusFactory
    {
        StatusLevel CreateFrom(CustomerDTO customer);
    }

    public enum StatusLevel
    {
        Bronze, Silver, Gold
    }

    public class Customer
    {
        public Customer(string fn, string ln)
        {

        }

        public Customer(string fn)
        {

        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Email { get; set; }

        public int? WorkstationId { get; internal set; }

        public StatusLevel StatusLevel { get; set; }
    }

    public class CustomerDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public StatusLevel DesiredLevel { get; set; }
    }

    public class CustomerService
    {
        private readonly ICustomerRepository _repositry;
        private readonly IEmailBuilder _emailBuilder;
        private readonly IIdFactory _idFactory;
        private readonly INameBuilder _nameBuilder;
        private readonly ICustomerStatusFactory _statusFactory;
        IWorkstationSettings _workstationSettings;

        private IMailingAddressFactory _mailingAddressFactory;

        public CustomerService(ICustomerRepository repository, IEmailBuilder emailBuilder, 
            IIdFactory idFactory, IMailingAddressFactory mailingAddressFactory, INameBuilder nameBuilder, ICustomerStatusFactory statusFactory,
            IWorkstationSettings workstationSettings)
        {
            _repositry = repository;
            _emailBuilder = emailBuilder;
            _idFactory = idFactory;
            _mailingAddressFactory = mailingAddressFactory;
            _nameBuilder = nameBuilder;
            _statusFactory = statusFactory;
            _workstationSettings = workstationSettings;

            // установка обработчика события
            _repositry.Notify += (o, e) => _mailingAddressFactory.CreatenewMessage(e.CustomerName);
        }

        public void Create(CustomerDTO customerDTO)
        {
            Customer customer = new Customer(customerDTO.FirstName, customerDTO.LastName);

            // При тестирование необходимо определить возвращаемое значение для _emailBuilder.From
            customer.Email = _emailBuilder.From(customerDTO);

            if (customer.Email == null)
            {
                throw new ApplicationException("Email не может быть пустым.");
            }

            _repositry.Save(customer);
        }

        public void CreateWithId(IEnumerable<CustomerDTO> customers)
        {
            foreach (var currentCustomer in customers)
            {
                Customer newCustomer = new Customer(currentCustomer.FirstName, currentCustomer.LastName);
                newCustomer.Id = _idFactory.Create();
                _repositry.Save(newCustomer);
            }
        }

        public void CreateOutParam(CustomerDTO customerDTO)
        {
            Customer customer = new Customer(customerDTO.FirstName, customerDTO.LastName);

            Address mailingAddress;

            // Для того чтобы проверить корректность работы этого метода, mock объект должен подставить значение в качестве out параметра
            _mailingAddressFactory.TryParse(customerDTO.Email, out mailingAddress);

            if (mailingAddress == null)
            {
                throw new ApplicationException();
            }

            customer.Email = mailingAddress;
            _repositry.Save(customer);

        }

        public void CreateTrackingArg(CustomerDTO customer)
        {
            string fullName = _nameBuilder.From(customer.FirstName, customer.LastName);

            Customer newCustomer = new Customer(fullName);

            _repositry.Save(newCustomer);
        }

        public void CreateStatus(CustomerDTO customer)
        {
            Customer newCustomer = new Customer("")
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };

            newCustomer.StatusLevel = _statusFactory.CreateFrom(customer);

            if (newCustomer.StatusLevel == StatusLevel.Gold)
            {
                _repositry.SaveExtended(newCustomer);
            }
            else
            {
                _repositry.Save(newCustomer);
            }
        }

        public void CreateExc(CustomerDTO customer)
        {
            try
            {
                Customer newCustomer = new Customer("");
                newCustomer.FirstName = customer.FirstName;
                newCustomer.LastName = customer.LastName;
                newCustomer.Email = _mailingAddressFactory.CreateFrom(customer);

                _repositry.Save(newCustomer);
            }
            catch (InvalidCustomerAddressException)
            {
                throw new CustomerCreateException();
            }
        }

        public void CreateProperty(CustomerDTO customer)
        {
            Customer newCustomer = new Customer("");
            newCustomer.FirstName = customer.FirstName;
            newCustomer.LastName = customer.LastName;

            // Для того чтобы метод выполнился без исключений, свойство WorkstationId должно вернуть значение 
            int? id = _workstationSettings.WorkstationId;

            if (!id.HasValue)
            {
                throw new ApplicationException();
            }

            newCustomer.WorkstationId = id;
            // тест должен проверить что свойство было установлено
            _repositry.LocalTimeZone = TimeZoneInfo.Utc.DisplayName;

            _repositry.Save(newCustomer);
        }

        public void CreateEvent(CustomerDTO customer)
        {
            Customer newCustomer = new Customer("");
            newCustomer.FirstName = customer.FirstName;
            newCustomer.LastName = customer.LastName;

            _repositry.Save(newCustomer);
        }
    }

    #region Exceptions
    [Serializable]
    public class InvalidCustomerAddressException : Exception
    {
        public InvalidCustomerAddressException()
        {
        }

        public InvalidCustomerAddressException(string message) : base(message)
        {
        }

        public InvalidCustomerAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCustomerAddressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class CustomerCreateException : Exception
    {
        public CustomerCreateException()
        {
        }

        public CustomerCreateException(string message) : base(message)
        {
        }

        public CustomerCreateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomerCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    #endregion

public class Address
    {
    }

    public interface IEmailBuilder
    {
        Address From(CustomerDTO customer);
    }

    public interface IIdFactory
    {
        int Create();
    }
    public interface IMailingAddressFactory
    {
        Address CreateFrom(CustomerDTO customer);
        bool TryParse(string mail, out Address address);

        void CreatenewMessage(string name);
    }
}
