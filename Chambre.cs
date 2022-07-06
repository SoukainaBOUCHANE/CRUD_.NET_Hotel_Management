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
    public partial class Chambre : Form
    {
        public Chambre()
        {
            InitializeComponent();
            resetMe();
        }


        private string id = "";
        private int intRow = 0;

        private void resetMe()
        {

        this.id = string.Empty;

        telephTextBox.Text = "";
        prixChambTextBox.Text = "";

               

        if (nomHotelComboBox.Items.Count > 0)
        {
            nomHotelComboBox.SelectedIndex = 0;
        }

        if (categChambComboBox.Items.Count > 0)
        {
            categChambComboBox.SelectedIndex = 0;
        }

        if (statusChambCombBox.Items.Count > 0)
        {
            statusChambCombBox.SelectedIndex = 0;
        }

        updateButton.Text = "Modifier ()";
        deleteButton.Text = "Supprimer ()";

        keywordTextBox.Clear();

        if (keywordTextBox.CanSelect)
        {
        keywordTextBox.Select();
        }

        }

        private void Chambre_Load(object sender, EventArgs e)
        {
            loadData("");
        }

        private void loadData(string keyword)
        {

            /* CRUD.sql = "SELECT idchambre, tphchambre, prixchambre, nomhotel, nomcatg, statusChambre FROM chambre " +
                      "WHERE CONCAT(CAST(idchambre as varchar), ' ', tphchambre, ' ', prixchambre) LIKE @keyword::varchar " +
                      "OR TRIM(nomhotel) LIKE @keyword::varchar " + "ORDER BY idchambre DESC"; */
            CRUD.sql = "SELECT idchambre, tphchambre, prixchambre, nomhotel, nomcatg, stachambre FROM chambre " +
           "WHERE CONCAT(CAST(idchambre as varchar), ' ', tphchambre, ' ', prixchambre, ' ', stachambre) LIKE @keyword::varchar " +
           "OR TRIM(nomhotel) LIKE @keyword::varchar " + "ORDER BY idchambre DESC";

            //"OR TRIM(nomhotel) LIKE @keyword::varchar " + "OR TRIM(nomcatg) LIKE @keyword::varchar " + "OR TRIM(statusChambre) LIKE @keyword::varchar " +  "ORDER BY idchambre DESC";

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

        toolStripStatusLabel1.Text = "Nomber d'enregistrement(s): " + intRow.ToString();

        DataGridView dgv1 = dataGridView1;

        dgv1.MultiSelect = false;
        dgv1.AutoGenerateColumns = true;
        dgv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        dgv1.DataSource = dt;

        dgv1.Columns[0].HeaderText = "ID";
        dgv1.Columns[1].HeaderText = "Télephone chambre";
        dgv1.Columns[2].HeaderText = "Prix chambre";
        dgv1.Columns[3].HeaderText = "Nome hotel";
        dgv1.Columns[4].HeaderText = "Catégorie";
        dgv1.Columns[5].HeaderText = "Status";

        dgv1.Columns[0].Width = 85;
        dgv1.Columns[1].Width = 120;
        dgv1.Columns[2].Width = 120;
        dgv1.Columns[3].Width = 120;
        dgv1.Columns[4].Width = 120;
        dgv1.Columns[5].Width = 170;

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
        CRUD.cmd.Parameters.AddWithValue("teleph", telephTextBox.Text.Trim());
        CRUD.cmd.Parameters.AddWithValue("prixchambre", prixChambTextBox.Text.Trim());
        CRUD.cmd.Parameters.AddWithValue("nomhotel", nomHotelComboBox.SelectedItem.ToString());
        CRUD.cmd.Parameters.AddWithValue("categchambre", categChambComboBox.SelectedItem.ToString());
        CRUD.cmd.Parameters.AddWithValue("statchambre", statusChambCombBox.SelectedItem.ToString());



        if (str == "Update" || str == "Delete" && !string.IsNullOrEmpty(this.id))
        {
        CRUD.cmd.Parameters.AddWithValue("id", this.id);
        }
        }

        // private void insertButton_Click(object sender, EventArgs e)
        //{


        //}
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
        if (string.IsNullOrEmpty(telephTextBox.Text.Trim()) || string.IsNullOrEmpty(prixChambTextBox.Text.Trim()))
        {
        MessageBox.Show("Veuillez remplir tous les champs !!", "Gestion Chambres",
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
        }

            //CRUD.sql = "INSERT INTO chambre(tphchambre, prixchambre, nomhotel, nomcatg, statusChambre) VALUES(@teleph, @prixchambre, @nomhotel, @categchambre, @statchambre)";
            CRUD.sql = "INSERT INTO chambre(tphchambre, prixchambre, nomhotel, nomcatg, stachambre) VALUES(@teleph, @prixchambre, @nomhotel, @categchambre, @statchambre)";

            execute(CRUD.sql, "Insert");

        MessageBox.Show("Ajout avec succès.", "Gestion Chambres",
        MessageBoxButtons.OK, MessageBoxIcon.Information);

        loadData("");

        resetMe();
        }
        
        private void updateButton_Click(object sender, EventArgs e)
        {
        if (dataGridView1.Rows.Count == 0)
        {
        return;
        }

        if (string.IsNullOrEmpty(this.id))
        {
        MessageBox.Show("Veuillez sélectionner un identifiant.", "Gestion Chambres",
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
        }

        if (string.IsNullOrEmpty(telephTextBox.Text.Trim()) || string.IsNullOrEmpty(prixChambTextBox.Text.Trim()))
        {
        MessageBox.Show("Veuillez remplir tous les champs !!", "Gestion Chambres",
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
        }

            //CRUD.sql = "UPDATE chambre SET tphchambre = @teleph, prixchambre = @prixchambre, nomhotel = @nomhotel, nomcatg = @categchambre, statusChambre = @statchambre WHERE idchambre = @id::integer";

            CRUD.sql = "UPDATE chambre SET tphchambre = @teleph, prixchambre = @prixchambre, nomhotel = @nomhotel," +
                " nomcatg = @categchambre, stachambre = @statchambre WHERE idchambre = @id::integer";
            execute(CRUD.sql, "Update");

        MessageBox.Show("Mise à jour avec succès.", "Données modifiées",
        MessageBoxButtons.OK, MessageBoxIcon.Information);

        loadData("");

        resetMe();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
        if (dataGridView1.Rows.Count == 0)
        {
        return;
        }

        if (string.IsNullOrEmpty(this.id))
        {
        MessageBox.Show("Veuillez remplir tous les champs !!", "Gestion Chambres",
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
        }

        if (MessageBox.Show("Voulez vous supprimer cet enregistrement ?", "Gestion Chambres",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
        {

        CRUD.sql = "DELETE FROM chambre WHERE idchambre = @id::integer";

        execute(CRUD.sql, "Update");

        MessageBox.Show("Enregistrement supprimé avec succès.", "Gestion Chambres",
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Reservation reservation = new Reservation();
            reservation.Show();
            this.Hide();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Chambre chambre = new Chambre();
            chambre.Show();
            this.Hide();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridView dgv1 = dataGridView1;

                this.id = Convert.ToString(dgv1.CurrentRow.Cells[0].Value);
                updateButton.Text = "Modifier (" + this.id + ")";
                deleteButton.Text = "Supprimer (" + this.id + ")";

                telephTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);
                prixChambTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[2].Value);
                nomHotelComboBox.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[3].Value);
                categChambComboBox.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[4].Value);
                statusChambCombBox.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[5].Value);

            }

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Acceuil acceuil = new Acceuil();
            acceuil.Show();
            this.Hide();

        }






        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}


    }
}
