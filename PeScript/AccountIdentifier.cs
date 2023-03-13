using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeScript
{
    internal class AccountIdentifier : IdentifierTerminal
    {
        public AccountIdentifier(string name) : base(name,extraChars:"")
        {
        }
        
    }
}
