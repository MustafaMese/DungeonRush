using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DungeonRush.Managers;
using DungeonRush.Saving;
using System;

namespace DungeonRush.UI
{
    public class StartCanvas : MonoBehaviour
    {
        public enum MenuPanelState { SHOPPING, START, CHARACTER }
        public MenuPanelState currentState = MenuPanelState.START;

        [Serializable]
        public class Panel
        {
            public GameObject obj;
            public int listnumber;
            public MenuPanelState state;
            public Sprite image;

            public Panel(GameObject panel, int listnumber, MenuPanelState state, Sprite image)
            {
                this.obj = panel;
                this.listnumber = listnumber;
                this.state = state;
                this.image = image;
            }
        }

        [SerializeField] GameObject startingPanel = null;
        [SerializeField] GameObject shoppingPanel = null;
        [SerializeField] GameObject characterPanel = null;

        [SerializeField] Image marker = null;
        [SerializeField] Sprite startImage = null;
        [SerializeField] Sprite shopImage = null;
        [SerializeField] Sprite characterImage = null;

        [SerializeField] List<Panel> panels = new List<Panel>();

        [SerializeField] float endValue = 0f;
        [SerializeField] float endTime = 0f;
        private bool touched = false;

        private void Start()
        {
            DOTween.Init();

            Panel p;
            p = new Panel(characterPanel, 0, MenuPanelState.CHARACTER, characterImage);
            panels.Add(p);
            
            p = new Panel(startingPanel, 1, MenuPanelState.START, startImage);
            panels.Add(p);

            p = new Panel(shoppingPanel, 2, MenuPanelState.SHOPPING, shopImage);
            panels.Add(p);
        }

        private void Update()
        {
            if(!touched && SwipeManager.swipeDirection != Swipe.NONE)
            {
                touched = true;
                ChagePanelState(SwipeManager.swipeDirection);
            }
        }

        private void ChagePanelState(Swipe swipe)
        {
            Panel p = GetPanel(currentState);

            if (p == null)
            {
                touched = false;
                return;
            }

            Vector3 distance = GetDistance(swipe, p);

            if (distance != Vector3.zero)
            {
                Invoke("SetTouchedFalse", 0.6f);
                for (int i = 0; i < panels.Count; i++)
                {
                    GameObject obj = panels[i].obj;

                    obj.transform.DOMove(obj.transform.position + distance, 0.5f);
                }
            }
            else
                touched = false;

           
        }

        private void SetTouchedFalse()
        {
            touched = false;
        }

        private Panel GetPanel(MenuPanelState currentState)
        {
            foreach (var p in panels)
            {
                if (p.state == currentState)
                    return p;
            }
            return null;
        }

        private Panel GetPanel(int listnumber)
        {
            foreach (var p in panels)
            {
                if (p.listnumber == listnumber)
                    return p;
            }
            return null;
        }

        private Vector3 GetDistance(Swipe swipe, Panel panel)
        {
            int number = 0;
            if (swipe == Swipe.LEFT)
                number = 1;
            else if (swipe == Swipe.RIGHT)
                number = -1;
            else
                return Vector3.zero;

            number += panel.listnumber;

            Panel p = GetPanel(number);
            
            if(p != null)
            {
                Vector3 v1 = p.obj.transform.position;
                Vector3 v2 = panel.obj.transform.position;
                Vector3 distance = Distance(v1, v2);

                currentState = p.state;
                marker.sprite = p.image;

                return distance;
            }
            return Vector3.zero;
        }

        private static Vector3 Distance(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                  v1.x - v2.x,
                  v1.y - v2.y,
                  v1.z - v2.z);
        }

        public void ExecuteStartButton()
        {
            if (!touched)
            {
                touched = true;
                StartCoroutine(Next());
            }
        }

        private IEnumerator Next()
        {
            SavingSystem.DeletePlayerInstantSaveFile();
            yield return new WaitForSeconds(endTime);
            GameManager.Instance.SetGameState(GameState.LEVEL_TRANSITION);
        }
    }
}