using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class User
    {
        string _username;
        int _balance;
        string _hashedPin;
        List<Transaction> _transactions;

        public string Username { get { return _username; } set { _username = value; } }
        public int Balance { get { return _balance; } set { _balance = value; } }
        public string HashedPin { get { return _hashedPin; } set { _hashedPin = value; } }
        public List<Transaction> Transactions { get { return _transactions; } }

        public User() { }
        public User(string name, int balance, string hashedPin)
        {
            this._username = name;
            this._balance = balance;
            this._hashedPin = hashedPin;
            this._transactions = new List<Transaction>();
        }
    }
}
