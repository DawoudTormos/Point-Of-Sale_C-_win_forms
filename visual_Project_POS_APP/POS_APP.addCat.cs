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


        public void addCatToDB(string cat)
        {
            category newcat = new category { name = cat };
          var checkCat = db.categories
                 .Where(p => p.name == cat)
                .OrderByDescending(p => p.Cat_ID)
                .FirstOrDefault();

            if (checkCat == null)
            {

                //Add new Employee to database
                db.categories.InsertOnSubmit(newcat);

                //Save changes to Database.
                db.SubmitChanges();
                MessageBox.Show("Category Added successfully!");
            }
            else { MessageBox.Show("This category already exists!"); return; }






        }  
        


        
        
        private void addCatBtn_Click(object sender, EventArgs e)
        {

            addCatToDB(catTxt.Text);
            fillCategories();


        }





    }
}
