using System;
using System.Collections.Generic;
using System.Windows.Media;
using Carcassonne.Model;
using Carcassonne.Model.Expansions;
using Carcassonne.WPF.ViewModel;

namespace Carcassonne.WPF
{
    /// <summary>
    /// Interaction logic for CarcassonneWindow.xaml
    /// </summary>
    public partial class GameView
    {
        private readonly GameViewModel m_gameVm;

        public GameView()
        {
            var args = Environment.GetCommandLineArgs();
            var packs = new List<ExpansionPack>();
            foreach (var a in args)
            {
                switch (a.ToLower().Trim())
                {
                    case "abbot":
                        packs.Add(AbbotExpansion.Instance);
                        break;
                    case "river":
                        packs.Add(RiverExpansion.Instance);
                        break;
                    case "farmer":
                        packs.Add(FarmerExpansion.Instance);
                        break;
                }
            }

            m_gameVm = new GameViewModel(packs);
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = m_gameVm;
            m_gameVm.AddPlayer("Luke", Colors.Blue);
            m_gameVm.AddPlayer("Vader", Colors.Red);

            m_gameVm.Game.Start();
        }
    }
}
