using BankingV1._7.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._7.Account.CreditAccount
{
    class CreditBO : AccountBO
    {
        public override float MonthEndBalance(Account account)
        {
            Credit creditAccount = (Credit)account;

            if (creditAccount.Balance > 0)
                return creditAccount.Balance + creditAccount.Balance * creditAccount.Interest / (100 * 12);
            else
                return 0;
        }
        public override void Withdraw(LinkedListNode<Account> account)
        {
            float withdrawal;
            bool validWithdrawal = false;
            Console.WriteLine("Pay with your credit");
            Credit creditAccount = (Credit)account.Value;
            float availableCredit = creditAccount.Limit - creditAccount.Balance;
            Console.WriteLine("Balance0:"+ account.Value.Balance);
            Console.WriteLine("aki:"+availableCredit);
            do
            {
                Console.WriteLine("Type the amount you want to pay");
                validWithdrawal = float.TryParse(Console.ReadLine(), out withdrawal);
            } while (!validWithdrawal);
            if (withdrawal > availableCredit)
            {
                Console.WriteLine($"Your available credit is less than {withdrawal} \n Transaction failed");
            }
            else
            {
                Console.WriteLine("credito anterior: "+ availableCredit);

                account.Value.Balance += withdrawal;
                OperationBO.operations.Add(DateTime.Now, new Operation("Pay with credit",(Account) account.Value.Clone(), availableCredit, withdrawal));
                Console.WriteLine(((Credit)account.Value).ToString());
            }

           
        }

        public override void Deposit(LinkedListNode<Account> account)
        {
            bool validDeposit = false;
            float deposit;
            Credit creditAccount = (Credit)account.Value.Clone();
            Console.WriteLine("\nPay your credit");
            do
            {
                Console.WriteLine("Type the amount you want to pay");
                validDeposit = float.TryParse(Console.ReadLine(), out deposit);
            } while (!validDeposit || deposit < 0);
            account.Value.Balance -= deposit;
            OperationBO.operations.Add(DateTime.Now, new Operation("Credit payment", (Account)account.Value.Clone(), creditAccount.Balance, deposit));
            Console.WriteLine(account.Value.ToString());
        }

        public override Account NewAccount()
        {
            bool validAccount, validBalance = false;
            long accountNumber;
            float balance, interest = 30;

            Credit account = null;
            do
            {
                Console.WriteLine("\nHow much do you want to deposit? Your deposit will be equal to your credit limit");
                validBalance = Single.TryParse(Console.ReadLine(), out balance);
               
            } while (!validBalance || !CheckBalance(balance));
           account = new Credit(BankMenu.email_session, interest, balance);

            account.AccountType = "Credit account";


            do
            {
                Console.WriteLine("Type your account numbers");
                validAccount = Int64.TryParse(Console.ReadLine(), out accountNumber);
                account.AccountNumber = accountNumber;

                //check if it's not in list
            } while (!validAccount);

            do
            {
                Console.WriteLine("Type account name");
                account.AccountName = Console.ReadLine();
                if (string.IsNullOrEmpty(account.AccountName))
                    Console.WriteLine("Error:Name can not be empty");
            } while (string.IsNullOrEmpty(account.AccountName));
            Console.WriteLine(((Credit)account).ToString());

            return account;
        }

    }
}
