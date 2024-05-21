using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookStore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App Me => ((App)Application.Current);
        public string loaiTK {  get; set; }
        public Visibility NhapSachVisibility { get; set; }
        public Visibility BanSachVisibility { get; set; }
        public Visibility BaoCaoCongNoVisibility { get; set; }
        public Visibility BaoCaoTonVisibility { get; set; }

        public Visibility KhachHangVisibility { get; set; }
        public Visibility TuyChinhVisibility { get; set; }
    }
}
 