using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace File360
{
    class OccupyStack : StackPanel
    {
        public OccupyStack()
        {
            foreach (Button but in Children)
            {
                but.Height = ((Grid)Parent).ActualHeight;
                but.Width = ActualWidth / Children.Count;
            }
            SizeChanged += OccupyStack_SizeChanged;
        }

        private void OccupyStack_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            Width = Window.Current.Bounds.Width;
            UpdateLayout();
            foreach (Button but in Children)
            {
                but.Height = ((Grid)Parent).ActualHeight;
                but.Width = ActualWidth / Children.Count;
            }
        }
    }
}
