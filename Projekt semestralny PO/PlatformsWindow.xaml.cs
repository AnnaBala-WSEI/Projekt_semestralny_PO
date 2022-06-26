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
using System.Windows.Shapes;

namespace Projekt_semestralny_PO
{
    /// <summary>
    /// Logika interakcji dla klasy PlatformsWindow.xaml
    /// </summary>
    public partial class PlatformsWindow : Window
    {
        VideoGamesPortalEntities db = new VideoGamesPortalEntities();

        public PlatformsWindow()
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

        private void clear_Form()
        {
            this.platformName.Text = "";
            this.platformReleaseYear.Text = "";
            platformType.SelectedItem = platformType.Items[0];
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            platform newPlatform = new platform() { platform_name = platformName.Text, release_year = short.Parse(platformReleaseYear.Text), platform_type = platformType.Text };

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

        private int platformIdToUpdate = 0;
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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            platform platformToUpdate = (from platform in db.platforms where platform.platform_id == this.platformIdToUpdate select platform).SingleOrDefault();

            if (platformToUpdate != null)
            {
                platformToUpdate.platform_name = this.platformName.Text;
                platformToUpdate.release_year = short.Parse(this.platformReleaseYear.Text);
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
    }
}
