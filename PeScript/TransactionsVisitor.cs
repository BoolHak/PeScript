using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeScript
{
    public class TransactionsVisitor
    {
        public List<string> Errors { get; } = new List<string>();
        public void Visit(ParseTreeNode node)
        {

            foreach (var child in node.ChildNodes)
            {
                VisitExpression(child);
            }

        }

        private void VisitExpression(ParseTreeNode node)
        {
            
            
        }

    }
}
