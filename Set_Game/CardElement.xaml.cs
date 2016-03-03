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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Set_Game
{
    public enum ElementShape
    {
        bolt = 1,
        rhombus = 2,
        rect = 3
    }

    public enum ElementFill
    {
        empty = 1,
        striped = 2,
        filled = 3
    }



    /// <summary>
    /// Interaktionslogik für CardElement.xaml
    /// </summary>
    public partial class CardElement : UserControl
    {

        /// <summary>
        /// Generates a LinearGradientBrush of the given color which is striped.
        /// </summary>
        /// <param name="color">The desired color of the stripes</param>
        /// <returns>The LinearGradientBrush.</returns>
        private LinearGradientBrush getGradientBrush(Color color) {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(1, 1);
            brush.SpreadMethod = GradientSpreadMethod.Repeat;
            GradientStopCollection stopCollection = new GradientStopCollection();
            stopCollection.Add(new GradientStop(color, 0));
            stopCollection.Add(new GradientStop(color, 0.5));
            stopCollection.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.5));
            stopCollection.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));

            brush.GradientStops = stopCollection;

            brush.RelativeTransform = new ScaleTransform(Settings.StripeDistance, Settings.StripeDistance);
            return brush;
        }

        /// <summary>
        /// Creates a CardElement.
        /// </summary>
        /// <param name="shape">Desired shape.</param>
        /// <param name="color">Desired color.</param>
        /// <param name="fill">Desired fill.</param>
        public CardElement(ElementShape shape, Color color, ElementFill fill)
        {
            InitializeComponent();

            switch (shape)
            {
                case ElementShape.bolt: 
                    // in XAML
                    break;
                case ElementShape.rect:
                    Polygon.Points.Clear();
                    Polygon.Points.Add(new Point(0, 5));
                    Polygon.Points.Add(new Point(140, 5));
                    Polygon.Points.Add(new Point(140, 75));
                    Polygon.Points.Add(new Point(0, 75));
                    break;
                case ElementShape.rhombus:
                    Polygon.Points.Clear();
                    Polygon.Points.Add(new Point(0,35));
                    Polygon.Points.Add(new Point(70,0));
                    Polygon.Points.Add(new Point(140,35));
                    Polygon.Points.Add(new Point(70,70));
                    Polygon.Points.Add(new Point(0,35));
                    break;
            }

            Polygon.StrokeThickness = Settings.CardElementStrokeThickness;

            var elementColor = new SolidColorBrush(color);
            this.Polygon.Stroke = elementColor;
            switch (fill)
            {
                case ElementFill.empty: Polygon.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case ElementFill.filled: Polygon.Fill = elementColor; break;
                case ElementFill.striped:
                    Polygon.Fill = getGradientBrush(color);
                    break;
            }

            this.LayoutTransform = new ScaleTransform(Settings.CardElementSizeFactor, Settings.CardElementSizeFactor);
        }

    }
}
