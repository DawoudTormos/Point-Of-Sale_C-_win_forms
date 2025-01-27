using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MaterialSkin2Framework.Controls;
using System.Runtime.Remoting.Contexts;
using MaterialSkin2Framework;


using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;
using projec_visual;


namespace WindowsFormsApp1
{
    public partial class POS_APP : MaterialForm
    {

        ///////             global use varaiables                   ///////////
       static DataClasses1DataContext  db = new DataClasses1DataContext();




        ////////////                Invertotry tab varaibles                    ///////////
        HashSet<int> selectedRowsInventory = new HashSet<int>();










        public POS_APP()
        {
            InitializeComponent();
            this.SkinManager.ColorScheme = new MaterialSkin2Framework.ColorScheme(Primary.Blue800 , Primary.Blue900 , Primary.Blue700, Accent.Blue400, TextShade.WHITE);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
  

            // MessageBox.Show(ff);
            checkTables();
            initVars();
            checkForAnActiveBalance();
            refreshCustomersTable();





        }









        /////////         Functions          /////////


















        /////////         EventListeners          /////////





        private void button1_Click(object sender, EventArgs e)
        {   /// fro testing and debugging only



            int bar = 1223;

            product newProdcut = new product();
            newProdcut.name = "juicy11";
            newProdcut.barcode = "226663";
            newProdcut.price = 22;
            newProdcut.description = "343434343";
            newProdcut.size = 3;
            newProdcut.C_ID = 1;



            var insertedEmployee = db.products  
                             .Where(p => p.barcode == newProdcut.barcode)
                            .OrderByDescending(p => p.P_ID)
                            .FirstOrDefault();

            if (insertedEmployee == null){

                //Add new Employee to database
                db.products.InsertOnSubmit(newProdcut);

                //Save changes to Database.
                db.SubmitChanges();

            }
            else { MessageBox.Show("Please enter only a new barcode. The barcode u are trying to enter is alraedyb used with a product."); }
             insertedEmployee = db.products
                                         .Where(p => p.name == "juicy11")
                                        .OrderByDescending(p => p.P_ID)
                                        .FirstOrDefault();

            string ff = String.Format("Employee Id = {0} , Name = {1}, Email = {2}, ContactNo = {3},barcode = {4} Address = {5}",
                             insertedEmployee.P_ID, insertedEmployee.name, insertedEmployee.price,
                             insertedEmployee.description,insertedEmployee.barcode , insertedEmployee.C_ID );

            ///textBox1.Text = ff;
        }











       

        private void InventoryTab_Click(object sender, EventArgs e)
        {

        }



        private void materialTabSelector1_Click(object sender, EventArgs e)
        {

        }



        private void button1_Click_1(object sender, EventArgs e)
        {

            var query = from product in db.products
                        join category in db.categories on product.C_ID equals category.Cat_ID
                        select new
                        {
                            product.name,
                            product.barcode,
                            product.price,
                            Category = category.name,
                            product.description,
                            product.size
                        };


           
            materialDataTable1.DataSource = query.ToList();


        }



        private void button2_Click(object sender, EventArgs e)
        {
            string str = "";

            selectedRowsInventory.Clear();
            for (int i = 0; i <  materialDataTable1.SelectedCells.Count; i++)
            {
                
                selectedRowsInventory.Add(materialDataTable1.SelectedCells[i].RowIndex);
            }
            
            foreach (int row in selectedRowsInventory)
            {
                str += ", " + Convert.ToString(row);

            }

            MessageBox.Show(str);
        }

        private void materialDataTable1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void materialTextBox22_Click(object sender, EventArgs e)
        {

        }

        private void materialTextBox21_Click(object sender, EventArgs e)
        {

        }

        private void materialMultiLineTextBox21_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {

        }

        private void CustomerTerminalComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // MessageBox.Show(    Convert.ToString((   (MaterialComboBox)sender).SelectedIndex    ));


            //MessageBox.Show(currInvoice.C_ID);
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
