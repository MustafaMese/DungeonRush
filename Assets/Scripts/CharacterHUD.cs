using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterHUD : MonoBehaviour
{
    [SerializeField] Gradient gradient = null;
    [SerializeField] float maxHealth = 0f;
    [SerializeField] Transform bar;
    [SerializeField] SpriteRenderer barBG;
    [SerializeField] SpriteRenderer barSprite;
    [SerializeField] GameObject statusPanel;
    [SerializeField] SpriteRenderer statusIconPrefab;
    [SerializeField] TextMeshPro nameText;

    public SpriteRenderer BarBG { get => barBG; }
    public SpriteRenderer BarSprite { get => barSprite; }

    private void HealthChange(float healthValue)
    {
        float amount = (healthValue / maxHealth);
        bar.localScale = new Vector2(amount, 1f);
    }

    private void ColorChange(float healthValue)
    {
        float amount = healthValue / maxHealth;
        Color c = gradient.Evaluate(amount);
        BarSprite.color = c;
    }

    public void ActiveChanges(float healthValue, float maxHealth)
    {
        SetMaxHealth(maxHealth);
        HealthChange(healthValue);
        ColorChange(healthValue);
    }

    public GameObject AddImageToPanel(Sprite sprite)
    {
        SpriteRenderer sR = Instantiate(statusIconPrefab, statusPanel.transform);
        sR.sprite = sprite;
        return sR.gameObject;
    }

    public void SetMaxHealth(float h)
    {
        maxHealth = h;
    }

    public TextMeshPro GetName()
    {
        return nameText;
    }
}
