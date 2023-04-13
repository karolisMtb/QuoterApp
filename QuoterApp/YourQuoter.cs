using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuoterApp
{
    public class YourQuoter : IQuoter
    {
        public double GetQuote(string instrumentId, int quantity)
        {
            HardcodedMarketOrderSource source = new HardcodedMarketOrderSource();
            List<MarketOrder> orders = new List<MarketOrder>();

            Thread fetchThread = new Thread(() =>
            {
                while (true)
                {
                    MarketOrder marketOrder = source.GetNextMarketOrder();
                    if (marketOrder.InstrumentId == instrumentId)
                    {
                        orders.Add(marketOrder);
                    }
                }
            });

            double quote = 0;

            Thread serviceThread = new Thread(() =>
            {
                quote = orders.OrderByDescending(x => x.Price).Last().Price;

            });

            fetchThread.Start();
            fetchThread.Join();
            serviceThread.Start();
            serviceThread.Join();

            return quote;
        }

        public double GetVolumeWeightedAveragePrice(string instrumentId)
        {
            HardcodedMarketOrderSource source = new HardcodedMarketOrderSource();
            List<MarketOrder> orders = new List<MarketOrder>();

            double temp = 0;
            int totalQuantity = 0;
            double vwap = 0;

            Thread fetchThread = new Thread(() =>
            {
                while (true)
                {
                    MarketOrder marketOrder = source.GetNextMarketOrder();
                    if (marketOrder.InstrumentId == instrumentId)
                    {
                        orders.Add(marketOrder);
                    }
                }
            });


            Thread vwapServiceThread = new Thread(() =>
            {
                if (orders.Count != 0)
                {
                    foreach (MarketOrder order in orders)
                    {
                        temp += order.Price * order.Quantity;
                        totalQuantity += order.Quantity;
                    }

                    vwap = totalQuantity / totalQuantity;
                }
            });

            fetchThread.Start();
            vwapServiceThread.Start();
            vwapServiceThread.Join();

            return vwap;
        }
    }
}
