namespace ElderHomeMonitoringSystem.Exceptions
{
    public class AccountExceptions : Exception
    {
        public class UsernameTakenException : Exception
        {
            public UsernameTakenException(string message) : base(message) { }
        }

        public class EmailTakenException : Exception
        {
            public EmailTakenException(string message) : base(message) { } 
        }

        public class UserNotFoundException : Exception
        {
            public UserNotFoundException(string message) : base(message) { }
        }

        public class PasswordMismatchException : Exception
        {
            public PasswordMismatchException(string message) : base(message) { }
        }
    }
}
