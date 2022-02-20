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
    public partial class Form1 : Form
    {
        public SqlConnection myConnection;
        public SqlCommand myCommand;
        public SqlDataReader myReader;
        public int intCounter;
        public Form1()
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string firstOp = comboBox1.Text == "Pickup Date" ? "r.pickup_date" : "r.return_date";
                string secondOp = comboBox2.Text == "Pickup Date" ? "r.pickup_date" : "r.return_date";
                string thirdOp = comboBox3.Text == "Less Than" ? "<" : ">";

                myCommand.CommandText = "SELECT c.CustomerID," +
                    "c.FirstName," +
                    "c.LastName," +
                    "c.DrivingLicense," +
                    "c.Membership," +
                    "c.StreetName," +
                    "c.StreetNumber," +
                    "c.AptNumber," +
                    "c.City," +
                    "c.Province," +
                    "c.PhoneNumber," +
                    "c.Zip " +
                    "From Customers as c " +
                    "RIGHT JOIN Rental_Trans as r " +
                    "ON c.CustomerID = r.Customer_ID " +
                    "WHERE Price " + thirdOp + " " + textBox1.Text +
                    " AND " + firstOp + " >= '" + monthCalendar1.SelectionRange.Start.ToString("yyy-MM-dd") +
                    "' AND " + secondOp + " <= '" + monthCalendar2.SelectionRange.Start.ToString("yyy-MM-dd") + "'";
                try
                {
                    MessageBox.Show(myCommand.CommandText);
                    myReader = myCommand.ExecuteReader();
                    myCommand.Parameters.Clear();
                    //Clear rows to add
                    student.Rows.Clear();
                    while (myReader.Read())
                    {
                        student.Rows.Add(myReader["CustomerID"].ToString(), myReader["FirstName"].ToString(), myReader["LastName"].ToString(),
                            myReader["DrivingLicense"].ToString(), myReader["Membership"].ToString(),
                            myReader["StreetName"].ToString(), myReader["StreetNumber"].ToString(), myReader["AptNumber"].ToString(), myReader["City"].ToString(), myReader["Province"].ToString(),
                            myReader["Zip"].ToString(), myReader["PhoneNumber"].ToString());
                    }
                    myReader.Close();
                }
                catch (Exception e3)
                {
                    MessageBox.Show(e3.ToString(), "Error");
                }
            }
        }

        private void student_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
