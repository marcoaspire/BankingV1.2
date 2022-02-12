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
        //it´s like a session variable
        public static string email_session;
        public bool Register()
        {
            string password = "", email;
            bool validEmail = false;
            do
            {
                Console.WriteLine("\n\nSign Up");
                Console.WriteLine("Type your email");
                email = Console.ReadLine();
                try
                {
                    email = new MailAddress(email).Address;
                    validEmail = true;
                    Console.WriteLine("\nType your password, it needs to follow the below rules");
                    Console.WriteLine("It should contain at least one uppercase and lowercase alphabets");
                    Console.WriteLine("It should contain at least one numerical value");
                    Console.WriteLine("It should contain at least one special character");
                    Console.WriteLine("It should not contain any whitespaces");
                    Console.WriteLine("The length of the password should more than 7 characters");
                    password = Console.ReadLine();
                    if (!UserBO.ValidatePassword(password))
                    {
                        throw new Exception("The password does not meet the requirements, try again, please");
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid format");
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
            User newUser = new User(email, password);
            try
            {
                if (UserBO.users.Find(newUser) != null)
                    throw new Exception("Someone already has this email address. Try again, please.\n");
                else
                {
                    UserBO.users.AddLast(newUser);
                    FileBO.SaveUsers();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        public void LogIn()
        {
            
            bool verification = false;
            string password = "";
            bool validEmail = false;
            do
            {
                Console.WriteLine("\nLogin");
                Console.WriteLine("Email");
                email_session = Console.ReadLine();
                Console.WriteLine("\nPassword");
                password = Console.ReadLine();
                try
                {
                    if (UserBO.users.Find(new User(email_session, password)) is null)
                    {
                        Console.WriteLine("Incorrect email address or password. Please try again.");
                    }
                    else
                    {
                        verification = true;
                        FileBO.DataLoad();
                        DisplayMenu();
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid format");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please provide a valid email address");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
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
                    Console.WriteLine("\nInvalid option. Please enter a number between 1 and 3.");
            } while (!validNumber || choice > 3 || choice < 1);
            if (choice == 1)
            {
                LogIn();
            }
            else if (choice == 2)
            {
                if (Register())
                    LogIn();
                else
                    LoginMenu();
            }
            else
            {
                Console.WriteLine("\nThank you for using this system");
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
                        try
                        {
                            if (!AccountBO.accounts.Contains(newAccount))
                            {
                                AccountBO.accounts.AddLast(newAccount);
                                OperationBO.operations.Add(DateTime.Now, new Operation("NewAccount", (Account.Account)newAccount.Clone(), newAccount.Balance, 0));
                            }
                            else
                                Console.WriteLine("Error: We could not open the new account because Someone already has that account number.");
                        }
                        catch (InvalidCastException)
                        {
                            Console.WriteLine("Specified cast is not valid");
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Error");
                        }
                        
                        break;
                    case '2':
                        if (AccountBO.accounts != null)
                        {
                            LinkedListNode<Account.Account> accoutFound = AccountBO.AskAccountNumber();
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
                            LinkedListNode<Account.Account> accoutFound = AccountBO.AskAccountNumber();
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
                                catch (InvalidCastException)
                                {
                                    Console.WriteLine("Specified cast is not valid");
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
                            LinkedListNode<Account.Account> accoutFound = AccountBO.AskAccountNumber();
                            
                            
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
                            LinkedListNode<Account.Account> accoutFound = AccountBO.AskAccountNumber();
                            try
                            {
                                if (accoutFound != null)
                                {
                                    string type = accoutFound.Value.GetType().Name;
                                    Console.WriteLine("Account Statement");

                                    if (type.Equals("Credit"))
                                    {
                                        accoutFound.Value.Balance = creditBO.MonthEndBalance((Credit)accoutFound.Value);
                                        Console.WriteLine();
                                        Console.WriteLine(accoutFound.Value.ToString());
                                    }
                                    else if (type.Equals("Current"))
                                    {
                                        accoutFound.Value.Balance = currentBO.MonthEndBalance(accoutFound.Value);

                                        Console.WriteLine(accoutFound.Value.ToString());
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
                            catch (InvalidCastException)
                            {
                                Console.WriteLine("Specified cast is not valid");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error:"+ e.Message);
                            }
                            
                        }
                        else
                            Console.WriteLine("You don't have an account with us. Open your account now!");
                        break;
                    case '7':
                        LinkedListNode<Account.Account> accoutFound2 = AccountBO.AskAccountNumber();
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

                            try
                            {
                                string type = accoutFound2.Value.GetType().Name;
                                if (type.Equals("Credit"))
                                {
                                    Credit account = (Credit)accoutFound2.Value;
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
                            catch (InvalidCastException)
                            {
                                Console.WriteLine("Specified cast is not valid");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error:" + e.Message);
                            }
                            
                        }
                        else
                            Console.WriteLine("We couldn’t find account with that number");
                        break;
                    case '8':
                        Console.WriteLine("Thank you for using this system");
                        new OperationBO().PrintOperations();
                        FileBO.SaveData();
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
