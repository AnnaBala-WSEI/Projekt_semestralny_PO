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
    /// Logika interakcji dla klasy GamesWindow.xaml
    /// </summary>
    public partial class GamesWindow : Window
    {
        VideoGamesPortalEntities db = new VideoGamesPortalEntities();

        public GamesWindow()
        {
            InitializeComponent();

            var games = from game in db.games
                        select new
                        {
                            ID = game.game_id,
                            Title = game.game_title,
                            ReleaseYear = game.release_year,
                            IsASerie = game.is_a_serie,
                        };

            this.gridVideoGames.ItemsSource = games.ToList();
        }

        private void clear_Form()
        {
            this.gameTitle.Text = "";
            this.gameReleaseYear.Text = "";
            this.gameIsASerie.IsChecked = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            game newGame = new game() { game_title = gameTitle.Text, release_year = short.Parse(gameReleaseYear.Text), is_a_serie = (bool)gameIsASerie.IsChecked };

            db.games.Add(newGame);
            db.SaveChanges();

            var games = from game in db.games
                        select new
                        {
                            ID = game.game_id,
                            Title = game.game_title,
                            ReleaseYear = game.release_year,
                            IsASerie = game.is_a_serie,
                        };

            this.gridVideoGames.ItemsSource = games.ToList();
        }

        private int gameIdToUpdate = 0;
        private void gridVideoGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.gridVideoGames.SelectedItems.Count == 1)
            {
                var game = this.gridVideoGames.SelectedItems[0];

                this.gameTitle.Text = game?.GetType().GetProperty("Title")?.GetValue(game, null).ToString();
                this.gameReleaseYear.Text = game?.GetType().GetProperty("ReleaseYear")?.GetValue(game, null).ToString();
                this.gameIsASerie.IsChecked = (bool)game?.GetType().GetProperty("IsASerie")?.GetValue(game, null);

                this.gameIdToUpdate = (int)game?.GetType().GetProperty("ID")?.GetValue(game, null);
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            game gameToUpdate = (from game in db.games where game.game_id == this.gameIdToUpdate select game).SingleOrDefault();

            if (gameToUpdate != null)
            {
                gameToUpdate.game_title = this.gameTitle.Text;
                gameToUpdate.release_year = short.Parse(this.gameReleaseYear.Text);
                gameToUpdate.is_a_serie = (bool)this.gameIsASerie.IsChecked;

                clear_Form();
                db.SaveChanges();
            }


            var games = from game in db.games
                        select new
                        {
                            ID = game.game_id,
                            Title = game.game_title,
                            ReleaseYear = game.release_year,
                            IsASerie = game.is_a_serie,
                        };

            this.gridVideoGames.ItemsSource = games.ToList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedGame = this.gridVideoGames.SelectedItems[0];
            string selectedGameTitle = selectedGame?.GetType().GetProperty("Title")?.GetValue(selectedGame, null).ToString();

            MessageBoxResult answer = MessageBox.Show($"Are you sure you want to delete {selectedGameTitle} game?", "Delete game", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            if (answer == MessageBoxResult.Yes)
            {
                game gameToDelete = (from game in db.games where game.game_id == this.gameIdToUpdate select game).SingleOrDefault();

                if (gameToDelete != null)
                {
                    db.games.Remove(gameToDelete);
                    clear_Form();
                    db.SaveChanges();
                }

                var games = from game in db.games
                            select new
                            {
                                ID = game.game_id,
                                Title = game.game_title,
                                ReleaseYear = game.release_year,
                                IsASerie = game.is_a_serie,
                            };

                this.gridVideoGames.ItemsSource = games.ToList();

            }

        }
    }
}
