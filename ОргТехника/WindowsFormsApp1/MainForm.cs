using System;
using System.Drawing;
using System.Windows.Forms;
using РаботаСБД;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label3.Text = "ФИО: " + Class1.FIO;
            toolStripStatusLabel1.Text = "Роль: " + Class1.role;
            if (Class1.role == "Оператор")
            {
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
                button7.Visible = true;
                button7.Text = "Посмотреть отзывы";
                label2.Visible = false;
                button1.Location = new Point(51, 46);
                button2.Location = new Point(51, 103);
                button3.Location = new Point(51, 160);
                button4.Location = new Point(51, 217);
                button5.Location = new Point(51, 274);
            }
            else if (Class1.role == "Мастер")
            {
                button1.Visible = true;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = false;
                button7.Visible = false;
                label2.Visible = true;
                button1.Location = new Point(51, 46);
                button4.Location = new Point(51, 103);
                button5.Location = new Point(51, 160);
            }
            else
            {
                button1.Visible = true;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = true;
                button7.Text = "Оставить отзыв";
                label2.Visible = true;
                button1.Location = new Point(51, 46);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Request RequestForm = new Request();
            RequestForm.ShowDialog();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Masters MastersForm = new Masters();
            MastersForm.ShowDialog();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Users UsersForm = new Users();
            UsersForm.ShowDialog();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Detail DetailForm = new Detail();
            DetailForm.ShowDialog();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            ModelOrg OrgTechForm = new ModelOrg();
            OrgTechForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LogForm LogsForm = new LogForm();
            LogsForm.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Autorization AutoForm = new Autorization();
            AutoForm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            QRCode QRForm = new QRCode();
            QRForm.ShowDialog();
        }
    }
}
