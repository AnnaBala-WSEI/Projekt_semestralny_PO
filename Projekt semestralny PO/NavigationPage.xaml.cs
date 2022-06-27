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

namespace Projekt_semestralny_PO
{
    /// <summary>
    /// Logika interakcji dla klasy NavigationPage.xaml
    /// </summary>
    public partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            InitializeComponent();
        }

        private void navigateGames_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new GamesPage());
        }

        private void navigateGenres_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new GenresPage());
        }

        private void navigateProducers_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProducersPage());
        }

        private void navigatePlatforms_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PlatformsPage());
        }
    }
}
