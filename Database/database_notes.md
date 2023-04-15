# Notes for the database

- ChatLieu
  - MaChatLieu: nvarchar
  - TenChatLieu: nvarchar

- ChiTietHoaDon
  - MaHDBan: nvarchar
  - MaHang: nvarchar
  - SoLuong: float ?maybe int
  - DonGia: float
  - GiamGia: float
  - ThanhTien: float

- HoaDonBan
  - MaHDBan: nvarchar
  - MaNhanVien: nvarchar
  - NgayBan: datetime
  - MaKhach: nvarchar
  - TongTien: float

- KhachHang
  - MaKhachHang: nvarchar
  - TenKhachHang: nvarchar
  - DiaChi: nvarchar
  - SDT: nvarchar

- NhanVien
  - MaNhanVien: nvarchar
  - TenNhanVien: nvarchar
  - GioiTinh: nvarchar
  - NgaySinh: datetime
  - DiaChi: nvarchar
  - SDT: nvarchar

- SanPham
  - MaSanPham: nvarchar
  - TenSanPham: nvarchar
  - MaChatLieu: nvarchar
  - SoLuong: float ?maybe int
  - DonGiaNhap: float
  - DonGiaBan: float
  - Anh: nvarchar
  - GhiChu: nvarchar