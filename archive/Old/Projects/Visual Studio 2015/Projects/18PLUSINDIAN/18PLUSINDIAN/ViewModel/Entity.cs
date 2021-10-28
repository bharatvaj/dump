using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace _18PLUSINDIAN.ViewModel
{
    public class Entity
    {
        public string Name{ get; set; }

        public Entity(string name,string cardType, Color gridColor)
        {
            Name = name;
            GridBrush = new SolidColorBrush(gridColor);
        }

        public Entity(string name, string cardType, SolidColorBrush gridBrush)
        {
            Name = name;
            GridBrush = gridBrush;
        }


        public SolidColorBrush GridBrush { get; set; }

        public double ItemWidth
        {
            get
            {
                return (Window.Current.Bounds.Width / 2.1);
            }
        }
        public double ItemHeight
        {
            get
            {
                return (Window.Current.Bounds.Width / 2.1);
            }
        }

        public double GridWidth
        {
            get
            {
                return (Window.Current.Bounds.Width / 2.1) - 10 ;
            }
        }
        public double GridHeight
        {
            get
            {
                return (Window.Current.Bounds.Width / 2.1) - 10;
            }
        }


    }
}
