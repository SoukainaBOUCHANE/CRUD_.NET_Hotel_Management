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
    public partial class Acceuil : Form
    {
        private string username;

       /* public Acceuil(string username)
        {
           // this.username = username;
            InitializeComponent();
        }*/

        public Acceuil()
        {
            InitializeComponent();
        }

       

        private void clientBtnn_Click_1(object sender, EventArgs e)
        {
            Client form1 = new Client();
            form1.Show();
            this.Hide();

        }

        private void button11_Click(object sender, EventArgs e)
        {
            
        }


        private void Acceuil_Load(object sender, EventArgs e)
        {
            //lblUser.Text = lblUser.Text + username;
        }




        private void button3_Click_1(object sender, EventArgs e)
        {
            Reservation reservation = new Reservation();
            reservation.Show();
            this.Hide();
        }

        private void consomBtn_Click_1(object sender, EventArgs e)
        {
            Consomation consomation = new Consomation();
            consomation.Show();
            this.Hide();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            Chambre chambre = new Chambre();
            chambre.Show();
            this.Hide();
        }







    }
}
