using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._7.Account.SavingAccount
{
    class SavingBO : AccountBO
    {
        public override float MonthEndBalance(Account account)
        {
            //interest earning per month
            Saving savAccount = (Saving)account;
            Console.WriteLine(savAccount.Balance + savAccount.Balance * savAccount.Interest / (100 * 12));
            return savAccount.Balance + savAccount.Balance * savAccount.Interest / (100 * 12);
        }

        public override Account NewAccount()
        {
            bool validAccount, validBalance = false;
            long accountNumber;
            float balance, interest = 10;

            Account account = new Saving(interest);
            account.AccountType = "Savings account";


            do
            {
                Console.WriteLine("\nType your account numbers");
                validAccount = Int64.TryParse(Console.ReadLine(), out accountNumber);
                account.AccountNumber = accountNumber;

                //check if it's not in list



            } while (!validAccount);

            do
            {
                Console.WriteLine("Type account name");
                account.AccountName = Console.ReadLine();
                if (string.IsNullOrEmpty(account.AccountName))
                    Console.WriteLine("Error:Name can not be empty, try again");
            } while (string.IsNullOrEmpty(account.AccountName));

            do
            {
                Console.WriteLine("Type the balance");
                validBalance = float.TryParse(Console.ReadLine(), out balance);
                //validate balance
            } while (!validBalance || !CheckBalance(balance));
            account.Balance = balance;

            

            Console.WriteLine($"Hello dear user, you have a new saving account {account.AccountName} , its account number is {account.AccountNumber} and you have ${account.Balance}");
            return account;

        }
        /*
        public float CheckBalance(float balance)
        {
            if (balance < 0)
                return 0;
            else
                return balance;
        }
        */
    }
}
