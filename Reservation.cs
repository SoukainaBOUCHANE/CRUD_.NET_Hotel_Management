using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_test1
{
    public partial class Reservation : Form
    {

        private string id = "";
        private int intRow = 0;
        public Reservation()
        {
            InitializeComponent();
            resetMe();
        }

        private void resetMe()
        {

            this.id = string.Empty;

            montArrTextBox.Text = "";
            cneCltTextBox.Text = "";
            dateTimePicker1.Text = "";
            dateTimePicker2.Text = "";
            dateTimePicker3.Text = "";
            idChambreTextBox.Text = "";



            //if (genderComboBox.Items.Count > 0)
            //{
            //genderComboBox.SelectedIndex = 0;
            //}

            updateButton.Text = "Modifier ()";
            deleteButton.Text = "Supprimer ()";

            keywordTextBox.Clear();

            if (keywordTextBox.CanSelect)
            {
                keywordTextBox.Select();
            }

        }

        private void Reservation_Load(object sender, EventArgs e)
        {
            loadData("");
        }

        private void loadData(string keyword)
        {


            CRUD.sql = "SELECT idres, montarr, cneclient, datedebut, datefin, datepyarr, idchambre FROM reservationtest " +
                          "WHERE CONCAT(CAST(idres as varchar), ' ', montarr, ' ', cneclient, ' ', datedebut, ' ', datefin, ' ', datepyarr, ' ', idchambre) "
                        + "LIKE @keyword::varchar ORDER BY idres ASC";


            string strKeyword = string.Format("%{0}%", keyword);

            CRUD.cmd = new NpgsqlCommand(CRUD.sql, CRUD.con);
            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("keyword", strKeyword);

            DataTable dt = CRUD.PerformCRUD(CRUD.cmd);

            if (dt.Rows.Count > 0)
            {
                intRow = Convert.ToInt32(dt.Rows.Count.ToString());
            }
            else
            {
                intRow = 0;
            }

            toolStripStatusLabel1.Text = "Nombre d'enregistrement(s): " + intRow.ToString();

            DataGridView dgv1 = dataGridView1;
            //dataGridView1.ColumnCount = 10;

            dgv1.MultiSelect = false;
            dgv1.AutoGenerateColumns = true;
            dgv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv1.DataSource = dt;


            dgv1.Columns[0].HeaderText = "ID";
            dgv1.Columns[1].HeaderText = "Montant Arrnes";
            dgv1.Columns[2].HeaderText = "CNE Clinet";
            dgv1.Columns[3].HeaderText = "Date Début";
            dgv1.Columns[4].HeaderText = "Date Fin";
            dgv1.Columns[5].HeaderText = "Date Payment Arrhnes";
            dgv1.Columns[6].HeaderText = "Id Chambre";



            dgv1.Columns[0].Width = 85;
            dgv1.Columns[1].Width = 170;  //170
            dgv1.Columns[2].Width = 170;  //170
            dgv1.Columns[3].Width = 170;  //220
            dgv1.Columns[4].Width = 170;  //220
            dgv1.Columns[5].Width = 170;
            dgv1.Columns[6].Width = 170;


        }

        private void execute(string mySQL, string param)
        {
            CRUD.cmd = new NpgsqlCommand(mySQL, CRUD.con);
            addParameters(param);
            CRUD.PerformCRUD(CRUD.cmd);
        }

        private void addParameters(string str)
        {


            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("montArr", montArrTextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("cneClt", cneCltTextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("datedeb", dateTimePicker1.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("datefn", dateTimePicker2.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("datepyArr", dateTimePicker3.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("idcham", idChambreTextBox.Text);
            //CRUD.cmd.Parameters.AddWithValue("gender", genderComboBox.SelectedItem.ToString());

            if (str == "Update" || str == "Delete" && !string.IsNullOrEmpty(this.id))
            {
                CRUD.cmd.Parameters.AddWithValue("id", this.id);
            }
        }

        // private void insertButton_Click(object sender, EventArgs e)
        //{


        //}



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
            {
                return;
            }


            if (e.RowIndex != -1)
            //if(e.RowIndex >= 0)
            {
                DataGridView dgv1 = dataGridView1;

                this.id = Convert.ToString(dgv1.CurrentRow.Cells[0].Value);
                updateButton.Text = "Modifier (" + this.id + ")";
                deleteButton.Text = "Supprimer (" + this.id + ")";


                montArrTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);
                cneCltTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[2].Value);
                dateTimePicker1.Text = Convert.ToString(dgv1.CurrentRow.Cells[3].Value);
                dateTimePicker2.Text = Convert.ToString(dgv1.CurrentRow.Cells[4].Value);
                dateTimePicker3.Text = Convert.ToString(dgv1.CurrentRow.Cells[5].Value);
                idChambreTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[6].Value);



                //genderComboBox.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[4].Value);
            }

        }

        //private void updateButton_Click(object sender, EventArgs e)
        //{


        //}

        //private void deleteButton_Click(object sender, EventArgs e)
        //{



        //}

        //private void searchButton_Click(object sender, EventArgs e)
        //{


        //}

        private void insertButton_Click_1(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(montArrTextBox.Text.Trim()) || string.IsNullOrEmpty(cneCltTextBox.Text.Trim()))
            {
                MessageBox.Show("Veuillez remplir tous les champs !!", "Gestion Reservation",
                  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }



            CRUD.sql = "INSERT INTO reservationtest(montarr, cneclient, datedebut, datefin, datepyarr, idchambre) " +
                        "VALUES(@montArr, @cneClt, @datedeb, @datefn, @datepyArr, @idcham)";
            // WHERE reservationtest.idchambre = chambre.idchambre";


            execute(CRUD.sql, "Insert");




            MessageBox.Show("Ajout  avec succès.", "Gestion Reservation",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");

            resetMe();
        }

        private void updateButton_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Veuillez sélectionner un identifiant.", "Gestion Reservation",
                  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(montArrTextBox.Text.Trim()) || string.IsNullOrEmpty(cneCltTextBox.Text.Trim()))
            {
                MessageBox.Show("Veuillez remplir tous les champs !!", "Gestion Reservation",
                  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CRUD.sql = "UPDATE reservationtest SET montarr = @montArr, cneclient = @cneClt, datedebut = @datedeb, datefin = @datefn, datepyarr = @datepyArr, " +
                        "idchambre = @idcham WHERE idres = @id::integer";
            //  INNER JOIN reservationtest ON chambre.idchambre = reservationtest.idchambre

            execute(CRUD.sql, "Update");

            MessageBox.Show("Mise à jour avec succès.", "Gestion Reservation",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");

            resetMe();
        }

        private void deleteButton_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Veuillez sélectionner un identifiant.", "Gestion Reservation",
                  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Voullez vous supprimer cet enregistrement?", "Gestion Reservation",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {

                CRUD.sql = "DELETE FROM reservationtest WHERE idres = @id::integer";

                execute(CRUD.sql, "Update");

                MessageBox.Show("Suppression avec succès.", "Gestion Reservation",
                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                loadData("");

                resetMe();

            }
        }

        private void searchButton_Click_1(object sender, EventArgs e)
        {
            // Let's try :)
            if (string.IsNullOrEmpty(keywordTextBox.Text.Trim()))
            {
                loadData("");
            }
            else
            {
                loadData(keywordTextBox.Text.Trim());
            }

            resetMe();

        }

        private void firstNameTextBox_Click(object sender, EventArgs e)
        {

        }

        private void lastNameTextBox_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        //private void Reservation_Load_1(object sender, EventArgs e)
        //{

        //}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void keywordTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void idChambreTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Acceuil acceuil = new Acceuil();
            acceuil.Show();
            this.Hide();

        }

        private void clientBtnn_Click_1(object sender, EventArgs e)
        {
            Client form1 = new Client();
            form1.Show();
            this.Hide();
        }


     
        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            Reservation reservation = new Reservation();
            reservation.Show();
            this.Hide();

        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            Consomation consomation = new Consomation();
            consomation.Show();
            this.Hide();

        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            Chambre chambre = new Chambre();
            chambre.Show();
            this.Hide();

        }

        private void toolStripStatusLabel1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }






        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}*/
    }


      