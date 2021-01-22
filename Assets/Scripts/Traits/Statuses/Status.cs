using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Managers;
using DungeonRush.UI.HUD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.Traits
{
    public abstract class Status : MonoBehaviour, IStatus
    {
        [SerializeField] StatusType statusType;
        [SerializeField] int power;
        [SerializeField] int turnCount;
        [SerializeField] float effectTime;
        [SerializeField] GameObject effect;
        [SerializeField] Sprite icon;
        [SerializeField] bool isUsingTextPopup;

        private Image canvasImage;
        private int tempTurnCount;
        private ObjectPool<GameObject> effectPool;
        private StatusController statusController;

        public int Power { get => power; }
        public StatusType StatusType { get => statusType; }
        
        public abstract void Execute(Card card);

        public virtual void Adjust()
        {
            Animate();
            if(isUsingTextPopup)
                TextPopupManager.Instance.TextPopup(transform.position, power.ToString());
            
            tempTurnCount--;
            print(tempTurnCount);
            if (tempTurnCount <= 0)
                StartCoroutine(KillStatus());
        }

        public virtual void Initialize(CharacterCanvas canvas, StatusController statusController)
        {
            this.statusController = statusController;
            tempTurnCount = turnCount;
            effectPool = new ObjectPool<GameObject>();
            FillPool(effectPool, effect, 2);
            canvasImage = canvas.AddImageToPanel(icon);
        }

        private void FillPool(ObjectPool<GameObject> pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.FillPool(objectCount, transform);
        }

        protected void Animate()
        {
            EffectOperator.Instance.Operate(effectPool, transform.position, effectTime);
        }

        protected IEnumerator KillStatus()
        {
            statusController.Notify(this);
            yield return new WaitForSeconds(effectTime);
            Destroy(canvasImage.gameObject);
            Destroy(this.gameObject);
        }
    }

    
}