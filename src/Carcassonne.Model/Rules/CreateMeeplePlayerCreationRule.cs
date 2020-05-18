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

        public void UpdatePlayer(IPlayer player)
        {
            for (var i = 0; i < m_count; i++)
            {
                player.ReturnMeeple(new Meeple_(m_type, player));
            }
        }
    }
}
