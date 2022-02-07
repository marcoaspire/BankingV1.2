using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingV1._7.Menu;
using BankingV1._7.UserFolder;

namespace BankingV1._7
{
    
    class Program
    {
        static void Main(string[] args)
        {
            BankMenu bank = new BankMenu();
            
            FileBO.UsersLoad();
            bank.LoginMenu();
            
            Console.ReadKey();
        }
    }
}
