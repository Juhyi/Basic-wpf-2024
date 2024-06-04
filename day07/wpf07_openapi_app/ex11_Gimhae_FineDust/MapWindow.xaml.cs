using System.Windows;

namespace ex11_Gimhae_FineDust
{
    /// <summary>
    /// MapWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MapWindow : Window
    {
        public MapWindow()
        {
            InitializeComponent();
        }

        public MapWindow(double coordy, double coordx) : this()
        {
            BrsLoc.Address = $"https://google.com/maps/place/{coordy},{coordx}";
        }
    }
}
