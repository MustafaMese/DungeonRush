using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.UI.HUD
{
public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image bar = null;
        [SerializeField] Gradient gradient = null;
        [SerializeField] float maxHealth = 0f;

        private void HealthChange(float healthValue)
        {
            float amount = (healthValue / maxHealth);
            bar.fillAmount = amount;
        }

        private void ColorChange(float healthValue)
        {
            float amount = healthValue / maxHealth;
            Color c = gradient.Evaluate(amount);
            bar.color = c;
        }

        public void ActiveChanges(float healthValue, float maxHealth)
        {
            SetMaxHealth(maxHealth);
            HealthChange(healthValue);
            ColorChange(healthValue);
        }

        public void SetMaxHealth(float h)
        {
            maxHealth = h;
        }
    }
}
