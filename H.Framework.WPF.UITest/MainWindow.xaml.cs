using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace H.Framework.WPF.UITest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private Visibility _SBVisibility = Visibility.Visible;

        public Visibility SBVisibility { get => _SBVisibility; set { _SBVisibility = value; OnPropertyChanged("SBVisibility"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (SBVisibility == Visibility) SBVisibility = Visibility.Collapsed;
            else SBVisibility = Visibility.Visible;
            warmBlock.Show("hahahahah");
            //warmBlock.Show("haaaaaa");
            //warmBlock.Show("ggggggggggggggggggg");
            //warmBlock.Show("ha");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SBVisibility = Visibility.Collapsed;
        }
    }
}