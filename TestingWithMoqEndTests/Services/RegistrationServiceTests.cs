using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingWithFakes.Services;

namespace TestingWithFakesTests.Services
{
    [TestClass]
    public class RegistrationServiceTests
    {
        private readonly RegistrationService _sut;

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        public RegistrationServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _emailServiceMock = new Mock<IEmailService>();

            _sut = new RegistrationService(_userRepositoryMock.Object, _emailServiceMock.Object);
        }

        // Test 1 Kontrollerar att användaren inte redan finns
        [TestMethod]
        public void Registration_Not_Allowed_If_User_Already_Exists()
        {
            // ARRANGE
            var email = "richard@gmail.com";
            _userRepositoryMock.Setup(u => u.UserExists(email)).Returns(true);

            // ACT
            var result = _sut.Register(email);

            // ASSERT
            Assert.AreEqual(RegistrationStatus.AlreadyRegistered, result);
        }

        // Test 2. Kontrollerar e-mailets domän är korrekt
        [TestMethod]
        public void Incorrect_Domain_Returns_Fail()
        {
            // ARRANGE (endast outlook - gmail godkänns!)
            var email = "richard@hotmail.com";

            // ACT
            var result = _sut.Register(email);

            // ASSERT
            Assert.AreEqual(RegistrationStatus.WrongDomain, result);
        }

        // Test 3 Endast 10 nya användare per dag
        [TestMethod]
        public void Registration_Not_Allowed_If_Greater_Than_10_Today()
        {
            // ARRANGE
            var email = "richard@gmail.com";

            // Varje gång GetRegisteredCountToday() anrops... ska 11 returneras
            _userRepositoryMock.Setup(u => u.GetRegisteredCountToday()).Returns(11);

            // ACT
            var result = _sut.Register(email);

            // ASSERT
            Assert.AreEqual(RegistrationStatus.TooManyRegistrationsToday, result);
        }

        // Test 4 Efter lyckad registrering ska ett välkomst-email skickas
        // Här testar vi att om registreringen har blivit OK...
        // ... delegerar vi till EmailService...
        // Så vi vill bekräfta att metoden SendWelcomeEmail(string email)
        // har blivit anropad!
        [TestMethod]
        public void
        Registration_OK_SendWelcomeEmail_Should_Be_Called()
        {
            // ARRANGE
            var email = "richard@gmail.com";

            // ACT
            var result = _sut.Register(email);

            // ASSERT
            // Kontrollera att SendWelcomeEmail() 
            // anropas 1 gång om successful
            _emailServiceMock.Verify(
                e => e.SendWelcomeEmail(
                It.IsAny<string>()), Times.Once());
        }

        // Test 5 Bonus
        // Om registreringen har blivit OK...
        // ... får vi tillbaka RegistrationStatus.Ok
        // Eftersom vi inte längre använda oss av listan "ExistingUsers"
        // ... behöver inte vi anropa ett mockobjekt alls...
        [TestMethod]
        public void Registration_OK_Returns_Ok()
        {
            // ARRANGE
            var email = "richard@gmail.com";

            // ACT
            var result = _sut.Register(email);

            // ASSERT
            Assert.AreEqual(RegistrationStatus.Ok, result);
        }



    }
}
