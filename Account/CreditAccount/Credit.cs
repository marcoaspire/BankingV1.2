using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._7.Account.CreditAccount
{
    class Credit : Account
    {
        float interest;
        float limit;

        public Credit(string owner,string accountName, long accountNumber, string accountType, float limit, float interest) : base(owner,accountName, accountNumber, accountType, 0)
        {
            Interest = interest;
            Limit = limit;
            Balance = 0;
        }

        public Credit(string owner,string accountName, long accountNumber, string accountType, float balance, float limit, float interest) : base(owner,accountName, accountNumber, accountType, 0)
        {
            Interest = interest;
            Limit = limit;
            Balance = balance;
        }


        public Credit(string owner,float interest, float limit) : base(owner)
        {
            Interest = interest;
            Limit = limit;
            Balance = 0;
        }

        //Properties 
        public float Interest { get => interest; set => interest = value; }
        public float Limit { get => limit; set => limit = value; }
        //public float Balance { get => balance; set => balance = value; }


        public override string ToString()
        {
            return String.Format($"-Hello dear user, your " +
                $"{this.AccountType} {this.AccountName}, the account number is {this.AccountNumber}, it was opened on { this.CreatedAt}.\n" +
                $"Balance: {this.Balance} \n" +
                $"Available Credit: {this.Limit - this.Balance} \n" +
                $"Credit Limit: {this.Limit} \n");
        }






    }
}
