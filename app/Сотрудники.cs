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
    public partial class Sotrudnik : Form
    {
        public Sotrudnik()
        {
            InitializeComponent();
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
            }
            if (radioButton1.Checked)
                сотрудникDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Ascending);
            else
                сотрудникDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Descending);
        }

        private void сотрудникBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.сотрудникBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.ветеринарная_клиникаDataSet);

        }

        private void Sotrudnik_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.График". При необходимости она может быть перемещена или удалена.
            this.графикTableAdapter.Fill(this.ветеринарная_клиникаDataSet.График);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.сотрудникTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Сотрудник);
            id_ГрафикаComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            сотрудникBindingSource.Filter = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Replace("'", "''"); // Экранируем одинарные кавычки
            сотрудникBindingSource.Filter = $"ФИО LIKE '%{searchText}%'"; // Используем интерполяцию строк и правильное расположение %
            //клиентBindingSource.Filter = "ФИО Like N'" + textBox1.Text + "%'";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id_ГрафикаComboBox.Text) || id_ГрафикаComboBox.Text == "0")
                {
                    MessageBox.Show("Поле Id_Графика не должно быть пустым или равным нулю.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idГрафика;
                if (!int.TryParse(id_ГрафикаComboBox.Text, out idГрафика))
                {
                    MessageBox.Show("Id_Графика должно быть числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (idГрафика <= 0)
                {
                    MessageBox.Show("Id_Графика должно быть больше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                    throw new ArgumentException("ФИО не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox3.Text))
                    throw new ArgumentException("Должность не может быть пустой.");
                if (dateTimePicker1.Value >= DateTime.Now.AddYears(-18))
                    throw new ArgumentException("Сотрудник должен быть старше 18 лет.");
                if (string.IsNullOrWhiteSpace(textBox5.Text))
                    throw new ArgumentException("Паспортные данные не могут быть пустыми.");
                if (string.IsNullOrWhiteSpace(textBox6.Text))
                    throw new ArgumentException("Адрес электронной почты не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox7.Text))
                    throw new ArgumentException("Телефон не может быть пустым.");
                if (id_ГрафикаComboBox.SelectedValue == null)
                    throw new ArgumentException("График должен быть выбран.");

                sqlCommand.Parameters["@ФИО"].Value = textBox2.Text.Trim();
                sqlCommand.Parameters["@Должность"].Value = textBox3.Text.Trim();
                sqlCommand.Parameters["@ДатаРождения"].Value = dateTimePicker1.Value;
                sqlCommand.Parameters["@ПаспортныеДанные"].Value = textBox5.Text.Trim();
                sqlCommand.Parameters["@АдресЭлектроннойПочты"].Value = textBox6.Text.Trim();
                sqlCommand.Parameters["@Телефон"].Value = textBox7.Text.Trim();
                sqlCommand.Parameters["@Id_Графика"].Value = id_ГрафикаComboBox.SelectedValue;

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                this.сотрудникTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Сотрудник);
                MessageBox.Show("Сотрудник успешно добавлен.");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();
            }
        }





        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                    throw new ArgumentException("ФИО не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox3.Text))
                    throw new ArgumentException("Должность не может быть пустой.");
                if (dateTimePicker1.Value >= DateTime.Now.AddYears(-18))
                    throw new ArgumentException("Сотрудник должен быть старше 18 лет.");
                if (string.IsNullOrWhiteSpace(textBox5.Text))
                    throw new ArgumentException("Паспортные данные не могут быть пустыми.");
                if (string.IsNullOrWhiteSpace(textBox6.Text))
                    throw new ArgumentException("Адрес электронной почты не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox7.Text))
                    throw new ArgumentException("Телефон не может быть пустым.");
                if (id_ГрафикаComboBox.SelectedValue == null)
                    throw new ArgumentException("График должен быть выбран.");

                // Присвоить значения входным параметрам процедуры с преобразованием типов
                sqlCommandUpdate.Parameters["@ФИО"].Value = textBox2.Text;
                sqlCommandUpdate.Parameters["@Должность"].Value = textBox3.Text;
                sqlCommandUpdate.Parameters["@ДатаРождения"].Value = dateTimePicker1.Value; // Используем Value для даты
                sqlCommandUpdate.Parameters["@ПаспортныеДанные"].Value = textBox5.Text;
                sqlCommandUpdate.Parameters["@АдресЭлектроннойПочты"].Value = textBox6.Text;
                sqlCommandUpdate.Parameters["@Телефон"].Value = textBox7.Text;
                sqlCommandUpdate.Parameters["@Id_Графика"].Value = id_ГрафикаComboBox.SelectedValue; // Используем SelectedValue

                // Открыть соединение с БД
                sqlConnection.Open();

                // Выполнить sql-выражение (хранимую процедуру)
                sqlCommandUpdate.ExecuteNonQuery();
                this.сотрудникTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Сотрудник);
                // Уведомить пользователя об успешном обновлении
                MessageBox.Show("Данные сотрудника успешно обновлены.");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
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



        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void сотрудникDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, чтобы не был кликнут заголовок
            if (e.RowIndex >= 0)
            {
                // Получаем данные выбранной строки
                var row = сотрудникDataGridView.Rows[e.RowIndex];

                // Предположим, что у нас есть столбцы с именами "Id_Услуги", "Название"
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                dateTimePicker1.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
                textBox6.Text = row.Cells[5].Value.ToString();
                textBox7.Text = row.Cells[6].Value.ToString();
                id_ГрафикаComboBox.Text = row.Cells[7].Value.ToString();
            }
        }
    }
}
