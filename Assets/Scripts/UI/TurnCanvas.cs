using DungeonRush.Cards;
using DungeonRush.Controller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnCanvas : MonoBehaviour
{
    private class UIImage
    {
        public Sprite image;
        public bool lastOne;

        public UIImage(Sprite i, bool l)
        {
            image = i;
            lastOne = l;
        }
    }

    private const string playerTurnText = "Your turn!";
    private const string enemyTurnText = "Enemy turn!";

    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] Sprite playerIcon;
    [SerializeField] List<Image> images = new List<Image>();
    [SerializeField] List<UIImage> cards = new List<UIImage>();
    [SerializeField] CanvasGroup canvasGroup;

    private int activeIndex = 0;

    private void Start()
    {
        SetImageAsPlayerIcon();
    }

    private IEnumerator EnablePanel()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / 0.1f;
            yield return null;
        }
    }

    private IEnumerator DisablePanel()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 0.1f;
            yield return null;
        }
    }

    private void ChangeText(bool playerTurn)
    {
        if (playerTurn)
            textMesh.text = playerTurnText;
        else
            textMesh.text = enemyTurnText;
    }

    public IEnumerator ActivatePanel(bool playerTurn)
    {
        yield return DisablePanel();
        ChangeText(playerTurn);
        yield return EnablePanel();
    }

    public void SetCardIcons(List<AIController> aIs)
    {
        bool lastOne = false;
        activeIndex = 0;
        cards.Clear();
        for (int i = 0; i < aIs.Count; i++)
        {
            if (i + 1 == aIs.Count)
                lastOne = true;

            UIImage image = new UIImage(aIs[i].GetCard().GetCharacterIcon(), lastOne);
            cards.Add(image);
        }
    }

    public void SetImages(int index)
    {
        print("1");

        if (index > 0 && cards.Count > index - 1 && cards[index - 1].image != null)
            images[0].sprite = cards[index - 1].image;
        else
            images[0].sprite = playerIcon;

        if (cards.Count > index && cards[index].image != null)
        {
            print("2");
            images[1].sprite = cards[index].image;
            Debug.Log(images[1].sprite);
        }

        if (cards.Count > index + 1 && cards[index + 1].image != null)
            images[2].sprite = cards[index + 1].image;

        if(cards.Count > index && cards[index].lastOne)
            images[2].sprite = playerIcon;
    }

    public void Next()
    {
        activeIndex++;
        SetImages(activeIndex);
    }

    public void SetImageAsPlayerIcon()
    {
        if (cards.Count > 0)
            images[0].sprite = cards[cards.Count - 1].image;
        else
            images[0].sprite = null;

        images[1].sprite = playerIcon;
        images[2].sprite = null;
    }
}
