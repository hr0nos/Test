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
    public partial class Form_View : Form
    {
        string strfilename;
        public int movieID;

        public Form_View()
        {
            InitializeComponent();
        }

        #region load_form
        private void Form_View_Load(object sender, EventArgs e)
        {
            try
            {
                var context = new KINOEntities6();
                var film = context.Proba_film.Single(c => c.id_film == movieID);
                textBox1.Text = film.name_film;
                textBox2.Text = Convert.ToString(film.year_film);
                textBox4.Text = film.description_film;
                /// for reading show
                

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
                //var filmgenre = context.Filmgenre.Single(c => c.id_film == film.id_film);
                //var genre = context.film_genre.Single(c => c.ID == filmgenre.id_film);
                foreach (var g in genres)
                {
                    //if (genre.Count() != 1)
                    textBox5.Text = g.Name_genre;
                    //else
                    //MessageBox.Show("Malo genres!");
                }
                var countrys = from c in context.film_country
                               join fc in context.Filmcountry on c.ID equals fc.id_country
                               join f in context.Proba_film on fc.id_film equals f.id_film
                               where f.id_film == movieID
                               select new
                               {
                                   Name_country = c.Name,
                                   Name_film = f.name_film,
                                   id_country = c.ID
                               };
                textBox3.Text = "";

                foreach (var c in countrys)
                {

                    if (textBox3.Text == "")
                        textBox3.Text = textBox3.Text + c.Name_country;
                    else
                        textBox3.Text = textBox3.Text + "," + c.Name_country;


                }

                ///read only
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox1.BackColor = System.Drawing.SystemColors.Window;
                textBox2.BackColor = System.Drawing.SystemColors.Window;
                textBox3.BackColor = System.Drawing.SystemColors.Window;
                textBox4.BackColor = System.Drawing.SystemColors.Window;
                textBox5.BackColor = System.Drawing.SystemColors.Window;
                textBox4.HideSelection = false;
                textBox4.SelectionStart = textBox4.TextLength;
                textBox4.SelectionLength = 0;
                button1.Focus();


            }
            catch { }
        }
        #endregion

        #region close_button_form
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
