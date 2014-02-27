using System.Linq;
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
