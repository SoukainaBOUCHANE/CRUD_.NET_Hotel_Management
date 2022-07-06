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
    public partial class Consomation : Form
    {
            public Consomation()
            {
                InitializeComponent();
                resetMe();
            }

            private void Consomation_Load(object sender, EventArgs e)
            {
                loadData("");
            }


            private string id = "";
            private int intRow = 0;

            private void resetMe()
            {

                this.id = string.Empty;

                cneClientTextBox.Text = "";
                prixConsoTextBox.Text = "";

                //dateTimePicker1.Text = "";

                if (typeConsoComboBox.Items.Count > 0)
                {
                    typeConsoComboBox.SelectedIndex = 0;
                }

                updateButton.Text = "Modifier ()";
                deleteButton.Text = "Supprimer ()";


            keywordTextBox.Clear();

            if (keywordTextBox.CanSelect)
            {
                keywordTextBox.Select();
            }

        }

           

            private void loadData(string keyword)
            {

                CRUD.sql = "SELECT idconso, prixconso, dateconso, cneclient, typeconso FROM consomation " +
                 "WHERE CONCAT(CAST(idconso as varchar), ' ', prixconso, ' ', dateconso, ' ', cneclient) LIKE @keyword::varchar " +
                    "OR TRIM(typeconso) LIKE @keyword::varchar ORDER BY cneclient ASC";


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

                dgv1.MultiSelect = false;
                dgv1.AutoGenerateColumns = true;
                dgv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                dgv1.DataSource = dt;

                dgv1.Columns[0].HeaderText = "ID";
                dgv1.Columns[1].HeaderText = "Prix";
                dgv1.Columns[2].HeaderText = "Date Consommation";
                dgv1.Columns[3].HeaderText = "CNE Client";
                dgv1.Columns[4].HeaderText = "Type";

                dgv1.Columns[0].Width = 85;
                dgv1.Columns[1].Width = 170;
                dgv1.Columns[2].Width = 170;
                dgv1.Columns[3].Width = 170;
                dgv1.Columns[4].Width = 170;

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
               

                //float f1 = float.Parse(prixConsTextBox);

                CRUD.cmd.Parameters.AddWithValue("prix", prixConsoTextBox.Text.Trim());

                //prixConsNumber = float.Parse(prixConsTextBox.Text);


                CRUD.cmd.Parameters.AddWithValue("cnecl", cneClientTextBox.Text.Trim());

                CRUD.cmd.Parameters.AddWithValue("date", dateTimePicker1.Text.Trim());

                CRUD.cmd.Parameters.AddWithValue("type", typeConsoComboBox.SelectedItem.ToString());


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

                prixConsoTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);
                dateTimePicker1.Text = Convert.ToString(dgv1.CurrentRow.Cells[2].Value);
                cneClientTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[3].Value);
                typeConsoComboBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[4].Value);


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

            private void insertButton_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(prixConsoTextBox.Text.Trim()))
                {
                    MessageBox.Show("Veuillez remplir tous les champs !!", "Gestion Consommation",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                CRUD.sql = "INSERT INTO consomation(typeconso, prixconso, dateconso, cneclient) VALUES(@type, @prix, @date, @cnecl)";
            
                execute(CRUD.sql, "Insert");

                MessageBox.Show("Ajout avec succès.", "Gestion Consommation",MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                     MessageBox.Show("Veuillez sélectionner un identifiant.", "Gestion Consommation",
                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return;
                 }

                 if (string.IsNullOrEmpty(prixConsoTextBox.Text.Trim()))
                 {
                     MessageBox.Show("Veuillez remplir tous les champs !!", "Gestion Consommation",
                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return;
                 }

                 CRUD.sql = "UPDATE consomation SET typeconso = @type, prixconso = @prix, cneclient = @cnecl WHERE idconso = @id::integer";

                 execute(CRUD.sql, "Update");

                 MessageBox.Show("Mise à jour avec succès.", "Gestion Consommation",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);

                 loadData("");

                 resetMe();
             }

            /*private void deleteButton_Click_1(object sender, EventArgs e)
            {

            }*/

            private void prixConsTextBox_TextChanged(object sender, EventArgs e)
            {

            }

            private void deleteButton_Click(object sender, EventArgs e)
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    return;
                }

                if (string.IsNullOrEmpty(this.id))
                {
                    MessageBox.Show("Veuillez sélectionner un identifiant.", "Gestion Consommation",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (MessageBox.Show("Voulez vous supprimer cet enregistrement?", "Gestion Consommation",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {

                    CRUD.sql = "DELETE FROM consomation WHERE idconso = @id::integer";

                    execute(CRUD.sql, "Delete");

                    MessageBox.Show("Suppresion avec succès.", "Gestion Consommation",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    loadData("");

                    resetMe();

                }

            }

            private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

            private void prixConsoTextBox_KeyPress(object sender, KeyPressEventArgs e)
            {
                char ch = e.KeyChar;

                if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
                {
                    e.Handled = true;
                    MessageBox.Show("Prix doit être un nombre!!");
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }















        /*private void firstNameTextBox_Click(object sender, EventArgs e)
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

         }*/

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}
    }
}

