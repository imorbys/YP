using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace РаботаСБД
{
    public class Class1
    {
        public static SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\429195-6\Desktop\ОргТехника\БДТест.mdf;Integrated Security=True;Connect Timeout=30");
        public static void LoadData(string q, DataGridView dgv)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(q, connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tb = new DataTable();
                da.Fill(tb);
                dgv.DataSource = tb;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                if (dgv.Rows.Count > 0)
                {
                    dgv.Rows[0].Selected = true;
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Данные не найдены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }
        public static void LoadDataLog(string q, DataGridView dgv)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(q, connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tb = new DataTable();
                da.Fill(tb);
                dgv.DataSource = tb;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                if (dgv.Rows.Count > 0)
                {
                    dgv.Rows[0].Selected = true;
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Данные не найдены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static void LoadDataWithParameter(string q, DataGridView dgv, string paramName, string paramValue)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(q, connection);
                cmd.Parameters.AddWithValue(paramName, paramValue);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tb = new DataTable();
                da.Fill(tb);
                dgv.DataSource = tb;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                if (dgv.Rows.Count > 0)
                {
                    dgv.Rows[0].Selected = true;
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Данные не найдены! ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
            }
        }
        public static string role;
        public static string FIO;
        public static string CheckUser(string login, string password)
        {
            try
            {
                connection.Open();
                string checkLogin = "SELECT COUNT(*) FROM Пользователи WHERE login = @login";
                SqlCommand cmd = new SqlCommand(checkLogin, connection);
                cmd.Parameters.AddWithValue("@login", login);
                int user = (int)cmd.ExecuteScalar();
                connection.Close();
                if (user == 0)
                {
                    return "Данного логина не существует";
                }
                connection.Open();
                string checkPass = "SELECT FIO FROM Пользователи WHERE login = @login AND password = @password";
                SqlCommand cmd1 = new SqlCommand(checkPass, connection);
                cmd1.Parameters.AddWithValue("@login", login);
                cmd1.Parameters.AddWithValue("@password", password);
                string name = (string)cmd1.ExecuteScalar();
                FIO = name;
                connection.Close();
                if (!string.IsNullOrEmpty(name))
                {
                    connection.Open();
                    string rolename = "SELECT role FROM Пользователи WHERE login = @login AND password = @password";
                    SqlCommand cmd2 = new SqlCommand(rolename, connection);
                    cmd2.Parameters.AddWithValue("@login", login);
                    cmd2.Parameters.AddWithValue("@password", password);
                    role = (string)cmd2.ExecuteScalar();
                    connection.Close();
                    return $"Авторизация прошла успешно.";
                }
                else
                {
                    return "Пароль неверный";
                }
            }
            catch (Exception ex)
            {
                return "Ошибка при проверке" + ex.Message;
            }
        }
        public static void DeleteRow(string fieldName, int selectedId, string tableName)
        {
            connection.Open();
            string query = $"DELETE FROM {tableName} WHERE {fieldName} = @SelectedId";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@SelectedId", selectedId);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Запись успешно удалена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            connection.Close();
        }
        public static void LoadDataToCombobox(string columnName, string tableName, ComboBox cmb, string valueMember)
        {
            try
            {
                cmb.Items.Clear();
                connection.Open();
                string query = $"SELECT * FROM {tableName}";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tb = new DataTable();
                da.Fill(tb);
                cmb.DataSource = tb;
                cmb.DisplayMember = columnName;
                cmb.ValueMember = valueMember;
                cmb.SelectedIndex = -1;
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
            }
        }
    }
}
