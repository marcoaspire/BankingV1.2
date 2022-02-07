using BankingV1._7.Account;
using BankingV1._7.Account.CreditAccount;
using BankingV1._7.Account.CurrentAccount;
using BankingV1._7.Account.SavingAccount;
using BankingV1._7.UserFolder;
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

        public static void DataLoad()
        {
            Console.WriteLine("entre");
            //string path = @"C:\Users\marco.antonio\OneDrive - Aspire Systems (India) Private Limited\Documents\test.txt";
            string path = @"accounts.txt";

            //structure of the file
            //accountType accountName accounNumber balance
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
                            switch (Int32.Parse(accounts[0]))
                            {
                                case 1:
                                    account = new Current(accounts[1], Int64.Parse(accounts[2]), "Current account", float.Parse(accounts[3]), 10000);
                                    break;
                                case 2:
                                    account = new Saving(accounts[1], Int64.Parse(accounts[2]), "Savings account", float.Parse(accounts[3]), 10);
                                    break;
                                case 3:
                                    account = new Credit(accounts[1], Int64.Parse(accounts[2]), "Credit account", float.Parse(accounts[3]), 20);
                                    break;
                                default:
                                    Console.WriteLine("ERROR");
                                    break;
                            }
                            if (account != null)
                            {
                                AccountBO.accounts.AddLast(account);

                                Console.WriteLine("nueva cuenta:" + account.Balance);
                                OperationBO.operations.Add(DateTime.Now, new Operation("NewAccount", (Account.Account)account.Clone(), account.Balance));
                                Thread.Sleep(1000);
                            }
                        }
                        Console.WriteLine("External content loaded successfully!");
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
                    //AccountBO.accounts = new List<Account.Account>();
                    
                    Account.Account account = null;
                    try
                    {
                        sr.ReadLine();
                        while ((lineRead = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(lineRead);
                            
                            user = lineRead.Split(' ');
                            //validate email
                            UserBO.users.AddLast(new User(new MailAddress(user[0]).Address, user[1]));
                            if (account != null)
                            {
                                AccountBO.accounts.AddLast(account);

                                Console.WriteLine("nueva cuenta:" + account.Balance);
                                OperationBO.operations.Add(DateTime.Now, new Operation("NewAccount", (Account.Account)account.Clone(), account.Balance));
                                Thread.Sleep(1000);
                            }
                            
                        }
                        Console.WriteLine("External content loaded successfully!");
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
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid format, data muy grande");
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
