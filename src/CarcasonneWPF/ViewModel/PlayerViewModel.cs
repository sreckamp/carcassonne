using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Carcassonne.Model;
using GameBase.Model;
using MeepleColor = Carcassonne.Model.Color;

namespace Carcassonne.WPF.ViewModel
{
    public class PlayerViewModel : IMeepleViewDataContext
    {
        private static readonly Brush STransparent = new SolidColorBrush(Colors.Transparent);
        internal static readonly Dictionary<MeepleColor, Brush> BrushForColor = new Dictionary<MeepleColor, Brush>();

        static PlayerViewModel()
        {
            STransparent.Freeze();
            foreach (var mc in Enum.GetValues(typeof(MeepleColor)).Cast<MeepleColor>())
            {
                AddColor(mc);
            }
        }

        private static void AddColor(MeepleColor mc)
        {
            BrushForColor[mc] = new SolidColorBrush(mc switch
            {
                MeepleColor.None => Colors.Cyan,
                MeepleColor.Red => Colors.Red,
                MeepleColor.Green => Colors.Green,
                MeepleColor.Yellow => Colors.Yellow,
                MeepleColor.Blue => Colors.Blue,
                MeepleColor.Black => Colors.Black,
                MeepleColor.Pink => Colors.DeepPink,
                MeepleColor.Orange => Colors.DarkOrange,
                MeepleColor.Gray => Colors.SlateGray,
                _ => Colors.Transparent
            });
            BrushForColor[mc].Freeze();
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

        public Brush Color => BrushForColor[m_player.Color];
        public string Name => m_player.Name;
        public int Score => m_player.Score;
    }
}
