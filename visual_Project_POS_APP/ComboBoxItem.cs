using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ComboBoxItem
    {

        public ComboBoxItem(long Value , string Text) { /// value willmost likely be the id in the db
            this.Value = Value;
            this.Text = Text;   
        }
        public string Text { get; set; }
        public long Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
