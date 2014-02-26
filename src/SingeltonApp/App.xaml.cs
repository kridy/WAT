using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace SingeltonApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {            
        protected override void OnStartup(StartupEventArgs e)
        {
            var manager = new ApplicationService(this, e );       
     
            manager.TryStart(
                a => new MainWindow().Show(),
                b => b.Processes.First().Focus());           
        }
    }
}
