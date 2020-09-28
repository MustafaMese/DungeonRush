using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.UI
{
    public class ActiveSkillCanvas : MonoBehaviour, ICanvasController
    {
        [SerializeField] GameObject panel;

        public void PanelControl(bool activate)
        {
            panel.SetActive(activate);
        }
    }
}
