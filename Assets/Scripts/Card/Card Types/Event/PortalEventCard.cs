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
            FindObjectOfType<PlayerController>().SavePlayer();
            GameManager.Instance.SetGameState(GameState.LEVEL_TRANSITION);
        }
    }
}