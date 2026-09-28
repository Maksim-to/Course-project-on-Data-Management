using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Итоговая_проектная_работа
{
    public partial class Питомцы : Form
    {
        public Питомцы()
        {
            InitializeComponent();
        }

        private void Питомцы_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.Питомцы_с_ФИО". При необходимости она может быть перемещена или удалена.
            this.питомцы_с_ФИОTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Питомцы_с_ФИО);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DataGridViewColumn Col = default(System.Windows.Forms.DataGridViewColumn);
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    Col = dataGridViewTextBoxColumn2;
                    break;
                case 1:
                    Col = dataGridViewTextBoxColumn3;
                    break;
                case 2:
                    Col = dataGridViewTextBoxColumn4;
                    break;
                case 3:
                    Col = dataGridViewTextBoxColumn5;
                    break;
                case 4:
                    Col = dataGridViewTextBoxColumn6;
                    break;
                case 5:
                    Col = dataGridViewTextBoxColumn7;
                    break;
            }
            if (radioButton1.Checked)
                питомцы_с_ФИОDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Ascending);
            else
                питомцы_с_ФИОDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Descending);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            питомцы_с_ФИОBindingSource.Filter = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string searchText = textBox7.Text.Replace("'", "''"); // Экранируем одинарные кавычки
            питомцы_с_ФИОBindingSource.Filter = $"Кличка LIKE '%{searchText}%'"; // Используем интерполяцию строк и правильное расположение %
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, что все необходимые поля заполнены
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                    throw new ArgumentException("ФИО клиента не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                    throw new ArgumentException("Кличка не может быть пустой.");
                if (string.IsNullOrWhiteSpace(textBox3.Text))
                    throw new ArgumentException("Порода не может быть пустой.");
                if (string.IsNullOrWhiteSpace(textBox4.Text))
                    throw new ArgumentException("Окрас не может быть пустым.");
                if (comboBox1.SelectedItem == null)
                    throw new ArgumentException("Пол должен быть выбран.");
                if (numericUpDown1.Value <= 0)
                    throw new ArgumentException("Возраст должен быть больше 0.");

                // Присвоить значения входным параметрам процедуры с преобразованием типов
                sqlCommand.Parameters["@ФИО_Клиента"].Value = textBox1.Text.Trim();
                sqlCommand.Parameters["@Кличка"].Value = textBox2.Text.Trim();
                sqlCommand.Parameters["@Порода"].Value = textBox3.Text.Trim();
                sqlCommand.Parameters["@Окрас"].Value = textBox4.Text.Trim();
                sqlCommand.Parameters["@Пол"].Value = comboBox1.SelectedItem.ToString(); // Пол из ComboBox
                sqlCommand.Parameters["@Возраст"].Value = (int)numericUpDown1.Value; // Возраст из NumericUpDown

                // Открыть соединение с БД
                sqlConnection.Open();

                // Выполнить sql-выражение (хранимую процедуру)
                sqlCommand.ExecuteNonQuery();

                this.питомцы_с_ФИОTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Питомцы_с_ФИО);

                // Уведомить пользователя об успешном обновлении данных питомца
                MessageBox.Show("Питомец успешно добавлен.");
            }
            catch (ArgumentException ex)
            {
                // Обработка ошибок заполнения полей
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                // Выводим сообщение ошибки, которое возвращает хранимая процедура
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Закрыть соединение с БД
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }



        private void питомцы_с_ФИОDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, чтобы не был кликнут заголовок
            if (e.RowIndex >= 0)
            {
                // Получаем данные выбранной строки
                var row = питомцы_с_ФИОDataGridView.Rows[e.RowIndex];

                // Предположим, что у нас есть столбцы с именами "Id_Услуги", "Название"
                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                textBox4.Text = row.Cells[4].Value.ToString();
                comboBox1.Text = row.Cells[5].Value.ToString();
                numericUpDown1.Text = row.Cells[6].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, что все необходимые поля заполнены
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                    throw new ArgumentException("ФИО клиента не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                    throw new ArgumentException("Кличка не может быть пустой.");
                if (numericUpDown1.Value <= 0)
                    throw new ArgumentException("Возраст должен быть больше 0.");

                // Присвоить значения входным параметрам процедуры с преобразованием типов
                sqlCommandUpdate.Parameters["@ФИО_Клиента"].Value = textBox1.Text.Trim();
                sqlCommandUpdate.Parameters["@Кличка"].Value = textBox2.Text.Trim();
                sqlCommandUpdate.Parameters["@Возраст"].Value = (int)numericUpDown1.Value; // Возраст из NumericUpDown

                // Открыть соединение с БД
                sqlConnection.Open();

                // Выполнить sql-выражение (хранимую процедуру)
                sqlCommandUpdate.ExecuteNonQuery();

                this.питомцы_с_ФИОTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Питомцы_с_ФИО);

                // Уведомить пользователя об успешном обновлении данных питомца
                MessageBox.Show("Питомец успешно обновлен.");
            }
            catch (ArgumentException ex)
            {
                // Обработка ошибок заполнения полей
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                // Выводим сообщение ошибки, которое возвращает хранимая процедура
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Закрыть соединение с БД
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }
        private BindingSource питомцыBindingSource = new BindingSource();
        // Метод для перезагрузки данных

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
