using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Customization
{
    public class EnvironmentCustomization : MonoBehaviour, ICustomization
    {
        [SerializeField] ParticleSystemRenderer[] particles;
        [SerializeField] bool isParticle = true;
        [SerializeField] SpriteRenderer spriteRenderer;

        public void ChangeLayer(bool top, int multiplier = 1)
        {
            if(isParticle)
            {
                for (var i = 0; i < particles.Length; i++)      
                {
                    if (top)
                        particles[i].sortingOrder += 6 * multiplier;
                    else
                        particles[i].sortingOrder -= 6 * multiplier;
                }
            }
            else
            {
                if (top)
                    spriteRenderer.sortingOrder += 6 * multiplier;
                else
                    spriteRenderer.sortingOrder -= 6 * multiplier;
            }
            
        }

        public void ChangeSkinState(bool state)
        {
            return;
        }
    }
}