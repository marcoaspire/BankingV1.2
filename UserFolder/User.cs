using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._7.UserFolder
{
    class User
    {
        private string email;
        private string password;

        public User(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }

        public override bool Equals(object obj)
        {
            User user = obj as User;
            if (user.Email.Equals(this.Email) && user.Password.Equals(this.Password))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return String.Format($"-Welcome dear user, your email is " +
                 $"{this.Email} ");
        }
    }
}
