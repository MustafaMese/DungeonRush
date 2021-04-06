using DungeonRush.UI.HUD;
using UnityEngine;

namespace DungeonRush.Customization
{
    public class EnemyCustomization : MonoBehaviour, ICustomization
    {
        [SerializeField] CharacterHUD characterHUD = null;
        
        [SerializeField] SpriteRenderer[] sprites;
        [SerializeField] GameObject skin;

        private void ChangeLayer(SpriteRenderer sR, bool top, int multiplier = 1)
        {
            if (top)
                sR.sortingOrder += 6 * multiplier;
            else
                sR.sortingOrder -= 6 * multiplier;
        }

        private void ChangeLayer(CharacterHUD c, bool top, int multiplier = 1)
        {
            if (top)
            {
                c.BarBG.sortingOrder += 6 * multiplier;
                c.BarSprite.sortingOrder += 6 * multiplier;
                c.GetName().sortingOrder += 6 * multiplier;
            }
            else
            {
                c.BarBG.sortingOrder -= 6 * multiplier;
                c.BarSprite.sortingOrder -= 6 * multiplier;
                c.GetName().sortingOrder -= 6 * multiplier;
            }
        }

        public void ChangeLayer(bool top, int multiplier = 1)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                ChangeLayer(sprites[i], top, multiplier);
            }
            if (characterHUD != null)
                ChangeLayer(characterHUD, top, multiplier);
        }

        private void HUDControl(bool state)
        {
            characterHUD.gameObject.SetActive(!state);
        }

        public void ChangeSkinState(bool state)
        {
            skin.SetActive(state);
            HUDControl(!state);
        }
    }
}