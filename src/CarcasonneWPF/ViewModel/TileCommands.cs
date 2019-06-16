using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Carcassonne.WPF.ViewModel
{
    public static class TileCommands
    {
        public static readonly RoutedCommand Rotate = new RoutedUICommand("Rotate", "Rotate", typeof(TileCommands));
        public static readonly RoutedCommand Select = new RoutedUICommand("Select", "Select", typeof(TileCommands));
    }
}
