using System;
using System.Collections;

namespace PokerCA
{
    class Program
    {
        static void Main(string[] args)
        {
            PokerServices pokerServices = new PokerServices();
            for(int i = 0; i < 100; i++)
            {
                Console.WriteLine("Please key 5 poker cards: ");
                ArrayList cards = new ArrayList();
                cards.Add(Console.ReadLine());
                cards.Add(Console.ReadLine());
                cards.Add(Console.ReadLine());
                cards.Add(Console.ReadLine());
                cards.Add(Console.ReadLine());

                string[] strList_Cards = (string[])cards.ToArray(typeof(string));
                string result = pokerServices.PokerHandRanking(strList_Cards);
                Console.WriteLine(result);
            }  
        }
    }
}
