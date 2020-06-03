using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;

namespace CustomerServiceLibrary.Tests
{
    public class CustomerServiceTest
    {
        private readonly CustomerService _customerService;
        private Mock<ICustomerRepository> _repo = new Mock<ICustomerRepository>();
        private Mock<IEmailBuilder> _ebuilder = new Mock<IEmailBuilder>();
        private Mock<IIdFactory> _idfactory = new Mock<IIdFactory>();
        private Mock<IMailingAddressFactory> _mailingAddressFactory = new Mock<IMailingAddressFactory>();
        private Mock<INameBuilder> _namebuilder = new Mock<INameBuilder>();
        private readonly Mock<ICustomerStatusFactory> _statusFactory = new Mock<ICustomerStatusFactory>();
        private readonly Mock<IWorkstationSettings> _workstationSettings = new Mock<IWorkstationSettings>();
        public CustomerServiceTest()
        {
            _customerService = new CustomerService(_repo.Object, _ebuilder.Object, _idfactory.Object, _mailingAddressFactory.Object, _namebuilder.Object, _statusFactory.Object, _workstationSettings.Object);
        }

        /// <summary>
        /// тестим проверку количества раз сколько выполнился метод
        /// </summary>
        [Fact(Skip = "easily")]
        public void CustomerAddTest()
        {
            //arrange

            List<CustomerDTO> list = new List<CustomerDTO>() {
                new CustomerDTO() { FirstName ="Ivan", LastName="Ivanov" },
                new CustomerDTO() { FirstName ="Petr", LastName="Petrov" },
                new CustomerDTO() { FirstName ="Fedor", LastName="Fedorov" }
            };

            //_customerService.Create(list);

            
            _repo.Verify(x => x.Save(It.IsAny<Customer>()), Times.Exactly(3));

        }
        /// <summary>
        /// Тестим имитацию возврата и передачу пустого экземпляра
        /// </summary>
        [Fact]
        public void CustomerAddTest_ReturnedValue()
        {
            //arrange
            var c = new CustomerDTO
            {
                FirstName = "A",
                LastName = "B",
                Email = "sdfsfd"
            };

            _ebuilder.Setup(x => x.From(c)).Returns(new Address());

            _customerService.Create(c);

            _ebuilder.Verify();

            _repo.Verify(x => x.Save(It.IsAny<Customer>()));
        }
        /// <summary>
        /// тестим возврат и проверку исключения
        /// </summary>
        [Fact]
        public void CustomerAddTest_ReturnedValue_Exception()
        {
            //arrange
            var c = new CustomerDTO
            {
                FirstName = "A",
                LastName = "B"                
            };

            _ebuilder.Setup(x => x.From(c)).Returns<Address>(null);

            _ebuilder.Verify();

            Assert.Throws<ApplicationException>(() => _customerService.Create(c));           
        }
        /// <summary>
        /// тестим OUT параметры
        /// </summary>
        [Fact]
        public void CustomerAddTest_Out()
        {
            //arrange            
            var a = new CustomerDTO
            {
                FirstName = "A",
                LastName = "B",
                Email = "asd"
            };


            var c = new CustomerDTO
            {
                FirstName = "A",
                LastName = "B"
            };

            Address addr = new Address();

            _mailingAddressFactory
                .Setup(x => x.TryParse(a.Email, out addr))
                .Returns(true);

            _customerService.CreateOutParam(a);            

            _repo.Verify(x => x.Save(It.IsAny<Customer>()));       

        }
        /// <summary>
        /// тестим колбэк. Т.е. что будет происходить после вызова замоканного метода 
        /// также количество вызовов метода
        /// </summary>
        [Fact]
        public void CustomerAddTest_Id()
        {
            //arrange            

            List<CustomerDTO> list = new List<CustomerDTO>() {
                new CustomerDTO() { FirstName ="Ivan", LastName="Ivanov" },
                new CustomerDTO() { FirstName ="Petr", LastName="Petrov" },
                new CustomerDTO() { FirstName ="Fedor", LastName="Fedorov" }
            };

            int id = 1;

            _idfactory.Setup(x => x.Create()).Returns(() => id).Callback(() => ++id);

            _customerService.CreateWithId(list);

            _idfactory.Verify(x => x.Create(), Times.Exactly(3));

            _repo.Verify(x => x.Save(It.IsAny<Customer>()));
        }
        /// <summary>
        /// тестим, что при вызове мока в него будут переданы правильные параметры
        /// </summary>
        [Fact]
        public void CustomerAddTest_TrackFields()
        {
            //arrange            
            var c = new CustomerDTO() { FirstName = "Ivan", LastName = "Ivanov" };

            //_namebuilder.Setup(x => x.From(c.FirstName, c.LastName)).Returns($"{c.FirstName} {c.LastName}");

            _customerService.CreateTrackingArg(c);

            _namebuilder.Verify(x => x.From(
                It.Is<string>(z => z.Equals(c.FirstName) ), 
                It.Is<string>(z => z.Equals(c.LastName))));

            _repo.Verify(x => x.Save(It.IsAny<Customer>()));
        }
        /// <summary>
        /// тестим кейс, что метод возвращает результат в зависимости от переданного параметра
        /// </summary>
        [Fact]
        public void CustomerAddTest_Status()
        {
            //arrange            
            var c = new CustomerDTO() { FirstName = "Ivan", LastName = "Ivanov", DesiredLevel = StatusLevel.Bronze};


            _statusFactory
                // Если объект переданый в параметры в свойстве DesiredLevel содержит значение Bronze
                .Setup(x => x.CreateFrom(It.Is<CustomerDTO>(c => c.DesiredLevel == StatusLevel.Bronze)))
                // Метод CreateFrom должен вернуть значение Bronze
                .Returns(StatusLevel.Bronze);

            _customerService.CreateStatus(c);

            _repo.Verify(x => x.Save(It.IsAny<Customer>()));
        }

        /// <summary>
        /// тестим кейс, что метод возвращает результат в зависимости от переданного параметра
        /// </summary>
        [Fact]
        public void CustomerAddTest_StatusGold()
        {
            //arrange            
            var c = new CustomerDTO() { FirstName = "Ivan", LastName = "Ivanov", DesiredLevel = StatusLevel.Bronze };


            _statusFactory
                // Если объект переданый в параметры в свойстве DesiredLevel содержит значение Gold
                .Setup(x => x.CreateFrom(It.Is<CustomerDTO>(c => c.DesiredLevel == StatusLevel.Gold)))
                // Метод CreateFrom должен вернуть значение Bronze
                .Returns(StatusLevel.Gold);

            _customerService.CreateStatus(c);

            _repo.Verify(x => x.Save(It.IsAny<Customer>()));
        }

        /// <summary>
        /// Тестим исключения в моке
        /// </summary>
        [Fact]
        public void CustomerAddTest_Exception()
        {
            //arrange            
            var c = new CustomerDTO() { FirstName = "Ivan", LastName = "Ivanov"};

            _mailingAddressFactory.Setup(x => x.CreateFrom(c))
                .Throws<InvalidCustomerAddressException>();

           Assert.Throws<CustomerCreateException>(() =>_customerService.CreateExc(c));

           _mailingAddressFactory.Verify();

            //_repo.Verify(x => x.Save(It.IsAny<Customer>()));
        }
        /// <summary>
        /// test propetry
        /// </summary>
        [Fact]
        public void CustomerAddTest_Property()
        {
            //arrange            
            var c = new CustomerDTO() { FirstName = "Ivan", LastName = "Ivanov" };

            _workstationSettings
                .Setup(x => x.WorkstationId)
                .Returns(555);

            _customerService.CreateProperty(c);

            _repo.VerifySet(x => x.LocalTimeZone = TimeZoneInfo.Utc.DisplayName);
        }
        /// <summary>
        /// test stub propetry
        /// </summary>
        [Fact]
        public void Create_ShouldCallSaveMethod_Sample1()
        {
            Mock<ICustomerRepository> customerRespositoryMock = new Mock<ICustomerRepository>();
            Mock<IWorkstationSettings> workstationSettingsMock = new Mock<IWorkstationSettings>();

            // 1 вариант
            //workstationSettingsMock.SetupProperty(x => x.WorkspaceName, "TestWorkspace");
            workstationSettingsMock.SetupProperty(x => x.WorkstationId, 111);
            //workstationSettingsMock.Object.WorkstationId = 123; // после вызова метода SetupProperty можно использовать метод 

            _customerService.Create(new CustomerDTO());

            customerRespositoryMock.Verify(x => x.Save(It.IsAny<Customer>()));
        }
        /// <summary>
        /// test stub propetry
        /// </summary>
        [Fact]
        public void Create_ShouldCallSaveMethod_Sample2()
        {
            Mock<ICustomerRepository> customerRespositoryMock = new Mock<ICustomerRepository>();
            Mock<IWorkstationSettings> workstationSettingsMock = new Mock<IWorkstationSettings>();

            // 2 вариант
            workstationSettingsMock.SetupAllProperties();
            //workstationSettingsMock.Object.WorkspaceName = "TestWorkspace";
            workstationSettingsMock.Object.WorkstationId = 111;

            _customerService.Create(new CustomerDTO());

            customerRespositoryMock.Verify(x => x.Save(It.IsAny<Customer>()));
        }

        [Fact]
        public void CustomerAddTest_Event()
        {
            ////arrange            
            //var c = new CustomerDTO() { FirstName = "Ivan", LastName = "Ivanov" };

            //_repo
            //    .Setup(x => x.Save(It.IsAny<Customer>()))
            //    .Raises(x => x.Notify += null, new Customer("") {FirstName = "Ivan", LastName = "Kuzmin" }, new NotifyEventArgs("Ivan"));

            //_customerService.CreateEvent(c);

            //_repo.Verify();

            // Raise - инициирует событие Notify и передает в качестве параметра значение new NotifyEventArgs("Ivan")
            _repo.Raise(x => x.Notify += null, new NotifyEventArgs("Ivan"));

            _mailingAddressFactory.Verify(x => x.CreatenewMessage(It.IsAny<string>()));
        }
    }
}
