using System;
using System.ComponentModel;
using Carcassonne.Model;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows;

namespace Carcassonne.WPF.ViewModel
{
    public class MeepleViewModel: INotifyPropertyChanged
    {
        protected readonly Dispatcher Dispatcher;
        protected readonly Meeple Meeple;

        public MeepleViewModel(Meeple meeple)
        {
            Dispatcher = Application.Current.Dispatcher;
            Meeple = meeple;
        }

        public Brush Fill => PlayerViewModel.ColorsForName[Meeple.Player.Name];
        public Brush Stroke => new SolidColorBrush(Colors.Transparent);

        public Visibility MeepleVisibility
        {
            get
            {
                if (Meeple.Type == MeepleType.Meeple)
                {
                    return Visibility.Visible;
                }
                return Visibility.Hidden;
            }
        }

        public Visibility AbbotVisibility
        {
            get
            {
                if (Meeple.Type == MeepleType.Abbot)
                {
                    return Visibility.Visible;
                }
                return Visibility.Hidden;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (Dispatcher.CheckAccess())
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            else
            {
                Dispatcher.Invoke(new Action<string>((n) =>
                { NotifyPropertyChanged(n); }), name);
            }
        }

        #endregion
    }
}
