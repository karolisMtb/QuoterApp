using System;

namespace QuoterApp
{
    class Program
    {

        static void Main(string[] args)
        {
            var gq = new YourQuoter();
            var qty = 120;

            double quote = 0;
            double vwap = 0;

            quote = gq.GetQuote("DK50782120", qty);
            vwap = gq.GetVolumeWeightedAveragePrice("DK50782120");

            Console.WriteLine($"Quote: {quote}, {quote / (double)qty}");
            Console.WriteLine($"Average Price: {vwap}");
            Console.WriteLine();
            Console.WriteLine($"Done");
        }
    }
}