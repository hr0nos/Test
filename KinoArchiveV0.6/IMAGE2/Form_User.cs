using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMAGE2
{
    public partial class Form_User : Form
    {
        DataGridViewButtonColumn showButton;
        int movieIDInt;

        public Form_User()
        {
            InitializeComponent();
        }

        #region Load_form
        private void Form_User_Load(object sender, EventArgs e)
        {
            
            LoadDataGrid();
            var context = new KINOEntities6();
            var films = from f in context.Proba_film
                        select f;
            int countfilms = films.Count();

            toolStripStatusLabel1.Text = "Фильмов в базе данных: " + countfilms.ToString();

        }
        #endregion

        #region Load_Data
        public void LoadDataGrid()
        {
            dataGridView1.Columns.Clear();
            using (var context = new KINOEntities6())
            {
                var query = from g in context.film_genre
                            join fg in context.Filmgenre on g.ID equals fg.id_genre
                            join f in context.Proba_film on fg.id_film equals f.id_film
                            select new
                            {
                                Ид_фильма = f.id_film,
                                Название_фильма = f.name_film,
                                Год = f.year_film,
                                Жанр = g.Name
                            };


                var films = query.ToList();

                dataGridView1.DataSource = films;

                dataGridView1.AllowUserToAddRows = false; // remove the null line
                dataGridView1.ReadOnly = true;

                dataGridView1.Columns[0].Width = 130;
                dataGridView1.Columns[1].Width = 300;
                dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[3].Width = 160;
                // insert show button to datagridview
                showButton = new DataGridViewButtonColumn();
                showButton.HeaderText = "Просмотр";
                showButton.Text = "Просмотреть";
                showButton.UseColumnTextForButtonValue = true;
                showButton.Width = 150;
                dataGridView1.Columns.Add(showButton);



            }
        }
        #endregion

        #region Click_on_dataGridView
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var context = new KINOEntities6();
            var query = from g in context.film_genre
                        join fg in context.Filmgenre on g.ID equals fg.id_genre
                        join f in context.Proba_film on fg.id_film equals f.id_film
                        select new
                        {
                            Ид_фильма = f.id_film,
                            Название_фильма = f.name_film,
                            Год = f.year_film,
                            Жанр = g.Name
                        };

            var films = query.ToList();

            int currentRow = int.Parse(e.RowIndex.ToString());


            try
            {
                string movieIDString = dataGridView1[0, currentRow].Value.ToString();
                movieIDInt = int.Parse(movieIDString);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }


            if (dataGridView1.Columns[e.ColumnIndex] == showButton && currentRow >= 0)
            {
                //runs form_view for view    
                Form_View fv = new Form_View();
                fv.movieID = movieIDInt;
                fv.Show();

            }
        }
        #endregion

        #region button_refresh
        private void button1_Click(object sender, EventArgs e)
        {
            LoadDataGrid();
            comboBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }
        #endregion

        #region check_year_func
        private int checkYear(string year)
        {
            foreach (char c in year)
            {
                if (!Char.IsDigit(c)) return 1;
            }

            if (year != "")
            {
                int yr = int.Parse(year);
                if (yr >= 2100 || yr <= 1900)
                {
                    return 1;
                }
                else
                {
                    return yr;
                }
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region search
        
        #region search_by_title
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            using (var context = new KINOEntities6())
            {
                var query = from g in context.film_genre
                            join fg in context.Filmgenre on g.ID equals fg.id_genre
                            join f in context.Proba_film on fg.id_film equals f.id_film
                            where f.name_film == textBox3.Text
                            select new
                            {
                                Ид_фильма = f.id_film,
                                Название_фильма = f.name_film,
                                Год = f.year_film,
                                Жанр = g.Name
                            };


                var films = query.ToList();
                if (textBox3.Text != "")
                {
                    if (films.Count > 0)
                    {
                        dataGridView1.DataSource = films;

                        dataGridView1.AllowUserToAddRows = false; // remove the null line
                        dataGridView1.ReadOnly = true;

                        dataGridView1.Columns[0].Width = 130;
                        dataGridView1.Columns[1].Width = 300;
                        dataGridView1.Columns[2].Width = 120;
                        dataGridView1.Columns[3].Width = 160;
                        
                        // insert show button to datagridview
                        showButton = new DataGridViewButtonColumn();
                        showButton.HeaderText = "Просмотр";
                        showButton.Text = "Просмотреть";
                        showButton.UseColumnTextForButtonValue = true;
                        showButton.Width = 150;
                        dataGridView1.Columns.Add(showButton);

                        int count = dataGridView1.RowCount;
                        if (count == 1)
                            MessageBox.Show("Найдено " + count + " совпадение.");
                        else if (count > 1 & count < 5)
                            MessageBox.Show("Найдено " + count + " совпадения.");
                        else
                            MessageBox.Show("Найдено " + count + " совпадений.");

                    }
                    else
                    {
                        MessageBox.Show("Совпадений не найдено!");
                        textBox3.Text = "";
                        LoadDataGrid();
                    }
                }
                else
                {
                    MessageBox.Show("Пустое поле!");
                    LoadDataGrid();
                }


            }
        }
        #endregion

        #region search_by_genre
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();

            using (var context = new KINOEntities6())
            {
                var query = from g in context.film_genre
                            join fg in context.Filmgenre on g.ID equals fg.id_genre
                            join f in context.Proba_film on fg.id_film equals f.id_film
                            where g.Name == comboBox2.Text
                            select new
                            {
                                Ид_фильма = f.id_film,
                                Название_фильма = f.name_film,
                                Год = f.year_film,
                                Жанр = g.Name
                            };


                var films = query.ToList();
                if (comboBox2.Text != "")
                {
                    if (films.Count > 0)
                    {
                        dataGridView1.DataSource = films;

                        dataGridView1.AllowUserToAddRows = false; // remove the null line
                        dataGridView1.ReadOnly = true;

                        dataGridView1.Columns[0].Width = 130;
                        dataGridView1.Columns[1].Width = 300;
                        dataGridView1.Columns[2].Width = 120;
                        dataGridView1.Columns[3].Width = 160;
                        
                        // insert show button to datagridview
                        showButton = new DataGridViewButtonColumn();
                        showButton.HeaderText = "Просмотр";
                        showButton.Text = "Просмотреть";
                        showButton.UseColumnTextForButtonValue = true;
                        showButton.Width = 150;
                        dataGridView1.Columns.Add(showButton);

                        int count = dataGridView1.RowCount;
                        if(count == 1)
                            MessageBox.Show("Найдено " + count + " совпадение.");
                        else if (count > 1 &  count < 5)
                            MessageBox.Show("Найдено " + count + " совпадения.");
                        else
                            MessageBox.Show("Найдено " + count + " совпадений.");

                       
                    }
                    else
                    {
                        //dataGridView1.DataSource = films;
                        MessageBox.Show("Совпадений не найдено!");
                        comboBox2.Text = "";
                        LoadDataGrid();
                    }
                }
                else
                {
                    MessageBox.Show("Пустое поле!");
                    LoadDataGrid();
                }
            }
        }
        #endregion
        
        #region search_by_year
        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            using (var context = new KINOEntities6())
            {
                if (textBox5.Text != "" && textBox6.Text != "")
                {
                    #region check_numerical
                    if (checkYear(textBox5.Text) == 1 && checkYear(textBox6.Text) == 1)
                    {
                        MessageBox.Show("Некорректный год!\nПожалуйста, повторите ввод.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox5.Focus();
                        LoadDataGrid();
                        return;
                    }
                    if (checkYear(textBox5.Text) == 1)
                    {
                        MessageBox.Show("Некорректный год!\nПожалуйста, повторите ввод.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox5.Text = "";
                        textBox5.Focus();
                        LoadDataGrid();
                        return;
                    }

                    if (checkYear(textBox6.Text) == 1)
                    {
                        MessageBox.Show("Некорректный год!\nПожалуйста, повторите ввод.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox6.Text = "";
                        textBox6.Focus();
                        LoadDataGrid();
                        return;
                    }
                    
                    #endregion

                    if (Convert.ToInt16(textBox5.Text) < 1900 || Convert.ToInt16(textBox5.Text) > 2100)
                    {
                        MessageBox.Show("Некорректный год!");
                        LoadDataGrid();
                        textBox5.Text = "";
                        return;
                    }
                    else if (Convert.ToInt16(textBox6.Text) < 1900 || Convert.ToInt16(textBox6.Text) > 2100)
                    {
                        MessageBox.Show("Некорректный год!");
                        LoadDataGrid();
                        textBox6.Text = "";
                        return;
                    }
                    else if (Convert.ToInt16(textBox5.Text) > Convert.ToInt16(textBox6.Text))
                    {
                        MessageBox.Show("Некорректный ввод!");
                        LoadDataGrid();
                        textBox5.Text = "";
                        textBox6.Text = "";
                        return;
                    }
                }
                
                if (textBox5.Text != "" && textBox6.Text != "")
                {
                    int first_year = Convert.ToInt16(textBox5.Text);
                    int second_year = Convert.ToInt16(textBox6.Text);
                var query = from g in context.film_genre
                            join fg in context.Filmgenre on g.ID equals fg.id_genre
                            join f in context.Proba_film on fg.id_film equals f.id_film
                            where f.year_film >= first_year
                            where f.year_film <= second_year
                            select new
                            {
                                Ид_фильма = f.id_film,
                                Название_фильма = f.name_film,
                                Год = f.year_film,
                                Жанр = g.Name
                            };


                var films = query.ToList();
                
                
                    if (films.Count > 0)
                    {
                        dataGridView1.DataSource = films;

                        dataGridView1.AllowUserToAddRows = false; // remove the null line
                        dataGridView1.ReadOnly = true;

                        dataGridView1.Columns[0].Width = 130;
                        dataGridView1.Columns[1].Width = 300;
                        dataGridView1.Columns[2].Width = 120;
                        dataGridView1.Columns[3].Width = 160;
                        // insert show button to datagridview
                        showButton = new DataGridViewButtonColumn();
                        showButton.HeaderText = "Просмотр";
                        showButton.Text = "Просмотреть";
                        showButton.UseColumnTextForButtonValue = true;
                        showButton.Width = 150;
                        dataGridView1.Columns.Add(showButton);

                        int count = dataGridView1.RowCount;
                        MessageBox.Show("Найдено " + count + " совпадение.");
                        
                    }
                    else
                    {
                        //dataGridView1.DataSource = films;
                        MessageBox.Show("Совпадений не найдено!");
                        textBox5.Text = "";
                        textBox6.Text = "";
                        LoadDataGrid();
                    }
                }
                else
                {
                    MessageBox.Show("Пустое поле!");
                    LoadDataGrid();
                }


            }
        }
        #endregion
        #endregion
    }
}
