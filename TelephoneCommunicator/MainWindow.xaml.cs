using System;
using System.Windows;
/*
 TODO:     
    work with different input var (alpha, dep, ar, serv time)        
 */
namespace TelephoneCommunicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Task task;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {
            if (!IsCorrectInput())
            {
                MessageBox.Show("Хибні вхідні дані!\nЗверніть увагу, що числа можуть бути лише додатніми!\nДробові числа варто записувати у вигляді '0,3333333'.");
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
                MessageBox.Show($"Помилка!\n{ex.Message}");
                task = null;
            }
        }

        private bool IsCorrectInput()
        {
            return uint.TryParse(TextBoxNumberOfChannels.Text, out uint res)
                && decimal.TryParse(TextBoxServiceTime.Text, out decimal dec) && dec >= 0
                && decimal.TryParse(TextBoxAverageArrivaleRate.Text, out dec) && dec >= 0;
        }
    }
}
