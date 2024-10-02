using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using РаботаСБД;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Data;
using System.Data.Common;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Setup()
        {
            string query = "DELETE FROM testTable";
            Class1.connection.Open();
            SqlCommand cmd = new SqlCommand(query, Class1.connection);
            int i = cmd.ExecuteNonQuery();
            Class1.connection.Close();
        }
        [TestCleanup]
        public void CleanUp()
        {
            Class1.connection.Close();
        }
        [TestMethod]
        public void TestAddRecordToATable()
        {
            string[,] a = new string[,] {  
                {"user1", "password1" },
                {"user2", "password2" },
                {"user3", "password3" },
                {"user4", "password4" },
                {"user5", "password5" },
                {"user6", "password6" },
                {"user7", "password7" }
            };
            Class1.connection.Open();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                string query = "INSERT INTO testTable (login, password) VALUES (@login, @password)";
                using (SqlCommand cmd = new SqlCommand(query, Class1.connection))
                {
                    cmd.Parameters.AddWithValue("@login", a[i, 0]);
                    cmd.Parameters.AddWithValue("@password", a[i, 1]);
                    cmd.ExecuteNonQuery();
                }
            }
            string query1 = "SELECT COUNT(*) FROM testTable";
            SqlCommand cmd1 = new SqlCommand(query1, Class1.connection);
            int result1 = (int)cmd1.ExecuteScalar();
            Assert.AreEqual(7, result1);
            Class1.connection.Close();
        }
        [TestMethod]
        public void TestEditRecordToATable()
        {
            Class1.connection.Open();
            string checkQuery = "SELECT COUNT(*) FROM testTable WHERE login = @login";
            using (SqlCommand checkCmd = new SqlCommand(checkQuery, Class1.connection))
            {
                checkCmd.Parameters.AddWithValue("@login", "user3");
                int recordExists = (int)checkCmd.ExecuteScalar();
                if (recordExists > 0)
                {
                    string query = "UPDATE testTable SET password = @password WHERE login = @login";
                    using (SqlCommand cmd = new SqlCommand(query, Class1.connection))
                    {
                        cmd.Parameters.AddWithValue("@login", "user3");
                        cmd.Parameters.AddWithValue("@password", "newpassword3");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Assert.AreEqual(1, rowsAffected, "Запись не была обновлена");
                        string query1 = "SELECT password FROM testTable WHERE login = @login";
                        using (SqlCommand cmd1 = new SqlCommand(query1, Class1.connection))
                        {
                            cmd1.Parameters.AddWithValue("@login", "user3");
                            string result1 = (string)cmd1.ExecuteScalar();
                            Assert.AreEqual("newpassword3", result1, "Пароль не был обновлен правильно");
                        }
                    }
                }
            }
            Class1.connection.Close();
        }


        [TestMethod]
        public void TestDeleteRecordToATable()
        {
            Class1.connection.Open();
            string query = "DELETE FROM testTable WHERE login = @login";
            using (SqlCommand cmd = new SqlCommand(query, Class1.connection))
            {
                cmd.Parameters.AddWithValue("@login", "user4");
                cmd.ExecuteNonQuery();
            }
            string query1 = "SELECT COUNT(*) FROM testTable WHERE login = @login";
            using (SqlCommand cmd1 = new SqlCommand(query1, Class1.connection))
            {
                cmd1.Parameters.AddWithValue("@login", "user4");
                cmd1.ExecuteNonQuery();
                int result1 = (int)cmd1.ExecuteScalar();
                Assert.AreEqual(0, result1);
            }
            Class1.connection.Close();
        }
        [TestMethod]
        public void TestDuplicateRecordInsert()
        {
            Class1.connection.Open();
            string query = "INSERT INTO testTable (login, password) VALUES (@login, @password)";
            using (SqlCommand cmd = new SqlCommand(query, Class1.connection))
            {
                cmd.Parameters.AddWithValue("@login", "user1");
                cmd.Parameters.AddWithValue("@password", "password1");
                cmd.ExecuteNonQuery();
            }
            bool insertFailed = false;
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, Class1.connection))
                {
                    cmd.Parameters.AddWithValue("@login", "user1");
                    cmd.Parameters.AddWithValue("@password", "password123");
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                insertFailed = true;
            }
            Assert.IsTrue(insertFailed, "Дублирующая запись была успешно добавлена, что не допускается.");
            Class1.connection.Close();
        }
        [TestMethod]
        public void TestInsertEmptyFields()
        {
            Class1.connection.Open();

            string query = "INSERT INTO testTable (login, password) VALUES (@login, @password)";
            bool insertFailed = false;

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, Class1.connection))
                {
                    cmd.Parameters.AddWithValue("@login", null);
                    cmd.Parameters.AddWithValue("@password", null);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                insertFailed = true;
            }

            Assert.IsTrue(insertFailed, "Запись с пустыми полями была успешно добавлена, что не допускается.");

            Class1.connection.Close();
        }
    }
}

