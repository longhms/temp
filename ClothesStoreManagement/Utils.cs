using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ClothesStoreManagement {
    public enum Table {
        ChatLieu,
        ChiTietHoaDon,
        HoaDonBan,
        KhachHang,
        NhanVien,
        SanPham
    }
    public class Utils {
        static readonly MainWindow mainWindow = (MainWindow) Application.Current.MainWindow;

        public static void HideAllMenu() {
            foreach (var element in mainWindow.grid.Children) {
                if (element is Label l) {
                    if (l.Name == "labelChooseTable")
                        continue;
                    l.Visibility = Visibility.Collapsed;
                }
                if (element is TextBox) {
                    TextBox tb = element as TextBox;
                    tb.Visibility = Visibility.Collapsed;
                }
            }
        }
        public static void UnloadAllFields() {
            foreach (var element in mainWindow.grid.Children)
                if (element is TextBox) {
                    TextBox tb = element as TextBox;
                    tb.Text = string.Empty;
                }
        }
        public static void DisableAllFields() {
            foreach (var element in mainWindow.grid.Children)
                if (element is TextBox tb)
                    tb.IsReadOnly = true;
        }
        public static void EnableAllFields() {
            foreach (var element in mainWindow.grid.Children)
                if (element is TextBox tb)
                    tb.IsReadOnly = false;
        }
        public static void HideButtons() {
            foreach (var element in mainWindow.grid.Children)
                if (element is Button bt)
                    bt.Visibility = Visibility.Collapsed;
        }
        public static void EnableButtons() {
            foreach (var element in mainWindow.grid.Children)
                if (element is Button bt)
                    bt.IsEnabled = true;
        }
        public static void DisableButtons() {
            foreach (var element in mainWindow.grid.Children)
                if (element is Button bt)
                    if (bt.Name.Contains("Modify") || bt.Name.Contains("Delete"))
                        bt.IsEnabled = false;
        }
        public static void ChangeButtonState( bool IsEditing ) {
            if (!IsEditing) {
                mainWindow.buttonInsert.Visibility = Visibility.Visible;
                mainWindow.buttonModify.Visibility = Visibility.Visible;
                mainWindow.buttonDelete.Visibility = Visibility.Visible;

                mainWindow.buttonConfirm.Visibility = Visibility.Collapsed;
                mainWindow.buttonCancel.Visibility = Visibility.Collapsed;
            }
            else {
                mainWindow.buttonInsert.Visibility = Visibility.Collapsed;
                mainWindow.buttonModify.Visibility = Visibility.Collapsed;
                mainWindow.buttonDelete.Visibility = Visibility.Collapsed;

                mainWindow.buttonConfirm.Visibility = Visibility.Visible;
                mainWindow.buttonCancel.Visibility = Visibility.Visible;
            }
        }
        private static bool IsKeyPresent( string[] keys ) {
            foreach ( var key in keys )
                if (key == string.Empty)
                    return false;
            return true;
        }
        public static void InsertToDatabase( Table? table, string[] data, bool isNew ) {
            string updateStatement = "";
            if (isNew) {
                switch (table) {
                    case Table.ChatLieu:
                        if (IsKeyPresent(new string[] { data[0] }))
                            updateStatement += $"(MaChatLieu, TenChatlieu) values " +
                                $"(N'{data[0]}',N'{data[1]}')";
                        break;
                    case Table.ChiTietHoaDon:
                        if (IsKeyPresent(new string[] { data[0], data[1] }))
                            updateStatement += $"(MaHDBan,MaHang,SoLuong,DonGia,GiamGia,ThanhTien) values " +
                                $"(N'{data[0]}',N'{data[1]}',{data[2]},{data[3]},{data[4]},{data[5]})";
                        break;
                    case Table.HoaDonBan:
                        if (IsKeyPresent(new string[] { data[0] }))
                            updateStatement += $"(MaHDBan,MaNhanVien,NgayBan,MaKhach,TongTien) values " +
                                $"(N'{data[0]}',N'{data[1]}',N'{data[2]}',N'{data[3]}',{data[4]})";
                        break;
                    case Table.KhachHang:
                        if (IsKeyPresent(new string[] { data[0] }))
                            updateStatement += $"(MaKhachHang,TenKhachHang,DiaChi,SDT) values " +
                            $"(N'{data[0]}',N'{data[1]}',N'{data[2]}',N'{data[3]}')";
                        break;
                    case Table.NhanVien:
                        if (IsKeyPresent(new string[] { data[0] }))
                            updateStatement += $"(MaNhanVien,TenNhanVien,DiaChi,SDT,NgaySinh,GioiTinh) values " +
                                $"(N'{data[0]}',N'{data[1]}',N'{data[2]}',N'{data[3]}',N'{data[4]}',N'{data[5]}')";
                        
                        break;
                    case Table.SanPham:
                        if (IsKeyPresent(new string[] { data[0] }))
                            updateStatement += $"(MaSanPham,TenSanPham,MaChatLieu,SoLuong,DonGiaNhap,DonGiaBan,Anh,GhiChu) values " + $"(N'{data[0]}',N'{data[1]}',N'{data[2]}',{data[3]},{data[4]},{data[5]},N'Anh',N'{data[6]}')";
                        break;
                }
                updateStatement = "insert into " + table.ToString() + " " + updateStatement;
            }
            else {
                switch (table) {
                    case Table.ChatLieu:
                        updateStatement += $"MaChatLieu=N'{data[0]}',TenChatlieu=N'{data[1]}' where MaChatLieu=N'{data[0]}'";
                        break;
                    case Table.ChiTietHoaDon:
                        updateStatement += $"MaHDBan=N'{data[0]}',MaHang=N'{data[1]}',SoLuong={data[2]},DonGia={data[3]},GiamGia={data[4]},ThanhTien={data[5]} where MaHDBan=N'{data[0]}' and MaHang=N'{data[1]}'";
                        break;
                    case Table.HoaDonBan:
                        updateStatement += $"MaHDBan=N'{data[0]}',MaNhanVien=N'{data[1]}',NgayBan=N'{data[2]}',MaKhach=N'{data[3]}',TongTien={data[4]} where MaHDBan=N'{data[0]}'";
                        break;
                    case Table.KhachHang:
                        updateStatement += $"MaKhachHang=N'{data[0]}',TenKhachHang=N'{data[1]}',DiaChi=N'{data[2]}',SDT=N'{data[3]}' where MaKhachHang=N'{data[0]}'";
                        break;
                    case Table.NhanVien:
                        updateStatement += $"MaNhanVien=N'{data[0]}',TenNhanVien=N'{data[1]}',DiaChi=N'{data[2]}',SDT=N'{data[3]}',NgaySinh=N'{data[4]}',GioiTinh=N'{data[5]}' where MaNhanVien=N'{data[0]}'";
                        break;
                    case Table.SanPham:
                        updateStatement += $"MaSanPham=N'{data[0]}',TenSanPham=N'{data[1]}',MaChatLieu=N'{data[2]}',SoLuong={data[3]},DonGiaNhap={data[4]},DonGiaBan={data[5]},Anh=N'Anh',GhiChu=N'{data[6]}' where MaSanPham=N'{data[0]}'";
                        break;
                }
                updateStatement = "update " + table.ToString() + " set " + updateStatement;
            }
            try {
                new SqlCommand(updateStatement, mainWindow.connection).ExecuteNonQuery();
            }
            catch (Exception) {
                MessageBox.Show("Đã xảy ra lỗi với dữ liệu.\nXin hãy kiểm tra lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void DeleteFromDatabase( Table? table, string[] data) {
            string deleteStatement = "delete from " + table.ToString() + " where ";
            switch (table) {
                case Table.ChatLieu:
                    deleteStatement += $"MaChatLieu=N'{data[0]}'";
                    break;
                case Table.ChiTietHoaDon:
                    deleteStatement += $"MaHDBan=N'{data[0]}' and MaHang=N'{data[1]}'";
                    break;
                case Table.HoaDonBan:
                    deleteStatement += $"MaHDBan=N'{data[0]}'";
                    break;
                case Table.KhachHang:
                    deleteStatement += $"MaKhachHang=N'{data[0]}'";
                    break;
                case Table.NhanVien:
                    deleteStatement += $"MaNhanVien=N'{data[0]}'";
                    break;
                case Table.SanPham:
                    deleteStatement += $"MaSanPham=N'{data[0]}'";
                    break;
            }
            try {
                new SqlCommand(deleteStatement, mainWindow.connection).ExecuteNonQuery();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
        public static string[] GetFields( Table? table ) {
            List<string> fieldData = new List<string>();
            switch (table) {
                case Table.ChatLieu:
                    fieldData.Add(mainWindow.textboxMaChatLieu.Text);
                    fieldData.Add(mainWindow.textboxTenChatLieu.Text);
                    break;
                case Table.ChiTietHoaDon:
                    fieldData.Add(mainWindow.textboxMaHDBan.Text);
                    fieldData.Add(mainWindow.textboxMaHang.Text);
                    fieldData.Add(mainWindow.textboxSoLuong.Text);
                    fieldData.Add(mainWindow.textboxDonGia.Text);
                    fieldData.Add(mainWindow.textboxGiamGia.Text);
                    fieldData.Add(mainWindow.textboxThanhTien.Text);
                    break;
                case Table.HoaDonBan:
                    fieldData.Add(mainWindow.textboxMaHDBan.Text);
                    fieldData.Add(mainWindow.textboxMaNhanVien.Text);
                    fieldData.Add(mainWindow.textboxNgayBan.Text);
                    fieldData.Add(mainWindow.textboxMaKhach.Text);
                    fieldData.Add(mainWindow.textboxTongTien.Text);
                    break;
                case Table.KhachHang:
                    fieldData.Add(mainWindow.textboxMaKhachHang.Text);
                    fieldData.Add(mainWindow.textboxTenKhachHang.Text);
                    fieldData.Add(mainWindow.textboxDiaChiKH.Text);
                    fieldData.Add(mainWindow.textboxSDTKH.Text);
                    break;
                case Table.NhanVien:
                    fieldData.Add(mainWindow.textboxMaNhanVien.Text);
                    fieldData.Add(mainWindow.textboxTenNhanVien.Text);
                    fieldData.Add(mainWindow.textboxDiaChiNV.Text);
                    fieldData.Add(mainWindow.textboxSDTNV.Text);
                    fieldData.Add(mainWindow.textboxNgaySinh.Text);
                    fieldData.Add(mainWindow.textboxGioiTinh.Text);
                    break;
                case Table.SanPham:
                    fieldData.Add(mainWindow.textboxMaSanPham.Text);
                    fieldData.Add(mainWindow.textboxTenSanPham.Text);
                    fieldData.Add(mainWindow.textboxMaChatLieu.Text);
                    fieldData.Add(mainWindow.textboxSoLuong.Text);
                    fieldData.Add(mainWindow.textboxDonGiaNhap.Text);
                    fieldData.Add(mainWindow.textboxDonGiaBan.Text);
                    fieldData.Add(mainWindow.textboxGhiChu.Text);
                    break;
            }
            return fieldData.ToArray();
        }
        public static void LoadMenu( Table? table ) {
            if (mainWindow.connection.State != ConnectionState.Open || table == null)
                return;
            switch (table) {
                case Table.ChatLieu:
                    HideAllMenu();
                    mainWindow.labelMaChatLieu.Visibility = Visibility.Visible;
                    mainWindow.labelTenChatLieu.Visibility = Visibility.Visible;

                    mainWindow.textboxMaChatLieu.Visibility = Visibility.Visible;
                    mainWindow.textboxTenChatLieu.Visibility = Visibility.Visible;
                    break;
                case Table.ChiTietHoaDon:
                    HideAllMenu();
                    mainWindow.labelMaHDBan.Visibility = Visibility.Visible;
                    mainWindow.labelMaHang.Visibility = Visibility.Visible;
                    mainWindow.labelSoLuong.Visibility = Visibility.Visible;
                    mainWindow.labelDonGia.Visibility = Visibility.Visible;
                    mainWindow.labelGiamGia.Visibility = Visibility.Visible;
                    mainWindow.labelThanhTien.Visibility = Visibility.Visible;

                    mainWindow.textboxMaHDBan.Visibility = Visibility.Visible;
                    mainWindow.textboxMaHang.Visibility = Visibility.Visible;
                    mainWindow.textboxSoLuong.Visibility = Visibility.Visible;
                    mainWindow.textboxDonGia.Visibility = Visibility.Visible;
                    mainWindow.textboxGiamGia.Visibility = Visibility.Visible;
                    mainWindow.textboxThanhTien.Visibility = Visibility.Visible;
                    break;
                case Table.HoaDonBan:
                    HideAllMenu();
                    mainWindow.labelMaHDBan.Visibility = Visibility.Visible;
                    mainWindow.labelMaNhanVien.Visibility = Visibility.Visible;
                    mainWindow.labelNgayBan.Visibility = Visibility.Visible;
                    mainWindow.labelMaKhach.Visibility = Visibility.Visible;
                    mainWindow.labelTongTien.Visibility = Visibility.Visible;

                    mainWindow.textboxMaHDBan.Visibility = Visibility.Visible;
                    mainWindow.textboxMaNhanVien.Visibility = Visibility.Visible;
                    mainWindow.textboxNgayBan.Visibility = Visibility.Visible;
                    mainWindow.textboxMaKhach.Visibility = Visibility.Visible;
                    mainWindow.textboxTongTien.Visibility = Visibility.Visible;
                    break;
                case Table.KhachHang:
                    HideAllMenu();
                    mainWindow.labelMaKhachHang.Visibility = Visibility.Visible;
                    mainWindow.labelTenKhachHang.Visibility = Visibility.Visible;
                    mainWindow.labelDiaChiKH.Visibility = Visibility.Visible;
                    mainWindow.labelSDTKH.Visibility = Visibility.Visible;

                    mainWindow.textboxMaKhachHang.Visibility = Visibility.Visible;
                    mainWindow.textboxTenKhachHang.Visibility = Visibility.Visible;
                    mainWindow.textboxDiaChiKH.Visibility = Visibility.Visible;
                    mainWindow.textboxSDTKH.Visibility = Visibility.Visible;
                    break;
                case Table.NhanVien:
                    HideAllMenu();
                    mainWindow.labelMaNhanVien.Visibility = Visibility.Visible;
                    mainWindow.labelTenNhanVien.Visibility = Visibility.Visible;
                    mainWindow.labelDiaChiNV.Visibility = Visibility.Visible;
                    mainWindow.labelSDTNV.Visibility = Visibility.Visible;
                    mainWindow.labelNgaySinh.Visibility = Visibility.Visible;
                    mainWindow.labelGioiTinh.Visibility = Visibility.Visible;

                    mainWindow.textboxMaNhanVien.Visibility = Visibility.Visible;
                    mainWindow.textboxTenNhanVien.Visibility = Visibility.Visible;
                    mainWindow.textboxDiaChiNV.Visibility = Visibility.Visible;
                    mainWindow.textboxSDTNV.Visibility = Visibility.Visible;
                    mainWindow.textboxNgaySinh.Visibility = Visibility.Visible;
                    mainWindow.textboxGioiTinh.Visibility = Visibility.Visible;
                    break;
                case Table.SanPham:
                    HideAllMenu();
                    mainWindow.labelMaSanPham.Visibility = Visibility.Visible;
                    mainWindow.labelTenSanPham.Visibility = Visibility.Visible;
                    mainWindow.labelMaChatLieu.Visibility = Visibility.Visible;
                    mainWindow.labelSoLuong.Visibility = Visibility.Visible;
                    mainWindow.labelDonGiaNhap.Visibility = Visibility.Visible;
                    mainWindow.labelDonGiaBan.Visibility = Visibility.Visible;
                    mainWindow.labelGhiChu.Visibility = Visibility.Visible;

                    mainWindow.textboxMaSanPham.Visibility = Visibility.Visible;
                    mainWindow.textboxTenSanPham.Visibility = Visibility.Visible;
                    mainWindow.textboxMaChatLieu.Visibility = Visibility.Visible;
                    mainWindow.textboxSoLuong.Visibility = Visibility.Visible;
                    mainWindow.textboxDonGiaNhap.Visibility = Visibility.Visible;
                    mainWindow.textboxDonGiaBan.Visibility = Visibility.Visible;
                    mainWindow.textboxGhiChu.Visibility = Visibility.Visible;
                    break;
            }
        }
        public static void LoadFields( Table? table, DataRowView row ) {
            try {
                switch (table) {
                    case Table.ChatLieu:
                        mainWindow.textboxMaChatLieu.Text = row["MaChatLieu"].ToString();
                        mainWindow.textboxTenChatLieu.Text = row["TenChatLieu"].ToString();
                        break;
                    case Table.ChiTietHoaDon:
                        mainWindow.textboxMaHDBan.Text = row["MaHDBan"].ToString();
                        mainWindow.textboxMaHang.Text = row["MaHang"].ToString();
                        mainWindow.textboxSoLuong.Text = row["SoLuong"].ToString();
                        mainWindow.textboxDonGia.Text = row["DonGia"].ToString();
                        mainWindow.textboxGiamGia.Text = row["GiamGia"].ToString();
                        mainWindow.textboxThanhTien.Text = row["ThanhTien"].ToString();
                        break;
                    case Table.HoaDonBan:
                        mainWindow.textboxMaHDBan.Text = row["MaHDBan"].ToString();
                        mainWindow.textboxMaNhanVien.Text = row["MaNhanVien"].ToString();
                        mainWindow.textboxNgayBan.Text = row["NgayBan"].ToString().Split(' ')[0];
                        mainWindow.textboxMaKhach.Text = row["MaKhach"].ToString();
                        mainWindow.textboxTongTien.Text = row["TongTien"].ToString();
                        break;
                    case Table.KhachHang:
                        mainWindow.textboxMaKhachHang.Text = row["MaKhachhang"].ToString();
                        mainWindow.textboxTenKhachHang.Text = row["TenKhachHang"].ToString();
                        mainWindow.textboxDiaChiKH.Text = row["DiaChi"].ToString();
                        mainWindow.textboxSDTKH.Text = row["SDT"].ToString();
                        break;
                    case Table.NhanVien:
                        mainWindow.textboxMaNhanVien.Text = row["MaNhanVien"].ToString();
                        mainWindow.textboxTenNhanVien.Text = row["TenNhanVien"].ToString();
                        mainWindow.textboxDiaChiNV.Text = row["DiaChi"].ToString();
                        mainWindow.textboxSDTNV.Text = row["SDT"].ToString();
                        mainWindow.textboxNgaySinh.Text = row["NgaySinh"].ToString().Split(' ')[0];
                        mainWindow.textboxGioiTinh.Text = row["GioiTinh"].ToString();
                        break;
                    case Table.SanPham:
                        mainWindow.textboxMaSanPham.Text = row["MaSanPham"].ToString();
                        mainWindow.textboxTenSanPham.Text = row["TenSanPham"].ToString();
                        mainWindow.textboxMaChatLieu.Text = row["MaChatLieu"].ToString();
                        mainWindow.textboxSoLuong.Text = row["SoLuong"].ToString();
                        mainWindow.textboxDonGiaNhap.Text = row["DonGiaNhap"].ToString();
                        mainWindow.textboxDonGiaBan.Text = row["DonGiaBan"].ToString();
                        mainWindow.textboxGhiChu.Text = row["GhiChu"].ToString();
                        break;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
