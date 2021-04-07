using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Set_Game
{
    public enum ElementBrushType
    {
        empty = 1,
        filled = 2,
        striped = 3,
        polkadot = 4
    }

    public class ElementBrush
    {
        static string[] ElementBrushes = new string[] { "empty", "filled", "striped", "polka-dot" };


        //internal string name;
        //internal Brush brush;

        //private ElementBrush(string name, Brush brush)
        //{
        //    this.name = name;
        //    this.brush = brush;
        //}

        //public override string ToString()
        //{
        //    return name;
        //}
        internal static Brush GetInstance(ElementBrushType type, Color color)
        {
            switch (type)
            {
                case ElementBrushType.empty: return new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                case ElementBrushType.filled: return new SolidColorBrush(color);
                case ElementBrushType.striped: return getStripedBrush(color);
                case ElementBrushType.polkadot: return getPolkaDotBrush(color, Settings.dotSize);
            }
            throw new InvalidOperationException("Invalid ElementBrushType.");
        }

        /// <summary>
        /// Generates a LinearGradientBrush of the given color which is striped.
        /// </summary>
        /// <param name="color">The desired color of the stripes</param>
        /// <returns>The LinearGradientBrush.</returns>
        private static LinearGradientBrush getStripedBrush(Color color)
        {
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
        /// Returns a polka-dot brush.
        /// </summary>
        /// <param name="color">The desired Color of the dots.</param>
        /// <param name="dotSize">The size of the dots.</param>
        /// <returns></returns>
        private static DrawingBrush getPolkaDotBrush(Color color, double dotSize)
        {
            SolidColorBrush colorBrush = new SolidColorBrush(color);
            CombinedGeometry combined = new CombinedGeometry();
            combined.GeometryCombineMode = GeometryCombineMode.Union;

            Geometry ellipse1 = new EllipseGeometry(new Rect(0, 0, dotSize, dotSize));
            Geometry ellipse2 = new EllipseGeometry(new Rect(dotSize + 2 * Settings.dotStrokeThickness, dotSize + 2 * Settings.dotStrokeThickness, dotSize, dotSize));
            combined.Geometry1 = ellipse1;
            combined.Geometry2 = ellipse2;
            Drawing drawing = new GeometryDrawing(colorBrush, new Pen(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)), Settings.dotStrokeThickness), combined);
            DrawingBrush polkaDotBrush = new DrawingBrush(drawing);
            polkaDotBrush.TileMode = TileMode.Tile;
            polkaDotBrush.RelativeTransform = new ScaleTransform(Settings.dotScaling, Settings.dotScaling);
            return polkaDotBrush;
        }



    }
}
