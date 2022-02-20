using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _291_Project
{
    public partial class Main : Form
    {
        public SqlConnection myConnection;
        public SqlCommand myCommand;
        public SqlDataReader myReader;
        public int intCounter;
        public Main()
        {
            InitializeComponent();
            CProvince.Items.Clear();
            CProvince.Items.Add("AB");
            CProvince.Items.Add("BC");
            CProvince.Items.Add("ON");
            CProvince.Items.Add("MB");
            CProvince.Items.Add("NB");
            CProvince.Items.Add("NL");
            CProvince.Items.Add("NT");
            CProvince.Items.Add("NS");
            CProvince.Items.Add("NU");
            CProvince.Items.Add("PE");
            CProvince.Items.Add("QC");
            CProvince.Items.Add("SK");
            CProvince.Items.Add("YT");

            comboBox2.Items.Clear();
            comboBox2.Items.Add("AB");
            comboBox2.Items.Add("BC");
            comboBox2.Items.Add("ON");
            comboBox2.Items.Add("MB");
            comboBox2.Items.Add("NB");
            comboBox2.Items.Add("NL");
            comboBox2.Items.Add("NT");
            comboBox2.Items.Add("NS");
            comboBox2.Items.Add("NU");
            comboBox2.Items.Add("PE");
            comboBox2.Items.Add("QC");
            comboBox2.Items.Add("SK");
            comboBox2.Items.Add("YT");
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                myCommand.CommandText = "select * from Customers";
            }
            else
            {
                myCommand.CommandText = "select * from Customers "+
                    "WHERE CustomerID Like '" + textBox1.Text + "' OR "+
                    "FirstName Like '" + textBox1.Text + "' OR " +
                    "LastName Like '" + textBox1.Text + "' OR " +
                    "DrivingLicense Like '" + textBox1.Text + "' OR " +
                    "Membership Like '" + textBox1.Text + "' OR " +
                    "StreetName Like '" + textBox1.Text + "' OR " +
                    "StreetNumber Like '" + textBox1.Text + "' OR " +
                    "AptNumber Like '" + textBox1.Text + "' OR " +
                    "City Like '" + textBox1.Text + "' OR " +
                    "Province Like '" + textBox1.Text + "' OR " +
                    "Zip Like '" + textBox1.Text + "' OR " +
                    "PhoneNumber Like '" + textBox1.Text + "'";
            }

            try
            {
                MessageBox.Show(myCommand.CommandText);
                myReader = myCommand.ExecuteReader();

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

        private void customerSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                //First searches for entries with some of the data that won't likely overlap.
                myCommand.CommandText = "SELECT * FROM Customers WHERE FirstName = '" + CFirstName.Text
                + "' OR LastName = '" + CLastName.Text
                + "' OR DrivingLicense = '" + DrivingLicense.Text
                + "'";

                SqlDataReader reader = myCommand.ExecuteReader();

                //Checks to see if the search returned any rows
                if (!reader.HasRows)
                {
                    reader.Close();
                    myCommand.CommandText = "insert into Customers values ('" + CFirstName.Text + "','" + CLastName.Text + "','" + DrivingLicense.Text + "','" +
                                                Membership.Text + "','" + CStreetName.Text + "','" + CStreetNumber.Text + "','" + CUnit.Text + "','" + CCity.Text +
                                                "','" + CProvince.Text + "','" + CPostalCode.Text + "','" + CPhoneNumber.Text + "')";

                    MessageBox.Show(myCommand.CommandText);

                    myCommand.ExecuteNonQuery();
                    
                }
                else
                {
                    reader.Close();
                    //Using update format found here: https://stackoverflow.com/questions/15246182/sql-update-statement-in-c-sharp
                    //Note that in order to edit both first and last name, or driver's license must be the same.
                    myCommand.CommandText = "UPDATE Customers SET FirstName = @FN, LastName = @LN," +
                        " DrivingLicense = @DL, Membership = @MS, StreetName = @SN, StreetNumber = @SNum," +
                        "AptNumber = @AN, City = @CT, Province = @PR, Zip = @ZP, PhoneNumber = @PN " +
                        "WHERE (FirstName = @FN AND LastName = @LN) OR DrivingLicense = @DL";
                    //Will, at the moment, erase data with empty entries in the text fields

                    myCommand.Parameters.AddWithValue("@FN", CFirstName.Text);

                    myCommand.Parameters.AddWithValue("@LN", CLastName.Text);

                    myCommand.Parameters.AddWithValue("@DL", DrivingLicense.Text);

                    myCommand.Parameters.AddWithValue("@MS", Membership.Text);

                    myCommand.Parameters.AddWithValue("@SN", CStreetName.Text);

                    myCommand.Parameters.AddWithValue("@SNum", CStreetNumber.Text);

                    myCommand.Parameters.AddWithValue("@AN", CUnit.Text);

                    myCommand.Parameters.AddWithValue("@CT", CCity.Text);

                    myCommand.Parameters.AddWithValue("@PR", CProvince.Text);

                    myCommand.Parameters.AddWithValue("@ZP", CPostalCode.Text);

                    myCommand.Parameters.AddWithValue("@PN", CPhoneNumber.Text);

                    MessageBox.Show(myCommand.CommandText);

                    myCommand.ExecuteNonQuery();
                    myCommand.Parameters.Clear();
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString(), "Error");
            }
        }

        private void student_MouseClick(object sender, MouseEventArgs e)
        {
            if (student.SelectedRows[0].Cells[0].Value != null)
            {
                CFirstName.Text = student.SelectedRows[0].Cells[1].Value.ToString();
                CLastName.Text = student.SelectedRows[0].Cells[2].Value.ToString();
                DrivingLicense.Text = student.SelectedRows[0].Cells[3].Value.ToString();
                Membership.Text = student.SelectedRows[0].Cells[4].Value.ToString();
                CStreetName.Text = student.SelectedRows[0].Cells[5].Value.ToString();
                CStreetNumber.Text = student.SelectedRows[0].Cells[6].Value.ToString();
                CUnit.Text = student.SelectedRows[0].Cells[7].Value.ToString();
                CProvince.Text = student.SelectedRows[0].Cells[9].Value.ToString();
                CCity.Text = student.SelectedRows[0].Cells[8].Value.ToString();
                CPostalCode.Text = student.SelectedRows[0].Cells[10].Value.ToString();
                CPhoneNumber.Text = student.SelectedRows[0].Cells[11].Value.ToString();
            }
            else
            {
                CFirstName.Text = null;
                CLastName.Text = null;
                DrivingLicense.Text = null;
                Membership.Text = null;
                CStreetName.Text = null;
                CStreetNumber.Text = null;
                CUnit.Text = null;
                CProvince.Text = null;
                CCity.Text = null;
                CPostalCode.Text = null;
                CPhoneNumber.Text = null;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                //First searches for entries with some of the data that won't likely overlap.
                myCommand.CommandText = "SELECT * FROM Customers WHERE FirstName LIKE '" + CFirstName.Text
                + "' OR LastName LIKE '" + CLastName.Text
                + "' OR DrivingLicense LIKE '" + DrivingLicense.Text
                + "' OR Membership LIKE '" + Membership.Text
                + "'";

                SqlDataReader reader = myCommand.ExecuteReader();

                //Checks to see if the search returned any rows
                if (reader.HasRows)
                {
                    reader.Close();
                    //Note that in order to delete either both first and last name, or driver's license must be the same as text fields.
                    myCommand.CommandText = "DELETE FROM CUSTOMERS WHERE (FirstName = @FN AND LastName = @LN) OR DrivingLicense = @DL";


                    myCommand.Parameters.AddWithValue("@FN", CFirstName.Text);

                    myCommand.Parameters.AddWithValue("@LN", CLastName.Text);

                    myCommand.Parameters.AddWithValue("@DL", DrivingLicense.Text);

                    MessageBox.Show(myCommand.CommandText);

                    myCommand.ExecuteNonQuery();
                    myCommand.Parameters.Clear();
                }
                else { reader.Close(); }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString(), "Error");
            }
        }

        private void textBox1_KeyEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                myCommand.CommandText = "select * from Rental_Trans";
            }
            else
            {
                myCommand.CommandText = "select * from Rental_Trans";
                /*myCommand.CommandText = "select * from Customers " +
                    "WHERE CustomerID Like '" + textBox1.Text + "' OR " +
                    "FirstName Like '" + textBox1.Text + "' OR " +
                    "LastName Like '" + textBox1.Text + "' OR " +
                    "DrivingLicense Like '" + textBox1.Text + "' OR " +
                    "Membership Like '" + textBox1.Text + "' OR " +
                    "StreetName Like '" + textBox1.Text + "' OR " +
                    "StreetNumber Like '" + textBox1.Text + "' OR " +
                    "AptNumber Like '" + textBox1.Text + "' OR " +
                    "City Like '" + textBox1.Text + "' OR " +
                    "Province Like '" + textBox1.Text + "' OR " +
                    "Zip Like '" + textBox1.Text + "' OR " +
                    "PhoneNumber Like '" + textBox1.Text + "'";*/
            }

            try
            {
                MessageBox.Show(myCommand.CommandText);
                myReader = myCommand.ExecuteReader();

                dataGridView1.Rows.Clear();
                while (myReader.Read())
                {
                    dataGridView1.Rows.Add(myReader["Rental_ID"].ToString(), myReader["Employee_ID"].ToString(), myReader["Customer_ID"].ToString(),
                        myReader["Customer_ID"].ToString(), myReader["pickup_date"].ToString(),
                        myReader["return_date"].ToString(), myReader["pickup_Branch_ID"].ToString(), myReader["return_Branch_ID"].ToString(),
                        myReader["VIN"].ToString(), myReader["price"].ToString());
                }

                myReader.Close();
            }
            catch (Exception e3)
            {
                MessageBox.Show(e3.ToString(), "Error");
            }
        }
        //Book button
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                myCommand.CommandText = "SELECT * FROM Cars WHERE VIN = '" + bookingVIN.Text + "'";

                myReader = myCommand.ExecuteReader();
                if (myReader.HasRows)// if selected VIN is in the inverntory
                {
                    myReader.Close();
                        //First searches for entries with some of the data that won't likely overlap.
                        myCommand.CommandText = "SELECT * FROM Rental_Trans WHERE Rental_ID = '" + bookingPrice.Text + "'";
                        MessageBox.Show(myCommand.CommandText);
                        SqlDataReader reader = myCommand.ExecuteReader();

                        //Checks to see if the search returned any rows
                        if (!reader.HasRows)//if rental id does not exist create
                        {
                            reader.Close();
                            //Rental_Trans(Rental_ID, pickup_date, return_date, price, CustomerID, EmployeeID, pickup_Branch_ID, return_Branch_ID, VIN)
                            myCommand.CommandText = "insert into Rental_Trans values (@BPD, @BRD," +
                                                    "(select Car_types.Price_D from Cars,Car_types where Cars.CarType = Car_types.CarType and Cars.VIN = @VIN), " +
                                                    "(select CustomerID from Customers where CustomerID = @CID), " +
                                                    "(select Employee_ID from Employees where Employee_ID = @EID), " +
                                                    "(select Branch_ID from Branches where Branch_ID = @PBID), " +
                                                    "(select Branch_ID from Branches where Branch_ID = @RBID), " +
                                                    "(select VIN from Cars where VIN = @VIN))";


                        }
                        else//if rental id exist update 
                        {
                            reader.Close();
                            //Using update format found here: https://stackoverflow.com/questions/15246182/sql-update-statement-in-c-sharp
                            //Note that in order to edit both first and last name, or driver's license must be the same.
                            myCommand.CommandText = "UPDATE Rental_Trans SET pickup_date = @BPD, return_date = @BRD, " +
                                                    "price = (select Price_D from Cars,Car_types where Cars.CarType = Car_types.CarType and Cars.VIN = @VIN), " +
                                                    "Customer_ID = (select CustomerID from Customers where CustomerID = @CID), " +
                                                    "Employee_Id = (select Employee_ID from Employees where Employee_ID = @EID), " +
                                                    "pickup_Branch_ID = (select Branch_ID from Branches where Branch_ID = @PBID), " +
                                                    "return_Branch_ID = (select Branch_ID from Branches where Branch_ID = @RBID), " +
                                                    "VIN = (select VIN from Cars where VIN = @VIN) WHERE Rental_ID = '" + bookingPrice.Text + "'";

                        }
                        //After CommandText set, execute
                        //Will, at the moment, erase data with empty entries in the text fields

                        myCommand.Parameters.AddWithValue("@BPD", bookingPickUpDate.Value.Date);

                        myCommand.Parameters.AddWithValue("@BRD", bookingReturnDate.Value.Date);

                        myCommand.Parameters.AddWithValue("@CID", bookingCustomerID.Text);

                        myCommand.Parameters.AddWithValue("@EID", bookingEmployeeID.Text);

                        myCommand.Parameters.AddWithValue("@PBID", bookingPickupID.Text);

                        myCommand.Parameters.AddWithValue("@RBID", bookingReturnID.Text);

                        myCommand.Parameters.AddWithValue("@VIN", bookingVIN.Text);

                        MessageBox.Show(myCommand.CommandText);
                        myCommand.ExecuteNonQuery();
                        myCommand.Parameters.Clear();

                        //Afterward, need to remove vehicle from inventory  
                }
                else { myReader.Close(); MessageBox.Show("Vehicle Not In Inventory!"); }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString(), "Error");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

            if (textBox3.Text == "")
            {
                myCommand.CommandText = "select * from Cars,Car_types WHERE Cars.CarType = Car_types.CarType";
            }
                                        
            else
            {
                myCommand.CommandText = "select * from Cars,Car_types " +
                    "WHERE Cars.CarType = Car_types.CarType and " +
                    "(VIN Like '" + textBox3.Text + "' OR " +
                    "Color Like '" + textBox3.Text + "' OR " +
                    "Model Like '" + textBox3.Text + "' OR " +
                    "Cars.CarType Like '" + textBox3.Text + "' OR " +
                    "BranchID LIKE '" + textBox3.Text + "')";

            }
            try
            {
                MessageBox.Show(myCommand.CommandText);
                myReader = myCommand.ExecuteReader();

                vehicleGrid.Rows.Clear();
                while (myReader.Read())
                {                           
                    vehicleGrid.Rows.Add(myReader["VIN"].ToString(), myReader["Model"].ToString(), myReader["CarType"].ToString(),
                        myReader["Color"].ToString(), myReader["BranchID"].ToString(),
                        myReader["price_D"].ToString(), myReader["price_W"].ToString() , myReader["price_M"].ToString());
                }
                myReader.Close();
            }
            catch (Exception e3)
            {
                MessageBox.Show(e3.ToString(), "Error");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (textBox28.Text == "")
            {
                myCommand.CommandText = "select * from Branches";
            }

            else
            {
                myCommand.CommandText = "select * from Branches " +
                    "WHERE Branch_ID LIKE '" + textBox28.Text + "' OR " +
                    "street_name LIKE '" + textBox28.Text + "' OR " +
                    "street_number LIKE '" + textBox28.Text + "' OR " +
                    "city LIKE '" + textBox28.Text + "' OR " +
                    "province LIKE '" + textBox28.Text + "'" +
                    " OR zip LIKE '" + textBox28.Text + "'" +
                    " OR phone_number LIKE '" + textBox28.Text + "'";

            }
            try
            {
                MessageBox.Show(myCommand.CommandText);
                myReader = myCommand.ExecuteReader();

                branchGrid.Rows.Clear();
                while (myReader.Read())
                {
                    branchGrid.Rows.Add(myReader["Branch_ID"].ToString(), myReader["street_name"].ToString(), myReader["street_number"].ToString(),
                        myReader["city"].ToString(), myReader["province"].ToString(),
                        myReader["zip"].ToString(), myReader["phone_number"].ToString());
                }
                myReader.Close();
            }
            catch (Exception e3)
            {
                MessageBox.Show(e3.ToString(), "Error");
            }
        }
        //Vehicle search in bookings
        private void button16_Click(object sender, EventArgs e)
        {
            if (textBox38.Text == "")
            {
                //Get all cars in inventory, subtract VINs that are taken already
                myCommand.CommandText = "select * from Cars,Car_types WHERE Cars.CarType = Car_types.CarType AND Cars.VIN not in "+
                                        "(select VIN from Rental_Trans where return_date > @TODAY)";
            }

            else
            {
                //Get all cars that match search terms, subtract all VINs with return dates after today
                myCommand.CommandText = "select * from Cars,Car_types" +
                    "WHERE Cars.CarType = Car_types.CarType and " +
                    "VIN Like '" + textBox38.Text + "' OR " +
                    "Color Like '" + textBox38.Text + "' OR " +
                    "Model Like '" + textBox38.Text + "' OR " +
                    "CarType Like '" + textBox38.Text + "' OR " +
                    "BranchID Like '" + textBox38.Text + "'"+" AND Cars.VIN not in " +
                    "(select VIN from Rental_Trans where return_date > @TODAY)";

            }
            try
            {
                myCommand.Parameters.AddWithValue("@TODAY", dateTimePicker4.Value.Date);
                MessageBox.Show(myCommand.CommandText);
                myReader = myCommand.ExecuteReader();
                myCommand.Parameters.Clear();
                //Clear rows to add
                dataGridView2.Rows.Clear();
                while (myReader.Read())
                {
                    //VIN, Make, Model, Color, Branch, Price(D,W,M)
                    dataGridView2.Rows.Add(myReader["VIN"].ToString(), myReader["CarType"].ToString(), 
                        myReader["Model"].ToString(), myReader["Color"].ToString(), 
                        myReader["BranchID"].ToString(),
                        "$" + myReader["price_D"].ToString() + ", $" + myReader["price_W"].ToString() + ", $" + myReader["price_M"].ToString());
                }
                myReader.Close();
            }
            catch (Exception e3)
            {
                MessageBox.Show(e3.ToString(), "Error");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                myCommand.CommandText = "SELECT * FROM Branches WHERE Branch_ID = '" + VBranchID.Text + "'";//search for match branch
                myReader = myCommand.ExecuteReader();

                if (myReader.HasRows)//if there is a matching branch
                {
                    myReader.Close();
                    //First searches for entries with some of the data that won't likely overlap.
                    myCommand.CommandText = "SELECT * FROM Cars,Car_types WHERE Cars.CarType = Car_types.CarType and VIN = '" + VVIN.Text + "'";//search for matching vehicle

                    SqlDataReader reader = myCommand.ExecuteReader();

                    //Checks to see if the search returned any rows
                    if (!reader.HasRows)//if there is no matching vehicle
                    {
                        reader.Close();

                        myCommand.CommandText = "SELECT * FROM Car_types WHERE CarType = '" + VType.Text + "'";//search for matching car type
                        myReader = myCommand.ExecuteReader();

                        if (!myReader.HasRows)//if there is no matching car type create one.
                        {
                            myReader.Close();
                            //create new type
                            myCommand.CommandText = "insert into Car_types values ('" + VType.Text + "'," + VDPrice.Text + "," + VWPrice.Text + "," + VMPrice.Text + ")";
                            MessageBox.Show(myCommand.CommandText);
                            myCommand.ExecuteNonQuery();
                        }
                        else { myReader.Close(); }

                        myCommand.CommandText = "insert into Cars values (" + VVIN.Text + " , '" + VColor.Text + "','" + VModel.Text + "','" + VType.Text + "'," +
                                                        VBranchID.Text + ")";//insert new vehicle
                        MessageBox.Show(myCommand.CommandText);
                        myCommand.ExecuteNonQuery();

                    }
                    else//if there is matching vehicle
                    {
                        reader.Close();
                        MessageBox.Show("Existing Vehicle!");
                    }

                }
                else //branch does not exist.
                {   myReader.Close();
                    MessageBox.Show("Branch Does Not Exist"); 
                }

            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString(), "Error");
            }
        }
        //Remove car from inventory
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                myCommand.CommandText = "SELECT * FROM Rental_Trans WHERE VIN = '" + VVIN.Text + "'";
                myReader = myCommand.ExecuteReader();

                if (!myReader.HasRows)
                {
                    myReader.Close();
                    try
                    {
                        //First searches for entries with some of the data that won't likely overlap.
                        myCommand.CommandText = "SELECT * FROM Cars WHERE VIN = '" + VVIN.Text + "'";

                        SqlDataReader reader = myCommand.ExecuteReader();

                        //Checks to see if the search returned any rows
                        if (reader.HasRows)
                        {
                            reader.Close();
                            myCommand.CommandText = "DELETE FROM Cars WHERE VIN = '" + VVIN.Text + "'";

                            MessageBox.Show(myCommand.CommandText);

                            myCommand.ExecuteNonQuery();
                            myCommand.Parameters.Clear();
                        }
                        else { reader.Close(); }
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString(), "Error");
                    }
                }
                else
                {
                    myReader.Close();
                    MessageBox.Show("The Car need be Returned.");
                }
            }
            catch (Exception e3){ MessageBox.Show(e3.ToString(), "Error"); }
        }

        //return page search button
        private void button17_Click(object sender, EventArgs e)
        {
            if (textBox38.Text == "")
            {
                //Get all cars in inventory, subtract VINs that are taken already
                myCommand.CommandText = "SELECT * FROM Rental_Trans";
            }

            else
            {
                //Rental_Trans(Rental_ID, pickup_date, return_date, price, CustomerID, EmployeeID, pickup_Branch_ID, return_Branch_ID, VIN)
                myCommand.CommandText = "select * from Rental_Trans " +
                    "WHERE Rental_ID Like @SEARCHTEXT OR VIN Like @SEARCHTEXT OR Color Like @SEARCHTEXT" +
                    " OR Model Like @SEARCHTEXT OR CarType Like @SEARCHTEXT OR BranchID Like @SEARCHTEXT " +
                    "AND return_date > @TODAY";

            }
            try
            {
                //Format matches SQL date pattern
                myCommand.Parameters.AddWithValue("@TODAY", DateTime.Now.Date);
                myCommand.Parameters.AddWithValue("@SEARCHTEXT", returnSearchBar.Text);

                MessageBox.Show(myCommand.CommandText);
                myReader = myCommand.ExecuteReader();
                myCommand.Parameters.Clear();//Important to clear parameters
                //Clear rows to add
                dataGridView3.Rows.Clear();
                while (myReader.Read())
                {
                    dataGridView3.Rows.Add(myReader["VIN"].ToString(), myReader["Model"].ToString(), myReader["CarType"].ToString(),
                        myReader["Color"].ToString(), myReader["BranchID"].ToString(),
                        "$" + myReader["price_D"].ToString(), "$" + myReader["price_W"].ToString(), "$" + myReader["price_M"].ToString());
                }
                myReader.Close();
            }
            catch (Exception e3)
            {
                MessageBox.Show(e3.ToString(), "Error");
            }
        }
        private void student_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label56_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {   
                
            try
                
            {
                    
                myCommand.CommandText = "SELECT * FROM Branches WHERE street_name = '" + textBox23.Text + "' AND street_number = '" +
                    textBox30.Text + "' AND city = '" + textBox20.Text + "'";
                    
                myReader = myCommand.ExecuteReader();

                    
                if (!myReader.HasRows && textBox25.Text == "")
                {
                    myReader.Close();
                    myCommand.CommandText = "INSERT INTO Branches VALUES('" + textBox23.Text + "','" + textBox30.Text + "','" + textBox20.Text + "','" + comboBox2.Text +
                            "','" + textBox19.Text + "','" + textBox26.Text + "')";

                    MessageBox.Show(myCommand.CommandText);
                    myCommand.ExecuteNonQuery();
                }
                    
                else
                {
                    myReader.Close();
                        
                    myCommand.CommandText = "SELECT * FROM Branches WHERE Branch_ID = " + textBox25.Text;
                    myReader = myCommand.ExecuteReader();
                    if (myReader.HasRows)//ID exist in table
                    {
                        myReader.Close();
                        myCommand.CommandText = "UPDATE Branches SET street_name = @SN, street_number = @SB," +
                            "city = @CT, province = @PV, zip = @ZP, phone_number = @PH " +
                            "WHERE (street_name = @SN AND street_number = @SB AND city = @CT) OR Branch_ID = @BID";
                        //Will, at the moment, erase data with empty entries in the text fields

                        myCommand.Parameters.AddWithValue("@SN", textBox23.Text);

                        myCommand.Parameters.AddWithValue("@SB", textBox30.Text);

                        myCommand.Parameters.AddWithValue("@CT", textBox20.Text);

                        myCommand.Parameters.AddWithValue("@PV", comboBox2.Text);

                        myCommand.Parameters.AddWithValue("@ZP", textBox19.Text);

                        myCommand.Parameters.AddWithValue("@PH", textBox26.Text);

                        myCommand.Parameters.AddWithValue("@BID", int.Parse(textBox25.Text));

                        MessageBox.Show(myCommand.CommandText);

                        myCommand.ExecuteNonQuery();
                        myCommand.Parameters.Clear();
                    }
                    else 
                    {
                        myReader.Close();
                        MessageBox.Show("Branch ID not found!");
                    }
                    
                }
                
            }
                
            catch (Exception e4) { MessageBox.Show(e4.ToString(), "Error"); }
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (textBox25.Text != "")
            {
                myCommand.CommandText = "SELECT * FROM Cars C WHERE C.BranchID = " + textBox25.Text;
                myReader = myCommand.ExecuteReader();

                if (!myReader.HasRows)
                {
                    myReader.Close();
                    myCommand.CommandText = "SELECT * FROM Branches WHERE Branch_ID = " + textBox25.Text;
                    myReader = myCommand.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        myReader.Close();
                        myCommand.CommandText = "DELETE Branches WHERE Branch_ID = " + textBox25.Text;
                        MessageBox.Show(myCommand.CommandText);
                        myCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        myReader.Close();
                        MessageBox.Show("Branch Does Not Exist.");
                    }
                }
                else 
                {
                    myReader.Close();
                    MessageBox.Show("Cars Need To Be Moved."); 
                }
            }
            else { myReader.Close(); }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void branchGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (branchGrid.SelectedRows[0].Cells[0].Value != null)
            {
                textBox25.Text = branchGrid.SelectedRows[0].Cells[0].Value.ToString();
                textBox23.Text = branchGrid.SelectedRows[0].Cells[1].Value.ToString();
                textBox30.Text = branchGrid.SelectedRows[0].Cells[2].Value.ToString();
                textBox20.Text = branchGrid.SelectedRows[0].Cells[3].Value.ToString();
                textBox2.Text = branchGrid.SelectedRows[0].Cells[4].Value.ToString();
                textBox19.Text = branchGrid.SelectedRows[0].Cells[5].Value.ToString();
                textBox26.Text = branchGrid.SelectedRows[0].Cells[6].Value.ToString();

            }
            else
            {
                textBox25.Text = null;
                textBox23.Text = null;
                textBox30.Text = null;
                textBox20.Text = null;
                textBox2.Text = null;
                textBox19.Text = null;
                textBox26.Text = null;
            }
        }

        private void vehicleGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (vehicleGrid.SelectedRows[0].Cells[0].Value != null)
            {
                VVIN.Text = vehicleGrid.SelectedRows[0].Cells[0].Value.ToString();
                VModel.Text = vehicleGrid.SelectedRows[0].Cells[1].Value.ToString();
                VType.Text = vehicleGrid.SelectedRows[0].Cells[2].Value.ToString();
                UpdateCarType.Text = vehicleGrid.SelectedRows[0].Cells[2].Value.ToString();
                UpdateDP.Text = null;
                UpdateMP.Text = null;
                UpdateWP.Text = null;
                VColor.Text = vehicleGrid.SelectedRows[0].Cells[3].Value.ToString();
                VBranchID.Text = vehicleGrid.SelectedRows[0].Cells[4].Value.ToString();
                VDPrice.Text = vehicleGrid.SelectedRows[0].Cells[5].Value.ToString();
                VWPrice.Text = vehicleGrid.SelectedRows[0].Cells[6].Value.ToString();
                VMPrice.Text = vehicleGrid.SelectedRows[0].Cells[7].Value.ToString();

            }
            else
            {
                VVIN.Text = null;
                VModel.Text = null;
                VType.Text = null;
                VColor.Text = null;
                VBranchID.Text = null;
                VDPrice.Text = null;
                VWPrice.Text = null;
                VMPrice.Text = null;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            myCommand.CommandText = "SELECT * FROM Car_types WHERE CarType = '" + UpdateCarType.Text + "'";
            myReader = myCommand.ExecuteReader();
            if (myReader.HasRows)
            {
                myReader.Close();
                myCommand.CommandText = "UPDATE Car_types SET Price_D = @DP, Price_W = @WP, Price_M = @MP WHERE CarType = @CT";

                myCommand.Parameters.AddWithValue("@DP", UpdateDP.Text.ToString());
                myCommand.Parameters.AddWithValue("@WP", UpdateWP.Text.ToString());
                myCommand.Parameters.AddWithValue("@MP", UpdateMP.Text.ToString());
                myCommand.Parameters.AddWithValue("@CT", UpdateCarType.Text.ToString());

                MessageBox.Show(myCommand.CommandText);
                myCommand.ExecuteNonQuery();
                myCommand.Parameters.Clear();
            }
            else { MessageBox.Show("Type Does Not Exist."); }
        }

        private void returnSearch_Click(object sender, EventArgs e)
        {
            if (returnSearchBar.Text == "")
            {
                //Get all cars in inventory, subtract VINs that are taken already
                myCommand.CommandText = "select * from Rental_Trans WHERE return_date = @DTVAL";
            }

            else
            {
                //Get all cars that match search terms, subtract all VINs with return dates after today
                myCommand.CommandText = "select * FROM Rental_Trans" +
                    " WHERE return_date = @DTVAL AND (" +
                    "Rental_ID LIKE '" + returnSearchBar.Text + "' OR " +
                    "pickup_date LIKE '" + returnSearchBar.Text + "' OR " +
                    "return_date LIKE '" + returnSearchBar.Text + "' OR " +
                    "price LIKE '" + returnSearchBar.Text + "' OR " +
                    "Customer_ID LIKE '" + returnSearchBar.Text + "' OR" +
                    " Employee_ID LIKE '" + returnSearchBar.Text + "' OR " +
                    "pickup_Branch_ID LIKE '" + returnSearchBar.Text + "' OR " +
                    "return_Branch_ID LIKE '" + returnSearchBar.Text + "' OR " +
                    "VIN LIKE '" + returnSearchBar.Text + "')";
               
            }
            try
            {
                myCommand.Parameters.AddWithValue("@DTVAL", dateTimePicker5.Value.Date);
                MessageBox.Show(myCommand.CommandText);
                myReader = myCommand.ExecuteReader();
                myCommand.Parameters.Clear();
                //Clear rows to add
                dataGridView3.Rows.Clear();
                while (myReader.Read())
                {
                    dataGridView3.Rows.Add(myReader["Rental_ID"].ToString(), myReader["pickup_date"].ToString(), myReader["return_date"].ToString(),
                        myReader["price"].ToString(), myReader["Customer_ID"].ToString(),
                        myReader["Employee_ID"].ToString(), myReader["pickup_Branch_ID"].ToString(), myReader["return_Branch_ID"].ToString(),myReader["VIN"].ToString());
                }
                myReader.Close();
            }
            catch (Exception e3)
            {
                MessageBox.Show(e3.ToString(), "Error");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (returnCustomerID.Text != "")
            {
                try
                {
                    myCommand.CommandText = "DELETE FROM Rental_Trans WHERE Rental_ID = " + returnCustomerID.Text + " AND VIN = '" + returnVIN.Text + "'";
                    MessageBox.Show(myCommand.CommandText);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception e5) { MessageBox.Show(e5.ToString(), "Error"); }
            }
        }

        private void dataGridView3_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (dataGridView3.SelectedRows[0].Cells[0].Value != null)
                {
                    returnCustomerID.Text = dataGridView3.SelectedRows[0].Cells[0].Value.ToString();
                    returnPrice.Text = dataGridView3.SelectedRows[0].Cells[3].Value.ToString();
                    returnVIN.Text = dataGridView3.SelectedRows[0].Cells[8].Value.ToString();
                }
                else
                {
                    returnCustomerID.Text = null;
                    returnPrice.Text = null;
                    returnVIN.Text = null;
                }
            }
            catch (Exception e7) { MessageBox.Show(e7.ToString(), "Error"); } 
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void bestCustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 reporting = new Form1();
            reporting.Show();
        }

        private void rentedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 reporting2 = new Form2();
            reporting2.Show();
        }
    }
}
 