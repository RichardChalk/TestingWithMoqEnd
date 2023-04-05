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
        private readonly FakeUserRepository _userRepository;
        private readonly FakeEmailService _emailService;

        public RegistrationServiceTests()
        {
            _userRepository = new FakeUserRepository();
            _emailService = new FakeEmailService();
            _sut = new RegistrationService(_userRepository, _emailService);
        }

        // Test 1 Kontrollerar att användaren inte redan finns
        [TestMethod]
        public void Registration_Not_Allowed_If_User_Already_Exists()
        {
            // ARRANGE
            var email = "richard@gmail.com";
            _userRepository.ExistingUsers.Clear();
            _userRepository.ExistingUsers.Add(email);

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
            _userRepository.ExistingUsers.Clear();
            for (int i = 1; i <= 11; i++)
            {
                _userRepository.ExistingUsers.Add((i.ToString() + "@randomEmail.com") );
            }

            // ACT
            var result = _sut.Register(email);

            // ASSERT
            Assert.AreEqual(RegistrationStatus.TooManyRegistrationsToday, result);
        }

        // Test 4 Endast 10 nya användare per dag
        // Här testar vi att om registreringen har blivit OK...
        // ... delegerar vi till EmailService...
        // Så vi vill bekräfta att metoden SendWelcomeEmail(string email)
        // har blivit anropad!
        [TestMethod]
        public void Registration_OK_SendWelcomeEmail_Should_Be_Called()
        {
            // ARRANGE
            var email = "richard@gmail.com";
            _userRepository.ExistingUsers.Clear();

            // ACT
            var result = _sut.Register(email);

            // ASSERT
            Assert.IsTrue(_emailService.SendWelcomeEmailCalled);
        }

        // Test 5 Bonus
        // Om registreringen har blivit OK...
        // ... får vi tillbaka RegistrationStatus.Ok
        [TestMethod]
        public void Registration_OK_Returns_Ok()
        {
            // ARRANGE
            var email = "richard@gmail.com";
            _userRepository.ExistingUsers.Clear();

            // ACT
            var result = _sut.Register(email);

            // ASSERT
            Assert.AreEqual(RegistrationStatus.Ok, result);
        }



    }
}
