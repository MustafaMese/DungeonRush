using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Customization
{
    public class FlyingCreatureCustomization : MonoBehaviour, ICustomization
    {
        [SerializeField] SpriteRenderer rightLeg = null;
        [SerializeField] SpriteRenderer leftLeg = null;
        [SerializeField] SpriteRenderer leftArm = null;
        [SerializeField] SpriteRenderer rightArm = null;
        [SerializeField] SpriteRenderer body = null;

        [SerializeField] Material shadow = null;
        [SerializeField] Material lighted = null;

        public int oldLayer = 0;

        private void Start()
        {
            oldLayer = (int)transform.position.y;
        }

        public void Change()
        {
            throw new System.NotImplementedException();
        }

        public void OverShadow()
        {

            if (leftArm != null)
                leftArm.material = shadow;

            if (rightArm != null)
                rightArm.material = shadow;

            if (leftLeg != null)
                leftLeg.material = shadow;

            if (rightLeg != null)
                rightLeg.material = shadow;

            if (body != null)
                body.material = shadow;
        }

        public void RemoveShadow()
        {

            if (leftArm != null)
                leftArm.material = lighted;

            if (rightArm != null)
                rightArm.material = lighted;

            if (leftLeg != null)
                leftLeg.material = lighted;

            if (rightLeg != null)
                rightLeg.material = lighted;

            if (body != null)
                body.material = lighted;

        }
    }
}