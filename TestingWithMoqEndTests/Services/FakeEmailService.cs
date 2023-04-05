using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingWithFakes.Services;

namespace TestingWithFakesTests.Services
{
    public class FakeEmailService : IEmailService
    {
        public bool SendWelcomeEmailCalled { get; set; }
        public void SendWelcomeEmail(string email)
        {
            SendWelcomeEmailCalled= true;
        }
    }
}
