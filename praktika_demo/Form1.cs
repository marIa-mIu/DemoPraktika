using System.Reflection.Emit;
using System.Windows.Forms;
using Npgsql;

namespace praktika_demo
{
    public partial class Form1 : Form
    {
        //private form_Edit form_Edit;
        string nameForEdit = "";
        public static string tempUrre;
        
        public Form1()
        {
            InitializeComponent();
            //form_Edit = formEdit;
            panel1.BackColor = ColorTranslator.FromHtml("#F4E8D3");

            int getPercent(int num)
            {
                if (num < 10000)
                    return 0;
                else if (num < 50000)
                    return 5;
                else if (num <= 300000)
                    return 10;
                else return 15;

            }

            string ConnString = "Host = localhost; port = 5433; Username = postgres; password = 7492; database = masterPol";
            
            using (var Conn = new NpgsqlConnection(ConnString))
            {
                try
                {
                    Conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT p.type_partner, p.company_name, p.director_name, p.contact_phone, p.rating, SUM(d.quantity_order) AS total_quantity_order FROM partner p JOIN draft d ON p.id_partner = d.partner_id GROUP BY p.type_partner, p.company_name, p.director_name, p.contact_phone, p.rating ORDER BY p.rating DESC;;", Conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {

                            //while (reader.Read())
                            //{
                            //    // Чтение значений из каждого столбца в текущей строке
                            //    string typePartner = reader[0].ToString();  // Столбец 1
                            //    string companyName = reader[1].ToString(); // Столбец 2
                            //    string directorName = reader[2].ToString(); // Столбец 3
                            //    string contactPhone = reader[3].ToString(); // Столбец 4
                            //    string rating = reader[4].ToString();  // Столбец 5

                            //    // Вывод значений на консоль или выполнение других действий
                            //    MessageBox.Show($"Тип партнера: {typePartner}, \n Компания: {companyName},\n Директор: {directorName},\n Телефон: {contactPhone},\n Рейтинг: {rating}");
                            //}
                            int groupCounter = 0; // для сквозной нумерации создаваемых элементов управления
                            while (reader.Read())
                            {
                                int percentSale = getPercent(Convert.ToInt32(reader[5]));
                                string companyName = reader[1].ToString();
                                GroupBox groupBox = new GroupBox();
                                groupBox.Text = "редактировать ";
                                groupBox.Name = companyName;
                                //groupBox.Name = "groupBox" + groupCounter;
                                groupBox.Size = new Size(450, 135);
                                groupBox.Location = new Point(30, 40 * groupCounter);
                                groupBox.Font = new Font(" Segoe UI", 8);
                                groupBox.DoubleClick += groupBox_DoubleClick;
                                groupBox.MouseEnter += groupBox_MouseEnter;
                                groupBox.MouseLeave += groupBox_MouseLeave;



                                for (int j = 0; j < 5; j++)
                                {
                                    System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                                    switch (j)                      // форматированный вывод данных
                                    {
                                        case 0:
                                            label.Text = reader[j].ToString() + " | " + reader[++j].ToString() + "   " + percentSale + "%"; //тип + наименование + процент скидки
                                            label.Font = new Font(" Segoe UI", 13, FontStyle.Bold);
                                            break;
                                        case 1:
                                            j++;                                                                                               //тип + наименование
                                            break;
                                        case 2:
                                            label.Text = "Директор " + reader[j].ToString();
                                            label.Font = new Font(" Segoe UI", 10);
                                            break;
                                        case 3:
                                            label.Text = "+7 " + reader[j].ToString();
                                            label.Font = new Font(" Segoe UI", 10);
                                            break;
                                        case 4:
                                            label.Text = "Рейтинг: " + reader[j].ToString();
                                            label.Font = new Font(" Segoe UI", 10);
                                            break;
                                    }

                                    label.Name = "label" + groupCounter;
                                    label.Size = new Size(300, 20);
                                    label.Location = new Point(40, 25 * (j));
                                    groupBox.Controls.Add(label);
                                    groupCounter++;
                                }
                                panel1.Controls.Add(groupBox);                                
                            }
                        }
                    }

                    Conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"подключение НЕ прошло успешно/n ОШибка:  {ex.Message}");
                }

            }
        }
        private void groupBox_DoubleClick(object sender, EventArgs e)
        {
            GroupBox clickedGroupBox = sender as GroupBox;
            if (clickedGroupBox != null)
                tempUrre = clickedGroupBox.Name;           


            form_Edit form_Edit = new form_Edit();
            form_Edit.UpdatingForm(tempUrre);
            form_Edit.ShowDialog();
            

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            form_Edit form_Edit = new form_Edit();
            form_Edit.InsertingForm();
            form_Edit.ShowDialog();
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {

        }
        private void groupBox_MouseEnter(object sender, EventArgs e)
        {
            GroupBox clickedGroupBox = sender as GroupBox;
            if (clickedGroupBox != null)
                clickedGroupBox.BackColor = ColorTranslator.FromHtml("#67BA80");
        }

        private void groupBox_MouseLeave(object sender, EventArgs e)
        {
            GroupBox clickedGroupBox = sender as GroupBox;
            if (clickedGroupBox != null)
                clickedGroupBox.BackColor = Color.Transparent;
        }

    }
}