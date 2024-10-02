using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using РаботаСБД;
namespace WindowsFormsApp1
{
    public partial class Request : Form
    {
        private int selectedId;
        private SqlConnection connection;
        int count = 0;
        void LoadQuery()
        {
            
            if (Class1.role == "Заказчик")
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("Новая заявка");
                Class1.LoadDataWithParameter("SELECT requestID AS ID, dateRequest AS [Дата заявки], startDate AS [Начало работ], МодельОргТехники.orgTechModel AS [Модель оргтехники], problemDescryption AS [Описание проблемы], requestStatus AS [Статус], completionDate AS [Конец работ], Детали.detailName AS Деталь, cost AS Стоимость, Мастера.FIO AS [ФИО мастера], Пользователи.FIO AS [ФИО клиента], Пользователи.phone AS [Телефон клиента] FROM [dbo].[Заявки] LEFT JOIN [Мастера] ON Заявки.masterID = Мастера.masterID LEFT JOIN [Пользователи] ON Заявки.userID = Пользователи.userID LEFT JOIN [МодельОргТехники] ON Заявки.idorgTechModel = МодельОргТехники.modelID LEFT JOIN [Детали] ON Заявки.repairParts = Детали.detailID WHERE Пользователи.FIO = @clientFIO", dataGridView1, "clientFIO", Class1.FIO);
                count = dataGridView1.Rows.Count;
                toolStripStatusLabel1.Text = $"Записей: {count}";
            }
            else if (Class1.role == "Мастер")
            {
                Class1.LoadDataWithParameter("SELECT requestID AS ID, dateRequest AS [Дата заявки], startDate AS [Начало работ], МодельОргТехники.orgTechModel AS [Модель оргтехники], problemDescryption AS [Описание проблемы], requestStatus AS [Статус], completionDate AS [Конец работ], Детали.detailName AS Деталь, cost AS Стоимость, Мастера.FIO AS [ФИО мастера], Пользователи.FIO AS [ФИО клиента], Пользователи.phone AS [Телефон клиента] FROM [dbo].[Заявки] LEFT JOIN [Мастера] ON Заявки.masterID = Мастера.masterID LEFT JOIN [Пользователи] ON Заявки.userID = Пользователи.userID LEFT JOIN [МодельОргТехники] ON Заявки.idorgTechModel = МодельОргТехники.modelID LEFT JOIN [Детали] ON Заявки.repairParts = Детали.detailID WHERE Мастера.FIO = @masterFIO", dataGridView1, "masterFIO", Class1.FIO);
                count = dataGridView1.Rows.Count;
                toolStripStatusLabel1.Text = $"Записей: {count}";
            }
            else
            {
                Class1.LoadData("SELECT requestID AS ID, dateRequest AS [Дата заявки], startDate AS [Начало работ], МодельОргТехники.orgTechModel AS [Модель оргтехники], problemDescryption AS [Описание проблемы], requestStatus AS [Статус], completionDate AS [Конец работ], Детали.detailName AS Деталь, cost AS Стоимость, Мастера.FIO AS [ФИО мастера], Пользователи.FIO AS [ФИО клиента], Пользователи.phone AS [Телефон клиента] FROM [dbo].[Заявки] LEFT JOIN [Мастера] ON Заявки.masterID = Мастера.masterID LEFT JOIN [Пользователи] ON Заявки.userID = Пользователи.userID LEFT JOIN [МодельОргТехники] ON Заявки.idorgTechModel = МодельОргТехники.modelID LEFT JOIN [Детали] ON Заявки.repairParts = Детали.detailID", dataGridView1);
                count = dataGridView1.Rows.Count;
                toolStripStatusLabel1.Text = $"Записей: {count}";
            }
        }
        public Request()
        {
            InitializeComponent();
            connection = Class1.connection;
        }
        private void Request_Load(object sender, EventArgs e)
        {
            Class1.LoadDataToCombobox("orgTechModel", "МодельОргТехники", comboBox1, "modelID");
            Class1.LoadDataToCombobox("detailName", "Детали", comboBox3, "detailID");
            Class1.LoadDataToCombobox("FIO", "Мастера", comboBox4, "masterID");
            comboBox5.Items.Clear();
            connection.Open();
            string query = $"SELECT * FROM [dbo].[Пользователи] WHERE role = @role ";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@role", "Заказчик");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable tb = new DataTable();
            da.Fill(tb);
            comboBox5.DataSource = tb;
            comboBox5.DisplayMember = "FIO";
            comboBox5.ValueMember = "userID";
            comboBox5.SelectedIndex = -1;
            connection.Close();
            if (Class1.role == "Заказчик")
            {
                int index = comboBox5.FindStringExact(Class1.FIO);
                comboBox5.SelectedIndex = index;
            }
            else if (Class1.role == "Мастер")
            {
                int index = comboBox4.FindStringExact(Class1.FIO);
                comboBox4.SelectedIndex = index;
            }
            if (Class1.role == "Заказчик")
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                textBox1.Enabled = true;
                comboBox1.Enabled = true;
                button1.Visible = true;
                button1.Enabled = false;
                button3.Enabled = false;
                button2.Visible = false;
                button3.Visible = true;
                comboBox2.Enabled = false;
            }
            else if (Class1.role == "Мастер")
            {
                button2.Text = "Изменить статус/деталь";
                textBox1.Enabled = false;
                comboBox1.Enabled = false;
                button1.Visible = false;
                button2.Visible = true;
                button3.Visible = false;
                button2.Enabled = false;
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox2.Enabled = true;
                comboBox4.Enabled = false;
            }
            else if (Class1.role == "Оператор")
            {
                button2.Text = "Изменить статус/мастера";
                textBox1.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                comboBox1.Enabled = false;
                button1.Visible = false;
                button2.Visible = true;
                button3.Visible = true;
                button2.Enabled = false;
                button3.Enabled = false;
                comboBox2.Enabled = true;
                comboBox4.Enabled = true;
            }
            else if (Class1.role == "Менеджер")
            {
                button2.Text = "Изменить статус/мастера";
                textBox1.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = true;
                comboBox1.Enabled = false;
                button1.Visible = false;
                button2.Visible = true;
                button3.Visible = true;
                button2.Enabled = false;
                button3.Enabled = false;
                comboBox2.Enabled = true;
                comboBox4.Enabled = true;
            }
            LoadQuery();
        }
        private int masterID;
        private void button2_Click(object sender, EventArgs e)
        {
            if (Class1.role == "Оператор")
            {
                if (comboBox4.SelectedValue != null)
                {
                    if (comboBox2.SelectedItem == "В процессе ремонта")
                    {
                        masterID = comboBox1.SelectedIndex;
                        string query = "UPDATE [dbo].[Заявки] SET requestStatus = @requestStatus, startDate = @startDate, masterID = @master WHERE [requestID] = @SelectedId";
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = query;
                        connection.Open();
                        command.Parameters.AddWithValue("@requestStatus", comboBox2.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@startDate", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@master", comboBox4.SelectedValue.ToString());
                        command.Parameters.AddWithValue("@SelectedId", selectedId);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Можно поставить только статус \"В процессе ремонта\". Остальные статусы ставит мастер.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Чтобы поставить статус \"В процессе ремонта\", выберите мастера.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (Class1.role == "Мастер")
            {
                if ((comboBox2.SelectedItem.ToString() == "Готова к выдаче" || comboBox2.SelectedItem.ToString() == "В процессе ремонта" || comboBox2.SelectedItem.ToString() == "Завершена" ) && comboBox3.SelectedIndex != -1)
                {
                    TimeSpan time = dateTimePicker2.Value - dateTimePicker1.Value;
                    int days = time.Days;
                    string query1 = "SELECT detailCost from [dbo].[Детали] WHERE detailID = @ID";
                    SqlCommand command1 = connection.CreateCommand();
                    command1.CommandText = query1;
                    connection.Open();
                    int i = comboBox3.SelectedIndex + 1;
                    command1.Parameters.AddWithValue("@ID", i);
                    object price = command1.ExecuteScalar();
                    int pricedetail = (int)price;
                    int totalCost = days * 500 + pricedetail;
                    connection.Close();
                    string query = "UPDATE [dbo].[Заявки] SET requestStatus = @requestStatus, completionDate = @completionDate, repairParts = @repairParts, cost = @cost WHERE [requestID] = @SelectedId";
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = query;
                    connection.Open();
                    command.Parameters.AddWithValue("@requestStatus", (comboBox2.SelectedItem).ToString());
                    command.Parameters.AddWithValue("@completionDate", DateTime.Now);
                    command.Parameters.AddWithValue("@repairParts", comboBox3.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@cost", totalCost);
                    command.Parameters.AddWithValue("@SelectedId", selectedId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Не выбрана деталь, которая была/будет починена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (Class1.role == "Менеджер")
            {
                if (comboBox4.SelectedValue != null && comboBox2.SelectedItem.ToString() == "В процессе ремонта")
                {
                    TimeSpan time = dateTimePicker2.Value - dateTimePicker1.Value;
                    int days = time.Days;
                    if (days > 0)
                    {
                        string query = "UPDATE [dbo].[Заявки] SET requestStatus = @requestStatus, completionDate = @completionDate, masterID = @master WHERE [requestID] = @SelectedId";
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = query;
                        connection.Open();
                        command.Parameters.AddWithValue("@requestStatus", (comboBox2.SelectedItem).ToString());
                        command.Parameters.AddWithValue("@completionDate", dateTimePicker2.Value);
                        command.Parameters.AddWithValue("@master", comboBox4.SelectedValue.ToString());
                        command.Parameters.AddWithValue("@SelectedId", selectedId);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Не правильно указаны даты. Проверьте правильность!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Чтобы добавить мастера для выполнения заявки, нужно изменить статус на \"В процессе ремонта\"", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            LoadQuery();
            selectedId = 0;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = -1;
            numericUpDown1.Value = 0;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                selectedId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                if (selectedRow.Cells["Начало работ"].Value != DBNull.Value)
                {
                    dateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["Начало работ"].Value);
                }
                comboBox1.SelectedIndex = comboBox1.FindStringExact(selectedRow.Cells["Модель оргтехники"].Value.ToString());
                textBox1.Text = selectedRow.Cells["Описание проблемы"].Value.ToString();
                comboBox2.SelectedIndex = comboBox2.FindStringExact(selectedRow.Cells["Статус"].Value.ToString());
                if (selectedRow.Cells["Конец работ"].Value != DBNull.Value)
                {
                    dateTimePicker2.Value = Convert.ToDateTime(selectedRow.Cells["Конец работ"].Value);
                }
                comboBox3.SelectedIndex = comboBox3.FindStringExact(selectedRow.Cells["Деталь"].Value.ToString());
                numericUpDown1.Value = Convert.ToDecimal(selectedRow.Cells["Стоимость"].Value);
                comboBox4.SelectedIndex = comboBox4.FindStringExact(selectedRow.Cells["ФИО мастера"].Value.ToString());
                comboBox5.SelectedIndex = comboBox5.FindStringExact(selectedRow.Cells["ФИО клиента"].Value.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(Class1.role == "Заказчик")
            {
                if (comboBox2.SelectedItem == "Завершена" || comboBox2.SelectedItem == "Готова к выдаче"|| comboBox2.SelectedItem == "В процессе ремонта")
                {
                    MessageBox.Show("Нельзя удалить заявку самостоятельно, потому что она в работе. Сделеайте это через оператора", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            Class1.DeleteRow("requestID", selectedId, "Заявки");
            LoadQuery();
            selectedId = 0;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = -1;
            numericUpDown1.Value = 0;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string query = "INSERT INTO [dbo].[Заявки] (dateRequest, idorgTechModel,problemDescryption,requestStatus,cost, userID) VALUES (@dateRequest, @idorgTechModel,@problemDescryption,@requestStatus,@cost,@userID)";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@dateRequest", DateTime.Now);
            command.Parameters.AddWithValue("@idorgTechModel", comboBox1.SelectedValue.ToString());
            command.Parameters.AddWithValue("@problemDescryption", textBox1.Text);
            command.Parameters.AddWithValue("@requestStatus", "Новая заявка");
            command.Parameters.AddWithValue("@cost", 0);
            command.Parameters.AddWithValue("@userID", comboBox5.SelectedValue.ToString());
            MessageBox.Show("Запись успешно добавлена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            command.ExecuteScalar();
            connection.Close();
            LoadQuery();
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = -1;
            numericUpDown1.Value = 0;
            comboBox4.SelectedIndex = -1;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Class1.role == "Мастер")
            {
                Comment CommentForm = new Comment(selectedId, masterID);
                CommentForm.ShowDialog();
            }
            else
            {
                connection.Open();
                string checkQuery = "SELECT COUNT(*) FROM [dbo].[Комментарии] WHERE requestID = @requestID";
                SqlCommand checkCommand = connection.CreateCommand();
                checkCommand.CommandText = checkQuery;
                checkCommand.Parameters.AddWithValue("@requestID", selectedId);
                int count = (int)checkCommand.ExecuteScalar();
                connection.Close();
                if (count > 0)
                {
                    Comment CommentForm = new Comment(selectedId);
                    CommentForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Нету комментариев от мастера", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool allEmpty = string.IsNullOrEmpty(textBox1.Text) ||
                                     comboBox1.SelectedIndex == -1;
            button1.Enabled = !allEmpty;
            if (selectedId != 0)
            {
                button2.Enabled = !allEmpty;
                button3.Enabled = !allEmpty;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool allEmpty = string.IsNullOrEmpty(textBox1.Text) ||
                         comboBox1.SelectedIndex == -1;
            button1.Enabled = !allEmpty;
            if (selectedId != 0)
            {
                button2.Enabled = !allEmpty;
                button3.Enabled = !allEmpty;
            }

        }
        void LoadQuery1()
        {
            if (Class1.role == "Заказчик")
            {
                Class1.LoadDataWithParameter("SELECT requestID AS ID, dateRequest AS [Дата заявки], startDate AS [Начало работ], МодельОргТехники.orgTechModel AS [Модель оргтехники], problemDescryption AS [Описание проблемы], requestStatus AS [Статус], completionDate AS [Конец работ], Детали.detailName AS Деталь, cost AS Стоимость, Мастера.FIO AS [ФИО мастера], Пользователи.FIO AS [ФИО клиента], Пользователи.phone AS [Телефон клиента] FROM [dbo].[Заявки] LEFT JOIN [Мастера] ON Заявки.masterID = Мастера.masterID LEFT JOIN [Пользователи] ON Заявки.userID = Пользователи.userID LEFT JOIN [Детали] ON Заявки.repairParts = Детали.detailID LEFT JOIN [МодельОргТехники] ON Заявки.idorgTechModel = МодельОргТехники.modelID WHERE Пользователи.FIO = @clientFIO AND Заявки.requestStatus != N'Завершена'", dataGridView1, "clientFIO", Class1.FIO);
                toolStripStatusLabel1.Text = $"Записей: {dataGridView1.Rows.Count} из {count}";
            }
            else if (Class1.role == "Мастер")
            {
                Class1.LoadDataWithParameter("SELECT requestID AS ID, dateRequest AS [Дата заявки], startDate AS [Начало работ], МодельОргТехники.orgTechModel AS [Модель оргтехники], problemDescryption AS [Описание проблемы], requestStatus AS [Статус], completionDate AS [Конец работ], Детали.detailName AS Деталь, cost AS Стоимость, Мастера.FIO AS [ФИО мастера], Пользователи.FIO AS [ФИО клиента], Пользователи.phone AS [Телефон клиента] FROM [dbo].[Заявки] LEFT JOIN [Мастера] ON Заявки.masterID = Мастера.masterID LEFT JOIN [Пользователи] ON Заявки.userID = Пользователи.userID LEFT JOIN [Детали] ON Заявки.repairParts = Детали.detailID LEFT JOIN [МодельОргТехники] ON Заявки.idorgTechModel = МодельОргТехники.modelID WHERE Мастера.FIO = @masterFIO AND Заявки.requestStatus != N'Завершена'", dataGridView1, "masterFIO", Class1.FIO);
                toolStripStatusLabel1.Text = $"Записей: {dataGridView1.Rows.Count} из {count}";
            }
            else
            {
                Class1.LoadData("SELECT requestID AS ID, dateRequest AS [Дата заявки], startDate AS [Начало работ], МодельОргТехники.orgTechModel AS [Модель оргтехники], problemDescryption AS [Описание проблемы], requestStatus AS [Статус], completionDate AS [Конец работ], Детали.detailName AS Деталь, cost AS Стоимость, Мастера.FIO AS [ФИО мастера], Пользователи.FIO AS [ФИО клиента], Пользователи.phone AS [Телефон клиента] FROM [dbo].[Заявки] LEFT JOIN [Мастера] ON Заявки.masterID = Мастера.masterID LEFT JOIN [Пользователи] ON Заявки.userID = Пользователи.userID LEFT JOIN [Детали] ON Заявки.repairParts = Детали.detailID LEFT JOIN [МодельОргТехники] ON Заявки.idorgTechModel = МодельОргТехники.modelID WHERE Заявки.requestStatus != N'Завершена'", dataGridView1);
                toolStripStatusLabel1.Text = $"Записей: {dataGridView1.Rows.Count} из {count}";
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                LoadQuery1();
            }
            else
            {
                LoadQuery();
            }
        }
    }
}
