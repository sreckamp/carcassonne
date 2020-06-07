using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Carcassonne.Model;
using GameBase.Model;

namespace Carcassonne.WPF.ViewModel
{
    public class PlayerViewModel : IMeepleViewDataContext
    {
        private static readonly Brush SDefaultColor = new SolidColorBrush(Colors.Fuchsia);
        private static readonly Brush STransparent = new SolidColorBrush(Colors.Transparent);
        internal static readonly Dictionary<string, Brush> ColorsForName = new Dictionary<string, Brush>();

        static PlayerViewModel()
        {
            SDefaultColor.Freeze();
            STransparent.Freeze();
        }
        private readonly IPlayer m_player;

        public PlayerViewModel(IPlayer player)
        {
            m_player = player;
            MeepleViewModels = new MappingCollection<MeepleViewModel, IMeeple>(player.Meeple);
        }

        public Brush Fill => Color;
        public Brush Stroke => STransparent;

        public Visibility MeepleVisibility => Visibility.Visible;

        public Visibility AbbotVisibility => Visibility.Hidden;

        public MappingCollection<MeepleViewModel, IMeeple> MeepleViewModels { get; }

        public Brush Color => ColorsForName.ContainsKey(m_player.Name) ? ColorsForName[m_player.Name] : SDefaultColor;
        public string Name => m_player.Name;
        public int Score => m_player.Score;
    }
}
