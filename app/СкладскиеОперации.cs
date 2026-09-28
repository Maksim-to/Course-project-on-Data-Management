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
    public partial class СкладскиеОперации : Form
    {
        public СкладскиеОперации()
        {
            InitializeComponent();
        }

        private void СкладскиеОперации_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.СотрудникиСклада". При необходимости она может быть перемещена или удалена.
            this.сотрудникиСкладаTableAdapter.Fill(this.ветеринарная_клиникаDataSet.СотрудникиСклада);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.СотрудникиСкладаСГрафиком". При необходимости она может быть перемещена или удалена.
            this.сотрудникиСкладаСГрафикомTableAdapter.Fill(this.ветеринарная_клиникаDataSet.СотрудникиСкладаСГрафиком);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.Представление_СкладскаяОперация". При необходимости она может быть перемещена или удалена.
            this.представление_СкладскаяОперацияTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Представление_СкладскаяОперация);

            numericUpDown1.TextChanged += numericUpDown1_TextChanged;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            фИОComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
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
                case 6:
                    Col = dataGridViewTextBoxColumn8;
                    break;
            }
            if (radioButton1.Checked)
                представление_СкладскаяОперацияDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Ascending);
            else
                представление_СкладскаяОперацияDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Descending);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            представление_СкладскаяОперацияBindingSource.Filter = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string searchText = textBox3.Text.Replace("'", "''"); // Экранируем одинарные кавычки
            представление_СкладскаяОперацияBindingSource.Filter = $"Наименование_Медикамента LIKE '%{searchText}%'";
        }

        private bool numericUpDown1Cleared = false; // Флаг для отслеживания пустого состояния

        private void numericUpDown1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(numericUpDown1.Text))
            {
                numericUpDown1Cleared = true; // Устанавливаем флаг, если поле "очищено"
            }
            else
            {
                numericUpDown1Cleared = false; // Сбрасываем флаг, если есть текст
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (GlobalData.Password != "1234")
            {
                try
                {
                    // Проверка на наличие обязательных данных
                    if (string.IsNullOrWhiteSpace(фИОComboBox.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                        string.IsNullOrWhiteSpace(comboBox1.Text))
                    {
                        MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (numericUpDown1.Value == 0 || numericUpDown1Cleared)
                    {
                        sqlCommand.Parameters["@Id_Приема"].Value = DBNull.Value; // Передаем NULL в параметр
                    }
                    else
                    {
                        sqlCommand.Parameters["@Id_Приема"].Value = numericUpDown1.Value; // Передаем числовое значение
                    }
                    // Проверка на правильность введенного количества
                    if (!int.TryParse(numericUpDown2.Text, out int количествоЕдиниц) || количествоЕдиниц < 0)
                    {
                        MessageBox.Show("Пожалуйста, введите корректное количество единиц.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Присвоить значения входным параметрам процедуры с преобразованием типов
                    sqlCommand.Parameters["@ФИО_Сотрудника"].Value = фИОComboBox.Text;
                    sqlCommand.Parameters["@Наименование_Медикамента"].Value = textBox2.Text;
                    sqlCommand.Parameters["@Операция"].Value = comboBox1.Text;
                    sqlCommand.Parameters["@КоличествоЕдиниц"].Value = количествоЕдиниц;

                    // Открыть соединение с БД
                    sqlConnection.Open();

                    // Выполнить sql-выражение (хранимую процедуру) для добавления новой складской операции
                    sqlCommand.ExecuteNonQuery();
                    this.представление_СкладскаяОперацияTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Представление_СкладскаяОперация);
                    // Уведомить пользователя об успешном добавлении
                    MessageBox.Show("Складская операция успешно добавлена.");
                }
                catch (SqlException ex)
                {
                    // Обработка ошибок SQL: вывод ошибок, если они есть
                    MessageBox.Show($"Ошибка при добавлении складской операции. {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            else MessageBox.Show("В доступе отказано.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        private void представление_СкладскаяОперацияDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, чтобы не был кликнут заголовок
            if (e.RowIndex >= 0)
            {
                // Получаем данные выбранной строки
                var row = представление_СкладскаяОперацияDataGridView.Rows[e.RowIndex];

                // Предположим, что у нас есть столбцы с именами "Id_Услуги", "Название"
                фИОComboBox.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                numericUpDown1.Text = row.Cells[3].Value.ToString();
                comboBox1.Text = row.Cells[5].Value.ToString();
                numericUpDown2.Text = row.Cells[6].Value.ToString();
            }
        }

        private void фИОComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Применение фильтрации по выбранному значению
            сотрудникиСкладаСГрафикомBindingSource.Filter = $"ФИО = '{фИОComboBox.Text}'";
        }
    }
}
