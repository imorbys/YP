using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using РаботаСБД;

namespace WindowsFormsApp1
{
    public partial class ModelOrg : Form
    {
        private int selectedId;
        private SqlConnection connection;
        void LoadQuery()
        {
            Class1.LoadData("SELECT modelID AS ID, orgTechType AS [Тип оргтехники],orgTechModel AS [Модель оргтехники] FROM [dbo].[МодельОргТехники]", dataGridView1);
            dataGridView1.Columns["ID"].Width = 50;
            dataGridView1.Columns["Тип оргтехники"].Width = 220;
            dataGridView1.Columns["Модель оргтехники"].Width = 105;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            toolStripStatusLabel1.Text = $"Записей: {dataGridView1.RowCount}";
        }
        public ModelOrg()
        {
            InitializeComponent();
            connection = Class1.connection;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string checkQuery = "SELECT COUNT(*) FROM [dbo].[МодельОргТехники] WHERE orgTechModel = @Model";
            SqlCommand checkCommand = connection.CreateCommand();
            checkCommand.CommandText = checkQuery;
            checkCommand.Parameters.AddWithValue("@Model", textBox2.Text);
            int count = (int)checkCommand.ExecuteScalar();
            if (count > 0)
            {
                MessageBox.Show("Данная модель уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return;
            }
            string query = "INSERT INTO [dbo].[МодельОргТехники] (orgTechType, orgTechModel) VALUES (@Type, @Model)";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Type", textBox1.Text);
            command.Parameters.AddWithValue("@Model", textBox2.Text);
            MessageBox.Show("Запись успешно добавлена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            command.ExecuteScalar();
            connection.Close();
            textBox1.Clear();
            textBox2.Clear();
            LoadQuery();
        }
        private void Detail_Load(object sender, EventArgs e)
        {
            LoadQuery();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                selectedId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                textBox1.Text = selectedRow.Cells["Тип оргтехники"].Value.ToString();
                textBox2.Text = selectedRow.Cells["Модель оргтехники"].Value.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool allEmpty = string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text);
            button1.Enabled = !allEmpty;
            if (selectedId != 0)
            {
                button2.Enabled = !allEmpty;
                button3.Enabled = !allEmpty;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "UPDATE [dbo].[МодельОргТехники] SET [orgTechType] = @Type, [orgTechModel] = @Model WHERE [modelID] = @SelectedId";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            connection.Open();
            command.Parameters.AddWithValue("@Type", textBox1.Text);
            command.Parameters.AddWithValue("@Model", textBox2.Text);
            command.Parameters.AddWithValue("@SelectedId", selectedId);
            command.ExecuteNonQuery();
            MessageBox.Show("Запись успешно изменена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            connection.Close();
            LoadQuery();
            selectedId = 0;
            textBox1.Clear();
            textBox2.Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Class1.DeleteRow("modelID", selectedId, "МодельОргТехники");
            LoadQuery();
            selectedId = 0;
            textBox1.Clear();
        }
    }
}
