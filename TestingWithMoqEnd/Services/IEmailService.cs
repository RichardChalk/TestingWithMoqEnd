using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingWithFakes.Services
{
    public interface IEmailService
    {
        void SendWelcomeEmail(string email);
    }
}
