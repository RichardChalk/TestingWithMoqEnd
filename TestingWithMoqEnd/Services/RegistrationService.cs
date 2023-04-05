namespace TestingWithFakes.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        public RegistrationService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }
        public RegistrationStatus Register(string email)
        {
            // 1. Kontrollerar att användaren inte redan finns
            if (AlreadyRegistered(email)) 
                return RegistrationStatus.AlreadyRegistered;
            // 2.Kontrollerar e - mailets domän är korrekt
            if (!CorrectDomain(email))
                return RegistrationStatus.WrongDomain;
            // 3.Endast 10 nya användare per dag
            if (CountRegistrationsToday())
                return RegistrationStatus.TooManyRegistrationsToday;
            // 4.Efter lyckad registrering ska ett välkomst - email skickas
            _emailService.SendWelcomeEmail(email);

            return RegistrationStatus.Ok;
        }
        public bool AlreadyRegistered(string email)
        {
            return _userRepository.UserExists(email);
        }

        private bool CorrectDomain(string email)
        {
            return email.EndsWith("outlook.com") 
                || email.EndsWith("gmail.com");
        }

        private bool CountRegistrationsToday()
        {
            return _userRepository.GetRegisteredCountToday() > 10;
        }
    }
}
