using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model.Rules
{
    public class CreateMeeplePlayerCreationRule : IPlayerCreationRule
    {
        private readonly int m_count;
        private readonly MeepleType m_type;

        public CreateMeeplePlayerCreationRule(int count, MeepleType type)
        {
            m_count = count;
            m_type = type;
        }

        public void UpdatePlayer(Player player)
        {
            player.CreateMeeple(m_count, m_type);
        }
    }
}
