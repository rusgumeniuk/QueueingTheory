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
            Task task = new Task(4, 1 / 3, 6, 3);
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < task.Probabilities.Length; ++i)
            {
                sb.AppendLine($"#{i}: {task.Probabilities[i]}");
            }
            MessageBox.Show(sb.ToString());
        }
    }
}
