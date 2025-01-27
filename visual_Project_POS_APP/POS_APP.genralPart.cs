using projec_visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    partial class POS_APP
    {



        static bool ContainsNonNumeric(string text)
        {
            return !Regex.IsMatch(text, @"^[0-9]+$");
        }





        private static long getUnixTime() { return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;  }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }


        private void checkTables()
        {   
            if (db.Customers.Count() == 0)
            {

                Customer first = new Customer();
                first.C_ID = 0;
                first.fname = "Default";
                first.lname = "Customer";
                first.phone_number = "000000";
                first.time_added = getUnixTime();


                db.Customers.InsertOnSubmit(first);

                //Save changes to Database.
                db.SubmitChanges();


            }
        }

    }
}
