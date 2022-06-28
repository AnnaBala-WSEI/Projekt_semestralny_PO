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

        /// <summary>
        /// Void function clear_Form resets all form fields
        /// </summary>
        private void clear_Form()
        {
            this.producerName.Text = "";
            this.beginningOfActivity.SelectedDate = DateTime.Today;
        }

        /// <summary>
        /// Funtion validateDate is used to validate beginningOfActivity form field
        /// </summary>
        /// <returns>True when date is smaller or the same as current date, 
        /// False when date is bigger than current date</returns>
        private bool validateDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now;
            return date <= currentDate;
        }

        /// <summary>
        /// btnAdd_Click is a handler function for 'Add' button used for creating new record in database
        /// </summary>
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

        /// <summary>
        /// gridProducers_SelectionChanged is a handler function for selecting a row in datagrid list
        /// </summary>
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

        /// <summary>
        /// btnUpdate_Click is a handler function for 'Update' button used for updating a record in database
        /// </summary>
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

        /// <summary>
        /// btnDelete_Click is a handler function for 'Delete' button used for deleting a record in database
        /// </summary>
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

        /// <summary>
        /// gridProducers_AutoGeneratingColumn funtion changes formating in 
        /// datagrid BeginningOfActivity column so that date displays in "dd-MM-yyyy" format
        /// </summary>
        private void gridProducers_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "BeginningOfActivity")
            {
                DataGridTextColumn column = e.Column as DataGridTextColumn;
                Binding binding = column.Binding as Binding;
                binding.StringFormat = "dd-MM-yyyy";
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
