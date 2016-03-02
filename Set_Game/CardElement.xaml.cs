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

    public enum ElementColor
    {
        orange = 1,
        blue = 2,
        white = 3
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
        ElementShape shape;
        ElementColor color;
        ElementFill fill;

        private LinearGradientBrush getGradientBrush(Color color) {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(1, 1);
            brush.SpreadMethod = GradientSpreadMethod.Repeat;
            GradientStopCollection stopCollection = new GradientStopCollection();
            stopCollection.Add(new GradientStop(color, 0));
            stopCollection.Add(new GradientStop(color, 0.5));
            stopCollection.Add(new GradientStop(Color.FromArgb(0,0,0,0), 0.5));
            stopCollection.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));

            brush.GradientStops = stopCollection;

            brush.RelativeTransform = new ScaleTransform(0.1, 0.1);
            return brush;
        }

        /// <summary>
        /// Creates a CardElement.
        /// </summary>
        /// <param name="shape">Desired shape.</param>
        /// <param name="color">Desired color.</param>
        /// <param name="fill">Desired fill.</param>
        public CardElement(ElementShape shape, ElementColor color, ElementFill fill)
        {
            InitializeComponent();
            this.shape = shape;
            this.color = color;
            this.fill = fill;

            switch (shape)
            {
                case ElementShape.bolt: break;
                case ElementShape.rect:
                    //Polygon = new Polygon();
                    Polygon.Points.Clear();
                    Polygon.Points.Add(new Point(0, 5));
                    Polygon.Points.Add(new Point(140, 5));
                    Polygon.Points.Add(new Point(140, 75));
                    Polygon.Points.Add(new Point(0, 75));
                    Polygon.StrokeThickness = 2;
                    break;
                case ElementShape.rhombus:
                    //Polygon = new Polygon();
                    Polygon.Points.Clear();
                    Polygon.Points.Add(new Point(0,35));
                    Polygon.Points.Add(new Point(70,0));
                    Polygon.Points.Add(new Point(140,35));
                    Polygon.Points.Add(new Point(70,70));
                    Polygon.Points.Add(new Point(0,35));
                    Polygon.StrokeThickness = 2;
                    break;
            }

            var elementColor = new SolidColorBrush(getColor(color));
            this.Polygon.Stroke = elementColor;
            switch (fill)
            {
                case ElementFill.empty: Polygon.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case ElementFill.filled: Polygon.Fill = elementColor; break;
                case ElementFill.striped:
                    Polygon.Fill = getGradientBrush(getColor(color));
                    break;
            }

            this.LayoutTransform = new ScaleTransform(0.9, 0.9);
        }

        public CardElement()
        {
            InitializeComponent();
        }

        static Color getColor(ElementColor color) {
            switch (color)
            {
                case ElementColor.blue:   return Color.FromArgb(255, 0, 0, 255);
                case ElementColor.orange: return Color.FromArgb(0xFF,0xEE, 0x4D, 0x2E);
                case ElementColor.white:  return Colors.White;
            }
            throw new ArgumentException("Invalid color.");
        }
    }
}
