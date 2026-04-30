using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MegaloPeos
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public StackPanel selectedPanel;

        public MainPage()
        {
            InitializeComponent();
            Manager.controlFrame = ControlFrame;
            ControlFrame.Navigate(new ProfilePage());
            selectedPanel = profileStackPanel;
        }

        private void ControlFrame_Navigated(object sender, NavigationEventArgs e)
        {
            selectedPanel.Background = Brushes.LightGray;
        }

        private void coworkersStackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedPanel.Background = null;
            selectedPanel = coworkersStackPanel;
            ControlFrame.Navigate(new CoworkersPage());
        }

        private void profileStackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedPanel.Background = null;
            selectedPanel = profileStackPanel;
            ControlFrame.Navigate(new ProfilePage());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new LogInPage());
        }

        private void equipmentStackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedPanel.Background = null;
            selectedPanel = equipmentStackPanel;
            if(Manager.role != "Администратор")
            {
                ControlFrame.Navigate(new EquipmentPage());
            }
            else
            {
                ControlFrame.Navigate(new LockedPage());
            }
            
        }
    }
}
