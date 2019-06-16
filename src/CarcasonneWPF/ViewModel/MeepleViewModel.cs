using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Carcassonne.Model;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows;

namespace Carcassonne.WPF.ViewModel
{
    public class MeepleViewModel: INotifyPropertyChanged
    {
        protected readonly Dispatcher m_dispatcher;
        protected readonly Meeple m_meeple;

        public MeepleViewModel(Meeple meeple)
        {
            m_dispatcher = Application.Current.Dispatcher;
            m_meeple = meeple;
        }

        public Brush Fill { get { return PlayerViewModel.ColorsForName[m_meeple.Player.Name]; } }
        public Brush Stroke { get { return new SolidColorBrush(Colors.Transparent); } }
        public Visibility MeepleVisibility
        {
            get
            {
                if (m_meeple.Type == MeepleType.Meeple)
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
                if (m_meeple.Type == MeepleType.Abbot)
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
            if (m_dispatcher.CheckAccess())
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            else
            {
                m_dispatcher.Invoke(new Action<string>((n) =>
                { NotifyPropertyChanged(n); }), name);
            }
        }

        #endregion
    }
}
