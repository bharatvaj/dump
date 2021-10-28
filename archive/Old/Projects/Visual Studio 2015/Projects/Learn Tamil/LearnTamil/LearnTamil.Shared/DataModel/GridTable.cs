using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace LearnTamil.DataModel
{
    public class GridTable
    {
        public Grid Table
        {
            get; set;
        }
        
        public GridTable(Grid Table)
        {
            this.Table = Table;
        }
    }
}
