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
        public int Elements { get { return elements; } }
        private ElementShape shape;
        public ElementShape Shape { get { return shape; } }
        private ElementColor color;
        public ElementColor ElemColor { get { return color; } }
        private ElementFill fill;
        public ElementFill Fill { get { return fill; } }
        private bool disappearing = false;

        Controller controller;

        public GameCard(Controller controller, int elementCount, ElementShape shape, ElementColor color, ElementFill fill) {
            InitializeComponent();
            fillBrush.Color = Color.FromArgb(255, 51, 51, 51);
            this.controller = controller;
            if(elementCount < 1 || elementCount > 3)
                throw new ArgumentException("Invalid number of elements.");
            elements = elementCount;
            this.shape = shape;
            this.color = color;
            this.fill = fill;
            for(int i = 0; i < elementCount; i++)
            {
                CardElement tmp = new CardElement(shape, color, fill);
                addElement(tmp);
            }
            this.LayoutTransform = new ScaleTransform(0.5, 0.5);
//            this.RenderTransform = new ScaleTransform(0.5, 0.5);
        }

        public void addElement(CardElement elem) {
            ElementGrid.Children.Add(elem);
        }

        private bool highlighted = false;
        public bool Highlighted { 
            get { return highlighted; }
            set { toggleHighlight(); }
        }

        public void toggleHighlight(){
            if (disappearing)
                return;
            highlighted = !highlighted;
            if (!highlighted)
            {
                CardRectangle.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x33, 0x33));
                RenderTransform = new RotateTransform(0);
                controller.removeHighlighting();
            }
            else
            {
                CardRectangle.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x66, 0x66, 0x66));
                RenderTransform = new RotateTransform(10);
                controller.addHighlighting();
            }

        }

        /// <summary>
        /// Highlighting animation when help is requested to find a set.
        /// https://stackoverflow.com/questions/24184584/coloranimation-change-color-in-rectangle
        /// </summary>
        public void helpHighlight() {
            //CardRectangle.Fill = new SolidColorBrush(Colors.Black);
            //var storyboard = new Storyboard();
            //var colorFadeAnimation = new ColorAnimation(Colors.Black, new Duration(TimeSpan.FromSeconds(5)));
            //Storyboard.SetTarget(colorFadeAnimation, fillBrush);
            //Storyboard.SetTargetProperty(colorFadeAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
            //storyboard.Children.Add(colorFadeAnimation);
            //var colorFadeBackAnimation = new ColorAnimation(Color.FromArgb(255, 0x33, 0x33, 0x33), new Duration(TimeSpan.FromSeconds(5)));
            //Storyboard.SetTarget(colorFadeBackAnimation, fillBrush);
            //Storyboard.SetTargetProperty(colorFadeBackAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
            //storyboard.Children.Add(colorFadeBackAnimation);
            //this.BeginStoryboard(storyboard);
            CardRectangle.Fill = new SolidColorBrush(Colors.Black);
            var storyboard = new Storyboard();
            var colorFadeAnimation = new ColorAnimation(Color.FromArgb(255,0,0,0), new Duration(TimeSpan.FromSeconds(1)));
            colorFadeAnimation.From = Color.FromArgb(255, 51, 51, 51);
            Storyboard.SetTarget(colorFadeAnimation, CardRectangle);
            Storyboard.SetTargetProperty(colorFadeAnimation, new PropertyPath("Fill.Color"));
            storyboard.Children.Add(colorFadeAnimation);
            var colorFadeBackAnimation = new ColorAnimation(Color.FromArgb(255, 0x33, 0x33, 0x33), new Duration(TimeSpan.FromSeconds(2)));
            Storyboard.SetTarget(colorFadeBackAnimation, CardRectangle);
            Storyboard.SetTargetProperty(colorFadeBackAnimation, new PropertyPath("Fill.Color"));
            colorFadeBackAnimation.BeginTime = TimeSpan.FromSeconds(2);
            storyboard.Children.Add(colorFadeBackAnimation);
            this.BeginStoryboard(storyboard);
        }

        public void disappear() {
            disappearing = true;
            Storyboard storyboard = new Storyboard();
            ColorAnimation disappearAnimation = new ColorAnimation(Color.FromArgb(0,102,102,102), new Duration(TimeSpan.FromSeconds(1)));
            Storyboard.SetTarget(disappearAnimation, CardRectangle);
            Storyboard.SetTargetProperty(disappearAnimation, new PropertyPath("Fill.Color"));
            storyboard.Children.Add(disappearAnimation);
            storyboard.Completed += disappearCompleted;
            this.BeginStoryboard(storyboard);

        }

        private void disappearCompleted(object sender, EventArgs e)
        {
            controller.removeCardFromTable(this);
        }


        public GameCard()
        {
            InitializeComponent();
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            toggleHighlight();
        }
    }
}
