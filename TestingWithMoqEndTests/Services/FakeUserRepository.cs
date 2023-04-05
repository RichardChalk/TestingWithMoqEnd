using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingWithFakes.Services;

namespace TestingWithFakesTests.Services
{
    public class FakeUserRepository : IUserRepository
    {
        public List<string> ExistingUsers { get; set; } = new List<string>();
        public bool UserExists(string email)
        {
            return ExistingUsers.Contains(email);
        }
        public int GetRegisteredCountToday()
        {
            return ExistingUsers.Count();
        }
    }
}
