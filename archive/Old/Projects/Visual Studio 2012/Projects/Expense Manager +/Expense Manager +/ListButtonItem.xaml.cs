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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Expense_Manager__
{
    /// <summary>
    /// Interaction logic for ListButtonItem.xaml
    /// </summary>
    public partial class ListButtonItem : UserControl
    {
        public ListButtonItem()
        {
            InitializeComponent();
        }
        public string Image
        {
            get { return ImageText.Content.ToString(); }
            set { ImageText.Content = value; }
        }
        public string Text
        {
            get { return ContentText.Content.ToString(); }
            set { ContentText.Content = value; }
        }
    }
}
