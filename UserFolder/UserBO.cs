using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._7.UserFolder
{
    class UserBO
    {
        public static LinkedList<User> users = new LinkedList<User>();
        public static Boolean ValidatePassword(String password)
        {
            if (password.Length < 8 || password.Contains(" ") || !password.Any(char.IsLetterOrDigit) || !password.Any(char.IsDigit)
                 || !password.Any(char.IsLower) || !password.Any(char.IsUpper))
                return false;
            return true;
        }
    }
}
