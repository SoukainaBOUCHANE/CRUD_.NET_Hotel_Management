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
    public partial class Client : Form
    {
        

            private string id = "";
            private int intRow = 0;
            public Client()
            {
                InitializeComponent();
                resetMe();
            }

            private void resetMe()
            {

                this.id = string.Empty;

                cneClientTextBox.Text = "";
                firstNameTextBox.Text = "";
                lastNameTextBox.Text = "";

                if (genderComboBox.Items.Count > 0)
                {
                    genderComboBox.SelectedIndex = 0;
                }

                updateButton.Text = "Modifier ()";
                deleteButton.Text = "Supprimer ()";

                keywordTextBox.Clear();

                if (keywordTextBox.CanSelect)
                {
                    keywordTextBox.Select();
                }

            }
        // Veuillez remplit tous les champs !!

        private void Form1_Load(object sender, EventArgs e)
            {
                loadData("");
            }

            private void loadData(string keyword)
            {

                CRUD.sql = "SELECT cneclient, firstname, lastname, CONCAT(firstname, ' ', lastname) AS fullname, gender FROM client " +
                     "WHERE CONCAT(CAST(cneclient as varchar), ' ', firstname, ' ', lastname) LIKE @keyword::varchar " +
                     "OR TRIM(gender) LIKE @keyword::varchar ORDER BY cneclient ASC";

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

                toolStripStatusLabel1.Text = "Number of row(s): " + intRow.ToString();

                DataGridView dgv1 = dataGridView1;

                dgv1.MultiSelect = false;
                dgv1.AutoGenerateColumns = true;
                dgv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                dgv1.DataSource = dt;

                dgv1.Columns[0].HeaderText = "CNE";
                dgv1.Columns[1].HeaderText = "Nom";
                dgv1.Columns[2].HeaderText = "Prénom";
                dgv1.Columns[3].HeaderText = "Nom & Prénom";
                dgv1.Columns[4].HeaderText = "Genre";

                dgv1.Columns[0].Width = 85;
                dgv1.Columns[1].Width = 170;
                dgv1.Columns[2].Width = 170;
                dgv1.Columns[3].Width = 220;
                dgv1.Columns[4].Width = 100;

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
                CRUD.cmd.Parameters.AddWithValue("cne", cneClientTextBox.Text.Trim());
                CRUD.cmd.Parameters.AddWithValue("firstName", firstNameTextBox.Text.Trim());
                CRUD.cmd.Parameters.AddWithValue("lastName", lastNameTextBox.Text.Trim());
                CRUD.cmd.Parameters.AddWithValue("gender", genderComboBox.SelectedItem.ToString());

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

                if (e.RowIndex != -1)
                {
                    DataGridView dgv1 = dataGridView1;

                    this.id = Convert.ToString(dgv1.CurrentRow.Cells[0].Value);
                    updateButton.Text = "Modifier (" + this.id + ")";
                    deleteButton.Text = "Supprimer (" + this.id + ")";

                    cneClientTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);
                    firstNameTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[2].Value);
                    lastNameTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[3].Value);
                    genderComboBox.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[4].Value);

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
                if (string.IsNullOrEmpty(cneClientTextBox.Text.Trim()) || string.IsNullOrEmpty(firstNameTextBox.Text.Trim()))
                {
                    MessageBox.Show("Veuillez remplir tous les champs !!", "Gestion Client",
                      MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                CRUD.sql = "INSERT INTO client(cneclient, firstname, lastname, gender) VALUES(@cne, @firstName, @lastName, @gender)";

                execute(CRUD.sql, "Insert");

                MessageBox.Show("Ajout avec succès.", "Gestion Client ",
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
                    MessageBox.Show("Veullez sélectionner un identifiant !!", "Gestion Client",
                      MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (string.IsNullOrEmpty(cneClientTextBox.Text.Trim()) || string.IsNullOrEmpty(firstNameTextBox.Text.Trim()) || string.IsNullOrEmpty(lastNameTextBox.Text.Trim()))
                {
                    MessageBox.Show("Veullez remplir tous les champs.", "Gestion Client",
                      MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                CRUD.sql = "UPDATE client SET cneclient = @cne, firstName = @firstName, lastname = @lastName, gender = @gender WHERE cneclient = @id::varchar";

                execute(CRUD.sql, "Update");

                MessageBox.Show("Mise à jour avec succès.", "Gestion Client",
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
                    MessageBox.Show("Veuillez remplir tous les champs !!", "Gestion Client",
                      MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (MessageBox.Show("Voulez vous supprimer cet enregistrement ?", "Gestion Client",
                              MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {

                    CRUD.sql = "DELETE FROM client WHERE cneclient = @id::varchar";

                    execute(CRUD.sql, "Update");

                    MessageBox.Show("Enregistrement supprimé avec succès.", "Gestion Client",
                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                    loadData("");

                    resetMe();

                }
            }

            private void searchButton2_Click(object sender, EventArgs e)
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

            private void clientBtnn_Click(object sender, EventArgs e)
            {
                Client form1 = new Client();
                form1.Show();
                this.Hide();
            }

            private void guna2Button1_Click(object sender, EventArgs e)
            {
                Consomation consomation = new Consomation();
                consomation.Show();
                this.Hide();
            }

            private void guna2Button3_Click(object sender, EventArgs e)
            {
                Chambre chambre = new Chambre();
                chambre.Show();
                this.Hide();
            }

            private void guna2Button2_Click(object sender, EventArgs e)
            {
                Reservation reservation = new Reservation();
                reservation.Show();
                this.Hide();
            }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Acceuil acceuil = new Acceuil();
            acceuil.Show();
            this.Hide();

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {

        }










        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}
    }
    }




        


      
