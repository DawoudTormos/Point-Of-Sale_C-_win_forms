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




        public void refreshBalancesTable()
        {

            var currBalance = from b in db.Balances
                                   where b.isActive == false
                                   select new
                                   {
                                       Name = b.name,
                                       Start_Value = b.startValue,
                                       End_Value = b.endValue,
                                       start_time = UnixTimeStampToDateTime((long)b.start_time).ToString(),
                                       end_time = parseTime((long)b.end_time)
                                   };

            BalancesDataTable2.DataSource = currBalance.ToList();


        }

        private string parseTime(long start)
        {
            return ((long)start == 0) ? "x" : UnixTimeStampToDateTime((long)start).ToString();

        }









        public void endBalance()
        {

            Balance currBalance = (from b in db.Balances
                                   where b.isActive == true
                                   select b).FirstOrDefault();


            var sum = (from r in db.Invoice_Rows
                       join i in db.invoices on r.I_ID equals i.I_ID
                       join p in db.products on r.P_ID equals p.P_ID
                       where i.B_ID == currBalance.B_ID && i.isfinished == true
                       select (p.price * r.count)).Sum();

            if (sum== null)
            {
                sum = 0;
            }

            currBalance.isActive = false;
            currBalance.endValue = currBalance.startValue + sum;
            currBalance.end_time = getUnixTime();

            db.SubmitChanges();
            
            checkForAnActiveBalance();

        }

        public void checkForAnActiveBalance()
        {
            refreshBalancesTable();

            int count = (from b in db.Balances
                               where b.isActive == true
                               select b).Count();



            if (count == 1)
            {
                Balance currBalance = (from b in db.Balances
                                       where b.isActive == true
                                       select b).FirstOrDefault();



                System.DateTime startDate = UnixTimeStampToDateTime((long)currBalance.start_time);
                currBalanceStartDate.Text = startDate.ToString();



                currBalanceStartValue.Text = currBalance.startValue.ToString() + " $";



                var sum = (from r in db.Invoice_Rows
                           join i in db.invoices on r.I_ID equals i.I_ID
                           join p in db.products on r.P_ID equals p.P_ID
                           where i.B_ID == currBalance.B_ID && i.isfinished == true
                           select (p.price * r.count)).Sum();

                currBalanceCurrValue.Text = Convert.ToString(sum + currBalance.startValue) + " $";


                currBalanceName.Text = currBalance.name;


                endBalanceBtn.Enabled = true;
                startBalanceBtn.Enabled = false;
                newBalanceNameTxt.Enabled = false;
                barcodeTxt.Enabled = true;
                newBalanceStartValue.Enabled = false;
                finishBtn.Enabled = true;


                newBalanceNameTxt.Text = "";
                newBalanceStartValue.Text = "";


                currInvoice.B_ID = currBalance.B_ID;
                db.SubmitChanges();


            }
            else
            {
                endBalanceBtn.Enabled = false;
                startBalanceBtn.Enabled = true;
                newBalanceNameTxt.Enabled = true;
                barcodeTxt.Enabled = false;
                newBalanceStartValue.Enabled = true;
                finishBtn.Enabled = false;




                currBalanceStartValue.Text = "";
                currBalanceCurrValue.Text = "";
                currBalanceName.Text = "";
                currBalanceStartDate.Text = "";
            }




        }






        public void createNewbalance() 
        {


            if (newBalanceNameTxt.Text.Length > 45)
            {
                MessageBox.Show("Error! Please enter a name less than 45 characters.");
                return;
            }

            if(ContainsNonNumeric(newBalanceStartValue.Text))
            {
                MessageBox.Show("Error! Please enter only numbers in the start value");
                return;

            }

            var query = (from b in db.Balances
                        where b.isActive == true
                        select b).Count();

            if (query == 0)
            {


                Balance newBalance = new Balance
                {
                    name = newBalanceNameTxt.Text,
                    start_time = (int)getUnixTime(),
                    startValue = Convert.ToInt32(newBalanceStartValue.Text),
                    endValue = 0,
                    end_time = 0,
                    isActive = true

                };

                db.Balances.InsertOnSubmit(newBalance);

                db.SubmitChanges();
            }
            else
            {
                MessageBox.Show("Internal Error! Please restart the program and contact developer if error continues.");
                return;

            }



            checkForAnActiveBalance();



        }






        /////////               eventlistenser              ///////////
        private void startBalanceBtn_Click(object sender, EventArgs e)
        {
            createNewbalance();
        }

        private void endBalanceBtn_Click(object sender, EventArgs e)
        {
            endBalance();
        }





        private void balanceTab_Enter(object sender, EventArgs e)
        {
            checkForAnActiveBalance();

        }






    }
}
