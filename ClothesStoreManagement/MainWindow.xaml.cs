using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ClothesStoreManagement {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        public SqlConnection connection = new SqlConnection();
        private Table? CurrentTable = null;
        private bool IsNew = false;

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            Utils.HideAllMenu();
            Utils.HideButtons();
            Utils.DisableButtons();
        }

        private void buttonReconnect_Click( object sender, RoutedEventArgs e ) {
            ConnectToDatabase();
        }

        private void Window_ContentRendered( object sender, EventArgs e ) {
            ConnectToDatabase();
            foreach (Table table in Enum.GetValues(typeof(Table)))
                comboBoxSelectTable.Items.Add(table);
            LoadData();
            Utils.DisableAllFields();
        }
        private void ConnectToDatabase() {
            connection.ConnectionString = @"Data Source=.;Initial Catalog=QlyShopQuanAo;Integrated Security=True;";
            try {
                connection.Open();
                if (connection.State == ConnectionState.Open) {
                    buttonReconnect.Visibility = Visibility.Collapsed;
                    buttonReconnect.IsEnabled = false;
                    comboBoxSelectTable.IsEnabled = true;
                }
            }
            catch (Exception) {
                if (connection.State != ConnectionState.Open) {
                    MessageBox.Show("Không thể kết nối với Cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information);
                    buttonReconnect.Visibility = Visibility.Visible;
                    comboBoxSelectTable.IsEnabled = false;
                }
            }
        }
        private void LoadData() {
            if (connection.State != ConnectionState.Open || CurrentTable == null)
                return;
            GetTable(CurrentTable);
        }
        private void GetTable( Table? table ) {
            string GetTableQuery = "select * from " + table.ToString();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(GetTableQuery, connection);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            DataTable dataTable = dataSet.Tables[0];
            dataView.ItemsSource = dataTable.DefaultView;
            foreach (var column in dataView.Columns) {
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
        private void comboBoxSelectTable_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            //MessageBox.Show(Table.KhachHang.ToString() );
            Utils.ChangeButtonState(false);
            CurrentTable = (Table) Enum.ToObject(typeof(Table), comboBoxSelectTable.SelectedIndex);
            GetTable(CurrentTable);
            Utils.UnloadAllFields();
            Utils.LoadMenu(CurrentTable);
        }

        private void dataView_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            DataRowView row = (DataRowView) dataView.CurrentItem;
            if ( row == null ) {
                Utils.DisableButtons();
                Utils.UnloadAllFields();
                return;
            }
            Utils.LoadFields(CurrentTable, row);
            Utils.EnableButtons();
        }

        private void ActionButton_Click( object sender, RoutedEventArgs e ) {
            Button actionButton = (Button) sender;
            switch (actionButton.Name) {
                case "buttonInsert":
                    Utils.EnableAllFields();
                    Utils.ChangeButtonState(true);
                    Utils.UnloadAllFields();
                    dataView.SelectedIndex = -1;
                    foreach (var element in grid.Children)
                        if (element is TextBox tb)
                            if (tb.Visibility == Visibility.Visible) {
                                tb.Focus();
                                break;
                            }
                    IsNew = true;
                    break;
                case "buttonModify":
                    Utils.EnableAllFields();
                    Utils.ChangeButtonState(true);
                    IsNew = false;
                    break;
                case "buttonDelete":
                    if (MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác Nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;
                    Utils.DeleteFromDatabase(CurrentTable, Utils.GetFields(CurrentTable));
                    Utils.UnloadAllFields();
                    GetTable(CurrentTable);
                    break;
            }
        }

        private void EditingButton_Click( object sender, RoutedEventArgs e ) {
            if (((Button) sender).Name == "buttonConfirm") {
                Utils.InsertToDatabase(CurrentTable, Utils.GetFields(CurrentTable), IsNew);
                GetTable(CurrentTable);
            }
            Utils.ChangeButtonState(false);
            Utils.DisableAllFields();
            IsNew = false;
        }
    }
}
