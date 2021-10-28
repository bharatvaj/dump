using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Expense_Manager__
{
    class Expenses
    {
    public string NAME { get; set; }
    public int AMOUNT { get; set; }
    public string DATE {get; set;}
    public Button TRANSTYPE { get; set; }


    public Expenses(string line)
    {
        string[] parts = line.Split(',');
        
        this.NAME = parts[0];
        
        this.AMOUNT = int.Parse(parts[1]);

        if (parts[2] == "0")
            this.TRANSTYPE = new Button{Content="Expense"};
        if (parts[2] == "1")
            this.TRANSTYPE = new Button { Content = "Income" };
        this.DATE = parts[3];

    }

    }
}
