using System.Diagnostics;
using System.Windows.Threading;

namespace Carcassonne.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.ToString());
            Debug.WriteLine(e.Exception.StackTrace);
        }
    }
}
