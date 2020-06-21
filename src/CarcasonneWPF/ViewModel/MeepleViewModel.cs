using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Carcassonne.Model;
using GameBase.WPF.ViewModel;

namespace Carcassonne.WPF.ViewModel
{
    public class MeepleViewModel: IMeepleViewDataContext, INotifyPropertyChanged
    {
        private IMeeple m_meeple = new Meeple(MeepleType.None, NopPlayer.Instance);

        public MeepleViewModel(IMeeple meeple)
        {
            PropertyChanged += (sender, args) => { };
            m_meeple = meeple;
            Stroke.Freeze();
        }

        public Brush Fill => PlayerViewModel.BrushForColor[m_meeple.Player.Color];
        public Brush Stroke => new SolidColorBrush(Colors.Transparent);

        public void SetRotationAngle(float value)
        {
            RotationAngle = value;
            NotifyPropertyChanged(nameof(RotationAngle));
        }

        public float RotationAngle { get; private set; }

        public Visibility MeepleVisibility => (m_meeple.Type == MeepleType.Meeple).ToVisibility();

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
