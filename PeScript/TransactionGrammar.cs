using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeScript
{
    internal class TransactionGrammar : Grammar
    {
        public TransactionGrammar() : base(true)
        {
            // Terminals
            var send = ToTerm("send");
            var openParen = ToTerm("(");
            var closeParen = ToTerm(")");
            var source = ToTerm("source");
            var destination = ToTerm("destination");
            var percent = ToTerm("%");
            var to = ToTerm("to");
            var from = ToTerm("from");
            var remaining = ToTerm("remaining");
            var slash = ToTerm("/");
            var equal = ToTerm("=");

            var numberLiteral = new NumberLiteral("number", NumberOptions.NoDotAfterInt |  NumberOptions.IntOnly);
            var coin = ToTerm("COIN") | ToTerm("TND");
            var id = new IdentifierTerminal("id");

            // Non-terminals
            var amountpercent = new NonTerminal("amountpercent");
            var amountexpr = new NonTerminal("amountexpr");
            var amount = new NonTerminal("amount");
            var expression = new NonTerminal("expression");
            var expressions = new NonTerminal("expressions");
            var destinationItem = new NonTerminal("destinationItem");
            var destinations = new NonTerminal("destinations");
            var sourceItem = new NonTerminal("sourceItem");
            var sources = new NonTerminal("sources");

            // Rules
            amountpercent.Rule = numberLiteral + percent;
            amountexpr.Rule = numberLiteral + slash + numberLiteral;
            amount.Rule = amountpercent | numberLiteral + slash + numberLiteral | remaining;

            destinationItem.Rule = "destination" + equal + "@" + id;
            destinationItem.Rule |= "destination" + equal + "{" + destinations + "}";
            destinationItem.Rule |= amount + to + "{" + destinations + "}";
            destinationItem.Rule |= amount + to + "@" + id;

            destinations.Rule = MakePlusRule(destinations, destinationItem);

            sourceItem.Rule = "source" + equal + "@" + id;
            sourceItem.Rule |= amount + from + "{" + sources + "}";
            sources.Rule = MakePlusRule(sources, sourceItem);

            expression.Rule = send + "[" + coin + numberLiteral + "]" 
                + openParen 
                    + sources
                    + destinations
                + closeParen;

            expressions.Rule = MakeStarRule(expressions, expression);

            // Set the root rule
            this.Root = expressions;

            // Mark terminals and non-terminals
            MarkPunctuation("[", "]", "{", "}", "%", "/", "to", "@", "from", "=");
            MarkPunctuation(openParen, closeParen);
        }
    }
}
