using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeScript
{
    public class TransferBuilder
    {
        private int amount;
        private string? currency;
        private readonly Dictionary<string, int> participents;

        public TransferBuilder() { 
            participents = new Dictionary<string, int>();
        }

        public TransferBuilder Send(string currency, int amount)
        {
            if (amount < 1) throw new Exception("invalid amount");
            if (currency.Length != 3) throw new Exception("invalid currency");

            this.amount = amount;
            this.currency = currency.ToLower();
            return this;
        }
        
        public TransferBuilder From(int amount, string source)
        {
            if (string.IsNullOrWhiteSpace(source)) throw new Exception("invalid destination");

            return this;
        }

        public TransferBuilder From(string source)
        {
            if (string.IsNullOrWhiteSpace(source)) throw new Exception("invalid destination");

            return this;
        }

        public TransferBuilder To(int amount, string destination) 
        {
            if (string.IsNullOrWhiteSpace(destination)) throw new Exception("invalid destination");
            return this; 
        }
    }
}
