using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/InfoText")]
public class InfoText : ScriptableObject
{
    public List<string> texts = new List<string>();

    public string GetRandom()
    {
        int number = Random.Range(0, texts.Count);
        string text = texts[number];
        return text;
    }
}
