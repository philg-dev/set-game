using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Set_Game
{
    public class ElementShape
    {
        internal Shape shape;
        private string name;

        internal ElementShape(string name, Shape shape)
        {
            this.name = name;
            this.shape = shape;
        }

        public override string ToString()
        {
            return name;
        }

        static ElementShape(){
            initShapes();
        }

        internal static List<ElementShape> elemShapes = new List<ElementShape>();

        private static void initShapes()
        {
            // bolt polygon
            Polygon boltShape = new Polygon();
            boltShape.Points = new PointCollection(new List<Point>
                    (new Point[] { new Point(0, 35), new Point(85, 0), new Point(55, 30), new Point(135, 35), new Point(45, 70), new Point(75, 40) })
                ); ;
            elemShapes.Add(new ElementShape("Bolt", boltShape));
            // rhombus polygon
            Polygon rhombusShape = new Polygon();
            rhombusShape.Points = new PointCollection(new List<Point>
                    (new Point[] { new Point(0, 35), new Point(70, 0), new Point(140, 35), new Point(70, 70), new Point(0, 35) })
                ); ;
            elemShapes.Add(new ElementShape("Rhombus", rhombusShape));
            // rectangle
            Rectangle rectangleShape = new Rectangle();
            rectangleShape.Width = 130;
            rectangleShape.Height = 70;
            rectangleShape.HorizontalAlignment = HorizontalAlignment.Center;
            rectangleShape.VerticalAlignment = VerticalAlignment.Center;
            elemShapes.Add(new ElementShape("Rectangle", rectangleShape));
            // ellipse
            Ellipse ellipseShape = new Ellipse();
            ellipseShape.Width = 130;
            ellipseShape.Height = 70;
            ellipseShape.HorizontalAlignment = HorizontalAlignment.Center;
            ellipseShape.VerticalAlignment = VerticalAlignment.Center;
            elemShapes.Add(new ElementShape("Ellipse", ellipseShape));
        }
    }

   
}
