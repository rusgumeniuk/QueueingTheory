using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AntiAircraftComplex
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
            try
            {
                Task task = new Task(GetAverageArrivalRate(), GetAverageServiceTimes(), GetDestroyProbabilities());
                task.Solve();
                MessageBox.Show(task.GetResult());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private IList<decimal> GetAverageServiceTimes()
        {
            IList<decimal> values = new List<decimal>();
            for (int i = 0; i < MainGrid.Children.Count; ++i)
            {
                if (MainGrid.Children[i] is TextBox && (MainGrid.Children[i] as TextBox).Name.Contains("TextBoxServiceTime"))
                {
                    var text = (MainGrid.Children[i] as TextBox).Text;
                    if (!decimal.TryParse(text, out decimal result) || result < 0)
                    {                        
                        throw new ArgumentException($"Неправильне значення в полі введення часу обслуговування №{i + 1}");
                    }
                    else
                        values.Add(result/60);
                }
            }
            return values;
        }
        private IList<decimal> GetDestroyProbabilities()
        {
            IList<decimal> values = new List<decimal>();
            for (int i = 0; i < MainGrid.Children.Count; ++i)
            {
                if (MainGrid.Children[i] is TextBox && (MainGrid.Children[i] as TextBox).Name.Contains("TextBoxDestroyProbability"))
                {
                    var text = (MainGrid.Children[i] as TextBox).Text;
                    if (!decimal.TryParse(text, out decimal result) || result < 0 || result > 100)
                    {
                        throw new ArgumentException($"Неправильне значення в полі введення ймовірності збиття №{i + 1}");
                    }
                    else
                        values.Add(result/100);
                }
            }
            return values;
        }        
        private decimal GetAverageArrivalRate()
        {
            return decimal.TryParse(TextBoxAverageArrivalRate.Text, out var result) ? result : throw new ArgumentException("Невірно введена інтенсивність прольотів літаків!");
        }
    }
}
