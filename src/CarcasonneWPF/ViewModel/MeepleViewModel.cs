using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Carcassonne.Model;
using GameBase.WPF.ViewModel;

namespace Carcassonne.WPF.ViewModel
{
    public class MeepleViewModel: INotifyPropertyChanged
    {
        private IMeeple m_meeple = new Meeple(MeepleType.None, NopPlayer.Instance);

        public MeepleViewModel(IMeeple meeple)
        {
            PropertyChanged += (sender, args) => { };
            m_meeple = meeple;
            Stroke.Freeze();
        }

        public Brush Fill => new SolidColorBrush(Colors.Chartreuse);// PlayerViewModel.ColorsForName[m_meeple.Player.Name];
        public Brush Stroke => new SolidColorBrush(Colors.Transparent);

        public Visibility MeepleVisibility => Visibility.Visible;// (m_meeple.Type == MeepleType.Meeple).ToVisibility();

        public Visibility AbbotVisibility => (m_meeple.Type == MeepleType.Abbot).ToVisibility();

        public void SetMeeple(Meeple meeple)
        {
            m_meeple = meeple;
            NotifyPropertyChanged(nameof(Fill));
            NotifyPropertyChanged(nameof(MeepleVisibility));
            NotifyPropertyChanged(nameof(AbbotVisibility));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
