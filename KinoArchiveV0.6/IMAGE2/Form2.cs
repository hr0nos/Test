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
    public partial class Form2 : Form
    {
        string strfilename;
        public int movieID;
        public Form2()
        {
            InitializeComponent();
        }

        #region Load_Form
        private void Form2_Load(object sender, EventArgs e)
        {
           
            var context = new KINOEntities6();

            var countrys = from c in context.film_country  
                       select c;
            int i = 0;
            foreach (var c in countrys)
            {
               
                checkedListBox1.Items.Insert(i, c.Name);
                i++;
            }

            try
            {
                var film = context.Proba_film.Single(c => c.id_film == movieID);
                textBox1.Text = film.name_film;
                textBox2.Text = Convert.ToString(film.year_film);
                textBox4.Text = film.description_film;
                /// for reading show
                textBox3.ReadOnly = true;
                textBox3.BackColor = System.Drawing.SystemColors.Window;

                pictureBox1.InitialImage = null;
                pictureBox1.Image = Image.FromFile(film.image_film.ToString());
                strfilename = film.image_film;

                var genres = from g in context.film_genre
                             join fg in context.Filmgenre on g.ID equals fg.id_genre
                             join f in context.Proba_film on fg.id_film equals f.id_film
                             where f.id_film == movieID
                             select new
                             {
                                 Name_genre = g.Name,
                                 Name_film = f.name_film
                             };
                foreach (var g in genres)
                {
                    comboBox1.Text = g.Name_genre;
                }
                var countrys1 = from c in context.film_country
                               join fc in context.Filmcountry on c.ID equals fc.id_country
                               join f in context.Proba_film on fc.id_film equals f.id_film
                               where f.id_film == movieID
                               select new
                               {
                                   Name_country = c.Name,
                                   Name_film = f.name_film,
                                   id_country = c.ID
                               };

                foreach (var c in countrys1)
                {

                    if (textBox3.Text == "")
                        textBox3.Text = textBox3.Text + c.Name_country;
                    else
                        textBox3.Text = textBox3.Text + "," + c.Name_country;


                }

                textBox3.ReadOnly = true;
                textBox3.BackColor = System.Drawing.SystemColors.Window;
                /// setitemscheck


                foreach (int f in checkedListBox1.CheckedIndices)
                {
                    checkedListBox1.SetItemCheckState(f, CheckState.Unchecked);
                }

                foreach (var c in countrys1)
                {

                    checkedListBox1.SetItemChecked(c.id_country - 1, true);
                    checkedListBox1.SetSelected(c.id_country - 1, true);
                }

            }
            catch { }


        }
        #endregion

        #region button_download_poster
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Text|*.jpg;*.jpeg|All|*.*";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                var context = new KINOEntities6();
                strfilename = OFD.FileName;
                pictureBox1.InitialImage = null;

                string filename = System.IO.Path.GetFileName(strfilename); 
                strfilename = @"Images_posters\" + filename;
                pictureBox1.Image = Image.FromFile(strfilename);
            }
        }
        #endregion

        #region checkedListBox1_SelectedIndexChanged
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            label7.Text = "Страна(-ы): " + checkedListBox1.CheckedItems.Count.ToString() + "шт.";
            textBox3.Text = "";
            foreach (string s in checkedListBox1.CheckedItems)
            {

                if (textBox3.Text == "")
                    textBox3.Text = textBox3.Text + s;
                else
                    textBox3.Text = textBox3.Text + "," + s;
                
                
            }
        }
        #endregion

        #region button_close
        private void button5_Click(object sender, EventArgs e)
        {
            
            this.Close();
            
        }
        #endregion

        #region button_update
        private void button6_Click(object sender, EventArgs e)
        {

            var context = new KINOEntities6();
            if (Convert.ToInt16(textBox2.Text) > 1900 && Convert.ToInt16(textBox2.Text) < 2100)
            {
            if (strfilename != null)
            {
                
                var film = context.Proba_film.Single(f => f.id_film == movieID);
                film.name_film = textBox1.Text;
                film.year_film = Convert.ToInt16(textBox2.Text);
                film.description_film = textBox4.Text;

                film.image_film = strfilename;


                var genre = context.Filmgenre.Single(f => f.id_film == movieID);
                genre.id_genre = comboBox1.SelectedIndex+1;
                context.SaveChanges();


                 #region update chekboxes(countries)

                var context1 = new KINOEntities6();


                var rows = from fc in context1.Filmcountry
                           where fc.id_film == movieID
                           select fc;
                foreach (var row in rows)
                {
                    context1.Filmcountry.Remove(row);
                }
                context1.SaveChanges();
                checkedListBox1.Focus();
                if (checkedListBox1.CheckedIndices.Count != 0)

                    foreach (int indexChecked in checkedListBox1.CheckedIndices)
                    {
                        Filmcountry fc = context1.Filmcountry.Add(new Filmcountry
                        {
                            id_film = movieID,
                            id_country = indexChecked+1
                        });
                    }
                context1.SaveChanges();
               #endregion

                MessageBox.Show("Обновления сохранены!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
            }
            }
            else
            {
                MessageBox.Show("Некорректный год!");
            }
        }
        #endregion
    }
}
