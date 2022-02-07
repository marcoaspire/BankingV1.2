using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingV1._7.Account;
using BankingV1._7.Account.CurrentAccount;
using BankingV1._7.Account.CreditAccount;
using BankingV1._7.Account.SavingAccount;
using System.IO;
using System.Threading;
using BankingV1._7.UserFolder;
using System.Net.Mail;

namespace BankingV1._7.Menu
{
    class BankMenu
    {
        public bool Register()
        {
            string password = "", email;
            bool validEmail = false;
            do
            {
                Console.WriteLine("\nSign Up");
                Console.WriteLine("Type your email");
                email = Console.ReadLine();
                try
                {
                    email = new MailAddress(email).Address;
                    validEmail = true;
                    Console.WriteLine("Type your password, it needs to follow the below rules");
                    Console.WriteLine("It should contain at least one uppercase and lowercase alphabets");
                    Console.WriteLine("It should contain at least one numerical value");
                    Console.WriteLine("It should contain at least one special character");
                    Console.WriteLine("It should not contain any whitespaces");
                    Console.WriteLine("The length of the password should more than 7 characters");
                    password = Console.ReadLine();
                    if (!UserBO.ValidatePassword(password))
                    {
                        throw new Exception("The password does not meet the requirements, try again, please");
                    }
                }
                catch (FormatException)
                {

                    Console.WriteLine("Please provide a valid email address");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            } while (!UserBO.ValidatePassword(password) || !validEmail);
            UserBO.users.AddLast(new User(email, password));
            FileBO.SaveBinaryData();
            FileBO.DisplayBinaryValues();
            return true;
        }
        public void LogIn()
        {
            bool verification = false;
            string password = "", email;
            bool validEmail = false;
            do
            {
                Console.WriteLine("\nLogin");
                Console.WriteLine("Email");
                email = Console.ReadLine();
                Console.WriteLine("\nPassword");
                password = Console.ReadLine();
                //find  in UserBO.users
                if (UserBO.users.Find(new User(email, password)) is null)
                {
                    Console.WriteLine("Incorrect email address or password. Please try again.");
                }
                else
                {
                    verification = true;
                    FileBO.DataLoad();
                    DisplayMenu();
                }
            } while (!verification);

        }
        public void LoginMenu()
        {
            bool validNumber;
            int choice;
            do
            {
                Console.WriteLine("Welcome Banking System");
                Console.WriteLine("1-Login");
                Console.WriteLine("2-Register");
                Console.WriteLine("3-Exit");
                validNumber = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out choice);
                if (choice > 3 || choice < 1)
                    Console.WriteLine("Invalid option. Please enter a number between 1 and 3.");
            } while (!validNumber || choice > 4 || choice < 1);
            if (choice == 1)
            {
                LogIn();
            }
            else if (choice == 2)
            {
                if (Register())
                    LogIn();
            }
            else
            {
                Console.WriteLine("Thank you for using this system");
            }
        }
        public void DisplayMenu()
        {
            int op, choice;
            bool validNumber;
            CurrentBO currentBO = new CurrentBO();
            CreditBO creditBO = new CreditBO();
            SavingBO savingBO = new SavingBO();
            do
            {
                Console.WriteLine("Welcome Banking System");
                Console.WriteLine("1.Open a new bank account");
                Console.WriteLine("2.Display your account details as your ID");
                Console.WriteLine("3.Display all your accounts");
                Console.WriteLine("4.Deposit/Pay your credit");
                Console.WriteLine("5.Withdraw/Pay with your credit");
                Console.WriteLine("6.End of the month, get your new balance");
                Console.WriteLine("7.Delete/Change name of your account");
                Console.WriteLine("8.Exit");
                Console.WriteLine("Type an option, please");
                op = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (op)
                {
                    case '1':
                        do
                        {
                            Console.WriteLine("Which type of bank account would you like to open?");
                            Console.WriteLine("1-Current account");
                            Console.WriteLine("2-Savings account");
                            Console.WriteLine("3-Credit account");
                            Console.WriteLine("4-Return to menu");
                            validNumber = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out choice);
                            if (choice > 4 || choice < 1)
                                Console.WriteLine("Invalid option. Please enter a number between 1 and 4.");

                        } while (!validNumber || choice > 4 || choice < 1);
                        if (choice == 4)
                            break;
                        if (AccountBO.accounts is null)
                            AccountBO.accounts = new LinkedList<Account.Account>();
                         //   AccountBO.accounts = new List<Account.Account>();
                        
                        Account.Account newAccount = null;

                        switch (choice)
                        {
                            case 1:
                                newAccount = currentBO.NewAccount();
                                break;
                            case 2:
                                newAccount = savingBO.NewAccount();
                                break;
                            case 3:
                                newAccount = creditBO.NewAccount();
                                break;
                            default:
                                break;
                        }
                        if (!AccountBO.accounts.Contains(newAccount))
                        {
                            AccountBO.accounts.AddLast(newAccount);
                            OperationBO.operations.Add(DateTime.Now, new Operation("NewAccount", (Account.Account)newAccount.Clone(),newAccount.Balance));
                        }
                            //AccountBO.accounts.Add(newAccount); 
                        else
                            Console.WriteLine("Error: We could not open the new account because you alredy have one with that account number.");


                        break;
                    case '2':
                        if (AccountBO.accounts != null)
                        {
                            LinkedListNode<Account.Account> accoutFound = AccountBO.Find();
                            if (accoutFound != null)
                                Console.WriteLine(accoutFound.Value.ToString());
                            else
                                Console.WriteLine("We couldn't find an account with that number.");
                        }
                        else
                            Console.WriteLine("You don't have an account with us. Open your account now!");

                        break;
                    case '3':
                        if (AccountBO.accounts != null)
                        {
                            AccountBO.ShowAllAcounts();
                        }
                        else
                            Console.WriteLine("You don't have an account with us. Open your account now!");
                        break;
                    case '4':

                        if (AccountBO.accounts != null)
                        {
                            LinkedListNode<Account.Account> accoutFound = AccountBO.Find();
                            if (accoutFound != null)
                            {
                                try
                                {
                                    string type = accoutFound.Value.GetType().Name;
                                    
                                    if (type.Equals("Credit"))
                                    {
                                        creditBO.Deposit(accoutFound);
                                    }
                                    else if (type.Equals("Current"))
                                    {
                                        currentBO.Deposit(accoutFound);
                                    }
                                    else if (type.Equals("Saving"))
                                    {
                                        savingBO.Deposit(accoutFound);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Transaction has failed. " + e.Message);
                                }
                                


                            }
                            else
                                Console.WriteLine("We couldn’t find account with that number ");


                        }
                        else
                            Console.WriteLine("You don't have an account with us. Open your account now!");
                        break;

                    case '5':
                        if (AccountBO.accounts != null)
                        {
                            LinkedListNode<Account.Account> accoutFound = AccountBO.Find();
                            
                            
                            if (accoutFound != null)
                            {
                                string type = accoutFound.Value.GetType().Name;

                                if (type.Equals("Credit"))
                                {
                                    creditBO.Withdraw(accoutFound);
                                }
                                else if (type.Equals("Current"))
                                {
                                    currentBO.Withdraw(accoutFound);
                                }
                                else if (type.Equals("Saving"))
                                {
                                    savingBO.Withdraw(accoutFound);
                                }
                            }
                            else
                            {
                                Console.WriteLine("We couldn’t find account with that number ");
                            }
                            

                        }
                        else
                            Console.WriteLine("You don't have an account with us. Open your account now!");
                        break;
                    case '6':
                        if (AccountBO.accounts != null)
                        {
                            LinkedListNode<Account.Account> accoutFound = AccountBO.Find();
                            if (accoutFound != null)
                            {
                                string type = accoutFound.Value.GetType().Name;
                                Console.WriteLine("Account Statement");

                                if (type.Equals("Credit"))
                                {
                                    accoutFound.Value.Balance = creditBO.MonthEndBalance((Credit)accoutFound.Value);
                                    Console.WriteLine();
                                    Console.WriteLine(accoutFound.ToString());
                                }
                                else if (type.Equals("Current"))
                                {
                                    accoutFound.Value.Balance = currentBO.MonthEndBalance(accoutFound.Value);

                                    Console.WriteLine(accoutFound.ToString());
                                }
                                else if (type.Equals("Saving"))
                                {
                                    accoutFound.Value.Balance = savingBO.MonthEndBalance(accoutFound.Value);
                                    Console.WriteLine(accoutFound.Value.ToString());
                                }
                            }
                            else
                                Console.WriteLine("We couldn’t find account with that number");
                        }
                        else
                            Console.WriteLine("You don't have an account with us. Open your account now!");
                        break;
                    case '7':
                        LinkedListNode<Account.Account> accoutFound2 = AccountBO.Find();
                        if (accoutFound2 != null)
                        {
                            
                            do
                            {
                                Console.WriteLine("1- Modify the name of your account");
                                Console.WriteLine("2- Delete your account");
                                Console.WriteLine("3- Cancel");
                                validNumber = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out choice);
                                if (choice > 3 || choice < 1)
                                    Console.WriteLine("Invalid option. Please enter a number between 1 and 3.");

                            } while (!validNumber || choice > 3 || choice < 1);
                            if (choice == 3)
                                break;
                            

                            string type = accoutFound2.Value.GetType().Name;
                            if (type.Equals("Credit"))
                            {
                                Credit account=(Credit)accoutFound2.Value;
                                if (choice == 1)
                                    creditBO.UpdateAccount(account);
                                else
                                    creditBO.RemoveAccount(account);
                            }
                            else if (type.Equals("Current"))
                            {
                                Current account = (Current)accoutFound2.Value;
                                if (choice == 1)
                                    creditBO.UpdateAccount(account);
                                else
                                    currentBO.RemoveAccount(account);
                            }
                            else if (type.Equals("Saving"))
                            {
                                Saving account = (Saving)accoutFound2.Value;
                                if (choice == 1)
                                    creditBO.UpdateAccount(account);
                                else
                                    savingBO.RemoveAccount(account);
                            }
                        }
                        else
                            Console.WriteLine("We couldn’t find account with that number");
                        break;
                    case '8':
                        Console.WriteLine("Thank you for using this system");
                        new OperationBO().PrintOperations();
                        LoginMenu();
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please enter a number between 1 and 8.");
                        break;

                }
                Console.WriteLine("");

            } while (op != '8');



            Console.ReadKey();
        }

        

        

        

    }
    
}
