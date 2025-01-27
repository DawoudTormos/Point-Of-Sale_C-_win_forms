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


        ////////////                AddProduct tab varaibles                    ///////////
        List<category> catsList = new List<category> { };
        int catSelectedID = 0;





        ///////////                 Add Product Tab               ///////


        private void fillCategories(object sender, EventArgs e)
        {
            fillCategories();
        }

        private void categoriesComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }







        private void fillCategories()
        {// filling the combobox of categories at add profuct page
            var query = from category in db.categories
                        select new
                        {
                            category.name,
                            category.Cat_ID,

                        };

            catsList.Clear();



            int i = 0;
            /*foreach (var x in query)
            {
                catsList.Add(new category { Cat_ID = x.Cat_ID, name = x.name });
                i++;
            }*/

            List<ComboBoxItem> l = new List<ComboBoxItem>(); /// A custome class by me
            List<ComboBoxItem> l2 = new List<ComboBoxItem>(); /// A custome class by me
            foreach (var item in query)
            {
                if(item.Cat_ID == 1) {  }
                l.Add(new ComboBoxItem(item.Cat_ID, item.name));
                l2.Add(new ComboBoxItem(item.Cat_ID, item.name));
            }


            categoriesComboBox1.DataSource = l;

            categoriesComboBox1.DisplayMember = "Text";
            categoriesComboBox1.ValueMember = "Value";


            inventorytCategoriesComboBox1.DataSource = l2;

            inventorytCategoriesComboBox1.DisplayMember = "Text";
            inventorytCategoriesComboBox1.ValueMember = "Value";


        }










        private void addProductBtn_Click(object sender, EventArgs e)
        {
            /*MaterialTextBox2*/
            Control[] txtboxs = new Control[] { AddProduct_Name_Txt, AddProduct_Size_Txt, AddProduct_Price_Txt, AddProduct_Barcode_Txt, AddProduct_Description_Txt , qauntityTxt };
            string[] txts = new string[] { "Product Name", "Size", "Price", "Barcode", "Description" , "Initial Qauntity" };

            int i = 0;
            foreach (Control control in txtboxs)
            {
                if (control.Text == "")
                {
                    MessageBox.Show("Can't add product! No Value entered at " + txts[i] + ".");
                    return;
                }
                i++;
            }


            i = 0;
            foreach (Control control in txtboxs)
            {

                if (control.Text.Length > 45)
                {
                    if (i == 4 & control.Text.Length < 1000) // allow up to 1000 at description
                    {
                        continue;
                    }
                    MessageBox.Show("Can't add product! Value length So big at " + txts[i] + "."); /// database can't take in more 50 for some 
                    return;
                }
                i++;
            }


            i = 0;
            foreach (Control control in txtboxs)
            {

                if (ContainsNonNumeric(control.Text) &  (  (i > 0 & i < 3)|| i==5  )  )  //check for non-numric in 1,2 and 5
                {

                    MessageBox.Show("Can't add product! The value at " + txts[i] + " should be only numbers."); /// an int value
                    return;
                }
                i++;
            }





            product newProduct = new product();
            newProduct.name = txtboxs[0].Text;
            newProduct.size = Int32.Parse(txtboxs[1].Text);
            newProduct.price = Int32.Parse(txtboxs[2].Text);
            newProduct.barcode = txtboxs[3].Text;
            newProduct.description = txtboxs[4].Text;
            newProduct.qauntity = Convert.ToInt32(txtboxs[5].Text);
            //newProduct.C_ID = 1;// categoriesComboBox1.SelectedItem.Cat_ID;
            // Access the hidden ID
            newProduct.C_ID = ( categoriesComboBox1.SelectedItem as ComboBoxItem).Value;






            var checkProduct = db.products
                             .Where(p => p.barcode == newProduct.barcode || p.name == newProduct.name)
                            .OrderByDescending(p => p.P_ID)
                            .FirstOrDefault();

            if (checkProduct == null)
            {

                //Add new Employee to database
                db.products.InsertOnSubmit(newProduct);

                //Save changes to Database.
                db.SubmitChanges();

            }
            else { MessageBox.Show("Please enter only a new barcode and new name. The barcode or name u are trying to enter is alraedyb used with a product."); return; }

            var insertedEmployee2 = db.products
                                        .Where(p => p.name == txtboxs[0].Text)
                                       .OrderByDescending(p => p.P_ID)
                                       .FirstOrDefault();




            if (insertedEmployee2 != null)
            {
                foreach (Control control in txtboxs) { control.Text = ""; }
                MessageBox.Show("Product added successfully");

            }
            else
            {
                MessageBox.Show("Product wasn't added successfully. There was an error");

            }






        }


    }
}
