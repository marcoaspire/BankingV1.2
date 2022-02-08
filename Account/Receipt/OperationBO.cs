using BankingV1._7.Menu;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._7.Account
{
    class OperationBO
    {
        public static SortedList<DateTime, Operation> operations = new SortedList<DateTime, Operation>();
        public void PrintOperations()
        {
            string fileNameTxt = "operationReceipt.txt";
            try
            {
                using (StreamWriter sw = File.CreateText(fileNameTxt))
                {
                    sw.WriteLine("Bank");
                    sw.WriteLine(DateTime.Now.ToString("s", CultureInfo.GetCultureInfo("en-US")));
                    sw.WriteLine("All operations done:");
                    sw.WriteLine("Date\t\t\tOperation Type\tAccount Number\t Account Type\tPrevious Balance\tCurrent Balance/Available Credit\tAmount");
                    foreach (var item in operations)
                    {
                        sw.Write(item.Key + "\t");
                        sw.WriteLine(item.Value.ToString());
                    }
                    sw.Close();
                    Console.WriteLine("You can find your receipts at:" + Path.GetFullPath(fileNameTxt));
                }
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("'path' exceeds the maxium supported path length.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The directory cannot be found.");
            }
            

            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to create this file.");
            }
            catch (NotSupportedException )
            {
                Console.WriteLine("Invalid format");

            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Path is null, verify the path where you can save the report");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid format in the argument");
            }
            





        }
    }
}
