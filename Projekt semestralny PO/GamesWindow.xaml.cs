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
        public GamesWindow()
        {
            InitializeComponent();

            VideoGamesPortalEntities db = new VideoGamesPortalEntities();
            
            var games = from game in db.games select new {
                ID = game.game_id,
                Title = game.game_title,
                ReleaseYear = game.release_year,
                IsASerie =  game.is_a_serie,
            };

            this.gridVideoGames.ItemsSource = games.ToList();
        }
    }
}
