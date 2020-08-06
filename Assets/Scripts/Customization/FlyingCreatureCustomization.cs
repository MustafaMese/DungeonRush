using DungeonRush.Cards;
using System;
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

        private string r = "Row ";

        private void ChangeLayer(SpriteRenderer sR, int layer)
        {
            string sth = String.Concat(r, layer);
            sR.sortingLayerName = sth;
        }

        public void Change(float posY)
        {
            int layer = (int)Math.Truncate(posY);

            if (leftArm != null)
                ChangeLayer(leftArm, layer);

            if (rightArm != null)
                ChangeLayer(rightArm, layer);

            if (leftLeg != null)
                ChangeLayer(leftLeg, layer);

            if (rightLeg != null)
                ChangeLayer(rightLeg, layer);

            if (body != null)
                ChangeLayer(body, layer);
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