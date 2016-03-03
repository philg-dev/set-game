using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Set_Game
{
    public class Controller : INotifyPropertyChanged
    {
        private MainWindow mainWindow;
        private List<GameCard> deck = new List<GameCard>();
        private List<GameCard> table = new List<GameCard>();
        public List<GameCard> Table { get { return table; } }
        private int highlightedCards = 0;
        public int HighlightedCards {
            get { return highlightedCards; }
        }
        private Random random = new Random();
        private int score = 0;
        public int Score { get { return score; } }
        public int CardsInDeck { get { return deck.Count; } }
        private bool helpRequested = false;

        public Controller(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            // data bindings
            mainWindow.Controls.DataContext = this;
            // cards in deck counter binding
            Binding cardsInDeckBinding = new Binding("CardsInDeck");
            cardsInDeckBinding.Source = this;
            cardsInDeckBinding.Mode = BindingMode.OneWay;
            cardsInDeckBinding.StringFormat = "Verbleibende Karten im Deck: {0}";
            mainWindow.CardsInDeckTextBlock.SetBinding(TextBlock.TextProperty, cardsInDeckBinding);
            // score binding
            Binding scoreBinding = new Binding("Score");
            scoreBinding.Source = this;
            scoreBinding.Mode = BindingMode.OneWay;
            scoreBinding.StringFormat = "Punkte: {0}"; 
            mainWindow.ScoreTextBlock.SetBinding(TextBlock.TextProperty, scoreBinding);
            // table binding
            //Binding tableBinding = new Binding("Table");
            //tableBinding.Source = this;
            //tableBinding.Mode = BindingMode.OneWay;
            
            
            //mainWindow.gameArea.SetBinding()

            initGame();
        }

        public void SetScore(int newScore) {
            score = newScore;
            NotifyPropertyChanged("Score");
        }

        public void alterScore(int diff) {
            score += diff;
            NotifyPropertyChanged("Score");
        }

        /// <summary>
        /// Adds 1 to the counter for highlighted cards.
        /// </summary>
        public void addHighlighting()
        {
            highlightedCards++;
            if (highlightedCards == 3)
            {
                if (checkHighlightedSet())
                    foundSet();
                else
                {
                    foreach (var card in Table.Where(card => card.Highlighted))
                    {
                        card.toggleHighlight();
                    }
                }
            }
        }

        /// <summary>
        /// Subtracts 1 from the counter for highlighted cards.
        /// </summary>
        public void removeHighlighting()
        {
            highlightedCards--;
        }

        /// <summary>
        /// Clears the counter for highlighted cards, adds a point to the players score, removes found set from the deck and adds new cards to the table.
        /// </summary>
        public void foundSet()
        {
            highlightedCards = 0;
            if (!helpRequested)
            {
                alterScore(1);
            }
            helpRequested = false;
            //List<int> toRemoveIndizes = new List<int>();
            foreach (var card in Table.Where(card => card.Highlighted))
            {
                card.disappear();
            }
        }

        internal void removeCardFromTable(GameCard card)
        {
#warning replace gameArea actions with Data Binding
            mainWindow.gameArea.Children.Remove(card);
            Table.Remove(card);
            if(Table.Count < 12)
                drawRandomCard();
        }

        /// <summary>
        /// Checks if the 3 given cards meat the requirements to be a set.
        /// </summary>
        /// <returns>True if set found, false if invalid combination.</returns>
        private bool checkSet(List<GameCard> set) {
            // check element count
            /*
            IEnumerable<Item> items = ...
            var noDuplicates = items.GroupBy(x => x.Prop1).All(x => x.Count() == 1);
            // it returns true if all items have different Prop1, false otherwise
            */
            return (set.GroupBy(card => card.Elements).All(card => card.Count() == 1 || card.Count() == set.Count)
                && set.GroupBy(card => card.Shape).All(card => card.Count() == 1 || card.Count() == set.Count)
                && set.GroupBy(card => card.ElemColor).All(card => card.Count() == 1 || card.Count() == set.Count)
                && set.GroupBy(card => card.Fill).All(card => card.Count() == 1 || card.Count() == set.Count));
        }

        /// <summary>
        /// Checks if a set is available on the table.
        /// </summary>
        /// <returns></returns>
        private bool checkAvailableSet() {
            return checkAvailableSet(false);
        }

        /// <summary>
        /// Checks if a set is available on the table.
        /// </summary>
        /// <param name="helpRequested">If true, it highlights the found set.</param>
        /// <returns>Returns true if a set can be found, false if none is available.</returns>
        private bool checkAvailableSet(bool helpRequested) {
            foreach(var card1 in Table)
                foreach(var card2 in Table.Where(card => !card.Equals(card1)))
                    foreach (var card3 in Table.Where(card => !card.Equals(card1) && !card.Equals(card2)))
                    {
                        List<GameCard> set = new List<GameCard>();
                        set.Add(card1);
                        set.Add(card2);
                        set.Add(card3);
                        if (checkSet(set))
                        {
                            if (helpRequested)
                            {
                                foreach (var setCard in set)
                                {
                                    setCard.helpHighlight();
                                }
                            }
                            return true;
                        }
                    }
            return false;
        }

        /// <summary>
        /// Checks if the 3 currently highlighted cards meat the requirements to be a set.
        /// </summary>
        /// <returns>True if set found, false if invalid combination.</returns>
        private bool checkHighlightedSet()
        {
            List<GameCard> set = Table.Where(card => card.Highlighted).ToList();
            return checkSet(set);
        }

        /// <summary>
        /// Places 12 random cards from the full deck on the table.
        /// </summary>
        internal void initGame()
        {
            SetScore(0);
            highlightedCards = 0;
            initDeck();
            Table.Clear();
#warning replace gameArea actions with Data Binding
            mainWindow.gameArea.Children.Clear();
            NotifyPropertyChanged("CardsInDeck");

            for (int i = 0; i < 12; i++)
            {
                drawRandomCard();
            }
        }

        /// <summary>
        /// Draws a random card from the deck and moves it to the table.
        /// </summary>
        private void drawRandomCard()
        {
            helpRequested = false;
            if (deck.Count <= 0)
                return;
            int index = random.Next(deck.Count);
            addToTable(deck[index]);
        }

        /// <summary>
        /// Removes a card from the deck and adds it to the table.
        /// </summary>
        /// <param name="card">The card to move from deck to table.</param>
        private void addToTable(GameCard card) {
            Table.Add(card);
            NotifyPropertyChanged("Table");
#warning replace gameArea actions with Data Binding
            mainWindow.gameArea.Children.Add(card);
            deck.Remove(card);
            NotifyPropertyChanged("CardsInDeck");
        }

        /// <summary>
        /// Initializes the deck of 81 cards with all different combinations of attributes.
        /// </summary>
        private void initDeck() {
            deck.Clear();
            foreach (ElementShape shape in Enum.GetValues(typeof(ElementShape)))
                foreach (ElementFill fill in Enum.GetValues(typeof(ElementFill)))
                    foreach (Color color in Settings.elementColors)
                        for (int i = 1; i <= Settings.MaxNumberOfElementsPerCard; i++)
                        {
                            deck.Add(new GameCard(this, i, shape, color, fill));
                        }
        } // end initDeck

        /// <summary>
        /// Shows an available set if there is one.
        /// </summary>
        internal void handleHelpRequest()
        {
            checkAvailableSet(true);
            helpRequested = true;
        }

        /// <summary>
        /// Handles the click on the noSetAvailable-button.
        /// Gains 1 point if the user correctly assumed that there's no more available set on the table (if no help request was made before in that state of the table).
        /// Drains 2 points if the user incorrectly assumed that there's no more available set on the table.
        /// </summary>
        internal void handleNoSetAvailableCheck()
        {
            if (!checkAvailableSet())
            {
                // when help request found no set and this button is used, no score is gained
                if (!helpRequested){
                    alterScore(1);
                }
                if (CardsInDeck > 0)
                    drawRandomCard();
                else
                {
                    foreach (var card in Table)
                        card.disappear();
                }
            }
            else
            {
                // penalty
                alterScore(-2);
            }
        }

        // This method is called by the Set accessor of each property.
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
