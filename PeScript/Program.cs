// See https://aka.ms/new-console-template for more information
using Irony.Parsing;
using PeScript;
using System.Xml.Linq;

Console.WriteLine("Hello, World!");

var path = "C:/output/trans1.txt";
string numscript = System.IO.File.ReadAllText(path);

var grammar = new PaymentScriptGrammar();
var parser = new Parser(grammar);
var parseTree = parser.Parse(numscript);

Console.WriteLine($"Parsed in {parseTree.ParseTimeMilliseconds}");

if (parseTree.HasErrors())
{
    foreach (var error in parseTree.ParserMessages)
    {
        Console.WriteLine($"textFile Line {error.Location.Line+1}, column {error.Location.Column+1}: {error.Message}");

    }
    return;
}

var action = parseTree.Root.ChildNodes.First();

if(action.Term.Name == "send")
{
    Console.WriteLine("Action is send operation");

    var amount = parseTree.Root.ChildNodes[1];
    var currency = amount.ChildNodes[0].ChildNodes[0].Term.ToString();
    if(!int.TryParse(amount.ChildNodes[1].Token.Value.ToString(), out int totalValue)){
        Console.WriteLine("total amount is invalid");
        return;
    }

    if(totalValue <= 0)
    {
        Console.WriteLine("invalid total amount, should a strictly positive integer");
        return;
    }

    var sources = parseTree.Root.ChildNodes[2];
    var sourceVariant = sources.ChildNodes[2];

    Console.WriteLine(sourceVariant.Term.Name);
    switch (sourceVariant.Term.Name)
    {
        case "sourceExps":
            Console.WriteLine("source count:" + sourceVariant.ChildNodes.Count);
            foreach (var sourceNode in sourceVariant.ChildNodes)
            {
                Console.WriteLine(sourceNode.Term.ToString());
            }

            break;
        case "accountId":
            var sourceId = sourceVariant.ChildNodes[1].Token.Value.ToString();
            Console.WriteLine("signle source:" + sourceId);
            break;

    }

    Console.WriteLine(currency);
    Console.WriteLine(totalValue);
}



