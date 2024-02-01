using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    

    [Serializable]
    public class Transaction
    {
        private int _value;
        private DateTime _datetime;
        private int _typeOfTransaction;  // 0 for deposit; 1 for withdraw

        public int Value { get { return _value; }  set { _value = value; } }
        public DateTime DateTime { get { return _datetime; } set { _datetime = value; } }

        public int TypeOfTransaction { get { return _typeOfTransaction; } set { _typeOfTransaction = value; } }

        public Transaction() { }

        public Transaction(int value, DateTime datetime, int typeOfTransaction)
        {
            this._value = value;
            this._datetime = datetime;
            this._typeOfTransaction = typeOfTransaction;
        }
    }
}
