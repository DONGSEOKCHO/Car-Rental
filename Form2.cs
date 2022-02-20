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

namespace _291_Project
{
    public partial class Form2 : Form
    {
        public SqlConnection myConnection;
        public SqlCommand myCommand;
        public SqlDataReader myReader;
        public int intCounter;
        public Form2()
        {
            InitializeComponent();
            String connectionString = "Server = localhost; Database = CMPT291_Project; Trusted_Connection = yes;";
            intCounter = 2;


            /* Starting the connection */
            /*  SqlConnection myConnection = new SqlConnection("user id=temp2;" + // Username
                                         "password=adminadmin;" + // Password
                                         "server=localhost;" + // IP for the server
                                                               //"Trusted_Connection=yes;" +
                                         "database=ConnectTutorial; " + // Database to connect to
                                         "connection timeout=30"); // Timeout in seconds */

            SqlConnection myConnection = new SqlConnection(connectionString); // Timeout in seconds

            try
            {
                myConnection.Open(); // Open connection
                myCommand = new SqlCommand();
                myCommand.Connection = myConnection; // Link the command stream to the connection

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                this.Close();
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string firstOp = comboBox1.Text == "Pickup Date" ? "r.pickup_date" : "r.return_date";
            string secondOp = comboBox2.Text == "Pickup Date" ? "r.pickup_date" : "r.return_date";
            string date1 = monthCalendar1.SelectionRange.Start.ToString("yyy-MM-dd");
            string date2 = monthCalendar2.SelectionRange.Start.ToString("yyy-MM-dd");
            string rented = "SELECT c.VIN, c.Color, c.Model, c.CarType, c.BranchID, t.Price_D, t.Price_W, t.Price_M From Cars as c RIGHT JOIN Rental_Trans as r on r.VIN = c.VIN Left JOIN Car_types as t on c.CarType = t.CarType WHERE "+firstOp+" >= '"+date1+"' AND "+secondOp+" <= '"+date2+"'";
            string notRented = "SELECT c.VIN, c.Color, c.Model, c.CarType, c.BranchID, t.Price_D, t.Price_W, t.Price_M From Cars as c Left JOIN Car_types as t on c.CarType = t.CarType WHERE c.Vin NOT IN (SELECT c.VIN From Cars as c RIGHT JOIN Rental_Trans as r on r.VIN = c.VIN Left JOIN Car_types as t on c.CarType = t.CarType WHERE " + firstOp + " >= '" + date1 + "' AND " + secondOp + " <= '" + date2 + "')";
            
            myCommand.CommandText = comboBox3.Text == "Rented" ? rented : notRented;


            try
            {
                MessageBox.Show(myCommand.CommandText);
                myReader = myCommand.ExecuteReader();

                vehicleGrid.Rows.Clear();
                while (myReader.Read())
                {
                    vehicleGrid.Rows.Add(myReader["VIN"].ToString(), myReader["Model"].ToString(), myReader["CarType"].ToString(),
                        myReader["Color"].ToString(), myReader["BranchID"].ToString(),
                        myReader["price_D"].ToString(), myReader["price_W"].ToString(), myReader["price_M"].ToString());
                }
                myReader.Close();
            }
            catch (Exception e3)
            {
                MessageBox.Show(e3.ToString(), "Error");
            }
        }

        private void vehicleGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
