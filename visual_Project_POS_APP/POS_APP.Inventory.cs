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








        private void materialTabControl1_Enter(object sender, EventArgs e)
        {
            fillCategories();
        }


        private void refreshBtn_Click(object sender, EventArgs e)
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
                            product.qauntity,
                            product.size
                        };



            materialDataTable1.DataSource = query.ToList();

        }

        private void materialButton2_Click(object sender, EventArgs e)
        {

            var query = from product in db.products
                        join category in db.categories on product.C_ID equals category.Cat_ID
                        where product.C_ID == (inventorytCategoriesComboBox1.SelectedItem as ComboBoxItem).Value
                        select new
                        {
                            product.name,
                            product.barcode,
                            product.price,
                            Category = category.name,
                            product.description,
                            qauntity = product.qauntity,
                            product.size
                        };


            materialDataTable1.DataSource = query.ToList();

        }



    }
}
