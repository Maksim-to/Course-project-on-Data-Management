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
    public partial class Прием : Form
    {
        public Прием()
        {
            InitializeComponent();
        }

        private void Прием_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.Питомцы_с_ФИО". При необходимости она может быть перемещена или удалена.
            this.питомцы_с_ФИОTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Питомцы_с_ФИО);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.Питомец". При необходимости она может быть перемещена или удалена.
            this.питомецTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Питомец);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.Клиент". При необходимости она может быть перемещена или удалена.
            this.клиентTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Клиент);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.Услуга". При необходимости она может быть перемещена или удалена.
            this.услугаTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Услуга);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.V_Ветеринары_График". При необходимости она может быть перемещена или удалена.
            this.v_Ветеринары_ГрафикTableAdapter.Fill(this.ветеринарная_клиникаDataSet.V_Ветеринары_График);
            ветеринарная_клиникаDataSet.EnforceConstraints = false;
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.ПолнаяИнформацияОПриемах". При необходимости она может быть перемещена или удалена.
            this.полнаяИнформацияОПриемахTableAdapter.Fill(this.ветеринарная_клиникаDataSet.ПолнаяИнформацияОПриемах);
            фИОComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
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
                полнаяИнформацияОПриемахDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Ascending);
            else
                полнаяИнформацияОПриемахDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Descending);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            полнаяИнформацияОПриемахBindingSource.Filter = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string searchText = textBox3.Text.Replace("'", "''"); // Экранируем одинарные кавычки
            полнаяИнформацияОПриемахBindingSource.Filter = $"Кличка_Питомца LIKE '%{searchText}%'";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на пустые поля ФИО сотрудника и кличку питомца
                if (string.IsNullOrWhiteSpace(фИОComboBox.Text) || string.IsNullOrWhiteSpace(comboBox3.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректные ФИО сотрудника и кличку питомца.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Преобразование и проверка даты
                if (!DateTime.TryParse(dateTimePicker1.Text, out DateTime датаВремя))
                {
                    MessageBox.Show("Некорректная дата/время. Пожалуйста, выберите правильное значение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверка на пустое поле комментария
                if (string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Пожалуйста, введите комментарий.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Присвоить значения входным параметрам процедуры с преобразованием типов
                sqlCommand.Parameters["@ФИО_Сотрудника"].Value = фИОComboBox.Text;
                sqlCommand.Parameters["@ФИО_Клиента"].Value = comboBox2.Text;
                sqlCommand.Parameters["@Кличка_Питомца"].Value = comboBox3.Text;
                sqlCommand.Parameters["@Дата_время"].Value = датаВремя;  // Используем DateTime для даты и времени
                sqlCommand.Parameters["@Комментарий"].Value = textBox4.Text;
                sqlCommand.Parameters["@Оказанная_Услуга"].Value = comboBox1.Text;
                if (string.IsNullOrWhiteSpace(comboBox1.Text))
                {
                    sqlCommand.Parameters["@Оказанная_Услуга"].Value = DBNull.Value;
                }
                else
                {
                    sqlCommand.Parameters["@Оказанная_Услуга"].Value = comboBox1.Text;
                }

                // Открыть соединение с БД
                sqlConnection.Open();

                // Выполнить sql-выражение (хранимую процедуру) для добавления нового приема
                sqlCommand.ExecuteNonQuery();
                this.полнаяИнформацияОПриемахTableAdapter.Fill(this.ветеринарная_клиникаDataSet.ПолнаяИнформацияОПриемах);
                // Уведомить пользователя об успешном добавлении
                MessageBox.Show("Прием успешно добавлен.");
            }
            catch (SqlException ex)
            {
                //Обработка ошибок SQL(например, если сотрудник или питомец не существуют, или если данные не удовлетворяют ограничениям NOT NULL)
                string errorMessage = ex.Message;

                //Проверка на отсутствие сотрудника или питомца
                if (errorMessage.Contains("FK_Сотрудник_Прием") || errorMessage.Contains("FK_Питомец_Прием"))
                {
                    MessageBox.Show("Ошибка: Сотрудник с таким ФИО или питомец с такой кличкой не найдены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //Проверка на нарушение ограничения NOT NULL
                else if (errorMessage.Contains("NULL"))
                {
                    MessageBox.Show("Ошибка: Все поля (ФИО сотрудника, кличка питомца, дата и комментарий) должны быть заполнены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //Общая обработка других ошибок
                else
                {
                    MessageBox.Show($"Ошибка при добавлении приема: {errorMessage}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
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
            // Настраиваем DateTimePicker для отображения даты и времени
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MM.yyyy HH:mm"; // Пользовательский формат
        }
        private void представление_ПриемDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, чтобы не был кликнут заголовок
            if (e.RowIndex >= 0)
            {
                // Получаем данные выбранной строки
                var row = полнаяИнформацияОПриемахDataGridView.Rows[e.RowIndex];

                фИОComboBox.Text = row.Cells[1].Value.ToString();
                //textBox1.Text = row.Cells[2].Value.ToString();
                //textBox2.Text = row.Cells[3].Value.ToString();
                dateTimePicker1.Text = row.Cells[4].Value.ToString();
                textBox4.Text = row.Cells[5].Value.ToString();
                comboBox1.Text = row.Cells[6].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на пустые поля ФИО сотрудника и кличку питомца
                if (string.IsNullOrWhiteSpace(фИОComboBox.Text) || string.IsNullOrWhiteSpace(comboBox3.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректные ФИО сотрудника и кличку питомца.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Преобразование и проверка даты
                if (!DateTime.TryParse(dateTimePicker1.Text, out DateTime датаВремя))
                {
                    MessageBox.Show("Некорректная дата/время. Пожалуйста, выберите правильное значение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверка на пустое поле комментария
                if (string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Пожалуйста, введите комментарий.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Присвоить значения входным параметрам процедуры с преобразованием типов
                sqlCommandUpdate.Parameters["@ФИО_Сотрудника"].Value = фИОComboBox.Text;
                sqlCommandUpdate.Parameters["@ФИО_Клиента"].Value = comboBox2.Text;
                sqlCommandUpdate.Parameters["@Кличка_Питомца"].Value = comboBox3.Text;
                sqlCommandUpdate.Parameters["@Дата_время"].Value = датаВремя;  // Используем DateTime для даты и времени
                sqlCommandUpdate.Parameters["@Комментарий"].Value = textBox4.Text;

                // Открыть соединение с БД
                sqlConnection.Open();

                // Выполнить sql-выражение (хранимую процедуру) для добавления нового приема
                sqlCommandUpdate.ExecuteNonQuery();
                this.полнаяИнформацияОПриемахTableAdapter.Fill(this.ветеринарная_клиникаDataSet.ПолнаяИнформацияОПриемах);
                // Уведомить пользователя об успешном добавлении
                MessageBox.Show("Прием успешно обновлен.");
            }
            catch (SqlException ex)
            {
                // Обработка ошибок SQL (например, если сотрудник или питомец не существуют, или если данные не удовлетворяют ограничениям NOT NULL)
                MessageBox.Show($"Ошибка при обновлении приема: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на пустые поля ФИО сотрудника и кличку питомца
                if (string.IsNullOrWhiteSpace(фИОComboBox.Text) || string.IsNullOrWhiteSpace(comboBox3.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректные ФИО сотрудника и кличку питомца.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Преобразование и проверка даты
                if (!DateTime.TryParse(dateTimePicker1.Text, out DateTime датаВремя))
                {
                    MessageBox.Show("Некорректная дата/время. Пожалуйста, выберите правильное значение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверка на пустое поле комментария
                //if (string.IsNullOrWhiteSpace(textBox4.Text))
                //{
                //    MessageBox.Show("Пожалуйста, введите комментарий.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                // Присвоить значения входным параметрам процедуры с преобразованием типов
                sqlCommandDelete.Parameters["@ФИО_Сотрудника"].Value = фИОComboBox.Text;
                sqlCommandDelete.Parameters["@ФИО_Клиента"].Value = comboBox2.Text;
                sqlCommandDelete.Parameters["@Кличка_Питомца"].Value = comboBox3.Text;
                sqlCommandDelete.Parameters["@Дата_время"].Value = датаВремя;  // Используем DateTime для даты и времени
                //sqlCommandDelete.Parameters["@Комментарий"].Value = textBox4.Text;

                // Открыть соединение с БД
                sqlConnection.Open();

                // Выполнить sql-выражение (хранимую процедуру) для добавления нового приема
                sqlCommandDelete.ExecuteNonQuery();
                this.полнаяИнформацияОПриемахTableAdapter.Fill(this.ветеринарная_клиникаDataSet.ПолнаяИнформацияОПриемах);
                // Уведомить пользователя об успешном добавлении
                MessageBox.Show("Прием успешно удален.");
            }
            catch (SqlException ex)
            {
                // Обработка ошибок SQL (например, если сотрудник или питомец не существуют, или если данные не удовлетворяют ограничениям NOT NULL)
                MessageBox.Show($"Ошибка при удалении приема: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void фИОComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string searchText = фИОComboBox.Text.Replace("'", "''"); // Экранируем одинарные кавычки
            v_Ветеринары_ГрафикBindingSource.Filter = $"ФИО LIKE '%{searchText}%'";
        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.питомцы_с_ФИОTableAdapter.FillBy(this.ветеринарная_клиникаDataSet.Питомцы_с_ФИО);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
                // Применение фильтрации по выбранному значению
                питомцысФИОBindingSource.Filter = $"ФИО_Клиента = '{comboBox2.Text}'";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на пустые поля ФИО сотрудника и кличку питомца
                if (string.IsNullOrWhiteSpace(фИОComboBox.Text) || string.IsNullOrWhiteSpace(comboBox3.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректные ФИО сотрудника и кличку питомца.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Преобразование и проверка даты
                if (!DateTime.TryParse(dateTimePicker1.Text, out DateTime датаВремя))
                {
                    MessageBox.Show("Некорректная дата/время. Пожалуйста, выберите правильное значение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверка на пустое поле комментария
                //if (string.IsNullOrWhiteSpace(textBox4.Text))
                //{
                //    MessageBox.Show("Пожалуйста, введите комментарий.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                // Присвоить значения входным параметрам процедуры с преобразованием типов
                sqlCommandDelete2.Parameters["@ФИО_Сотрудника"].Value = фИОComboBox.Text;
                sqlCommandDelete2.Parameters["@ФИО_Клиента"].Value = comboBox2.Text;
                sqlCommandDelete2.Parameters["@Кличка_Питомца"].Value = comboBox3.Text;
                sqlCommandDelete2.Parameters["@Дата_время"].Value = датаВремя;  // Используем DateTime для даты и времени
                //sqlCommandDelete.Parameters["@Комментарий"].Value = textBox4.Text;
                sqlCommandDelete2.Parameters["@Оказанная_Услуга"].Value = comboBox1.Text;

                // Открыть соединение с БД
                sqlConnection.Open();

                // Выполнить sql-выражение (хранимую процедуру) для добавления нового приема
                sqlCommandDelete2.ExecuteNonQuery();
                this.полнаяИнформацияОПриемахTableAdapter.Fill(this.ветеринарная_клиникаDataSet.ПолнаяИнформацияОПриемах);
                // Уведомить пользователя об успешном добавлении
                MessageBox.Show("Услуга приема успешно удалена.");
            }
            catch (SqlException ex)
            {
                // Обработка ошибок SQL (например, если сотрудник или питомец не существуют, или если данные не удовлетворяют ограничениям NOT NULL)
                MessageBox.Show($"Ошибка при удалении услуги приема: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
