using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerCA
{
    public class PokerServices
    {
        public string PokerHandRanking(string[] str_Cards_List)
        {
            string str_Result = "";
            List<Card> HandCards = new List<Card>();
            try
            {
                if(str_Cards_List.Count() >= 5)
                {
                    string[] stringSeparatorsForSuit = new string[] { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4",
                    "3", "2"};
                    string[] stringSeparatorsForRank = new string[] { "H", "S", "D", "C", "h", "s", "d", "c" };                  foreach (string str_Card in str_Cards_List)
                    {
                        Card handCard = new Card();
                        string[] splited_ForSuit = str_Card.Split(stringSeparatorsForSuit, StringSplitOptions.None);
                        string[] splited_ForRank = str_Card.Split(stringSeparatorsForRank, StringSplitOptions.None);
                        if(splited_ForSuit.Count() > 0)
                        {
                            handCard.Suit = splited_ForSuit[1];
                        }
                        if (splited_ForRank.Count() > 0)
                        {
                            handCard.Rank = splited_ForRank[0];
                        }
                        HandCards.Add(handCard);
                    }
                }
                
                if (HandCards != null && HandCards.Count() == 5)
                {
                    List<AnalyzedCard> analyzedCardList = new List<AnalyzedCard>();
                    foreach (Card card in HandCards)
                    {
                        AnalyzedCard analyzedCard = new AnalyzedCard();
                        analyzedCard.Suit = card.Suit;
                        switch (card.Rank)
                        {
                            case "A":
                                analyzedCard.Value = 12;
                                break;
                            case "2":
                                analyzedCard.Value = 0;
                                break;
                            case "3":
                                analyzedCard.Value = 1;
                                break;
                            case "4":
                                analyzedCard.Value = 2;
                                break;
                            case "5":
                                analyzedCard.Value = 3;
                                break;
                            case "6":
                                analyzedCard.Value = 4;
                                break;
                            case "7":
                                analyzedCard.Value = 5;
                                break;
                            case "8":
                                analyzedCard.Value = 6;
                                break;
                            case "9":
                                analyzedCard.Value = 7;
                                break;
                            case "10":
                                analyzedCard.Value = 8;
                                break;
                            case "J":
                                analyzedCard.Value = 9;
                                break;
                            case "Q":
                                analyzedCard.Value = 10;
                                break;
                            case "K":
                                analyzedCard.Value = 11;
                                break;
                        }
                        analyzedCardList.Add(analyzedCard);
                    }

                    if (CheckRoyalFlush(analyzedCardList)) { str_Result = "Royal Flush"; }
                    else if (CheckStraightFlush(analyzedCardList)) { str_Result = "Straight Flush"; }
                    else if (CheckFourOfAKind(analyzedCardList)) { str_Result = "Four Of A Kind"; }
                    else if (CheckFullHouse(analyzedCardList)) { str_Result = "Full House"; }
                    else if (CheckFlush(analyzedCardList)) { str_Result = "Flush"; }
                    else if (CheckStraight(analyzedCardList)) { str_Result = "Straight"; }
                    else if (CheckThreeOfAKind(analyzedCardList)) { str_Result = "Three Of A Kind"; }
                    else if (CheckTwoPair(analyzedCardList)) { str_Result = "Two Pair"; }
                    else if (CheckPair(analyzedCardList)) { str_Result = "Pair"; }
                    else { str_Result = "High Card."; }
                }
            }
            catch (Exception ex)
            {

            }
            return str_Result;
        }

        public bool CheckStraight(List<AnalyzedCard> cards)
        {
            var ordered = cards.OrderByDescending(a => a.Value).ToList();
            for (var i = 0; i < ordered.Count - 4; i++)
            {
                var skipped = ordered.Skip(i);
                var possibleStraight = skipped.Take(5).ToList();
                if (IsStraight(possibleStraight))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsStraight(List<AnalyzedCard> cards)
        {
            List<string> circularStraightPattern = new List<string> { "12", "0", "1", "2", "3" };
            var cardsStartFromAce = cards.Select(x => x.Value.ToString()).Intersect(circularStraightPattern).ToList().Count();

            if (cardsStartFromAce >= 5)
            {
                return true;
            }
            else
            {
                return cards.GroupBy(card => card.Value).Count() == cards.Count() && cards
                .Max(card => card.Value - cards.Min(card => card.Value) == 4);
            }
        }

        public bool CheckPair(List<AnalyzedCard> cards)
        {
            return cards.GroupBy(card => card.Value).Count(group => group.Count() == 2) == 1;
        }

        public bool CheckTwoPair(List<AnalyzedCard> cards)
        {
            return cards.GroupBy(card => card.Value).Count(group => group.Count() >= 2) == 2;
        }

        public bool CheckThreeOfAKind(List<AnalyzedCard> cards)
        {
            return cards.GroupBy(card => card.Value).Any(group => group.Count() == 3);
        }

        public bool CheckFlush(List<AnalyzedCard> cards)
        {
            return cards.GroupBy(card => card.Suit).Count(group => group.Count() >= 5) == 1;
        }

        public bool CheckFullHouse(List<AnalyzedCard> cards)
        {
            return CheckPair(cards) && CheckThreeOfAKind(cards);
        }
        public bool CheckFourOfAKind(List<AnalyzedCard> cards)
        {
            return cards.GroupBy(card => card.Value).Any(group => group.Count() == 4);
        }

        public bool CheckStraightFlush(List<AnalyzedCard> cards)
        {
            return CheckFlush(cards) && CheckStraight(cards);
        }

        public bool CheckRoyalFlush(List<AnalyzedCard> cards)
        {
            return CheckFlush(cards) && CheckStraight(cards) && isRoyal(cards);
        }

        public bool isRoyal(List<AnalyzedCard> cards)
        {
            var validRoyal = cards.Where(x => x.Value.ToString().Contains("8")
                 || x.Value.ToString().Contains("9")
                 || x.Value.ToString().Contains("10")
                 || x.Value.ToString().Contains("11")
                 || x.Value.ToString().Contains("12")).ToList().Count();
            if (validRoyal >= 5) { return true; }
            else { return false; }
        }
    }
}
