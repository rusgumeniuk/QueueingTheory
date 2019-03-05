using System;
using System.Windows;
/*
 TODO: 
    error when n > 20
    work with different input var (alpha, dep, ar, serv time)
 */ 
namespace TelephoneCommunicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();        
        }

        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {

            if (!IsCorrectInput())
            {
                MessageBox.Show("Wrong input!");
                return;
            }
            try
            {
                Task task = new Task(uint.Parse(TextBoxNumberOfChannels.Text), decimal.Parse(TextBoxAverageArrivaleRate.Text), decimal.Parse(TextBoxServiceTime.Text));
                task.Solve();
                TextBlockResult.Text = task.GetResult();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error!\n{ex.Message}");
            }
        }

        private bool IsCorrectInput()
        {
            return uint.TryParse(TextBoxNumberOfChannels.Text, out uint res)
                && decimal.TryParse(TextBoxServiceTime.Text, out decimal dec)
                && decimal.TryParse(TextBoxAverageArrivaleRate.Text, out dec);
        }
    }
}
