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
    /// Logika interakcji dla klasy ProducersPage.xaml
    /// </summary>
    public partial class ProducersPage : Page
    {
        VideoGamesPortalEntities db = new VideoGamesPortalEntities();

        public ProducersPage()
        {
            InitializeComponent();

            var producers = from producer in db.producers
                            select new
                            {
                                ID = producer.producer_id,
                                Name = producer.producer_name,
                                BeginningOfActivity = producer.beginning_of_activity,
                            };

            this.gridProducers.ItemsSource = producers.ToList();
        }

        private void clear_Form()
        {
            this.producerName.Text = "";
            this.beginningOfActivity.SelectedDate = DateTime.Today;
        }

        private bool validateDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now;
            return date <= currentDate;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DateTime beginningOfActivityDate = (DateTime)this.beginningOfActivity.SelectedDate;

            if (validateDate(beginningOfActivityDate))
            {
                errorMessage.Visibility = Visibility.Hidden;

                producer newProducer = new producer() { producer_name = producerName.Text, beginning_of_activity = (DateTime)beginningOfActivity.SelectedDate };

                db.producers.Add(newProducer);
                db.SaveChanges();

                var producers = from producer in db.producers
                                select new
                                {
                                    ID = producer.producer_id,
                                    Name = producer.producer_name,
                                    BeginningOfActivity = producer.beginning_of_activity,
                                };

                this.gridProducers.ItemsSource = producers.ToList();
            }
            else errorMessage.Visibility = Visibility.Visible;
        }

        private int producerIdToUpdate = 0;
        private void gridProducers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.gridProducers.SelectedItems.Count == 1)
            {
                var producer = this.gridProducers.SelectedItems[0];

                this.producerName.Text = producer?.GetType().GetProperty("Name")?.GetValue(producer, null).ToString();
                this.beginningOfActivity.SelectedDate = (DateTime?)(producer?.GetType().GetProperty("BeginningOfActivity")?.GetValue(producer, null));

                this.producerIdToUpdate = (int)producer?.GetType().GetProperty("ID")?.GetValue(producer, null);
            }
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            DateTime beginningOfActivityDate = (DateTime)this.beginningOfActivity.SelectedDate;
            
            if (validateDate(beginningOfActivityDate))
            {
                errorMessage.Visibility = Visibility.Hidden;

                producer producerToUpdate = (from producer in db.producers where producer.producer_id == this.producerIdToUpdate select producer).SingleOrDefault();

                if (producerToUpdate != null)
                {
                    producerToUpdate.producer_name = this.producerName.Text;
                    producerToUpdate.beginning_of_activity = beginningOfActivityDate;

                    clear_Form();
                    db.SaveChanges();
                }

                var producers = from producer in db.producers
                                select new
                                {
                                    ID = producer.producer_id,
                                    Name = producer.producer_name,
                                    BeginningOfActivity = producer.beginning_of_activity,
                                };

                this.gridProducers.ItemsSource = producers.ToList();
            }
            else errorMessage.Visibility = Visibility.Visible;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedProducer = this.gridProducers.SelectedItems[0];
            string selectedProducerName = selectedProducer?.GetType().GetProperty("Name")?.GetValue(selectedProducer, null).ToString();

            MessageBoxResult answer = MessageBox.Show($"Are you sure you want to delete {selectedProducerName} producer?", "Delete producer", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            if (answer == MessageBoxResult.Yes)
            {
                producer producerToDelete = (from producer in db.producers where producer.producer_id == this.producerIdToUpdate select producer).SingleOrDefault();

                if (producerToDelete != null)
                {
                    db.producers.Remove(producerToDelete);
                    clear_Form();
                    db.SaveChanges();
                }

                var producers = from producer in db.producers
                                select new
                                {
                                    ID = producer.producer_id,
                                    Name = producer.producer_name,
                                    BeginningOfActivity = producer.beginning_of_activity,
                                };

                this.gridProducers.ItemsSource = producers.ToList();

            }

        }

        private void gridProducers_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "BeginningOfActivity")
            {
                DataGridTextColumn column = e.Column as DataGridTextColumn;
                Binding binding = column.Binding as Binding;
                binding.StringFormat = "dd-MM-yyyy";
            }
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NavigationPage());
        }
    }
}
