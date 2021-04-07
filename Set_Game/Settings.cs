using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Set_Game
{


    internal static class Settings
    {

        #region general
        /// <summary>
        /// Indicates whether cards should be drawn from the deck while the cards on the Table do not contain any valid set.
        /// </summary>
        internal static bool drawUntilSetAvailable = true;
        /// <summary>
        /// The amount of cards that's required to make a set.
        /// </summary>
        internal static int CardsPerSet = 3;
        /// <summary>
        /// The amount of cards that's initially drawn from the deck. This is also the amount of cards that will normally
        /// be on the Table when the deck is not yet empty and a set is available.
        /// </summary>
        internal static int DefaultCardsOnTable = CardsPerSet*(CardsPerSet+1);
        #endregion

        #region CardElement

        /// <summary>
        /// The colors of the Gamecards' CardElements. Default: blue, pr0gramm orange, white.
        /// </summary>
        internal static Color[] elementColors = new Color[] { Color.FromArgb(255, 0, 0, 255), Color.FromArgb(255, 0xEE, 0x4D, 0x2E), Colors.White, Colors.Crimson };
        /// <summary>
        /// The thickness of the outline-strokes of the CardElements (noticeable with striped or empty elements).
        /// </summary>
        internal static double CardElementStrokeThickness = 4;
        /// <summary>
        /// The size scale of the CardElements. Default: 0.9
        /// </summary>
        internal static double CardElementSizeFactor = 0.9;
        /// <summary>
        /// The distance between the stripes for the stripedBrush.
        /// For n: amount of stripes per element the value is ~1/n.
        /// </summary>
        internal static double StripeDistance = 0.15;
        /// <summary>
        /// The distance between the dots for the polkaDotBrush.
        /// For n: amount of dots per element the value is ~1/n.
        /// </summary>
        internal static double dotScaling= 0.3;
        /// <summary>
        /// The thickness of the fully transparent stroke of the dots in the polkaDotBrush.
        /// </summary>
        internal static double dotStrokeThickness = 0;
        /// <summary>
        /// The size of the dots in the polkaDotBrush.
        /// </summary>
        internal static double dotSize = 5;

        #region Shapes
        //public enum ElementShape
        //{
        //    bolt = 1,
        //    rhombus = 2,
        //    rect = 3,
        //    triangle = 4
        //}
        //internal static List<List<Point>> shapes = new List<List<Point>>(); //new List<Point>[] 
     

        //internal static Point[] boltShape = new Point[] { new Point(0, 35), new Point(85, 0), new Point(55, 30), new Point(135, 35), new Point(45, 70), new Point(75, 40) };
        //internal static Point[] rectShape = new Point[] { new Point(0, 5), new Point(140, 5), new Point(140, 75), new Point(0, 75) };
        //internal static Point[] rhombusShape = new Point[] { new Point(0, 35), new Point(70, 0), new Point(140, 35), new Point(70, 70), new Point(0, 35) };
        #endregion

        #endregion

        #region GameCard
        /// <summary>
        /// The maximum amount of elements per Card.
        /// </summary>
        internal static int MaxNumberOfElementsPerCard = 4;
        /// <summary>
        /// The size scale of the GameCards.
        /// </summary>
        internal static double GameCardSizeFactor = 0.5; 
        /// <summary>
        /// The background color of the GameCards.
        /// </summary>
        internal static Color GameCardBackgroundColor = Color.FromArgb(255, 51, 51, 51);
        /// <summary>
        /// The background color of the GameCards while highlighted.
        /// </summary>
        internal static Color GameCardHighlightColor = Color.FromArgb(255, 102, 102, 102);

        /// <summary>
        /// The color of the DropShadowEffect on the GameCard when requesting help.
        /// </summary>
        internal static Color helpDropShadowColor = Colors.Yellow;
        /// <summary>
        /// The BlurRadius of the DropShadowEffect on the GameCard when requesting help.
        /// </summary>
        internal static double helpDropShadowBlurRadius = 20;
        /// <summary>
        /// The ShadowDepth of the DropShadowEffect on the GameCard when requesting help.
        /// </summary>
        internal static double helpDropShadowDepth = 0;

        #endregion




    }
}
