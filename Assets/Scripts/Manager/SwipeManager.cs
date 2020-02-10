﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush
{
    namespace Managers
    {
        public enum Swipe { None, Up, Down, Left, Right };

        public class SwipeManager : MonoBehaviour
        {
            public float minSwipeLength = 5f;
            Vector2 firstPressPos;
            Vector2 secondPressPos;
            Vector2 currentSwipe;

            Vector2 firstClickPos;
            Vector2 secondClickPos;

            public static Swipe swipeDirection;

            void Update()
            {
                DetectSwipe();
            }

            private void DetectSwipe()
            {
                if (Input.touches.Length > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        firstPressPos = new Vector2(touch.position.x, touch.position.y);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        secondPressPos = new Vector2(touch.position.x, touch.position.y);
                        currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                        // Make sure it was a legit swipe, not a tap
                        if (currentSwipe.magnitude < minSwipeLength)
                        {
                            swipeDirection = Swipe.None;
                            return;
                        }

                        currentSwipe.Normalize();

                        // Swipe up
                        if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                        {
                            swipeDirection = Swipe.Up;
                        }
                        // Swipe down
                        else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                        {
                            swipeDirection = Swipe.Down;
                        }
                        // Swipe left
                        else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                        {
                            swipeDirection = Swipe.Left;
                        }
                        // Swipe right
                        else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                        {
                            swipeDirection = Swipe.Right;
                        }
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        firstClickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    }
                    else
                    {
                        swipeDirection = Swipe.None;
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        secondClickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                        currentSwipe = new Vector3(secondClickPos.x - firstClickPos.x, secondClickPos.y - firstClickPos.y);

                        // Make sure it was a legit swipe, not a tap
                        if (currentSwipe.magnitude < minSwipeLength)
                        {
                            swipeDirection = Swipe.None;
                            return;
                        }

                        currentSwipe.Normalize();

                        //Swipe directional check
                        // Swipe up
                        if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                        {
                            swipeDirection = Swipe.Up;
                        }
                        // Swipe down
                        else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                        {
                            swipeDirection = Swipe.Down;
                        }
                        // Swipe left
                        else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                        {
                            swipeDirection = Swipe.Left;
                        }
                        // Swipe right
                        else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                        {
                            swipeDirection = Swipe.Right;
                        }
                    }
                }
            }
        }
    }
}