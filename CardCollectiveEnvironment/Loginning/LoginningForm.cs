using System;

namespace CardCollectiveEnvironment
{
    [Serializable]
    public class LoginningForm
    {
        public LoginningForm(string login, string password)
        {
            try
            {
                Login = login;
                Password = password;
            }
            catch { throw; }
        }

        string login, password;
        public string Login
        {
            get { return login; }
            set
            {
                if (value == null) throw new ArgumentNullException("Null login");
                if ((login = value) == String.Empty) throw new ArgumentException("Empty login");
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                if (value == null) throw new ArgumentNullException("Null password");
                if ((password = value) == String.Empty) throw new ArgumentException("Empty password");
            }
        }
    }
}