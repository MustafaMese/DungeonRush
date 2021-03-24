using System.Collections;
using System.Collections.Generic;
using DungeonRush.Traits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using DungeonRush.Saving;
using DungeonRush.Data;

public class AttributePanel : MonoBehaviour
{
    [SerializeField] StatData playerStat;
    [SerializeField] Button strButton;
    [SerializeField] Button agiButton;
    [SerializeField] Button luckButton;
    [SerializeField] TextMeshProUGUI attributesText;
    [SerializeField] TextMeshProUGUI levelPointText;
    [SerializeField] TextMeshProUGUI strPointText;
    [SerializeField] TextMeshProUGUI agiPointText;
    [SerializeField] TextMeshProUGUI luckPointText;
    [SerializeField] TextMeshProUGUI maxHealthText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI criticChanceText;
    [SerializeField] TextMeshProUGUI dodgeChanceText;
    [SerializeField] TextMeshProUGUI lootChanceText;

    private int attributePoint = 0;
    private int currentLevel;
    private int targetLevel;

    private int str;
    private int agi;
    private int luck;
    private int maxHP;
    private int damage;
    private int critic;
    private int dodge;
    private int lootChance;

    private void Start()
    {
        PlayerProperties properties = SavingSystem.LoadPlayerProperties();
        PlayerUtility utilities = SavingSystem.LoadUtilities();

        str = properties.str;
        agi = properties.agi;
        luck = properties.luck;

        currentLevel = str + agi + luck;
        targetLevel = utilities.CalculateLevel();
        attributePoint = Mathf.Abs(targetLevel - currentLevel);
        attributesText.text = attributePoint.ToString();

        Level();
        Strength();
        Agility();
        Luck();
    }

    private void Level()
    {
        levelPointText.text = targetLevel.ToString();
    }

    private void Update() 
    {
        if(attributePoint <= 0)
        {
            strButton.interactable = false;
            agiButton.interactable = false;
            luckButton.interactable = false;
        }    
    }

    public void Increase(Button button)
    {
        attributePoint--;
        attributesText.text = attributePoint.ToString();

        if(button == strButton)
        {
            str++;
            Strength();
        }
        else if(button == agiButton)
        {
            agi++;
            Agility();
        }
        else if(button == luckButton)
        {
            luck++;
            Luck();
        }

        Save();
    }

    private void Luck()
    {
        CalculateLuck();
        PrintLuck();
    }

    private void Agility()
    {
        CalculateAgi();
        PrintAgi();
    }

    private void Strength()
    {
        CalculateStr();
        PrintStr();
    }

    private void Save()
    {
        SavingSystem.SaveProperties(str, agi, luck);
    }

    private void CalculateStr()
    {
        PlayerProperties.CalculateStr(str, out maxHP, out damage);
    }

    private void PrintStr()
    {
        strPointText.text = str.ToString();
        maxHealthText.text = maxHP.ToString();
        damageText.text = damage.ToString();
    }

    private void CalculateAgi()
    {
        PlayerProperties.CalculateAgi(agi, out critic, out dodge);
    }

    private void PrintAgi()
    {
        agiPointText.text = agi.ToString();
        criticChanceText.text = critic.ToString();
        dodgeChanceText.text = dodge.ToString();
    }

    private void CalculateLuck()
    {
        PlayerProperties.CalculateLuck(luck, out lootChance);
    }

    private void PrintLuck()
    {
        luckPointText.text = luck.ToString();
        lootChanceText.text = lootChance.ToString();
    }
    
}
