using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingV1._7.Menu;
namespace BankingV1._7.Account
{
    abstract class AccountBO :IAccountBO
    {
        //public static List<Account> accounts;
        public static LinkedList<Account> accounts;

        //Methods

        public virtual void Deposit(LinkedListNode<Account> account)
        {
            bool validDeposit = false;
            float deposit;
            Account accountAuxiliary  = (Account)account.Value.Clone();
            do
            {
                Console.WriteLine("Type the amount you want to deposit");
                validDeposit = float.TryParse(Console.ReadLine(), out deposit);
            } while (!validDeposit || deposit < 0);
            float previo = account.Value.Balance;
            account.Value.Balance += deposit;
            OperationBO.operations.Add(DateTime.Now, new Operation("Deposit", (Account)account.Value.Clone(), accountAuxiliary.Balance, deposit));
            
            Console.WriteLine($"Now your balance is ${account.Value.Balance}");
            //return account;
        }
        public virtual void Withdraw(LinkedListNode<Account> account)
        {
            float withdrawal;
            bool validWithdrawal = false;

            do
            {
                Console.WriteLine("Type the amount you want to withdraw");
                validWithdrawal = float.TryParse(Console.ReadLine(), out withdrawal);
            } while (!validWithdrawal);
            if (withdrawal > account.Value.Balance)
            {
                Console.WriteLine($"Your balance is less than {withdrawal} \n Transaction failed");
            }
            else
            {
                Account auxiliar = (Account)account.Value.Clone();
                account.Value.Balance -= withdrawal;
                OperationBO.operations.Add(DateTime.Now, new Operation("Withdraw", (Account)account.Value.Clone(), auxiliar.Balance, withdrawal));
                Console.WriteLine($"Now your balance is ${account.Value.Balance}");
            }
            //return account;
        }

        public virtual float MonthEndBalance(Account account)
        {
            return account.Balance;
        }

        public static void ShowAllAcounts()
        {
            Console.WriteLine("Your accounts are:");
            foreach (Account item in AccountBO.accounts)
            {
                if (BankMenu.email_session.Equals(item.Owner))
                    Console.WriteLine(item.ToString());
            }

        }
        public static LinkedListNode<Account> Find()
        {
            bool validAccount;
            long accountNumber;
            do
            {
                Console.WriteLine("Type the account numbrer");
                validAccount = Int64.TryParse(Console.ReadLine(), out accountNumber);
            } while (!validAccount);


            return Find2(accountNumber);
        }
        public static LinkedListNode<Account> Find2(long accountNumber)
        {
            LinkedListNode<Account> node = AccountBO.accounts.First;
            do
            {
                if (node.Value.AccountNumber == accountNumber && BankMenu.email_session.Equals(node.Value.Owner))
                {
                    return node;
                }
                node = node.Next;
            } while (node != null);

            return null;
        }
        //interface methods
        public abstract Account NewAccount();

        public void RemoveAccount(Account a)
        {
            Console.WriteLine("\nAccount was deleted");
            if (a.Balance!=0)
                Console.WriteLine("You need to withdraw all your money before to delete");
            else
            {
                OperationBO.operations.Add(DateTime.Now, new Operation("Account deleted", a, a.Balance,0));
                accounts.Remove(a);

            }
        }

        public void UpdateAccount(Account a)
        {
            String name;
            do
            {
                Console.WriteLine("\nWrite the new name of the account");
                name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                    Console.WriteLine("Error:Name can not be empty");
            } while (string.IsNullOrEmpty(name));

            OperationBO.operations.Add(DateTime.Now, new Operation("Account name updated", a, a.Balance,0));

            accounts.Where(account => account == a).ToList().ForEach(s => s.AccountName = name);
            Console.WriteLine("Your change is saved");
        }
        
        public bool CheckBalance(float balance)
        {
            if (balance < 1)
            {
                Console.WriteLine("Balance was negative. Try again");
                return false;
            }
            return true;
        }
    }
}
