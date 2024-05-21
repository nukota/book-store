using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStore.ViewModel
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        //public static QUANLYBENHVIENEntities global_db;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Visibility nhapsachVisibility;
        public Visibility bansachVisibility;
        public Visibility khachhangVisibility;
        public Visibility baocaocongnoVisibility;
        public Visibility baocaotonVisibility;
        public Visibility tuychinhVisibility;

        public Visibility NhapSachVisibility
        {
            get { return nhapsachVisibility; }
            set { nhapsachVisibility = value; }
        }
        public Visibility BanSachVisibility
        {
            get { return bansachVisibility; }
            set { bansachVisibility = value; }
        }
        public Visibility KhachHangVisibility
        {
            get { return khachhangVisibility; }
            set { khachhangVisibility = value; }
        }
        public Visibility BaoCaoCongNoVisibility
        {
            get { return baocaocongnoVisibility; }
            set { baocaocongnoVisibility = value; }
        }
        public Visibility BaoCaoTonVisibility
        {
            get { return baocaotonVisibility; }
            set { baocaotonVisibility = value; }
        }
        public Visibility TuyChinhVisibility
        {
            get { return tuychinhVisibility; }
            set { tuychinhVisibility = value; }
        }
    
        public MenuViewModel()
        {
            set_permission();
        }

        //Phân quyển bằng cách set visibility của các mục
        private void set_permission()
        {
            switch (App.Me.loaiTK)
            {
                case "QL":
                    NhapSachVisibility = Visibility.Visible;
                    BanSachVisibility = Visibility.Visible;
                    KhachHangVisibility = Visibility.Visible;
                    BaoCaoCongNoVisibility = Visibility.Visible;
                    BaoCaoTonVisibility = Visibility.Visible;
                    TuyChinhVisibility = Visibility.Visible;
                    break;
                case "NVBH":
                    NhapSachVisibility = Visibility.Collapsed;
                    BanSachVisibility = Visibility.Visible;
                    KhachHangVisibility = Visibility.Visible;
                    BaoCaoCongNoVisibility = Visibility.Visible;
                    BaoCaoTonVisibility = Visibility.Collapsed;
                    TuyChinhVisibility = Visibility.Collapsed;
                    break;
                case "TK":
                    NhapSachVisibility = Visibility.Visible;
                    BanSachVisibility = Visibility.Collapsed;
                    KhachHangVisibility = Visibility.Collapsed;
                    BaoCaoCongNoVisibility = Visibility.Collapsed;
                    BaoCaoTonVisibility = Visibility.Visible;
                    TuyChinhVisibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }
    }
}
