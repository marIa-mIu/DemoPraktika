using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace praktika_demo
{
    public partial class form_Edit : Form
    {
        Form1 Form1 = new Form1();
        public form_Edit()
        {
            InitializeComponent();
            LoadPartnerTypes();
        }


        private void LoadPartnerTypes()
        {
            // Код для загрузки типов партнеров в ComboBox
            comboBox1.Items.Add("ЗАО");
            comboBox1.Items.Add("ПАО");
            comboBox1.Items.Add("ООО");
            comboBox1.Items.Add("ОАО");
            comboBox1.SelectedItem = "ПАО";
        }

        public void InsertingForm()
        {
            button1.Text = "Добавить";
        }



        public void UpdatingForm(string message)
        {
            string ba = message;
            string connectionString = "Host = localhost; port = 5433; Username = postgres; password = 7492; database = masterPol";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT company_name, type_partner, rating, legal_address, director_name, contact_phone, contact_email FROM partner where company_name = @value1 LIMIT 1;";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("value1", ba);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox1.Text = reader[0].ToString(); // Для column1
                                comboBox1.Text = reader[1].ToString(); // Для column2
                                textBox2.Text = reader[2].ToString(); // Для column3
                                textBox3.Text = reader[3].ToString(); // Для column4
                                textBox4.Text = reader[4].ToString(); // Для column5
                                textBox5.Text = reader[5].ToString(); // Для column6
                                textBox6.Text = reader[6].ToString(); // Для column7
                                                                      //command.ExecuteNonQuery();
                            }
                            else
                            {
                                MessageBox.Show("Данные не найдены для указанной компании.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void UpdateQuerry(string message)
        {
            string connectionString = "Host = localhost; port = 5433; Username = postgres; password = 7492; database = masterPol";

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE partner SET company_name = @value1, type_partner = @value2, rating = @value3, legal_address = @value4, director_name = @value5, contact_phone = @value6, contact_email = @value7 WHERE company_name = @value8";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("value1", textBox1.Text);
                        command.Parameters.AddWithValue("value2", comboBox1.Text);
                        command.Parameters.AddWithValue("value3", Convert.ToInt32(textBox2.Text));
                        command.Parameters.AddWithValue("value4", textBox3.Text);
                        command.Parameters.AddWithValue("value5", textBox4.Text);
                        command.Parameters.AddWithValue("value6", textBox5.Text);
                        command.Parameters.AddWithValue("value7", textBox6.Text);
                        command.Parameters.AddWithValue("value8", message);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Данные успешно обновлены.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить данные. Проверьте, существует ли запись с указанным именем компании.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void InsertQuerry(string message)
        {
            string connectionString = "Host = localhost; port = 5433; Username = postgres; password = 7492; database = masterPol";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO partner (company_name, type_partner, rating, legal_address, director_name, contact_phone, contact_email) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("value1", textBox1.Text);
                    command.Parameters.AddWithValue("value2", comboBox1.SelectedIndex + 1);
                    command.Parameters.AddWithValue("value3", Convert.ToInt32(textBox2.Text));
                    command.Parameters.AddWithValue("value4", textBox3.Text);
                    command.Parameters.AddWithValue("value5", textBox4.Text);
                    command.Parameters.AddWithValue("value6", textBox5.Text);
                    command.Parameters.AddWithValue("value7", textBox6.Text);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Данные успешно записны.");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось добавить данные. Проверьте на наличие ошибок.");
                    }
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {                
                if (button1.Text == "Изменить")
                {

                    UpdateQuerry(Form1.tempUrre);
                    this.Close();
                }
                else
                {
                    InsertQuerry(Form1.tempUrre);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
