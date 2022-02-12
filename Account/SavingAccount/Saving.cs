using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._7.Account.SavingAccount
{
    class Saving : Account
    {
        private float interest;
        public Saving() : base()
        {}
        public Saving(string owner,string accountName, long accountNumber, string accountType, float balance, float interest) : base(owner,accountName, accountNumber,accountType, balance)
        {
            Interest = interest;
        }
        public Saving(string owner,float interest) : base(owner)
        {
            Interest = interest;

        }
        //Properties 
        public float Interest { get => interest; set => interest = value; }

        public override string ToString()
        {
            return String.Format($"-Hello dear user, your " +
                $"{this.AccountType} {this.AccountName}, the account number is {this.AccountNumber}, it was opened on { this.CreatedAt}.\n" +
                $"Balance: {this.Balance} \n" +
                $"interest rate: {this.Interest} %\n");
        }
    }
}
