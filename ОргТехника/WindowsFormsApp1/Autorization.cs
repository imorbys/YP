using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using РаботаСБД;
namespace WindowsFormsApp1
{
    public partial class Autorization : Form
    {
        private SqlConnection connection;
        public Autorization()
        {
            InitializeComponent();
            this.Size = new Size(297, 352);
            label4.Visible = false;
            connection = Class1.connection;
        }
        public void AddLog(string login, bool isSec)
        {
            string bol;
            if (isSec == true)
            {
                bol = "Успех";
            }
            else
            {
                bol = "Не успех";
            }
            connection.Open();
            string query = "INSERT INTO [dbo].[Логи] (dateTime, login, isSuccessful) VALUES (@dateTime, @login, @isSeccessful)";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@dateTime", DateTime.Now);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@isSeccessful", bol);
            command.ExecuteScalar();
            connection.Close();
        }
        private int countdown = 180;
        private int count = 3;
        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            string result = Class1.CheckUser(login, password);
            if (result == "Авторизация прошла успешно.")
            {
                MessageBox.Show("Добро пожаловать", "Приветствие", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AddLog(login, true);
                MainForm form1 = new MainForm();
                form1.Show();
                this.Hide();
            }
            else if (result == "Данного логина не существует")
            {
                MessageBox.Show($"Попробуйте ввести другой логин, данного логина не существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (result == "Пароль неверный")
            {
                if (count == 3 || count == 2)
                {
                    count--;
                    MessageBox.Show($"Вы ввели неправильный пароль. Осталось попыток: {count}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddLog(login, false);
                    textBox2.Enabled= false;
                    button2.Enabled = false;
                    button1.Enabled = false;
                    this.Size = new Size(559, 352);
                    pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
                }else if (count == 1)
                {
                    count--;
                    AddLog(login, false);
                    button2.Enabled = false;
                    textBox2.Enabled = false;
                    button1.Enabled = false;
                    label4.Visible = true;
                    label4.Text = $"Запрет на авторизацию истечет через: {countdown / 60:D2}:{countdown % 60:D2}";
                    timer1.Start();
                }else if (count == 0)
                {
                    MessageBox.Show("Для предотвращения взлома данных приложение закроется", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    AddLog(login, false);
                    Application.Exit();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.ShowDialog();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bool allEmpty = string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text);
            button1.Enabled = !allEmpty;
        }
        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();
            Bitmap result = new Bitmap(Width, Height);
            int Xpos = rnd.Next(0, Width - 50);
            int Ypos = rnd.Next(15, Height - 15);
            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };
            Graphics g = Graphics.FromImage((Image)result);
            g.Clear(Color.Gray);
            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];
            g.DrawString(text,
                         new Font("Times New Roman", 15),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));
            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);
            return result;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
        }
        private string text = String.Empty;
        private bool captch = false;
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == this.text)
            {
                this.Size = new Size(297, 352);
                textBox3.Clear();
                button2.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
                MessageBox.Show("Каптча введена правильно!", "Успешно", MessageBoxButtons.OK);
            }
            else
                MessageBox.Show("Каптча введена неверно!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            bool allEmpty = string.IsNullOrEmpty(textBox3.Text);
            button4.Enabled = !allEmpty;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            countdown--;
            label4.Text = $"Запрет на авторизацию истечет через: {countdown / 60:D2}:{countdown % 60:D2}";
            if (countdown <= 0)
            {
                timer1.Stop();
                button2.Enabled = true;
                button1.Enabled = true;
                textBox2.Enabled = true;
                label4.Visible = false;
            }
        }
        int current = 1;
        private void button5_Click(object sender, EventArgs e)
        {
            if (current == 1)
            {
                textBox2.PasswordChar = '\0';
                current = 0;
                button5.ForeColor = Color.Green;
            }
            else
            {
                textBox2.PasswordChar = '*';
                current = 1;
                button5.ForeColor = Color.Black;
            }
        }

        private void Autorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
