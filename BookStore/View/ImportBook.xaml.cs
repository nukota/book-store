﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using BookStore.Model;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace BookStore.View
{
    /// <summary>
    /// Interaction logic for ImportBook.xaml
    /// </summary>
    public partial class ImportBook : UserControl
    {
        public ImportBook()
        {
            InitializeComponent();
            dataPhieuNhap.IsEnabled = true;
            thamSo();
        }

        #region ISwitchable Member

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region DataContext

        QuanLySachEntities context = new QuanLySachEntities();

        int soLuongTonToiDa;
        int soLuongNhapToiThieu;
        private void thamSo()
        {
            ObservableCollection<THAMSO> _thamso = new ObservableCollection<THAMSO>(context.THAMSO);
            soLuongTonToiDa = Convert.ToInt32(_thamso[0].SoLuongTonToiDa);
            soLuongNhapToiThieu = Convert.ToInt32(_thamso[0].SoLuongNhapToiThieu);
        }

        private ObservableCollection<PHIEUNHAPSACH> getPhieuNhap()
        {
            return new ObservableCollection<PHIEUNHAPSACH>(context.PHIEUNHAPSACH);
        }

        private ObservableCollection<SACH> getBooks()
        {
            return new ObservableCollection<SACH>(context.SACH);
        }

        private PHIEUNHAPSACH _phieu;
        private CT_PHIEUNHAPSACH _chitiet;
        private int _sum;

        #endregion //Datacontext


        private void setEnabled()
        {
            pnlBooks.IsEnabled = true;
            pnlPhieuNhap.IsEnabled = true;
        }

        private void setMutable()
        {
            pnlBooks.IsEnabled = false;
            pnlPhieuNhap.IsEnabled = false;
        }

        #region Phiếu nhập

        /// <summary>
        /// Button Thêm phiếu nhập
        /// </summary>
        private void btnAdd(object sender, RoutedEventArgs e)
        {
            ImportReceipt wd = new ImportReceipt();
            Application.Current.MainWindow = wd;
            wd.ShowDialog();
        }

        /// <summary>
        /// Thêm Chi tiết phiếu nhập
        /// </summary>
        private void btnUpdate(object sender, RoutedEventArgs e)
        {
            if (dataPhieuNhap.SelectedItem == null)
            {
                MessageBox.Show("Hãy chọn phiếu để nhập", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //sửa phiếu nhập
                setEnabled();
                _phieu = dataPhieuNhap.SelectedItem as PHIEUNHAPSACH;
                _sum = Convert.ToInt32(_phieu.TongTien);
            }
        }

        /// <summary>
        /// Xóa phiếu nhập
        /// </summary>
        private void btnDelete(object sender, RoutedEventArgs e)
        {
            Button seleted = (Button)sender;
            var item = seleted.DataContext as PHIEUNHAPSACH;
            if (item != null)
            {
                var DeleteRecord = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu nhập số " + item.SoPNS + " không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (DeleteRecord == MessageBoxResult.Yes)
                {
                    if (context.PHIEUNHAPSACH.Find(item.SoPNS) != null)
                    {
                        //xóa Chi tiết.
                        var query = from b in context.CT_PHIEUNHAPSACH
                                    where b.SoPNS.Equals(item.SoPNS)
                                    select b;
                        foreach (var delete in query)
                        {
                            context.CT_PHIEUNHAPSACH.Remove(delete);
                        }
                        context.SaveChanges();


                        //xóa Phiếu
                        context.PHIEUNHAPSACH.Remove(item);
                        context.SaveChanges();
                        dataPhieuNhap.ItemsSource = getPhieuNhap();
                    }
                    else
                        dataPhieuNhap.ItemsSource = getPhieuNhap();
                }
            }
            else
                MessageBox.Show("Có gì đâu mà xóa!", "Phiếu nhập", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Lưu phiếu nhập
        /// </summary>
        private void btnSave(object sender, RoutedEventArgs e)
        {
            setMutable();
            var selected = dataPhieuNhap.SelectedItem as PHIEUNHAPSACH;
            if (selected != null)
            {
                _sum = 0;
                var query = from b in context.CT_PHIEUNHAPSACH
                            where b.SoPNS.Equals(selected.SoPNS)
                            select b;
                foreach (var sum in query)
                {
                    _sum += Convert.ToInt32(sum.ThanhTien);
                }
                var changed = context.PHIEUNHAPSACH.Find(selected.SoPNS);
                changed.TongTien = _sum;
                context.SaveChanges();
                dataPhieuNhap.ItemsSource = getPhieuNhap();
            }
            else dataPhieuNhap.ItemsSource = getPhieuNhap();
        }



        /// <summary>
        /// Thêm phiếu nhập
        /// </summary>
        bool isInsertMode = false;
        bool isBeingEdited = false;

        private void dataPhieuNhap_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            isInsertMode = true;
        }

        private void dataPhieuNhap_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isBeingEdited = true;
        }

        private void dataPhieuNhap_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            PHIEUNHAPSACH _phieu = new PHIEUNHAPSACH();
            PHIEUNHAPSACH selected = e.Row.DataContext as PHIEUNHAPSACH;
            if (isInsertMode)
            {
                var InsertRecord = MessageBox.Show("Bạn có chắc chắn muốn thêm phiếu nhập mã " + selected.SoPNS + " không?", "Lưu ý mã phiếu phải khác nhau", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    _phieu.SoPNS = selected.SoPNS;
                    _phieu.NgayNhap = DateTime.Now;
                    _phieu.TongTien = 0;

                    if (context.PHIEUNHAPSACH.Find(_phieu.SoPNS) == null)
                    {
                        context.PHIEUNHAPSACH.Add(_phieu);
                        context.SaveChanges();
                        dataPhieuNhap.ItemsSource = getPhieuNhap();
                    }
                    else
                    {
                        MessageBox.Show("Mã thể loại không được trùng!", "Thể loại", MessageBoxButton.OK, MessageBoxImage.Warning);
                        dataPhieuNhap.ItemsSource = getPhieuNhap();
                    }
                }
                else
                    dataPhieuNhap.ItemsSource = getPhieuNhap();
            }
        }
        
        #endregion //Phiếu nhập

        /// <summary>
        /// Load DataContext
        /// </summary>
        private void DataLoaded(object sender, RoutedEventArgs e)
        {
            dataPhieuNhap.ItemsSource = getPhieuNhap();

            dataBooks.ItemsSource = getBooks();
        }

        /// <summary>
        /// Thêm sách vào chi tiết
        /// </summary>
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
                else
                {
                    _chitiet = new CT_PHIEUNHAPSACH();
                    var _sach = dataBooks.SelectedItem as SACH;
                    if (_sach.SoLuongTon >= soLuongTonToiDa)
                    {
                        MessageBox.Show("Số sách vượt quá số lượng tồn tối đa (" + _sach.SoLuongTon + "/" + soLuongTonToiDa + ")!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else try
                    {
                         if (int.Parse(tbSoLuong.Text) <= 0 || int.Parse(tbDonGia.Text) <= 0)
                            {
                                MessageBox.Show("Thông tin nhập không hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else if (int.Parse(tbSoLuong.Text) < soLuongNhapToiThieu)
                            {
                                MessageBox.Show("Cần nhập số lượng ít nhất là " + soLuongNhapToiThieu + " cuốn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                _chitiet = context.CT_PHIEUNHAPSACH.Find(_phieu.SoPNS, _sach.MaSach);
                                if (_chitiet == null)
                                {
                                    _chitiet = new CT_PHIEUNHAPSACH();
                                    _chitiet.SoPNS = _phieu.SoPNS;
                                    _chitiet.SACH = dataBooks.SelectedItem as SACH;
                                    _chitiet.MaSach = _chitiet.SACH.MaSach;
                                    _chitiet.SoLuongNhap = Convert.ToInt32(tbSoLuong.Text);
                                    _chitiet.DonGiaNhap = Convert.ToInt32(tbDonGia.Text);
                                    _chitiet.ThanhTien = _chitiet.SoLuongNhap * _chitiet.DonGiaNhap;
                                    //update Bao Cao Ton
                                    BAOCAOTON baoCaoTon = (from b in context.BAOCAOTON
                                                           where b.MaSach.Equals(_chitiet.MaSach)
                                                           select b).FirstOrDefault();
                                    var sach = context.SACH.Find(_chitiet.MaSach);
                                    baoCaoTon.TonCuoi = Convert.ToInt32(sach.SoLuongTon);
                                    baoCaoTon.PhatSinh += Convert.ToInt32(_chitiet.SoLuongNhap);

                                    context.CT_PHIEUNHAPSACH.Add(_chitiet);
                                    sach.SoLuongTon += _chitiet.SoLuongNhap;
                                    context.SaveChanges();

                                    var query = from b in context.CT_PHIEUNHAPSACH
                                                where b.SoPNS.Equals(_phieu.SoPNS)
                                                select b;
                                    ObservableCollection<CT_PHIEUNHAPSACH> data = new ObservableCollection<CT_PHIEUNHAPSACH>(query);
                                    dataCT.ItemsSource = data;
                                }
                                else
                                {
                                    var InsertRecord = MessageBox.Show("Bạn có chắc chắn muốn sửa phiếu nhập mã " + _phieu.SoPNS + " không?", "Thông Báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                    if (InsertRecord == MessageBoxResult.Yes)
                                    {
                                        SACH sach = context.SACH.Find(_chitiet.MaSach);
                                        int chenhlech = 0;
                                        chenhlech -= _chitiet.SoLuongNhap;
                                        _chitiet.SoLuongNhap = Convert.ToInt32(tbSoLuong.Text);
                                        _chitiet.DonGiaNhap = Convert.ToInt32(tbDonGia.Text);
                                        _chitiet.ThanhTien = _chitiet.SoLuongNhap * _chitiet.DonGiaNhap;
                                        chenhlech += _chitiet.SoLuongNhap;
                                        sach.SoLuongTon = chenhlech;
                                        //update Bao Cao Ton
                                        BAOCAOTON baoCaoTon = (from b in context.BAOCAOTON
                                                               where b.MaSach.Equals(_chitiet.MaSach)
                                                               select b).FirstOrDefault();
                                        baoCaoTon.TonCuoi = Convert.ToInt32(sach.SoLuongTon);
                                        baoCaoTon.PhatSinh += Convert.ToInt32(chenhlech);

                                        context.SaveChanges();

                                        var query = from b in context.CT_PHIEUNHAPSACH
                                                    where b.SoPNS.Equals(_phieu.SoPNS)
                                                    select b;
                                        ObservableCollection<CT_PHIEUNHAPSACH> data = new ObservableCollection<CT_PHIEUNHAPSACH>(query);
                                        dataCT.ItemsSource = data;
                                    }
                                }
                            }
                        }
                        catch { MessageBox.Show("Thông tin không hợp lệ!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning); }
                    
                    
                }
            }
        }


        /// <summary>
        /// Xóa sách nhập
        /// </summary>
        private void btnDeleteBook(object sender, RoutedEventArgs e)
        {
            Button seleted = (Button)sender;
            var item = seleted.DataContext as CT_PHIEUNHAPSACH;
            var DeleteRecord = MessageBox.Show("Bạn có chắc chắn muốn xóa sách " + item.SACH.TenSach + " vừa nhập không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (DeleteRecord == MessageBoxResult.Yes)
            {
                context.CT_PHIEUNHAPSACH.Remove(item);
                SACH sach = context.SACH.Find(item.MaSach);
                sach.SoLuongTon -= item.SoLuongNhap;
                context.SaveChanges();
                var query = from b in context.CT_PHIEUNHAPSACH
                            where b.SoPNS.Equals(_phieu.SoPNS)
                            select b;
                ObservableCollection<CT_PHIEUNHAPSACH> data = new ObservableCollection<CT_PHIEUNHAPSACH>(query);
                dataCT.ItemsSource = data;
            }
        }

        /// <summary>
        /// SelectedItem Changed
        /// </summary>
        private void dataPhieuNhap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ((DataGrid)sender).SelectedItem as PHIEUNHAPSACH;
            if (selected != null)
            {
                //chọn chi tiết trùng với id
                var query = from b in context.CT_PHIEUNHAPSACH
                            where b.SoPNS.Equals(selected.SoPNS)
                            select b;
                ObservableCollection<CT_PHIEUNHAPSACH> data = new ObservableCollection<CT_PHIEUNHAPSACH>(query);
                dataCT.ItemsSource = data;
            }
        }
        private void updateBaoCaoTon(CT_PHIEUNHAPSACH ct_phieu)
        {
            BAOCAOTON baoCaoTon = (from b in context.BAOCAOTON
                                         where b.MaSach.Equals(ct_phieu.MaSach)
                                         select b).FirstOrDefault();
            var sach = context.SACH.Find(ct_phieu.MaSach);
            baoCaoTon.TonCuoi = Convert.ToInt32(sach.SoLuongTon);
            baoCaoTon.PhatSinh += Convert.ToInt32(ct_phieu.SoLuongNhap);
            context.SaveChanges();
        }

        private void dataBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _sach = dataBooks.SelectedItem as SACH;
            tbDonGia.Text = (from b in context.SACH
                             where b.MaSach.Equals(_sach.MaSach)
                             select b.GiaBan).FirstOrDefault().ToString();
        }
    }
}
