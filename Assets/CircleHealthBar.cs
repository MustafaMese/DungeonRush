using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleHealthBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] float appearanceTime = 0f;

    private void Start()
    {
        bar.gameObject.SetActive(false);
    }

    private void HealthChange(float healthValue)
    {
        float amount = (healthValue / 100.0f);
        bar.fillAmount = amount;
    }

    private void ColorChange(float healthValue)
    {
        float amount = (healthValue / 255.0f);
        Color c = bar.color;
        c.g = amount;
        bar.color = c;
    }

    public IEnumerator ActiveChanges(float healthValue)
    {
        bar.gameObject.SetActive(true);
        HealthChange(healthValue);
        ColorChange(healthValue);
        yield return new WaitForSeconds(appearanceTime);
        bar.gameObject.SetActive(false);
    }
}
