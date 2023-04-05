using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingWithFakes.Services
{
    public enum RegistrationStatus
    {
        Ok,
        AlreadyRegistered,
        WrongDomain,
        TooManyRegistrationsToday
    }
    public interface IRegistrationService
    {
        RegistrationStatus Register(string email);
    }
}
