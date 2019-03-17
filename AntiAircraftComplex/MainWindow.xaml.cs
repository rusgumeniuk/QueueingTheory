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
        public ObservableCollection<UIElement> Controls { get; set; } = new ObservableCollection<UIElement>();
        private uint countOfComplex = 0;
        public MainWindow()
        {
            InitializeComponent();
            VarGrid.DataContext = this;
        }

        private void TextBoxNumberOfComples_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TextBoxNumberOfComples.Text))
                return;
            if (!uint.TryParse(TextBoxNumberOfComples.Text, out countOfComplex))
            {
                MessageBox.Show("Помилка вводу!\nВведіть натуральне число.");
                return;
            }
            AddNewPanels(countOfComplex);
        }
        private void AddNewPanels(uint count)
        {
            Controls.Clear();
            for (uint i = 0; i < count; ++i)
            {
                AddNewUIElements(i);
            }
        }
        private void AddNewUIElements(uint index)
        {
            Controls.Add(new Label { Content = $"Введіть середню тривалість циклу стрільби комплексу №{index + 1}:" });
            Controls.Add(new TextBox { Name = $"TextBoxServiceTime{index + 1}" });
            Controls.Add(new Label { Content = $"Введіть ймовірність враження літака комплексом №{index + 1}:" });
            Controls.Add(new TextBox { Name = $"TextBoxDestroyProbability{index + 1}" });
            Controls.Add(new Separator());
        }       

        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (countOfComplex < 1 || !uint.TryParse(TextBoxAverageArrivalRate.Text, out uint averageArrivalRate))
                    throw new ArgumentException("Введено хибні дані. Будь ласка спробуйте ще раз.");
                Task task = new Task(countOfComplex, averageArrivalRate, GetAverageServiceTimes(), GetDestroyProbabilities());
                task.Solve();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private IList<decimal> GetAverageServiceTimes()
        {
            IList<decimal> values = new List<decimal>();
            for (int i = 0; i < Controls.Count; ++i)
            {
                if (Controls[i] is TextBox && (Controls[i] as TextBox).Name.Contains("TextBoxServiceTime"))
                {
                    var text = (Controls[i] as TextBox).Text;
                    if (!decimal.TryParse(text, out decimal result))
                    {
                        break;
                        throw new ArgumentException($"Неправильне значення в полі введення часу обслуговування №{i + 1}");
                    }
                    else
                        values.Add(result);
                }
            }
            return values;
        }
        private IList<decimal> GetDestroyProbabilities()
        {
            IList<decimal> values = new List<decimal>();
            for (int i = 0; i < Controls.Count; ++i)
            {
                if (Controls[i] is TextBox && (Controls[i] as TextBox).Name.Contains("TextBoxDestroyProbability"))
                {
                    var text = (Controls[i] as TextBox).Text;
                    if (!decimal.TryParse(text, out decimal result))
                    {
                        throw new ArgumentException($"Неправильне значення в полі введення ймовірності збиття №{i + 1}");
                    }
                    else
                        values.Add(result);
                }
            }
            return values;
        }        
    }
}
