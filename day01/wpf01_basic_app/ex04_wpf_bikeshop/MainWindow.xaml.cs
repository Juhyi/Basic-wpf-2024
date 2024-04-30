using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ex04_wpf_bikeshop
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 코드비하인드에서 Source 속성에 페이지를 넣을때는
            //  UriKind.RelativeOrAbsolute 파라미터를 반드시 넣어야함
            MainFrame.Source = new Uri("/MenuPage.xaml", UriKind.RelativeOrAbsolute);
            Debug.WriteLine("Debug!");
        }
    }
}
