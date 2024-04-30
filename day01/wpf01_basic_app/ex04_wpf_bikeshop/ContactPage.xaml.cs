using ex04_wpf_bikeshop;
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

namespace ex04_wpf_bikeshop
{
    /// <summary>
    /// ContactPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ContactPage : Page
    {
        public ContactPage()
        {
            InitializeComponent();
        }

        public double G0 { get; private set; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {   
            // 코드에서는 속성에 값을 저장할때 사용하는 방법
            Bike mybike = new Bike();
            mybike.speed = G0;
            mybike.color  = Colors.Black;  // Colors => 클래스

            TextBox text1 = new TextBox();
            StpBike.DataContext = mybike;
            //MessageBox.Show(DgBike.speed.ToString());
        }

        /*private void sldValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PrgValue.Value = (double)e.NewValue;
            LblValue.Content = Math.Round(PrgValue.Value, 1);
        }*/
    }
}
