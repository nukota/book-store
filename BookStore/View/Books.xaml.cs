﻿using BookStore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStore.View
{

    public partial class Books : UserControl
    {
        public Books()
        {
            InitializeComponent();
        }

        #region ISwitchable Member

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DataContext

        QuanLySachEntities context = new QuanLySachEntities();

        private ObservableCollection<SACH> getBooks()
        {
            return new ObservableCollection<SACH>(context.SACH);
        }

        private ObservableCollection<THELOAI> getTheLoai()
        {
            return new ObservableCollection<THELOAI>(context.THELOAI);
        }

        private ObservableCollection<NHAXUATBAN> getNhaXuatBan()
        {
            return new ObservableCollection<NHAXUATBAN>(context.NHAXUATBAN);
        }

        #endregion
        int soLuongTonToiDa;
        /// Set textbox Enabled
        private void Load()
        {
            ObservableCollection<THAMSO> _thamso = new ObservableCollection<THAMSO>(context.THAMSO);
            soLuongTonToiDa = Convert.ToInt32(_thamso[0].SoLuongTonToiDa);
        }
        private void setEnabled()
        {
            tbTenSach.IsEnabled = true;
            tbTacGia.IsEnabled = true;
            cbTheLoai.IsEnabled = true;
            cbNhaXuatBan.IsEnabled = true;
            tbGiaBan.IsEnabled = true;
            tbSoLuongTon.IsEnabled = true;
        }

        /// Set textbox mutable

        private void setMutable()
        {
            tbMaSach.IsEnabled = false;
            tbTenSach.IsEnabled = false;
            tbTacGia.IsEnabled = false;
            cbTheLoai.IsEnabled = false;
            cbNhaXuatBan.IsEnabled = false;
            tbGiaBan.IsEnabled = false;
            tbSoLuongTon.IsEnabled = false;
        }

        private void setClear()
        {
            tbMaSach.Clear();
            tbTenSach.Clear();
            tbTacGia.Clear();
            cbTheLoai.Text = "";
            cbNhaXuatBan.Text = "";
            tbGiaBan.Clear();
            tbSoLuongTon.Clear();
        }

        /// thêm sách

        private void addBook(object sender, RoutedEventArgs e)
        {
            dataBooks.SelectedItem = null;
            if (!tbTenSach.IsEnabled)
            {
                tbMaSach.IsEnabled = true;
                setClear();
                setEnabled();
                dataBooks.IsEnabled = false;
            }
            else
            {
                setMutable();
                dataBooks.IsEnabled = true;
            }
        }

        /// kiểm tra textbox null

        private bool isNull()
        {
            if (!String.IsNullOrEmpty(tbMaSach.Text)
                && !String.IsNullOrEmpty(tbTenSach.Text)
                && !String.IsNullOrEmpty(tbTacGia.Text)
                && !String.IsNullOrEmpty(cbTheLoai.Text)
                && !String.IsNullOrEmpty(cbNhaXuatBan.Text)
                && !String.IsNullOrEmpty(tbGiaBan.Text)
                && !String.IsNullOrEmpty(tbSoLuongTon.Text))
                return false;
            return true;
        }

   
        private void saveBook(object sender, RoutedEventArgs e)
        {
            SACH _sach = new SACH();
            if (!isNull())
            {
                int chenhlech = 0;
                bool kt = true;
                if (tbMaSach.IsEnabled)
                {
                    var Confirm = MessageBox.Show("Bạn có chắc muốn thêm sách " + tbTenSach.Text + " không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (Confirm == MessageBoxResult.Yes)
                    {
                        try
                        {
                            if (Convert.ToInt32(tbMaSach.Text) <= 0 || Convert.ToInt32(tbGiaBan.Text) <= 0 || Convert.ToInt32(tbSoLuongTon.Text) <= 0)
                            {
                            MessageBox.Show("Dữ liệu nhập không được phép bé hơn hoặc bằng 0!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning); kt = false;
                            }   
                            else
                            {
                                var _find = Convert.ToInt32(tbMaSach.Text);
                                if (context.SACH.Find(_find) == null)
                                {
                                    _sach.MaSach = Convert.ToInt32(tbMaSach.Text);

                                    _sach.TenSach = tbTenSach.Text;
                                    _sach.TacGia = tbTacGia.Text;

                                    _sach.THELOAI = cbTheLoai.SelectedValue as THELOAI;
                                    _sach.NHAXUATBAN = cbNhaXuatBan.SelectedValue as NHAXUATBAN;

                                    _sach.GiaBan = Convert.ToInt32(tbGiaBan.Text);
                                    _sach.SoLuongTon = Convert.ToInt32(tbSoLuongTon.Text);

                                    context.SACH.Add(_sach);
                                    context.SaveChanges();

                                    dataBooks.ItemsSource = getBooks();
                                    setMutable();
                                    dataBooks.IsEnabled = true;
                                } else { MessageBox.Show("Mã sách không được trùng", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning); kt = false; }
                            }        
                            
                        }
                        catch { MessageBox.Show("Thông tin không hợp lệ!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning); kt = false; }
                    }
                }
                else
                {
                    var Confirm = MessageBox.Show("Bạn có chắc muốn sửa sách " + tbTenSach.Text + " không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (Confirm == MessageBoxResult.Yes)
                    {
                        try
                        {
                            {
                                _sach = context.SACH.Find(Convert.ToInt32(tbMaSach.Text));
                                chenhlech = -_sach.SoLuongTon;
                                _sach.TenSach = tbTenSach.Text;
                                _sach.TacGia = tbTacGia.Text;

                                _sach.THELOAI = cbTheLoai.SelectedValue as THELOAI;
                                _sach.NHAXUATBAN = cbNhaXuatBan.SelectedValue as NHAXUATBAN;

                                _sach.GiaBan = Convert.ToInt32(tbGiaBan.Text);
                                _sach.SoLuongTon = Convert.ToInt32(tbSoLuongTon.Text);
                                chenhlech += _sach.SoLuongTon;
                                context.SaveChanges();

                                dataBooks.ItemsSource = getBooks();

                                MessageBox.Show("Sửa sách thành công", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                                setMutable();
                                dataBooks.IsEnabled = true;
                            }
                            
                        }
                        catch { MessageBox.Show("Thông tin không hợp lệ!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning); kt = false; }
                    }
                }
                setMutable();
                if (kt) { updateBaoCaoTon(_sach, chenhlech); }
                
            }
            else
                MessageBox.Show("Không được để trống!", "Sách", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// xóa sách
        private void deleteBook(object sender, RoutedEventArgs e)
        {
            Button seleted = (Button)sender;
            var item = seleted.DataContext as SACH;
            var DeleteRecord = MessageBox.Show("Bạn có chắc chắn muốn xóa sách " + item.TenSach + " không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (DeleteRecord == MessageBoxResult.Yes)
            {
                BAOCAOTON _baocaoton = (from b in context.BAOCAOTON
                                        where b.MaSach.Equals(item.MaSach)
                                        select b).FirstOrDefault();
                if (_baocaoton != null) { context.BAOCAOTON.Remove(_baocaoton);}
                
                context.SACH.Remove(item);
                context.SaveChanges();
                dataBooks.ItemsSource = getBooks();
            }
        }

        /// sửa sách

        private void updateBook(object sender, RoutedEventArgs e)
        {
            if (dataBooks.SelectedItem == null)
                MessageBox.Show("Hãy chọn sách để sửa!", "Sách", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                setEnabled();
        }


        private void Books_Loaded(object sender, RoutedEventArgs e)
        {
            dataBooks.ItemsSource = getBooks();

            cbTheLoai.ItemsSource = getTheLoai();
            cbTheLoai.DisplayMemberPath = "TenTheLoai";

            cbNhaXuatBan.ItemsSource = getNhaXuatBan();
            cbNhaXuatBan.DisplayMemberPath = "TenNhaXuatBan";
        }
        private void updateBaoCaoTon(SACH sach, int chenhlech = 0)
        {
            BAOCAOTON _baocaoton = (from b in context.BAOCAOTON
                                    where b.MaSach.Equals(sach.MaSach)
                                    select b).FirstOrDefault();
            if (_baocaoton == null) 
            {
                _baocaoton = new BAOCAOTON();
                _baocaoton.MaSach = sach.MaSach;
                _baocaoton.Thang = DateTime.Now.Month;
                _baocaoton.Nam = DateTime.Now.Year;
                _baocaoton.TonDau = sach.SoLuongTon;
                _baocaoton.TonCuoi = sach.SoLuongTon;
                _baocaoton.PhatSinh = 0;
                context.BAOCAOTON.Add(_baocaoton);
                context.SaveChanges();
            }
            else
            {
                _baocaoton.TonCuoi = sach.SoLuongTon;
                _baocaoton.PhatSinh += chenhlech;
                context.SaveChanges();
            }
        }
    }
}
