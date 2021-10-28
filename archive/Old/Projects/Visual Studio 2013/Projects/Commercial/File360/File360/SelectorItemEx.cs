using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace File360
{
    public static class SelectorItemEx
    {
        public static bool GetToggleSelectedOnHold(SelectorItem item)
        {
            return (bool)item.GetValue(ToggleSelectedOnHoldProperty);
        }

        public static void SetToggleSelectedOnHold(SelectorItem item, bool value)
        {
            item.SetValue(ToggleSelectedOnHoldProperty, value);
        }

        public static readonly DependencyProperty ToggleSelectedOnHoldProperty =
            DependencyProperty.RegisterAttached(
              "ToggleSelectedOnHold",
              typeof(bool),
              typeof(SelectorItemEx),
              new PropertyMetadata(
                false,
                ToggleSelectedOnHoldChanged));

        private static void ToggleSelectedOnHoldChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var item = o as SelectorItem;
            if (item == null)
                return;

            var oldValue = (bool)e.OldValue;
            var newValue = (bool)e.NewValue;

            if (oldValue && !newValue)
            {
                item.Holding -= Item_Holding;
            }
            else if (newValue && !oldValue)
            {
                item.Holding += Item_Holding;
            }
        }

        private static void Item_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var item = sender as SelectorItem;
            if (item == null)
                return;


            if (e.HoldingState == HoldingState.Started)
                item.IsSelected = !item.IsSelected;
        }
    }
}