using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SingeltonApp
{
    public class WindowService
    {
        public static readonly DependencyProperty EscapeClosesWindowProperty = DependencyProperty.RegisterAttached(
            "EscapeClosesWindowProperty", 
            typeof(bool), 
            typeof(WindowService), 
            new FrameworkPropertyMetadata(
                false,
                OnEscapeClosesWindowChanged));

        public static bool GetEscapeClosesWindow(DependencyObject d)
        {
            return (bool) d.GetValue(EscapeClosesWindowProperty);
        }
        public static void SetEscapeClosesWindow(DependencyObject d, bool value)
        {
            d.SetValue(EscapeClosesWindowProperty, value);
        }

        private static void OnEscapeClosesWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (Window) d;
            var newVal = (bool) e.NewValue;
            
            if (target == null) return;

            if (newVal)
                target.KeyDown += PreviewKeyDown;  
          
            else
                target.KeyDown -= PreviewKeyDown;                
            
        }

        static void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var target = (Window) sender;
            if(e.Key == Key.Escape)
                target.Close();
        }
    }
}
