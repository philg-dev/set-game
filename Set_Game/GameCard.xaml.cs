using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Set_Game
{
    /// <summary>
    /// Interaktionslogik für GameCard.xaml
    /// </summary>
    public partial class GameCard : UserControl
    {
        private int elements = 0;
        /// <summary>
        /// The amount of the GameCard's CardElements.
        /// </summary>
        public int Elements { get { return elements; } }
        private ElementShape shape;
        /// <summary>
        /// The shape of the GameCard's CardElements.
        /// </summary>
        public ElementShape Shape { get { return shape; } }
        private Color color;
        /// <summary>
        /// The color of the GameCard's CardElements.
        /// </summary>
        public Color ElemColor { get { return color; } }
        private ElementFill fill;
        /// <summary>
        /// The fill-type of the GameCard's CardElements.
        /// </summary>
        public ElementFill Fill { get { return fill; } }
        /// <summary>
        /// Indicates whether the GameCard is in the state of disappearing.
        /// </summary>
        private bool disappearing = false;
        private bool highlighted = false;
        /// <summary>
        /// Indicartes whether the GameCard is highlighted.
        /// </summary>
        public bool Highlighted { get { return highlighted; } }
        /// <summary>
        /// The Controller object that created this GameCard.
        /// </summary>
        private Controller controller;

        /// <summary>
        /// Creates a new GameCard object.
        /// </summary>
        /// <param name="controller">The controller object that calls this constructor.</param>
        /// <param name="elementCount">The amount of elements on the card.</param>
        /// <param name="shape">The shape-type of the card's elements.</param>
        /// <param name="color">The color of the card's elements.</param>
        /// <param name="fill">The fill-type of the card's elements.</param>
        public GameCard(Controller controller, int elementCount, ElementShape shape, Color color, ElementFill fill) {
            InitializeComponent();
            fillBrush.Color = Settings.GameCardBackgroundColor;
            this.controller = controller;
            if (elementCount < 1 || elementCount > Settings.MaxNumberOfElementsPerCard)
                throw new ArgumentException("Invalid number of elements.");
            elements = elementCount;
            this.shape = shape;
            this.color = color;
            this.fill = fill;
            for(int i = 0; i < elementCount; i++)
            {
                CardElement elem = new CardElement(shape, color, fill);
                ElementGrid.Children.Add(elem);
            }
            this.LayoutTransform = new ScaleTransform(Settings.GameCardSizeFactor, Settings.GameCardSizeFactor);
        }


        /// <summary>
        /// Toggles the highlighting-state of the GameCard.
        /// </summary>
        public void toggleHighlight(){
            if (disappearing)
                return;
            highlighted = !highlighted;
            if (!highlighted)
            {
                CardRectangle.Fill = new SolidColorBrush(Settings.GameCardBackgroundColor);
                RenderTransform = new RotateTransform(0);
                controller.removeHighlighting();
            }
            else
            {
                CardRectangle.Fill = new SolidColorBrush(Settings.GameCardHighlightColor);
                RenderTransform = new RotateTransform(10);
                controller.addHighlighting();
            }
        }

        /// <summary>
        /// Highlighting animation when help is requested to find a set.
        /// https://stackoverflow.com/questions/24184584/coloranimation-change-color-in-rectangle
        /// </summary>
        public void helpHighlight() {
            if (disappearing)
                return;
            Color startingColor = highlighted ? Settings.GameCardHighlightColor : Settings.GameCardBackgroundColor;
            CardRectangle.Fill = new SolidColorBrush(Colors.Black);
            var storyboard = new Storyboard();
            var colorFadeAnimation = new ColorAnimation(Color.FromArgb(255,0,0,0), new Duration(TimeSpan.FromSeconds(1)));
            colorFadeAnimation.From = startingColor;
            Storyboard.SetTarget(colorFadeAnimation, CardRectangle);
            Storyboard.SetTargetProperty(colorFadeAnimation, new PropertyPath("Fill.Color"));
            storyboard.Children.Add(colorFadeAnimation);
            var colorFadeBackAnimation = new ColorAnimation(startingColor, new Duration(TimeSpan.FromSeconds(2)));
            Storyboard.SetTarget(colorFadeBackAnimation, CardRectangle);
            Storyboard.SetTargetProperty(colorFadeBackAnimation, new PropertyPath("Fill.Color"));
            colorFadeBackAnimation.BeginTime = TimeSpan.FromSeconds(2);
            storyboard.Children.Add(colorFadeBackAnimation);
            this.BeginStoryboard(storyboard);
        }

        /// <summary>
        /// Triggers the disappearing animation of the GameCard.
        /// </summary>
        public void disappear() {
            disappearing = true;
            Storyboard storyboard = new Storyboard();
            Color startingColor = highlighted ? Settings.GameCardHighlightColor : Settings.GameCardBackgroundColor;
            startingColor.A = 0;
            ColorAnimation disappearAnimation = new ColorAnimation(startingColor, new Duration(TimeSpan.FromSeconds(1)));
            Storyboard.SetTarget(disappearAnimation, CardRectangle);
            Storyboard.SetTargetProperty(disappearAnimation, new PropertyPath("Fill.Color"));
            storyboard.Children.Add(disappearAnimation);
            storyboard.Completed += disappearCompleted;
            this.BeginStoryboard(storyboard);

        }

        /// <summary>
        /// Handles the event when the disappearing animation is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disappearCompleted(object sender, EventArgs e)
        {
            controller.removeCardFromTable(this);
        }

        /// <summary>
        /// Toggles the highlighting-state of the GameCard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            toggleHighlight();
        }
    }
}
