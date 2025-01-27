using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class Linq_for_revesion
    {

        /*
                var currInvoiceRows = db.Invoice_Rows
                        .Where(p.I_ID == currInvoice.I_ID)
                        .OrderByDescending(p => p.P_ID)
                        .FirstOrDefault();


                var currInvoiceRows = from row in db.Invoice_Rows
                                      join productx in db.products on row.P_ID equals productx.P_ID
                                      select new
                                      {
                                          name = productx.name,
                                          unit_price = productx.price,
                                          qauntity = row.count,
                                          sub_total = productx.price * row.count,
                                          unit_size = productx.size
                                      };


                ////        linq with lambda
                ///
                var query1 = db.Customers.Select(p => new { name = p.fname + " " + p.lname, id = p.C_ID });

               var invoicesToUpdate = context.Invoices.Where(i => i.Amount < amountThreshold).ToList();

                            var invoiceSummaries = context.Invoices
                .Where(i => i.Amount < amountThreshold)
                .Select(i => new 
                {
                    i.InvoiceId,
                    i.InvoiceNumber,
                    i.Amount
                })
                .ToList();


        */










        /////////////           retrive one record only             ///////////////

/*
        var user = (from u in dc.Users
                    where u.UserName == usn
                    select u).FirstOrDefault();

*/










        /*  /////////  updaate sql statments with linq                  ///////
         *  


                    var query = from r in db.Invoice_Rows
                        where r.I_ID == currInvoice.I_ID
                        select r;
            foreach (var row in query)
            {
                row.isfinished = true;
            }


                    db.SubmitChanges();







         *  
         *  /
         *  
  
        









        
        /*  /////////  updaate sql statments with linq                  ///////
         *  

            var total = (from rowx in context.Invoice_Rows
                         join productx in context.products on rowx.P_ID equals productx.P_ID
                         where rowx.I_ID == currInvoiceId && !rowx.isfinished
                         select rowx.cub_total).Sum();


                    var total = context.Invoice_Rows
                .Join(context.products,
                      rowx => rowx.P_ID,
                      productx => productx.P_ID,
                      (rowx, productx) => new { rowx, productx })
                .Where(x => x.rowx.I_ID == currInvoiceId && !x.rowx.isfinished)
                .Sum(x => x.rowx.cub_total);



         *  
         *  /










        /*             ///   to use linq with combobox containing id and strings
         * 
         *             List<ComboBoxItem> l = new List<ComboBoxItem>(); 
            foreach (var item in query1)
            {
                l.Add(new ComboBoxItem(item.id, item.name));
            }    

            //CustomersList  = query1.ToList();
            CustomerTerminalComboBox1.DataSource = l;
            CustomerTerminalComboBox1.DisplayMember = "Text";
            CustomerTerminalComboBox1.ValueMember = "Value";

                //getting id
                 newProduct.C_ID = ( categoriesComboBox1.SelectedItem as ComboBoxItem).Value


        */

    }
}
