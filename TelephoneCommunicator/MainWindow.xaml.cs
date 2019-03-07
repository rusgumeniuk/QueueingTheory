using System;
using System.Windows;
/*
 TODO:     
    work with different input var (alpha, dep, ar, serv time)    
    validate input decimal (it should be only positive)
 */
namespace TelephoneCommunicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Task task;
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
                task = new Task(uint.Parse(TextBoxNumberOfChannels.Text), decimal.Parse(TextBoxAverageArrivaleRate.Text), decimal.Parse(TextBoxServiceTime.Text));
                task.Solve();
                TextBlockProbablities.Text = task.GetProbabilities();
                TextBlockResult.Text = task.GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error!\n{ex.Message}");
                task = null;
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
