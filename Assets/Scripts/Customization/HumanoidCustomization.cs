using DungeonRush.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DungeonRush.Customization
{
    public class HumanoidCustomization : MonoBehaviour, ICustomization
    {
        [SerializeField] Canvas characterCanvas = null;
        [SerializeField] SpriteRenderer leftArm = null;
        [SerializeField] SpriteRenderer rightArm = null;
        [SerializeField] SpriteRenderer leftLeg = null;
        [SerializeField] SpriteRenderer rightLeg = null;
        [SerializeField] SpriteRenderer body = null;
        [SerializeField] SpriteRenderer bodyArmor = null;
        [SerializeField] SpriteRenderer head = null;
        [SerializeField] SpriteRenderer headArmor = null;
        [SerializeField] SpriteRenderer leftBoot = null;
        [SerializeField] SpriteRenderer rightBoot = null;

        [SerializeField] Material shadow = null;
        [SerializeField] Material lighted = null;

        private string r = "Row ";

        private void ChangeLayer(SpriteRenderer sR, int layer)
        {
            string sth = String.Concat(r, layer);
            sR.sortingLayerName = sth;
        }

        private void ChangeLayer(Canvas c, int layer)
        {
            string sth = String.Concat(r, layer);
            c.sortingLayerName = sth;
        }

        public void Change(float posY)
        {
            int layer = (int)Math.Truncate(posY);

            if (characterCanvas != null)
                ChangeLayer(characterCanvas, layer);

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

            if (bodyArmor != null)
                ChangeLayer(bodyArmor, layer);

            if (head != null)
                ChangeLayer(head, layer);

            if (headArmor != null)
                ChangeLayer(headArmor, layer);

            if (leftBoot != null)
                ChangeLayer(leftBoot, layer);

            if (rightBoot != null)
                ChangeLayer(rightBoot, layer);
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

            if (bodyArmor != null)
                bodyArmor.material = shadow;

            if (head != null)
                head.material = shadow;

            if (headArmor != null)
                headArmor.material = shadow;

            if (leftBoot != null)
                leftBoot.material = shadow;

            if (rightBoot != null)
                rightBoot.material = shadow;
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

            if (bodyArmor != null)
                bodyArmor.material = lighted;

            if (head != null)
                head.material = lighted;

            if (headArmor != null)
                headArmor.material = lighted;

            if (leftBoot != null)
                leftBoot.material = lighted;

            if (rightBoot != null)
                rightBoot.material = lighted;
        }
    }
}