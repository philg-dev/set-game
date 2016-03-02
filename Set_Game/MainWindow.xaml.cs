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
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller controller;

        public MainWindow()
        {
            InitializeComponent();

            controller = new Controller(this);
        }

        private void logTableButton_Click(object sender, RoutedEventArgs e)
        {
            controller.logTable();
        }

        private void noSetAvailable_Click(object sender, RoutedEventArgs e)
        {
            controller.handleNoSetAvailableCheck();
        }

        private void checkAvailableSetButton_Click(object sender, RoutedEventArgs e)
        {
            controller.handleHelpRequest();
        }
    }
}
