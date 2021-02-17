using DungeonRush.Managers;
using DungeonRush.Controller;

namespace DungeonRush.Cards
{
    public class PortalEventCard : EventCard
    {
        public override void GetEvent(Card card)
        {
            Portal();
        }

        private void Portal()
        {
            GameManager.Instance.SetGameState(GameState.REWARD);
        }
    }
}