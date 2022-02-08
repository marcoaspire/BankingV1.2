using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._7.Account.CurrentAccount
{
    class Current : Account
    {
        private float maxDepositLimit;

        public Current(string owner,string accountName, long accountNumber, string accountType, float balance, float max) : base(owner,accountName, accountNumber, accountType, balance)
        {
            this.maxDepositLimit = max;
        }
        public Current(string owner,float max) : base(owner)
        {
            this.maxDepositLimit = max;
        }

        //public float Balance { get => balance; set => balance = value; }
        public float MaxDepositLimit { get => maxDepositLimit; set => maxDepositLimit = value; }
    }
}
