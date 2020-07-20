using System;
using System.Collections.Generic;


namespace CardGame
{

    class Program
    {
        static void Main(string[] args)
        {

            Deck deck = new Deck();
            deck.FillDeck();
            List<List<Card>> playerDecks = deck.SplitHalfDeck();

            Player player1 = new Player(playerDecks[0], 1);
            Player player2 = new Player(playerDecks[1], 2);

            TheGame game = new TheGame(player1, player2);
            game.playGame();
           
        }
    }

    public class Card
    {
        public enum Suites
        {
            Hearts = 0,
            Diamonds,
            Clubs,
            Spades
        }

        public int Value
        {
            get;
            set;
        }

        public Suites Suite
        {
            get;
            set;
        }

        //Used to get full name, also useful 
        //if you want to just get the named value
        public string NamedValue
        {
            get
            {
                string name = string.Empty;
                switch (Value)
                {
                    case (14):
                        name = "Ace";
                        break;
                    case (13):
                        name = "King";
                        break;
                    case (12):
                        name = "Queen";
                        break;
                    case (11):
                        name = "Jack";
                        break;
                    default:
                        name = Value.ToString();
                        break;
                }

                return name;
            }
        }

        public string Name
        {
            get
            {
                return NamedValue + " of " + Suite.ToString();
            }
        }

        public Card(int Value, Suites Suite)
        {
            this.Value = Value;
            this.Suite = Suite;
        }
    }

    public class Deck
    {
        public List<Card> Cards = new List<Card>();

        public void FillDeck()
        {
            //Using divition based on 13 cards in a suited
            for (int i = 0; i < 52; i++)
            {
                Card.Suites suite = (Card.Suites)(Math.Floor((decimal)i / 13));
                //Add 2 to value as a cards start a 2
                int val = i % 13 + 2;
                Cards.Add(new Card(val, suite));
            }

            ShuffleDeck<Card>(Cards);
        }

        public void PrintDeck()
        {
            foreach (Card card in this.Cards)
            {
                Console.WriteLine(card.Name);
            }
        }


        public List<List<Card>> SplitHalfDeck()
        {
            Deck deck1 = new Deck();
            List<Card> cardsOfDeck1 = new List<Card>();

            for (int i = 0; i < 26; i++)
            {
                cardsOfDeck1.Add(this.Cards[i]);
            }

            Deck deck2 = new Deck();
            List<Card> cardsOfDeck2 = new List<Card>();

            for (int i = 26; i < 52; i++)
            {
                cardsOfDeck2.Add(this.Cards[i]);
            }
            List<List<Card>> playerDecks = new List<List<Card>>();
            playerDecks.Add(cardsOfDeck1);
            playerDecks.Add(cardsOfDeck2);

            return playerDecks;
        }
        // Randomized shuffler
       public Random rng = new Random();

        public void ShuffleDeck<Card>(IList<Card> cards)
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }


    }


    public class Player
    {
        public List<Card> playerDeck
        {
            get;
            set;
        }

        public int playerNumber
        {
            get;
            set;
        }

        public Player(List<Card> playerDeck, int playerNumber)
        { 
            this.playerDeck = playerDeck;
            this.playerNumber = playerNumber;
        }

        public void PrintDeck()
        {
            foreach (Card card in this.playerDeck)
            {
                Console.WriteLine(card.Name);
            }
        }
    }

    public class TheGame
    {
        public TheGame(Player player1, Player player2)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.warStack1 = new List<Card>();
            this.warStack2 = new List<Card>();
        }

        public Player player1
        {
            get;
            set;
        }

        public Player player2
        {
            get;
            set;
        }

        public List<Card> warStack1
        {
            get;
            set;
        }

        public List<Card> warStack2
        {
            get;
            set;
        }


        public void playGame()
        {
            int positionFaceUpCard = 0;
            
            checkPlayerDeckCards();

            if (player1.playerDeck[positionFaceUpCard].Value > player2.playerDeck[positionFaceUpCard].Value)
            {
                player1.playerDeck.Add(player2.playerDeck[positionFaceUpCard]);
                player1.playerDeck.Add(player1.playerDeck[positionFaceUpCard]);

                player2.playerDeck.Remove(player2.playerDeck[positionFaceUpCard]);
                player1.playerDeck.Remove(player1.playerDeck[positionFaceUpCard]);
            }

            else if (player2.playerDeck[0].Value > player1.playerDeck[positionFaceUpCard].Value)
            {
                player2.playerDeck.Add(player1.playerDeck[positionFaceUpCard]);
                player2.playerDeck.Add(player2.playerDeck[positionFaceUpCard]);

                player1.playerDeck.Remove(player1.playerDeck[positionFaceUpCard]);
                player2.playerDeck.Remove(player2.playerDeck[positionFaceUpCard]);
            }

            else
            {
                war(warStack1, warStack2, checkPlayerDeckCards());
            }

            printRemainingCards(player1);
            printRemainingCards(player2);

            int winCondition = 1;
            while (winCondition != 0)
            {
                playGame();
                winCondition = checkPlayerDeckCards();
            }
               
        }


        public void war(List<Card> warStack1, List<Card> warStack2, int numberOfCards)
        {
            checkPlayerDeckCards();
            for (int i = 0; i < numberOfCards; i++)
            {
                warStack1.Add(player1.playerDeck[0]);
                warStack2.Add(player2.playerDeck[0]);
                player1.playerDeck.Remove(player1.playerDeck[0]);
                player2.playerDeck.Remove(player2.playerDeck[0]);
            }

            // Declare War winner when value of faceUp card is higher.
            if (warStack1[warStack1.Count-1].Value > warStack2[warStack2.Count-1].Value)
            {
                for (int i = 0; i < warStack1.Count; i++)
                {
                    // First Winners Deck
                    player1.playerDeck.Add(warStack1[i]);
                    player1.playerDeck.Add(warStack2[i]);
                }
            }
            else if (warStack2[warStack1.Count-1].Value > warStack1[warStack2.Count-1].Value)
            {
                for (int i = 0; i < warStack2.Count; i++)
                {
                    player2.playerDeck.Add(warStack2[i]);
                    player2.playerDeck.Add(warStack1[i]);
                }
            }
            else
            {
                //Another war game must commence
                this.warStack1 = warStack1;
                this.warStack2 = warStack2;
                war(warStack1, warStack2, checkPlayerDeckCards());
            }
            // Empty warStack for next draw
            this.warStack1 = new List<Card>();
            this.warStack2 = new List<Card>();
        }

        public int checkPlayerDeckCards()
        {
            if (player1.playerDeck.Count == 0)
            {
                Console.WriteLine(" ");
                Console.WriteLine("Player 2 WINS!");
                System.Environment.Exit(0);
                return 0;
            } else if (player2.playerDeck.Count == 0)
            {
                Console.WriteLine(" ");
                Console.WriteLine("Player 1 WINS!");
                System.Environment.Exit(0);
                return 0;

            } else if (player1.playerDeck.Count < 3 || player2.playerDeck.Count < 3)
            {
                if (player1.playerDeck.Count < player2.playerDeck.Count)
                {
                    return player1.playerDeck.Count;
                }
                else if (player2.playerDeck.Count < player1.playerDeck.Count)
                {
                    return player2.playerDeck.Count;
                }
                else
                {
                    return player1.playerDeck.Count;
                }
            }
            else
            {
                return 4;
            }
        }

        public void printRemainingCards(Player player)
        {
            Console.WriteLine("");
            Console.Write("Player " + player.playerNumber + ":");
            foreach (Card card in player.playerDeck)
            {
                Console.Write(" " + card.Value);
            }
        }
    }
}
