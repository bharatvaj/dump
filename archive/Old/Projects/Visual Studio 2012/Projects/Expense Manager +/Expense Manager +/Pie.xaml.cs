using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Expense_Manager__
{
    /// <summary>
    /// Interaction logic for GaugeControl.xaml
    /// </summary>
    public partial class Pie : UserControl
    {
        public Pie()
        {
            InitializeComponent();
            Height = ActualHeight;
            Width = ActualWidth;
        }
        public double GaugeHeight
        {
            get { return grid.ActualHeight; }
            set { grid.Height = value; }
        }
        public double GaugeWidth
        {
            get { return grid.ActualWidth; }
            set { grid.Width = value; }
        }
        public double ProgressRing
        {
            get { return progressRing.EndAngle; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("The value should not be negative integer");
                }
                else if (value >= 0 && value <= 360)
                {
                    progressRing.EndAngle = value;
                }
                    //if( value > 360)
                else
                {
                    progressRing.EndAngle = value - 360;
                }
            }
        }
    }
}
