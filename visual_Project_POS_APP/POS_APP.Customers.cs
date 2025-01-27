using projec_visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class POS_APP
    {



        public void refreshCustomersTable()
        {
            var query2 = from c in db.Customers
                         select new
                         {
                             c.fname,
                             c.lname,
                             c.phone_number,
                         };

            materialDataTable2.DataSource = query2.ToList();
        }


        private void addCustomerBtn_Click(object sender, EventArgs e)
        {


            if (addFnameText.Text.Length > 45 || addLNameTxt.Text.Length > 45)
            {
                MessageBox.Show("Error! Please enter a name less than 45 characters.");
                return;
            }

            if (ContainsNonNumeric(addNumberTxt.Text))
            {
                MessageBox.Show("Error! Please enter only numbers in the start value");
                return;

            }

            var query = (from c in db.Customers
                         where ((c.fname == addFnameText.Text &&  c.lname == addLNameTxt.Text) || c.phone_number == addNumberTxt.Text)
                         select c).Count();

            if (query == 0)
            {


                Customer newCustomer = new Customer
                {
                    fname = addFnameText.Text,
                    lname = addLNameTxt.Text,
                    phone_number = addNumberTxt.Text,
                    time_added = getUnixTime(),


                };

                db.Customers.InsertOnSubmit(newCustomer);

                db.SubmitChanges();

                refreshCustomersTable();



            }
            else
            {
                MessageBox.Show("Error.Please enter a customer with unique name and phone number.");
                return;

            }





        }
    }
}
