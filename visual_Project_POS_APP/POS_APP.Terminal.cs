using MaterialSkin2Framework.Controls;
using projec_visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    partial class POS_APP
    {


        ////////        Termianl Tab variables              /////////////
        private invoice currInvoice;


        List<Invoice_Row> currInvoiceRowsList = new List<Invoice_Row>();
        List<Customer> CustomersList = new List<Customer>();




        ////////        Initilaize the varaibles                ///////

        private void initVars()
        {

            initNewInvoice();





            refreshCustomers();



        }










        /////////         EventListeners          /////////




        ////////                barcode textbox events
        private void barcodeTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            var MB = (BaseTextBox)sender;


            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                getProductFromBarcodeToCart();

            }
        }

        private void barcodeTxt_Enter(object sender, EventArgs e)
        {
            var MB = (BaseTextBox)sender;
            MB.Text = "";
        }

        private void barcodeTxt_Leave(object sender, EventArgs e)
        {
            var MB = (BaseTextBox)sender;
            MB.Text = "Enter Barcode";
        }


        private void TerminalGetBtn_Click(object sender, EventArgs e)
        {

            getProductFromBarcodeToCart();
            barcodeTxt.Text = "";



        }
        ////////////







        private void getProductFromBarcodeToCart() // from barcode textbox on Termninal page to datatable
        {
            string baracode = barcodeTxt.Text;

            product res = getProductFromBarcode(baracode);

  

            if (res != null)
            {       
                
                Invoice_Row row = new Invoice_Row
                {
                P_ID = res.P_ID,
                count = 1,
                I_ID = currInvoice.I_ID,
                isfinished = false


                };

                var checkForRow = db.Invoice_Rows
                 .Where(p => p.P_ID == res.P_ID && p.I_ID == currInvoice.I_ID)
                .OrderByDescending(p => p.P_ID)
                .FirstOrDefault();

                if (checkForRow == null)
                {

                    //Add new Employee to database
                    db.Invoice_Rows.InsertOnSubmit(row);

                    //Save changes to Database.
                    db.SubmitChanges();

                }else
                {
                    checkForRow.count++;
                    db.SubmitChanges();

                }




                refreshInvoice();



                // textBox1.Text = "name: " + res.name + ", barcode: " + res.barcode + ", pricce: " + res.price;
                barcodeTxt.Text = "";

            }



        }





        private void refreshInvoice()
        {
            var currInvoiceRows = from rowx in db.Invoice_Rows
                                  join productx in db.products on rowx.P_ID equals productx.P_ID
                                  where rowx.I_ID == currInvoice.I_ID & rowx.isfinished == false
                                  select new
                                  {
                                      name = productx.name,
                                      bardcode = productx.barcode,
                                      unit_price = productx.price.ToString() + " $",
                                      qauntity = rowx.count,
                                      sub_total = Convert.ToString(productx.price * rowx.count) + " $",
                                      unit_size = productx.size
                                  };

            //int total = 0;


            var total = (from rowx in db.Invoice_Rows
                         join productx in db.products on rowx.P_ID equals productx.P_ID
                         where rowx.I_ID == currInvoice.I_ID && !rowx.isfinished
                         select productx.price * rowx.count).Sum();

            totalTxt.Text = Convert.ToString(total) + " $";


            /*var total = from rowx in db.Invoice_Rows
                                  join productx in db.products on rowx.P_ID equals productx.P_ID
                                  where rowx.I_ID == currInvoice.I_ID & rowx.isfinished == false
                                  select new
                                  {
                                      name = productx.name,
                                      bardcode = productx.barcode,
                                      unit_price = productx.price.ToString() + " $",
                                      qauntity = rowx.count,
                                      sub_total = Convert.ToString(productx.price * rowx.count),
                                      unit_size = productx.size
                                  };*/


            terminalInvoiceDataTable2.DataSource = currInvoiceRows;
        }

        private void refreshCustomers()
        {
            //
            var query1 = db.Customers.Select(p => new { name = p.fname + " " + p.lname, id = p.C_ID });
            //.Where(p => p.name == "Default Customer");

            List<ComboBoxItem> l = new List<ComboBoxItem>(); /// A custome class by me
            foreach (var item in query1)
            {
                l.Add(new ComboBoxItem(item.id, item.name));
            }    

            //CustomersList  = query1.ToList();
            CustomerTerminalComboBox1.DataSource = l;
            CustomerTerminalComboBox1.DisplayMember = "Text";
            CustomerTerminalComboBox1.ValueMember = "Value";

        }


        private void initNewInvoice()
        {
            //Initialize invoice 
            currInvoice = new invoice
            {
                I_ID = getNewInvoiceID(),
                C_ID = 0,
                timeDate = getUnixTime(),
                isfinished = false

            };
            //Add new Employee to database
            db.invoices.InsertOnSubmit(currInvoice);

            //Save changes to Database.
            db.SubmitChanges();
            invoiceLabel1.Text = "Invoice #" + currInvoice.I_ID;


        }





        private product getProductFromBarcode(string barcode)
        {

            var insertedEmployee = db.products
                 .Where(p => p.barcode == barcode)
                .OrderByDescending(p => p.P_ID)
                .FirstOrDefault();

            if (insertedEmployee != null)
            {

                return insertedEmployee;

            }
            else { MessageBox.Show("Please enter a valid barcode."); return null; }



        }





        private long getNewInvoiceID()
        {
            long max = 0;
            checkTables();
            if (db.invoices.Count() != 0)
            {
                 max = db.invoices.Max(p => p.I_ID);
            }
            else
            {
                invoice first = new invoice();
                first.C_ID = 0;
                first.I_ID = 0;
                first.timeDate = getUnixTime();


                db.invoices.InsertOnSubmit(first);

                //Save changes to Database.
                db.SubmitChanges();

                 max = db.invoices.Max(p => p.I_ID);

            }
            return max + 1; 
        
        }








        private void finishBtn_Click(object sender, EventArgs e)
        {
            var query = from r in db.Invoice_Rows
                        join p in db.products on r.P_ID equals p.P_ID     // joining prodcuts to track stock
                        where r.I_ID == currInvoice.I_ID
                        select new{ r = r , p = p};

            foreach (var row in query)
            {
                row.r.isfinished = true;

                row.p.qauntity -= (int) row.r.count;  //deducting stock value as needed

            }


            currInvoice.C_ID =  (CustomerTerminalComboBox1.SelectedItem as ComboBoxItem).Value; 


            currInvoice.isfinished= true;
            db.SubmitChanges();
            initNewInvoice();


            refreshInvoice();
            refreshCustomers();

        }











    }
}
