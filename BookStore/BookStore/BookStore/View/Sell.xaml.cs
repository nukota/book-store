using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using BookStore.Model;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace BookStore.View
{
    /// <summary>
    /// Interaction logic for Sell.xaml
    /// </summary>
    public partial class Sell : UserControl
    {
        public Sell()
        {
            InitializeComponent();
            dataHoaDon.IsEnabled = true;
            thamSo();
        }
        QuanLySachEntities context = new QuanLySachEntities();

        int soLuongTonToiThieu;
        int soTienNoToiDa;
        private void thamSo()
        {
            ObservableCollection<THAMSO> _thamso = new ObservableCollection<THAMSO>(context.THAMSO);
            soLuongTonToiThieu = Convert.ToInt32(_thamso[0].SoLuongTonToiThieu);
            soTienNoToiDa = Convert.ToInt32(_thamso[0].SoTienNoToiDa.ToString());
        }

        #region ISwitchable Member

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DataContext


        private ObservableCollection<HOADON> getHoaDon()
        {
            return new ObservableCollection<HOADON>(context.HOADON);
        }

        private ObservableCollection<KHACHHANG> getCustomers()
        {
            return new ObservableCollection<KHACHHANG>(context.KHACHHANG);
        }

        private ObservableCollection<SACH> getBooks()
        {
            return new ObservableCollection<SACH>(context.SACH);
        }

        private HOADON _hoadon;
        private CT_HOADON _chitiet;
        private int _sum;

        #endregion //Datacontext

        private void setEnable()
        {
            pnlBooks.IsEnabled = true;
            pnlHoaDon.IsEnabled = true;
        }

        private void setMutable()
        {
            pnlBooks.IsEnabled = false;
            pnlHoaDon.IsEnabled = false;
        }


        private void btnDelete(object sender, RoutedEventArgs e)
        {
            {
                Button seleted = (Button)sender;
                var item = seleted.DataContext as HOADON;
                if (item != null)
                {
                    var DeleteRecord = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn số " + item.SoHD + " không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (DeleteRecord == MessageBoxResult.Yes)
                    {
                        if (context.HOADON.Find(item.SoHD) != null)
                        {
                            //xóa Chi tiết.
                            var query = from b in context.CT_HOADON
                                        where b.SoHD.Equals(item.SoHD)
                                        select b;
                            foreach (var delete in query)
                            {
                                context.CT_HOADON.Remove(delete);
                            }
                            context.SaveChanges();

                            //xóa Phiếu
                            context.HOADON.Remove(item);
                            context.SaveChanges();
                            dataHoaDon.ItemsSource = getHoaDon();
                        }
                        else
                            dataHoaDon.ItemsSource = getHoaDon();
                    }
                }
                else
                    MessageBox.Show("Có gì đâu mà xóa!", "Phiếu nhập", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnAdd(object sender, RoutedEventArgs e)
        {
            if (cbKhachHang.Text == null || cbKhachHang.Text == "")
            {
                MessageBox.Show("Hãy chọn khách hàng để lên hóa đơn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            } 
            else
            {
                int soNo = Convert.ToInt32((from b in context.KHACHHANG
                                            where b.TenKhachHang.Equals(cbKhachHang.Text)
                                            select b.SoTienNo).FirstOrDefault());
                if (soNo > soTienNoToiDa)
                {
                    MessageBox.Show(cbKhachHang.Text + " đã vượt quá số tiền nợ tối đa (" + soNo + "/" + soTienNoToiDa + ")!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    SellReceipt wd = new SellReceipt(cbKhachHang.Text);
                    Application.Current.MainWindow = wd;
                    wd.ShowDialog();
                }
            }
            
        }

        private void btnUpdate(object sender, RoutedEventArgs e)
        {
            if (dataHoaDon.SelectedItem == null)
            {
                MessageBox.Show("Hãy chọn hóa đơn để thêm sách!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //sửa phiếu nhập
                setEnable();
                _hoadon = dataHoaDon.SelectedItem as HOADON;
                _sum = Convert.ToInt32(_hoadon.ThanhToan);
            }
        }

        private void btnSave(object sender, RoutedEventArgs e)
        {
            setMutable();
            var selected = dataHoaDon.SelectedItem as HOADON;
            if (selected != null)
            {
                _sum = 0;
                var query = from b in context.CT_HOADON
                            where b.SoHD.Equals(selected.SoHD)
                            select b;
                foreach (var sum in query)
                {
                    _sum += Convert.ToInt32(sum.ThanhTien);
                }
                var changed = context.HOADON.Find(selected.SoHD);
                changed.ThanhToan = _sum;
                context.SaveChanges();
                dataHoaDon.ItemsSource = getHoaDon();
                updateBaoCaoCongNo(selected);
                updateTienNo(selected.MaKhachHang);
            }
            else dataHoaDon.ItemsSource = getHoaDon();

        }

        bool isInsertMode = false;
        bool isBeingEdited = false;

        private void dataHoaDon_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            isInsertMode = true;
        }

        private void dataHoaDon_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isBeingEdited = true;
        }

        private void dataHoaDon_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            HOADON _hoadon = new HOADON();
            HOADON selected = e.Row.DataContext as HOADON;
            if (isInsertMode)
            {
                var InsertRecord = MessageBox.Show("Bạn có chắc chắn muốn thêm hóa đơn mã " + selected.SoHD + " không?", "Lưu ý mã phiếu phải khác nhau", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    _hoadon.SoHD = selected.SoHD;
                    var customerID = cbKhachHang.SelectedValue as KHACHHANG;
                    _hoadon.MaKhachHang = customerID.MaKhachHang;
                    _hoadon.NgayLap = DateTime.Now;
                    _hoadon.ThanhToan = 0;

                    if (context.HOADON.Find(_hoadon.SoHD) == null)
                    {
                        context.HOADON.Add(_hoadon);
                        context.SaveChanges();
                        dataHoaDon.ItemsSource = getHoaDon();
                    }
                    else
                    {
                        MessageBox.Show("Mã hóa đơn không được trùng!", "Hóa Đơn", MessageBoxButton.OK, MessageBoxImage.Warning);
                        dataHoaDon.ItemsSource = getHoaDon();
                    }
                }
                else
                    dataHoaDon.ItemsSource = getHoaDon();
            }
        }

        private void dataHoaDon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ((DataGrid)sender).SelectedItem as HOADON;
            if (selected != null)
            {
                //chọn chi tiết trùng với id
                var query = from b in context.CT_HOADON
                            where b.SoHD.Equals(selected.SoHD)
                            select b;
                ObservableCollection<CT_HOADON> data = new ObservableCollection<CT_HOADON>(query);
                dataCT.ItemsSource = data;
            }
        }

        private void btnAddBook(object sender, RoutedEventArgs e)
        {
            if (dataBooks.SelectedItem == null)
            {
                MessageBox.Show("Hãy chọn sách để thêm", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (String.IsNullOrEmpty(tbDonGia.Text) && String.IsNullOrEmpty(tbSoLuong.Text))
                {
                    MessageBox.Show("Hãy nhập số lượng và đơn giá", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else try
                {
                    if (int.Parse(tbDonGia.Text) > 0 && int.Parse(tbSoLuong.Text) > 0)
                    {
                        _chitiet = new CT_HOADON();
                        var _sach = dataBooks.SelectedItem as SACH;
                        if (Convert.ToInt32(tbSoLuong.Text) > _sach.SoLuongTon - soLuongTonToiThieu)
                        {
                            MessageBox.Show("Không đủ số lượng sách để bán (số lượng tồn tối thiểu: " + soLuongTonToiThieu + " cuốn)!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            _chitiet = context.CT_HOADON.Find(_sach.MaSach, _hoadon.SoHD);
                            var khachhang = context.KHACHHANG.Find(_hoadon.MaKhachHang);
                            int thanhtien = Convert.ToInt32(tbSoLuong.Text) * Convert.ToInt32(tbDonGia.Text);
                            if (_chitiet == null && thanhtien + khachhang.SoTienNo <= soTienNoToiDa)
                            {
                                _chitiet = new CT_HOADON();
                                _chitiet.SoHD = _hoadon.SoHD;
                                _chitiet.SACH = _sach;
                                _chitiet.MaSach = _sach.MaSach;
                                _chitiet.SoLuong = Convert.ToInt32(tbSoLuong.Text);
                                _chitiet.DonGia = Convert.ToInt32(tbDonGia.Text);
                                _chitiet.ThanhTien = _chitiet.SoLuong * _chitiet.DonGia;

                                context.CT_HOADON.Add(_chitiet);
                                _sach.SoLuongTon -= Convert.ToInt32(tbSoLuong.Text);
                                context.SaveChanges();

                                var query = from b in context.CT_HOADON
                                            where b.SoHD.Equals(_hoadon.SoHD)
                                            select b;
                                ObservableCollection<CT_HOADON> data = new ObservableCollection<CT_HOADON>(query);
                                dataCT.ItemsSource = data;
                                btnSave(sender, e);
                            }
                            else if (thanhtien + khachhang.SoTienNo <= soTienNoToiDa)
                            {
                                var InsertRecord = MessageBox.Show("Bạn có chắc chắn muốn sửa hóa đơn mã " + _hoadon.SoHD + " không?", "Thông Báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (InsertRecord == MessageBoxResult.Yes)
                                {
                                    _sach.SoLuongTon += _chitiet.SoLuong;
                                    _chitiet.SoLuong = Convert.ToInt32(tbSoLuong.Text);
                                    _chitiet.DonGia = Convert.ToInt32(tbDonGia.Text);
                                    _chitiet.ThanhTien = _chitiet.SoLuong * _chitiet.DonGia;

                                    _sach.SoLuongTon -= Convert.ToInt32(tbSoLuong.Text);
                                    context.SaveChanges();

                                    var query = from b in context.CT_HOADON
                                                where b.SoHD.Equals(_hoadon.SoHD)
                                                select b;
                                    ObservableCollection<CT_HOADON> data = new ObservableCollection<CT_HOADON>(query);
                                    dataCT.ItemsSource = data;
                                    btnSave(sender, e);
                                }
                            }
                            else { MessageBox.Show("Vượt số tiền nợ tối đa (" + (thanhtien + khachhang.SoTienNo) + "/" + soTienNoToiDa + ")!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning); }
                        }
                    }
                    else { MessageBox.Show("Thông tin không hợp lệ!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning); }
                }
                catch { MessageBox.Show("Thông tin không hợp lệ!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning); }
            }
        }

        private void btnDeleteBook(object sender, RoutedEventArgs e)
        {
            Button seleted = (Button)sender;
            var item = seleted.DataContext as CT_HOADON;
            var DeleteRecord = MessageBox.Show("Bạn có chắc chắn muốn xóa sách " + item.SACH.TenSach + " vừa nhập không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (DeleteRecord == MessageBoxResult.Yes)
            {
                SACH sach = context.SACH.Find(item.MaSach);
                sach.SoLuongTon -= item.SoLuong;
                context.CT_HOADON.Remove(item);
                context.SaveChanges();
                var query = from b in context.CT_HOADON
                            where b.SoHD.Equals(_hoadon.SoHD)
                            select b;
                ObservableCollection<CT_HOADON> data = new ObservableCollection<CT_HOADON>(query);
                dataCT.ItemsSource = data;
                btnSave(sender, e);
            }
        }

        private void HoaDoa_Loaded(object sender, RoutedEventArgs e)
        {
            dataHoaDon.ItemsSource = getHoaDon();

            cbKhachHang.ItemsSource = getCustomers();
            cbKhachHang.DisplayMemberPath = "TenKhachHang";

            dataBooks.ItemsSource = getBooks();
        }
        private void _selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _sach = dataBooks.SelectedItem as SACH;
            tbDonGia.Text = (from b in context.SACH
                             where b.MaSach.Equals(_sach.MaSach)
                             select b.GiaBan).FirstOrDefault().ToString();
        }
        private void updateBaoCaoCongNo(HOADON hoadon)
        {
            BAOCAOCONGNO baoCaoCongNo =  (from b in context.BAOCAOCONGNO
                        where b.MaKhachHang.Equals(hoadon.MaKhachHang)
                        select b).FirstOrDefault();
            if (baoCaoCongNo == null)
            {
                BAOCAOCONGNO _baoCaoCongNo = new BAOCAOCONGNO();
                _baoCaoCongNo.MaKhachHang = hoadon.MaKhachHang;
                _baoCaoCongNo.Thang = DateTime.Now.Month;
                _baoCaoCongNo.Nam = DateTime.Now.Year;
                if (hoadon.ThanhToan == null)
                    _baoCaoCongNo.NoDau = 0;
                else _baoCaoCongNo.NoDau = int.Parse(hoadon.ThanhToan.ToString());
                _baoCaoCongNo.NoCuoi = _baoCaoCongNo.NoDau;
                _baoCaoCongNo.PhatSinh = 0;
                context.BAOCAOCONGNO.Add(_baoCaoCongNo);
                context.SaveChanges();
                MessageBox.Show("Đã thêm báo cáo công nợ cho khách hàng số " + hoadon.MaKhachHang + "!");
            }
            else
            {
                if (hoadon.ThanhToan != null)
                {
                    baoCaoCongNo.NoCuoi += Convert.ToInt32(hoadon.ThanhToan);
                    baoCaoCongNo.PhatSinh += Convert.ToInt32(hoadon.ThanhToan);
                }
                context.SaveChanges();
            }
        }
        private void updateTienNo(int makhachhang)
        {
            int tienNo = 0;
            var query = (from b in context.HOADON
                         where makhachhang.Equals(b.MaKhachHang)
                         select b).ToList();
            foreach (HOADON hoadon in query)
            {
                var query1 = (from b in context.CT_HOADON
                              where hoadon.SoHD.Equals(b.SoHD) 
                              select b).ToList();
                foreach (CT_HOADON ct_hoadon in query1)
                {
                    tienNo += ct_hoadon.ThanhTien;
                }
            }
            var query2 = (from b in context.PHIEUTHUTIEN
                          where makhachhang.Equals(b.MaKhachHang)
                          select b).ToList();
            foreach (PHIEUTHUTIEN phieuthutien in query2)
            {
                tienNo -= phieuthutien.SoTienThu;
            }
            var khachhang = context.KHACHHANG.Find(makhachhang);
            khachhang.SoTienNo = tienNo;
            context.SaveChanges();
        }
    }
}
