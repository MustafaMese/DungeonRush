using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using System.Collections;
using UnityEngine;

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
        [SerializeField] bool dontKillEffect;

        private GameObject HUDImage;
        private int tempTurnCount;
        private ObjectPool<GameObject> effectPool;
        private StatusController statusController;
        private bool effectUsed; // Use this on dontKillEffects situtations.

        public int Power { get => power; }
        public StatusType StatusType { get => statusType; }
        
        public abstract void Execute(Card card, Tile tile = null);

        public virtual void Adjust()
        {
            Animate();
            if(isUsingTextPopup)
                TextPopupManager.Instance.TextPopup(transform.position, power.ToString());
            
            tempTurnCount--;
            if (tempTurnCount <= 0)
                StartCoroutine(KillStatus());
        }

        public virtual void Initialize(CharacterHUD canvas, StatusController statusController)
        {
            this.statusController = statusController;
            tempTurnCount = turnCount;
            effectPool = new ObjectPool<GameObject>();
            FillPool(effectPool, effect, 2);
            effectUsed = false;
            HUDImage = canvas.AddImageToPanel(icon);
        }

        private void FillPool(ObjectPool<GameObject> pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.FillPool(objectCount, transform);
        }

        protected void Animate()
        {
            if(!dontKillEffect)
                EffectOperator.Instance.Operate(effectPool, transform, transform.position, effectTime);
            else
            {
                if(!effectUsed)
                {
                    effectUsed = true;
                    EffectOperator.Instance.Operate(effectPool, transform, transform.position);
                }
            }
        }

        protected IEnumerator KillStatus()
        {
            statusController.Notify(this);
            yield return new WaitForSeconds(effectTime);
            Destroy(HUDImage.gameObject);
            Destroy(this.gameObject);
        }
    }

    
}