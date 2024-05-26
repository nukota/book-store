using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using BookStore.Model;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace BookStore.View
{

    public partial class Setting : UserControl
    {
        public Setting()
        {
            InitializeComponent();
            Load();
        }

        #region ISwitchable Member

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

        QuanLySachEntities context = new QuanLySachEntities();
        private void Load()
        {
            ObservableCollection<THAMSO> _thamso = new ObservableCollection<THAMSO>(context.THAMSO);
            tbQD2.Text = _thamso[0].SoLuongTonToiThieu.ToString();
            tbQD4.Text = _thamso[0].SoTienNoToiDa.ToString();
            tbQD3.Text = _thamso[0].SoLuongTonToiDa.ToString();
            tbQD1.Text = _thamso[0].SoLuongNhapToiThieu.ToString();
        }
        private bool isNull()
        {
            if (!String.IsNullOrEmpty(tbQD1.Text)
                && !String.IsNullOrEmpty(tbQD2.Text)
                && !String.IsNullOrEmpty(tbQD3.Text)
                && !String.IsNullOrEmpty(tbQD4.Text))
                return false;
            return true;
        }

        private void btnChange(object sender, RoutedEventArgs e)
        {
            if (isNull())
                MessageBox.Show("Không được để trống quy định nào!","Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                var _thamso = (from b in context.THAMSO
                               select b).FirstOrDefault();
                _thamso.SoLuongNhapToiThieu = Convert.ToInt32(tbQD1.Text);
                _thamso.SoLuongTonToiThieu = Convert.ToInt32(tbQD2.Text);
                _thamso.SoLuongTonToiDa = Convert.ToInt32(tbQD3.Text);
                _thamso.SoTienNoToiDa = Convert.ToInt32(tbQD4.Text);
                //_thamso.ApDungQD4 = "Tùy chỉnh";
                context.SaveChanges();
                MessageBox.Show("Thay đổi quy định thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDefault(object sender, RoutedEventArgs e)
        {
            ObservableCollection<THAMSO> _thamso = new ObservableCollection<THAMSO>(context.THAMSO);
            _thamso[0].SoTienNoToiDa = 1000000;
            _thamso[0].SoLuongNhapToiThieu = 150;
            _thamso[0].SoLuongTonToiThieu = 20;
            _thamso[0].SoLuongTonToiDa = 300;
            context.SaveChanges();
            MessageBox.Show("Thay đổi các quy định về mặc định", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
