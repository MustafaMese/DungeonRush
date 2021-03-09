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

        private ObjectPool pool = new ObjectPool();

        private void Start()
        {
            FillThePool(pool, textPopup, 3);
        }

        void FillThePool(ObjectPool pool, TextPopup textPopup, int objectCount)
        {
            pool.SetObject(textPopup.gameObject);
            pool.Fill(objectCount, transform);
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
            TextPopup obj = pool.Pull(transform).GetComponent<TextPopup>();
            obj.gameObject.SetActive(true);
            obj.transform.position = tPos;
            obj.Setup(text, tPos);
            float t = obj.GetDisapperTime();
            yield return new WaitForSeconds(t);
            obj.gameObject.SetActive(false);
            pool.Push(obj.gameObject);
        }

        private IEnumerator StartTextPopup(Vector3 tPos, int damage, bool isCritical = false)
        {
            TextPopup obj = pool.Pull(transform).GetComponent<TextPopup>();
            obj.gameObject.SetActive(true);
            obj.transform.position = tPos;
            obj.Setup(damage, tPos, isCritical);
            float t = obj.GetDisapperTime();
            yield return new WaitForSeconds(t);
            obj.gameObject.SetActive(false);
            pool.Push(obj.gameObject);
        }

        public void DeleteObjectsInPool(ObjectPool pool)
        {
            for (int i = 0; i < pool.GetLength(); i++)
            {
                GameObject obj = pool.Pop();
                if (obj != null)
                    Destroy(obj);
            }
        }
    }
}