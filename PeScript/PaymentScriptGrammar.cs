using Irony.Parsing;

namespace PeScript
{
    [Language("PaymentScript", "1.0", "A scripting language for payments and transactions")]
    internal class PaymentScriptGrammar : Grammar
    {
        public PaymentScriptGrammar() : base(caseSensitive: true)
        {
            var openBracket = ToTerm("[");
            var closeBracket = ToTerm("]");
            var openParen = ToTerm("(");
            var closeParen = ToTerm(")");
            var atSymbol = ToTerm("@");
            var colon = ToTerm(":");
            var percentage = ToTerm("%");
            var slash = ToTerm("/");
            var openCurlyBrace = ToTerm("{");
            var closeCurlyBrace = ToTerm("}");
            var equal = ToTerm("=");

            var send = ToTerm("send");

            var max = ToTerm("max");
            var from = ToTerm("from");
            var to = ToTerm("to");
            var remaining = ToTerm("remaining");

            var source = ToTerm("source");
            var destination = ToTerm("destination");
            

            var number = new NumberLiteral("number", NumberOptions.NoDotAfterInt | NumberOptions.IntOnly);
            var identifier = new IdentifierTerminal("identifier", extraChars: ":", extraFirstChars:"1234567890");

            var paymentAmount = new NonTerminal("paymentAmount");
            var account = new NonTerminal("accountId");
            var paymentMaxAmount = new NonTerminal("paymentMaxAmount");
            var paymentShare = new NonTerminal("paymentShare");
            var paymentFraction = new NonTerminal("paymentFraction");

            
            var sourceStm = new NonTerminal("source");
            var sourceExpression = new NonTerminal("sourceExp");
            var sourceExpressions = new NonTerminal("sourceExps");
            var fromStm =  new NonTerminal("fromStm");

            var destinationStm = new NonTerminal("destination");
            var destinationExpression = new NonTerminal("destinationExp");
            var destinationExpressions = new NonTerminal("destinationExps");
            var toStm = new NonTerminal("toStm");

            var sendExpression = new NonTerminal("sendStm");

            var coin = ToTerm("COIN") | ToTerm("TND");
            coin.Name = "currency";

            //source
            sourceStm.Rule = source + equal + account;
            sourceStm.Rule |= source + equal + openCurlyBrace + sourceExpressions + closeCurlyBrace;
            
            MakePlusRule(sourceExpressions, sourceExpression);

            sourceExpression.Rule = account;
            sourceExpression.Rule |= paymentMaxAmount + fromStm;
            sourceExpression.Rule |= paymentShare + fromStm;
            sourceExpression.Rule |= paymentFraction + fromStm;
            sourceExpression.Rule |= number + fromStm;
            sourceExpression.Rule |= remaining + fromStm;

            fromStm.Rule = from + account;
            fromStm.Rule |= from + openCurlyBrace + sourceExpressions + closeCurlyBrace;

            //destination
            destinationStm.Rule = destination + equal + account;
            destinationStm.Rule |= destination + equal + openCurlyBrace + destinationExpressions + closeCurlyBrace;
            
            MakePlusRule(destinationExpressions, destinationExpression);

            destinationExpression.Rule = account;
            destinationExpression.Rule |= paymentMaxAmount + toStm;
            destinationExpression.Rule |= paymentShare + toStm;
            destinationExpression.Rule |= paymentFraction + toStm;
            destinationExpression.Rule |= number + toStm;
            destinationExpression.Rule |= remaining + toStm;

            toStm.Rule = to + account;
            toStm.Rule |= to + openCurlyBrace + destinationExpressions + closeCurlyBrace;


            paymentAmount.Rule = openBracket + coin + number + closeBracket;
            paymentMaxAmount.Rule = max + openBracket + coin + number + closeBracket;
            paymentShare.Rule = number + percentage;
            paymentFraction.Rule = number + slash + number;

            account.Rule = atSymbol + identifier;

            sendExpression.Rule = send + paymentAmount
                + openParen
                    + sourceStm
                    + destinationStm
                + closeParen;

            this.Root = sendExpression;

            MarkPunctuation("[", "]", "{", "}", "(", ")");

        }
    }
}
