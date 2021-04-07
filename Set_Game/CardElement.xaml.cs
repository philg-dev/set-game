using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Set_Game
{






    /// <summary>
    /// Interaktionslogik für CardElement.xaml
    /// </summary>
    public partial class CardElement : UserControl
    {

 

        /// <summary>
        /// Creates a CardElement.
        /// </summary>
        /// <param name="elemShape">Desired shape.</param>
        /// <param name="color">Desired color.</param>
        /// <param name="brushType">Desired fill.</param>
        public CardElement(ElementShape elemShape, Color color, ElementBrushType brushType)
        {
            InitializeComponent();

            //-----------------------
            string saved = XamlWriter.Save(elemShape.shape);

            Shape targetShape = (Shape)XamlReader.Load(XmlReader.Create(new StringReader(saved)));

            //------------------

            var elementColor = new SolidColorBrush(color);
            targetShape.StrokeThickness = Settings.CardElementStrokeThickness;
            targetShape.Stroke = elementColor;
            targetShape.Fill = ElementBrush.GetInstance(brushType, color);
//            switch (brushType)
//            {
//                case ElementFill.empty: targetShape.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
//                case ElementFill.filled: targetShape.Fill = elementColor; break;
////                case ElementFill.striped: targetShape.Fill = getGradientBrush(color); break;
//                case ElementFill.striped: targetShape.Fill = getPolkaDotBrush(color, Settings.dotSize); break;
//            }

            elementGrid.Children.Clear();
            elementGrid.Children.Add(targetShape);

            this.LayoutTransform = new ScaleTransform(Settings.CardElementSizeFactor, Settings.CardElementSizeFactor);
        }

    }
}
