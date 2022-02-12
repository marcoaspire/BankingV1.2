using BankingV1._7.Account;
using BankingV1._7.Account.CreditAccount;
using BankingV1._7.Account.CurrentAccount;
using BankingV1._7.Account.SavingAccount;
using BankingV1._7.UserFolder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace BankingV1._7.Menu
{
    static class FileBO
    {
        //path where is saved bin\Debug
        public static void SaveData()
        {
            const string fileNameTxt = "accounts.txt";
            try
            {
                using (StreamWriter sw = File.CreateText(fileNameTxt))
                {
                    sw.WriteLine("Owner AccountType AccountName AccounNumber Balance Limit(Credit)");
                    foreach (Account.Account item in AccountBO.accounts)
                    {
                        if (item.AccountType.Equals("Current account"))
                        {
                            sw.WriteLine("{0} 1 {1} {2} {3}", item.Owner, item.AccountName, item.AccountNumber, item.Balance);
                        }
                        else if (item.AccountType.Equals("Savings account"))
                        {
                            sw.WriteLine("{0} 2 {1} {2} {3}", item.Owner, item.AccountName, item.AccountNumber, item.Balance);

                        }
                        else if (item.AccountType.Equals("Credit account"))
                        {
                            sw.WriteLine("{0} 3 {1} {2} {3} {4}", item.Owner, item.AccountName, item.AccountNumber, item.Balance, ((Credit)item).Limit);
                        }
                    }
                    sw.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to create this file.");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Argument is null");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid format");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("'path' exceeds the maximum supported path length.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file cannot be found.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The directory cannot be found.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error "+e.Message);
            }


        }
        public static void DataLoad()
        {
            //string path = @"C:\Users\marco.antonio\OneDrive - Aspire Systems (India) Private Limited\Documents\test.txt";
            string path = @"accounts.txt";
            
                //structure of the file
                //email accountType accountName accounNumber balance
                
                try
                {
                    using (StreamReader sr = File.OpenText(path))
                    {
                        string[] accounts;
                        string s;
                        AccountBO.accounts = new LinkedList<Account.Account>();
                        Account.Account account = null;
                        try
                        {
                            sr.ReadLine();
                            while ((s = sr.ReadLine()) != null)
                            {
                                accounts = s.Split(' ');
                                switch (Int32.Parse(accounts[1]))
                                {
                                    case 1:
                                        account = new Current(accounts[0], accounts[2], Int64.Parse(accounts[3]), "Current account", float.Parse(accounts[4]), 10000);
                                        break;
                                    case 2:
                                        account = new Saving(accounts[0], accounts[2], Int64.Parse(accounts[3]), "Savings account", float.Parse(accounts[4]), 10);
                                        break;
                                    case 3:
                                        account = new Credit(accounts[0], accounts[2], Int64.Parse(accounts[3]), "Credit account", float.Parse(accounts[4]), float.Parse(accounts[5]), 20);
                                        break;
                                    default:
                                        Console.WriteLine("ERROR");
                                        break;
                                }
                                if (account != null)
                                {
                                    AccountBO.accounts.AddLast(account);
                                    Thread.Sleep(1000);
                                }
                            }
                            Console.WriteLine("External content loaded successfully!");
                        }
                        catch (PathTooLongException)
                        {
                            Console.WriteLine("'path' exceeds the maximum supported path length.");
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("ERROR:Invalid format" +e.Message);
                        }
                        finally
                        {
                            sr.Close();
                        }
                        Console.WriteLine();
                    }

                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("The file cannot be found.");
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("The directory cannot be found.");
                }
                catch (DriveNotFoundException)
                {
                    Console.WriteLine("The drive specified in 'path' is invalid.");
                }

                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("You do not have permission to create this file.");
                }
                catch (EndOfStreamException )
                {
                    Console.WriteLine("Invalid format");

                }
                catch (ArgumentException )
                {
                    Console.WriteLine("Invalid format");
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Exception IO:" + ex.HResult);
                }
                catch (IndexOutOfRangeException ex)
                {

                    Console.WriteLine("External content could not loaded: " + ex);
                }
        }
        public static void UsersLoad()
        {
            string path = @"users.txt";
            //structure of the file
            //accountType accountName accounNumber balance
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string[] user;
                    string lineRead;
                    
                    Account.Account account = null;
                    try
                    {
                        sr.ReadLine();
                        while ((lineRead = sr.ReadLine()) != null)
                        {
                            user = lineRead.Split(' ');
                            //validate email
                            UserBO.users.AddLast(new User(new MailAddress(user[0]).Address, user[1]));
                            if (account != null)
                            {
                                AccountBO.accounts.AddLast(account);
                                Thread.Sleep(1000);
                            }
                            
                        }
                        Console.WriteLine("External content loaded successfully.");
                    }
                    catch (PathTooLongException)
                    {
                        Console.WriteLine("'path' exceeds the maximum supported path length.");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("ERROR:Invalid format");
                    }
                    finally
                    {
                        sr.Close();
                    }
                    Console.WriteLine();
                }

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file {0} cannot be found.", path);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The directory cannot be found.");
            }
            catch (DriveNotFoundException)
            {
                Console.WriteLine("The drive specified in 'path' is invalid.");
            }

            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to create this file.");
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("Invalid format");

            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid format, data muy grande");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Exception IO:" + ex.HResult);
            }
            catch (IndexOutOfRangeException ex)
            {

                Console.WriteLine("External content could not loaded: " + ex);
            }
        }

        public static void SaveUsers()
        {
            const string fileNameTxt = "users.txt";
            try
            {
                using (StreamWriter sw = File.CreateText(fileNameTxt))
                {
                    sw.WriteLine("Email	Password");

                    foreach (User user in UserBO.users)
                    {
                        sw.WriteLine("{0} {1}", user.Email, user.Password);
                    }
                    sw.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to create this file.");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Argument is null");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid format");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("'path' exceeds the maximum supported path length.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file cannot be found.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The directory cannot be found.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e.Message);
            }

        }


        public static void SaveBinaryData()
        {
            FileStream mArchivoEscritor;

            BinaryWriter escritor;

            mArchivoEscritor = new FileStream("user.dat", FileMode.OpenOrCreate, FileAccess.Write); //archivo
            escritor = new BinaryWriter(mArchivoEscritor);

            foreach (var item in UserBO.users)
            {
                escritor.Write(item.Email + " "+ item.Password);
            }
            escritor.Close();
            mArchivoEscritor.Close();
        }
        public static void DisplayBinaryValues()
        {
            FileStream mArchivoLector = null;
            BinaryReader lector = null;
            string fileName = @"user.dat";
            
            //if (File.Exists(fileName))
            //{
            try
            {
                mArchivoLector = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                lector = new BinaryReader(mArchivoLector, Encoding.UTF8, false);
                while (mArchivoLector.Position != mArchivoLector.Length)
                {

                    Console.WriteLine(lector.ReadString());
                }
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("'path' exceeds the maxium supported path length.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file cannot be found.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The directory cannot be found.");
            }
            catch (DriveNotFoundException)
            {
                Console.WriteLine("The drive specified in 'path' is invalid.");
            }

            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to create this file.");
            }
            catch (EndOfStreamException e)
            {
                Console.WriteLine("Invalid format"+e.Message);

            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid format");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Exception IO:" + ex.HResult);
            }

            finally
            {
                if (lector != null)
                    lector.Close();
                if (mArchivoLector != null)
                    mArchivoLector.Close();
            }
            //}
            //else
            //Console.WriteLine("we could not find it");

        }

    }
}
