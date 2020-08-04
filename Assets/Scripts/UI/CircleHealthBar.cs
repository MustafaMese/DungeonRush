using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleHealthBar : MonoBehaviour
{
    [SerializeField] Image bar = null;
    [SerializeField] Image barBG = null;
    [SerializeField] float appearanceTime = 0f;
    [SerializeField] Gradient gradient = null;
    [SerializeField] float maxHealth = 0f;

    private void Start()
    {
        bar.gameObject.SetActive(false);
    }

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

    public IEnumerator ActiveChanges(float healthValue, float maxHealth)
    {
        SetMaxHealth(maxHealth);
        bar.gameObject.SetActive(true);
        barBG.gameObject.SetActive(true);
        HealthChange(healthValue);
        ColorChange(healthValue);
        yield return new WaitForSeconds(appearanceTime);
        bar.gameObject.SetActive(false);
        barBG.gameObject.SetActive(false);
    }

    public void SetMaxHealth(float h)
    {
        maxHealth = h;
    }
}
