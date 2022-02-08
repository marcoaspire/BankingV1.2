﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._7.Account
{
    abstract class Account : ICloneable
    {
        long accountNumber;
        string accountName;
        string accountType;
        protected float balance;
        string owner;
        DateTime createdAt;
        //constructor
        public Account(string owner, string accountName, long accountNumber, string accountType, float balance)
        {
            this.accountNumber = accountNumber;
            this.AccountName = accountName;
            this.AccountType = accountType;
            this.balance = balance;
            this.createdAt = DateTime.Now;
            this.owner = owner;
        }
        public Account(string owner)
        {
            this.createdAt = DateTime.Now;
            this.owner = owner;
        }

        //Properties 
        public long AccountNumber { get => accountNumber; set => accountNumber = value; }
        public string AccountName { get => accountName; set => accountName = value; }
        public string AccountType { get => accountType; set => accountType = value; }
        public float Balance { get => balance; set => balance = value; }
        public DateTime CreatedAt
        {
            get => createdAt;
            //set => createdAt = value; // read only
        }
        public string Owner { get => owner; set => owner = value; }


        //methods


        public override string ToString()
        {
            return String.Format($"-Hello dear user, your " +
                $"{this.AccountType} {this.AccountName}, the account number is {this.AccountNumber} and you have ${this.Balance}." +
                $"It was opened on {this.CreatedAt}");

        }

        public override bool Equals(Object obj)
        {
            Account account = obj as Account;
            return this.AccountNumber.Equals(account.AccountNumber);
        }

        public object Clone()
        {
            return (Account)MemberwiseClone();
        }

        public int CompareTo(object obj)
        {
            Account account = obj as Account;
            return String.Compare(this.accountType, account.accountType);
        }
    }
}
