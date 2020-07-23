using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidCustomization : MonoBehaviour, ICustomization
{
    [SerializeField] SpriteRenderer leftArm;
    [SerializeField] SpriteRenderer rightArm;
    [SerializeField] SpriteRenderer leftLeg;
    [SerializeField] SpriteRenderer rightLeg;
    [SerializeField] SpriteRenderer body;
    [SerializeField] SpriteRenderer bodyArmor;
    [SerializeField] SpriteRenderer head;
    [SerializeField] SpriteRenderer headArmor;
    [SerializeField] SpriteRenderer leftBoot;
    [SerializeField] SpriteRenderer rightBoot;

    [SerializeField] Material shadow;
    [SerializeField] Material lighted;

    public void Change()
    {

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
