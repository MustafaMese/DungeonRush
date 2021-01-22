using DungeonRush.Data;
using DungeonRush.UI.HUD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Managers
{
    public class TextPopupManager : MonoBehaviour
    {
        private static TextPopupManager instance = null;
        // Game Instance Singleton
        public static TextPopupManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        private void Awake()
        {
            Instance = this;
        }


        [SerializeField] TextPopup textPopup = null;

        private ObjectPool<TextPopup> poolForTextPopup = new ObjectPool<TextPopup>();

        private void Start()
        {
            FillThePool(poolForTextPopup, textPopup, 3);
        }

        void FillThePool(ObjectPool<TextPopup> pool, TextPopup effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.FillPool(objectCount, transform);
        }

        public void TextPopup(Vector3 tPos, int damage, bool isCritical = false)
        {
            StartCoroutine(StartTextPopup(tPos, damage, isCritical));
        }

        public void TextPopup(Vector3 tPos, string text)
        {
            StartCoroutine(StartTextPopup(tPos, text));
        }

        private IEnumerator StartTextPopup(Vector3 tPos, string text)
        {
            TextPopup obj = poolForTextPopup.Pull(transform);
            obj.gameObject.SetActive(true);
            obj.transform.position = tPos;
            obj.Setup(text, tPos);
            float t = obj.GetDisapperTime();
            yield return new WaitForSeconds(t);
            obj.gameObject.SetActive(false);
            poolForTextPopup.AddObjectToPool(obj);
        }

        private IEnumerator StartTextPopup(Vector3 tPos, int damage, bool isCritical = false)
        {
            TextPopup obj = poolForTextPopup.Pull(transform);
            obj.gameObject.SetActive(true);
            obj.transform.position = tPos;
            obj.Setup(damage, tPos, isCritical);
            float t = obj.GetDisapperTime();
            yield return new WaitForSeconds(t);
            obj.gameObject.SetActive(false);
            poolForTextPopup.AddObjectToPool(obj);
        }

        public void DeleteObjectsInPool(ObjectPool<GameObject> pool)
        {
            for (int i = 0; i < pool.GetStackLength(); i++)
            {
                GameObject obj = pool.PullForDestroy();
                if (obj != null)
                    Destroy(obj);
                else
                    return;
            }
        }
    }
}