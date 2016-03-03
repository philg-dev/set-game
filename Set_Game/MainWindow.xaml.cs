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
        private static RoutedCommand helpRequestCommand = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();
            controller = new Controller(this);

            CommandBinding helpRequestCommandBinding = new CommandBinding(helpRequestCommand, ExecutedHelpRequestCommand, CanExecuteHelpRequestCommand);
            CommandBindings.Add(helpRequestCommandBinding);
            helpButton.Command = helpRequestCommand;

            KeyBinding helpKeyBinding = new KeyBinding();
            helpKeyBinding.Command = helpRequestCommand;
            helpKeyBinding.Key = Key.F1;
            InputBindings.Add(helpKeyBinding);

        }

        private void noSetAvailable_Click(object sender, RoutedEventArgs e)
        {
            controller.handleNoSetAvailableCheck();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(this, "Möchtest du ein neues Spiel starten?", "Neues Spiel", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (messageBoxResult == MessageBoxResult.Yes)
                controller.initGame();
        }

        private void ExecutedHelpRequestCommand(object sender, ExecutedRoutedEventArgs e)
        {
            controller.handleHelpRequest();
        }


        private void CanExecuteHelpRequestCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            Control target = e.Source as Control;

            if (target != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

    }
}
