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
        [SerializeField] protected float effectTime;
        [SerializeField] protected GameObject effect;
        [SerializeField] Sprite icon;
        [SerializeField] bool isUsingTextPopup;
        [SerializeField] bool dontKillEffect;

        protected ObjectPool pool;
        protected bool effectUsed; // Use this on dontKillEffects situtations.

        private GameObject HUDImage;
        private int tempTurnCount;
        private StatusController statusController;
        
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
                StartCoroutine(Kill());
        }

        public virtual void Initialize(CharacterHUD canvas, StatusController statusController)
        {
            this.statusController = statusController;
            tempTurnCount = turnCount;
            pool = new ObjectPool();
            Fill(pool, effect, 2);
            effectUsed = false;
            HUDImage = canvas.AddImageToPanel(icon);
        }

        private void Fill(ObjectPool pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.Fill(objectCount, transform);
        }

        protected void Animate()
        {
            if(!dontKillEffect)
                EffectOperator.Instance.Operate(pool, transform, transform.position, effectTime);
            else
            {
                if(!effectUsed)
                {
                    effectUsed = true;
                    EffectOperator.Instance.Operate(pool, transform, transform.position);
                }
            }
        }

        protected virtual IEnumerator Kill()
        {
            statusController.Notify(this);
            yield return new WaitForSeconds(effectTime);
            Destroy(HUDImage.gameObject);
            Destroy(this.gameObject);
        }
    }

    
}