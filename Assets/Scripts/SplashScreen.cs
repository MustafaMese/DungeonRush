using DungeonRush.Managers;
using System.Collections;
using UnityEngine;
using TMPro;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] InfoText info;

    void Start()
    {
        infoText.text = info.GetRandom();
        StartCoroutine(Next());
    }

    private IEnumerator Next()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.SetGameState(GameState.LEVEL_TRANSITION);
    }
}
