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
    /// Logika interakcji dla klasy PlatformsPage.xaml
    /// </summary>
    public partial class PlatformsPage : Page
    {

        VideoGamesPortalEntities db = new VideoGamesPortalEntities();
        public PlatformsPage()
        {
            InitializeComponent();

            var platforms = from platform in db.platforms
                            select new
                            {
                                ID = platform.platform_id,
                                Name = platform.platform_name,
                                ReleaseYear = platform.release_year,
                                Type = platform.platform_type,
                            };

            this.gridPlatforms.ItemsSource = platforms.ToList();
        }

        /// <summary>
        /// Void function clear_Form resets all form fields
        /// </summary>
        private void clear_Form()
        {
            this.platformName.Text = "";
            this.platformReleaseYear.Text = "";
            platformType.SelectedItem = platformType.Items[0];
        }

        /// <summary>
        /// Funtion validateYear is used to validate platformReleaseYear form field
        /// </summary>
        /// <returns>True when year is smaller or the same as current year, 
        /// False when year is bigger than current year</returns>
        private bool validateYear(short year)
        {
            int currentYear = DateTime.Now.Year;
            return year <= currentYear;
        }

        /// <summary>
        /// btnAdd_Click is a handler function for 'Add' button used for creating new record in database
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            short platformReleaseYearConverted = short.Parse(platformReleaseYear.Text);

            if (validateYear(platformReleaseYearConverted))
            {
                errorMessage.Visibility = Visibility.Hidden;
                
                platform newPlatform = new platform() { platform_name = platformName.Text, release_year = platformReleaseYearConverted, platform_type = platformType.Text };

                db.platforms.Add(newPlatform);
                db.SaveChanges();

                var platforms = from platform in db.platforms
                                select new
                                {
                                    ID = platform.platform_id,
                                    Name = platform.platform_name,
                                    ReleaseYear = platform.release_year,
                                    Type = platform.platform_type,
                                };

                this.gridPlatforms.ItemsSource = platforms.ToList();
            }
            else errorMessage.Visibility = Visibility.Visible;
        }

        private int platformIdToUpdate = 0;

        /// <summary>
        /// gridPlatforms_SelectionChanged is a handler function for selecting a row in datagrid list
        /// </summary>
        private void gridPlatforms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.gridPlatforms.SelectedItems.Count == 1)
            {
                var platform = this.gridPlatforms.SelectedItems[0];

                this.platformName.Text = platform?.GetType().GetProperty("Name")?.GetValue(platform, null).ToString();
                this.platformReleaseYear.Text = platform?.GetType().GetProperty("ReleaseYear")?.GetValue(platform, null).ToString();
                this.platformType.Text = (platform?.GetType().GetProperty("Type")?.GetValue(platform, null)).ToString();

                this.platformIdToUpdate = (int)platform?.GetType().GetProperty("ID")?.GetValue(platform, null);
            }
        }

        /// <summary>
        /// btnUpdate_Click is a handler function for 'Update' button used for updating a record in database
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            short platformReleaseYearConverted = short.Parse(platformReleaseYear.Text);

            if (validateYear(platformReleaseYearConverted))
            {
                errorMessage.Visibility = Visibility.Hidden;
                platform platformToUpdate = (from platform in db.platforms where platform.platform_id == this.platformIdToUpdate select platform).SingleOrDefault();

                if (platformToUpdate != null)
                {
                    platformToUpdate.platform_name = this.platformName.Text;
                    platformToUpdate.release_year = platformReleaseYearConverted;
                    platformToUpdate.platform_type = this.platformType.Text;

                    clear_Form();
                    db.SaveChanges();
                }

                var platforms = from platform in db.platforms
                                select new
                                {
                                    ID = platform.platform_id,
                                    Name = platform.platform_name,
                                    ReleaseYear = platform.release_year,
                                    Type = platform.platform_type,
                                };

                this.gridPlatforms.ItemsSource = platforms.ToList();
            }
            else errorMessage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// btnDelete_Click is a handler function for 'Delete' button used for deleting a record in database
        /// </summary>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedPlatform = this.gridPlatforms.SelectedItems[0];
            string selectedPlatformName = selectedPlatform?.GetType().GetProperty("Name")?.GetValue(selectedPlatform, null).ToString();

            MessageBoxResult answer = MessageBox.Show($"Are you sure you want to delete {selectedPlatformName} platform?", "Delete platform", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            if (answer == MessageBoxResult.Yes)
            {
                platform platformToDelete = (from platform in db.platforms where platform.platform_id == this.platformIdToUpdate select platform).SingleOrDefault();

                if (platformToDelete != null)
                {
                    db.platforms.Remove(platformToDelete);
                    clear_Form();
                    db.SaveChanges();
                }

                var platforms = from platform in db.platforms
                                select new
                                {
                                    ID = platform.platform_id,
                                    Name = platform.platform_name,
                                    ReleaseYear = platform.release_year,
                                    Type = platform.platform_type,
                                };

                this.gridPlatforms.ItemsSource = platforms.ToList();
            }

        }

        /// <summary>
        /// btnMenu_Click is a handler function for 'Go back to main page' button used for navigating to MainWindow
        /// </summary>
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NavigationPage());
        }
    }
}
