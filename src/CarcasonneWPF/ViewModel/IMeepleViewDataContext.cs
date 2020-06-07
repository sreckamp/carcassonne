using System.Windows;
using System.Windows.Media;

namespace Carcassonne.WPF.ViewModel
{
    public interface IMeepleViewDataContext
    {
        Brush Fill { get; }
        Brush Stroke { get; }
        Visibility MeepleVisibility { get; }
        Visibility AbbotVisibility  { get; }
    }
}