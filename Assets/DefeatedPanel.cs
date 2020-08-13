using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedPanel : MonoBehaviour
{
    [SerializeField] GameObject defeatedPanel;

    public void SetDefeat()
    {
        defeatedPanel.SetActive(true);
    }
}
