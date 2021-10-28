using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Manager__
{
    class Expenses
    {
    public string NAME { get; set; }
    public int AMOUNT { get; set; }


    public Expenses(string line)
    {
        string[] parts = line.Split(',');
        this.NAME = parts[0];
        this.AMOUNT = int.Parse(parts[1]);
    }

    }
}
