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
    public partial class Form3 : Form
    {
        #region variables
        DataGridViewButtonColumn editButton;
        DataGridViewButtonColumn deleteButton;
        DataGridViewButtonColumn showButton;
        int movieIDInt;
        string strfilename;
        #endregion

        public Form3()
        {
            InitializeComponent();
        }

        #region dataGridView1_CellContentClick
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


            // edit button
            if (dataGridView1.Columns[e.ColumnIndex] == editButton && currentRow >= 0)
            {
                string title = dataGridView1[1, currentRow].Value.ToString();
                string year = dataGridView1[2, currentRow].Value.ToString();
                string genre = dataGridView1[3, currentRow].Value.ToString();
                //runs form 2 for editing    
                Form2 f2 = new Form2();
                f2.movieID = movieIDInt;
                f2.Show();

                dataGridView1.Refresh();

            }
            // delete button
            else if (dataGridView1.Columns[e.ColumnIndex] == deleteButton && currentRow >= 0)
            {
                // delete sql query

                var rows = from o in context.Proba_film
                           where o.id_film == movieIDInt
                           select o;
                foreach (var row in rows)
                {
                    context.Proba_film.Remove(row);
                }
                context.SaveChanges();

                
                LoadDataGrid();
                var films1 = from f in context.Proba_film
                            select f;
                int countfilms = films1.Count();
                toolStripStatusLabel1.Text = "Фильмов в базе данных: " + countfilms.ToString();
                

            }
            else if (dataGridView1.Columns[e.ColumnIndex] == showButton && currentRow >= 0)
            {
                    Form_View fv = new Form_View();
                    
                    fv.movieID = movieIDInt;
                    fv.Show();

                    dataGridView1.Refresh();
                
            }
        }
        #endregion 
        
        #region form_load
        private void Form3_Load(object sender, EventArgs e)
        {
            

            comboBox1.Text = "";
            comboBox2.Text = "";


            var context = new KINOEntities6();

            var countrys = from c in context.film_country
                           select c;
            int i = 0;
            foreach (var c in countrys)
            {

                checkedListBox1.Items.Insert(i, c.Name);
                i++;
            }

            var films = from f in context.Proba_film
                        select f;
            int countfilms = films.Count();

            toolStripStatusLabel1.Text = "Фильмов в базе данных: " + countfilms.ToString() ;

            tabControl1.SelectedIndex = 1;


            textBox7.ReadOnly = true;
            textBox7.BackColor = System.Drawing.SystemColors.Window;

            LoadDataGrid();
        }
        #endregion

        #region LoadDataGrid
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

                dataGridView1.Columns[0].Width = 90;
                dataGridView1.Columns[1].Width = 160;

                dataGridView1.Columns[3].Width = 140;
                // insert edit button to datagridview
                editButton = new DataGridViewButtonColumn();
                editButton.HeaderText = "Редактирование";
                editButton.Text = "Редактировать";
                editButton.UseColumnTextForButtonValue = true;
                editButton.Width = 130;
                dataGridView1.Columns.Add(editButton);
                // insert delete button to datagridview
                deleteButton = new DataGridViewButtonColumn();
                deleteButton.HeaderText = "Удаление";
                deleteButton.Text = "Удалить";
                deleteButton.UseColumnTextForButtonValue = true;
                deleteButton.Width = 100;
                dataGridView1.Columns.Add(deleteButton);
                // insert show button to datagridview
                showButton = new DataGridViewButtonColumn();
                showButton.HeaderText = "Просмотр";
                showButton.Text = "Просмотреть";
                showButton.UseColumnTextForButtonValue = true;
                showButton.Width = 115;
                dataGridView1.Columns.Add(showButton);



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
            var context = new KINOEntities6();
            var films = from f in context.Proba_film
                        select f;
            int countfilms = films.Count();
            toolStripStatusLabel1.Text = "Фильмов в базе данных: " + countfilms.ToString();
        }
        #endregion

        #region button_add_film
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (pictureBox1.Image == Image.FromFile(@"Images_posters\null.jpg"))
            {
                MessageBox.Show("Пожалуйста, добавьте постер к фильму!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (checkedListBox1.CheckedIndices.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите страны!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox2.Text != "" && textBox1.Text != "" && comboBox1.Text != "" && textBox4.Text != "")
            {
                if (checkYear(textBox2.Text) != 1)
                {
                    var context = new KINOEntities6();
                    Proba_film film = context.Proba_film.Add(new Proba_film
                    {
                        name_film = textBox1.Text,
                        year_film = Convert.ToInt16(textBox2.Text),
                        description_film = textBox4.Text,
                        image_film = strfilename
                    });
                    Filmgenre fg = context.Filmgenre.Add(new Filmgenre
                    {
                        id_film = film.id_film,
                        id_genre = comboBox1.SelectedIndex + 1
                    });

                    if (checkedListBox1.SelectedItems.Count != 0)
                        foreach (int indexChecked in checkedListBox1.CheckedIndices)
                        {
                            Filmcountry fc = context.Filmcountry.Add(new Filmcountry
                            {
                                id_film = film.id_film,
                                id_country = indexChecked + 1
                            });
                        }



                    context.SaveChanges();
                    MessageBox.Show("Фильм успешно добавлен в базу данных", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    comboBox1.Text = "";
                    textBox4.Text = "";
                    pictureBox1.Image = null;
                    pictureBox1.Image = Image.FromFile(@"Images_posters\null.jpg");
                    strfilename = "";

                    foreach (int indexChecked in checkedListBox1.CheckedIndices)
                    {
                        checkedListBox1.SetItemChecked(indexChecked, false);
                        checkedListBox1.SetSelected(indexChecked, false);
                    }
                    LoadDataGrid();

                    var films = from f in context.Proba_film
                                select f;
                    int countfilms = films.Count();

                    toolStripStatusLabel1.Text = "Фильмов в базе данных: " + countfilms.ToString();
                }
                else
                {
                    MessageBox.Show("Некорректный год!\nПожалуйста, повторите ввод.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox2.Text = "";
                    textBox2.Focus();
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        #region button_add_poster
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Text|*.jpg;*.jpeg|All|*.*";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                var context = new KINOEntities6();
                strfilename = OFD.InitialDirectory + OFD.FileName;
                pictureBox1.InitialImage = null;

                string filename = System.IO.Path.GetFileName(strfilename);
                strfilename = @"Images_posters\" + filename;
                pictureBox1.Image = Image.FromFile(strfilename);
            }
        }
        #endregion

        #region button_clear_form
        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
            textBox4.Text = "";
            pictureBox1.Image = null;
            pictureBox1.Image = Image.FromFile(@"Images_posters\null.jpg");
            strfilename = "";

            foreach (int i in checkedListBox1.CheckedIndices)
            {
                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
            }
            label5.Text = "Страна(-ы): ";
            checkedListBox1.SelectedIndex = 0;
        }
        #endregion

        #region find_all_types
        #region find_by_title
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

                        dataGridView1.Columns[0].Width = 150;
                        dataGridView1.Columns[1].Width = 160;
                        // insert edit button to datagridview
                        editButton = new DataGridViewButtonColumn();
                        editButton.HeaderText = "Редактирование";
                        editButton.Text = "Редактировать";
                        editButton.UseColumnTextForButtonValue = true;
                        editButton.Width = 130;
                        dataGridView1.Columns.Add(editButton);
                        // insert delete button to datagridview
                        deleteButton = new DataGridViewButtonColumn();
                        deleteButton.HeaderText = "Удаление";
                        deleteButton.Text = "Удалить";
                        deleteButton.UseColumnTextForButtonValue = true;
                        deleteButton.Width = 100;
                        dataGridView1.Columns.Add(deleteButton);
                        // insert show button to datagridview
                        showButton = new DataGridViewButtonColumn();
                        showButton.HeaderText = "Просмотр";
                        showButton.Text = "Просмотреть";
                        showButton.UseColumnTextForButtonValue = true;
                        showButton.Width = 115;
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
                        //dataGridView1.DataSource = films;
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

        #region find_by_genre
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

                        dataGridView1.Columns[0].Width = 150;
                        dataGridView1.Columns[1].Width = 160;
                        // insert edit button to datagridview
                        editButton = new DataGridViewButtonColumn();
                        editButton.HeaderText = "Редактирование";
                        editButton.Text = "Редактировать";
                        editButton.UseColumnTextForButtonValue = true;
                        editButton.Width = 130;
                        dataGridView1.Columns.Add(editButton);
                        // insert delete button to datagridview
                        deleteButton = new DataGridViewButtonColumn();
                        deleteButton.HeaderText = "Удаление";
                        deleteButton.Text = "Удалить";
                        deleteButton.UseColumnTextForButtonValue = true;
                        deleteButton.Width = 100;
                        dataGridView1.Columns.Add(deleteButton);
                        // insert show button to datagridview
                        showButton = new DataGridViewButtonColumn();
                        showButton.HeaderText = "Просмотр";
                        showButton.Text = "Просмотреть";
                        showButton.UseColumnTextForButtonValue = true;
                        showButton.Width = 115;
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
                        comboBox1.Text = "";
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

        #region find_by_year
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

                        dataGridView1.Columns[0].Width = 150;
                        dataGridView1.Columns[1].Width = 160;
                        // insert edit button to datagridview
                        editButton = new DataGridViewButtonColumn();
                        editButton.HeaderText = "Редактирование";
                        editButton.Text = "Редактировать";
                        editButton.UseColumnTextForButtonValue = true;
                        editButton.Width = 130;
                        dataGridView1.Columns.Add(editButton);
                        // insert delete button to datagridview
                        deleteButton = new DataGridViewButtonColumn();
                        deleteButton.HeaderText = "Удаление";
                        deleteButton.Text = "Удалить";
                        deleteButton.UseColumnTextForButtonValue = true;
                        deleteButton.Width = 100;
                        dataGridView1.Columns.Add(deleteButton);
                        // insert show button to datagridview
                        showButton = new DataGridViewButtonColumn();
                        showButton.HeaderText = "Просмотр";
                        showButton.Text = "Просмотреть";
                        showButton.UseColumnTextForButtonValue = true;
                        showButton.Width = 115;
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

        #region checkedListBox1_SelectedIndexChanged
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label5.Text = "Страна(-ы): " + checkedListBox1.CheckedItems.Count.ToString() + "шт.";
            textBox7.Text = "";
            foreach (string s in checkedListBox1.CheckedItems)
            {

                if (textBox7.Text == "")
                    textBox7.Text = textBox7.Text + s;
                else
                    textBox7.Text = textBox7.Text + "," + s;


            }
        }
        #endregion
    }
}
