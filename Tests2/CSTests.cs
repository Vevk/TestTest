using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTT.Domain.Customers;
using UTT.Domain.Customers.Repository;
using UTT.Domain.Customers.Services;
using Xunit;
using Bogus;
using Bogus.DataSets;
using FluentAssertions;

namespace Tests2
{
    public class CSTests
    {
        [Fact]
        public void GetAllActive_Returns()
        {
            var container = new WindsorContainer().Install(new AutoMoqInstaller());

            var customerRepository = new Mock<ICustomerRepository>()
            {
                CallBase = true,
                DefaultValue = DefaultValue.Mock
            }; 
            

            //var repo = container.Register(Resolve<ICustomerRepository>();

            
            container.Register(Component.For<ICustomerRepository>()
                .Instance(customerRepository.Object));
            customerRepository.Setup(o => o.GetAll()).Returns(GetMixedCustomers());

            var sut = container.Resolve<CustomerService>();


            // Arrange
            //var customerService = Fixture.GetCustomerService();
            //Fixture.CustomerRepositoryMock.Setup(c => c.GetAll()).Returns(Fixture.GetMixedCustomers());

            // Act
            var customers = sut.GetAllActive().ToList();

            // Assert Fluent Assertions
            customers.Should().HaveCount(c => c > 1).And.OnlyHaveUniqueItems();
            customers.Should().NotContain(c => !c.Active);
        }

        public IEnumerable<Customer> GetMixedCustomers()
        {
            var customers = new List<Customer>();

            customers.AddRange(GenerateCustomer(50, true).ToList());
            customers.AddRange(GenerateCustomer(50, false).ToList());

            return customers;
        }

        private static IEnumerable<Customer> GenerateCustomer(int number, bool isActive)
        {
            var customerTests = new Faker<Customer>("en")
                .CustomInstantiator(f => new Customer(
                    Guid.NewGuid(),
                    f.Name.FirstName(Name.Gender.Male),
                    f.Name.LastName(Name.Gender.Male),
                    f.Date.Past(80, DateTime.Now.AddYears(-18)),
                    f.Internet.Email().ToLower(),
                    isActive,
                    DateTime.Now));

            return customerTests.Generate(number);
        }
    }
}
