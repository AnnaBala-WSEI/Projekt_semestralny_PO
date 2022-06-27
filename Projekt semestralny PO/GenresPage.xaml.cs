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
    /// Logika interakcji dla klasy GenresPage.xaml
    /// </summary>
    public partial class GenresPage : Page
    {

        VideoGamesPortalEntities db = new VideoGamesPortalEntities();

        public GenresPage()
        {
            InitializeComponent();

            var genres = from genre in db.genres
                         select new
                         {
                             ID = genre.genre_id,
                             Name = genre.genre_name,
                         };

            this.gridGenres.ItemsSource = genres.ToList();
        }

        private void clear_Form()
        {
            this.genreName.Text = "";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            genre newGenre = new genre() { genre_name = genreName.Text };

            db.genres.Add(newGenre);
            db.SaveChanges();

            var genres = from genre in db.genres
                         select new
                         {
                             ID = genre.genre_id,
                             Name = genre.genre_name,
                         };

            this.gridGenres.ItemsSource = genres.ToList();
        }

        private int genreIdToUpdate = 0;
        private void gridGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.gridGenres.SelectedItems.Count == 1)
            {
                var genre = this.gridGenres.SelectedItems[0];

                this.genreName.Text = genre?.GetType().GetProperty("Name")?.GetValue(genre, null).ToString();
                this.genreIdToUpdate = (int)genre?.GetType().GetProperty("ID")?.GetValue(genre, null);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            genre genreToUpdate = (from genre in db.genres where genre.genre_id == this.genreIdToUpdate select genre).SingleOrDefault();

            if (genreToUpdate != null)
            {
                genreToUpdate.genre_name = this.genreName.Text;

                clear_Form();
                db.SaveChanges();
            }

            var genres = from genre in db.genres
                         select new
                         {
                             ID = genre.genre_id,
                             Name = genre.genre_name,
                         };

            this.gridGenres.ItemsSource = genres.ToList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenre = this.gridGenres.SelectedItems[0];
            string selectedGenreTitle = selectedGenre?.GetType().GetProperty("Name")?.GetValue(selectedGenre, null).ToString();

            MessageBoxResult answer = MessageBox.Show($"Are you sure you want to delete {selectedGenreTitle} genre?", "Delete genre", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            if (answer == MessageBoxResult.Yes)
            {
                genre genreToDelete = (from genre in db.genres where genre.genre_id == this.genreIdToUpdate select genre).SingleOrDefault();

                if (genreToDelete != null)
                {
                    db.genres.Remove(genreToDelete);
                    clear_Form();
                    db.SaveChanges();
                }

                var genres = from genre in db.genres
                             select new
                             {
                                 ID = genre.genre_id,
                                 Name = genre.genre_name,
                             };

                this.gridGenres.ItemsSource = genres.ToList();
            }

        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NavigationPage());
        }
    }
}
