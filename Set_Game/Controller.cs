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
        /// <summary>
        /// Counter that fills for every Card that completed it's disappearing animation. Once it reaches Settings.CardsPerSet refillTable will be triggered.
        /// </summary>
        private int disappearedCompleted = 0;

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

        /// <summary>
        /// Sets the score to a specified value.
        /// </summary>
        /// <param name="newScore">The new score.</param>
        public void SetScore(int newScore) {
            score = newScore;
            NotifyPropertyChanged("Score");
        }

        /// <summary>
        /// Changes the score.
        /// </summary>
        /// <param name="diff">Difference added to current score. Negative values will decrease the score.</param>
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
            if (highlightedCards == Settings.CardsPerSet)
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
            //refillTable();
        }

        /// <summary>
        /// Refills the table until Settings.DefaultCardsOnTable is reached or a valid set becomes available, if the option is active.
        /// </summary>
        internal void refillTable() 
        {
            while (deck.Count > 0 && 
                    (Table.Count < Settings.DefaultCardsOnTable
                    || (Settings.drawUntilSetAvailable
                    && !checkAvailableSet()))) // draw until set available if the option is active
                drawRandomCard();
        }

        /// <summary>
        /// Removes a disappeared card from the Table.
        /// </summary>
        /// <param name="card">The Card to remove.</param>
        internal void removeCardFromTable(GameCard card)
        {
#warning replace gameArea actions with Data Binding
            disappearedCompleted++;
            mainWindow.gameArea.Children.Remove(card);
            Table.Remove(card);
            if (disappearedCompleted == Settings.CardsPerSet)
            {
                disappearedCompleted = 0;
                refillTable();
            }
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
                && set.GroupBy(card => card.BrushType).All(card => card.Count() == 1 || card.Count() == set.Count));
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
            return checkAvailableSet(helpRequested, new List<GameCard>());
        }

        /// <summary>
        /// Recursively checks if the Table contains a valid set.
        /// </summary>
        /// <param name="helpRequested">Indicates whether help was requested or not. If so, a highlighting is triggered.</param>
        /// <param name="setAccumulator">The cards of the current potential set, built up recursively.</param>
        /// <returns></returns>
        private bool checkAvailableSet(bool helpRequested, List<GameCard> setAccumulator)
        {
            if(setAccumulator.Count == Settings.CardsPerSet)
            {
                if (checkSet(setAccumulator))
                {
                    if (helpRequested)
                    {
                        foreach (var setCard in setAccumulator)
                        {
                            setCard.helpHighlight();
                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            // if setAccumulator is empty take indexes > -1, else take indexes > setAccumulator's last card's index for sorted combinations to avoid duplicate checks
            foreach (var card in Table.Where(c => Table.IndexOf(c) > (setAccumulator.Count>0 ? Table.IndexOf(setAccumulator.Last()) : -1)))
            {
                setAccumulator.Add(card);
                if (checkAvailableSet(helpRequested, setAccumulator))
                    return true;
                else
                    setAccumulator.Remove(setAccumulator.Last());
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
        /// Places Settings.DefaultCardsOnTable random cards from the full deck on the table.
        /// </summary>
        internal void initGame()
        {
            SetScore(0);
            highlightedCards = 0;
            disappearedCompleted = 0;
            initDeck();
            Table.Clear();
#warning replace gameArea actions with Data Binding
            mainWindow.gameArea.Children.Clear();
            NotifyPropertyChanged("CardsInDeck");

            refillTable();

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
            int brushTypeCounter = 0;
            for (int shapeIndex = 0; shapeIndex < Settings.CardsPerSet; shapeIndex++)
                for (int colorIndex = 0; colorIndex < Settings.CardsPerSet; colorIndex++)
                {
                    brushTypeCounter = 0;
                    foreach (ElementBrushType brushType in Enum.GetValues(typeof(ElementBrushType)))
                    {
                        brushTypeCounter++;
                        if (brushTypeCounter > Settings.CardsPerSet)
                            break;
                        for (int elementCount = 1; elementCount <= Settings.CardsPerSet; elementCount++)
                            deck.Add(new GameCard(this, elementCount, ElementShape.elemShapes[shapeIndex], Settings.elementColors[colorIndex], brushType));
                    }
                }

            //foreach (ElementShape shape in ElementShape.elemShapes)
            //    foreach (ElementBrushType brushType in Enum.GetValues(typeof(ElementBrushType)))
            //        foreach (Color color in Settings.elementColors)
            //            for (int i = 1; i <= Settings.MaxNumberOfElementsPerCard; i++)
            //            {
            //                deck.Add(new GameCard(this, i, shape, color, brushType));
            //            }
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
                    // game ended
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
