using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Customization
{
    public class EnvironmentCustomization : MonoBehaviour, ICustomization
    {
        [SerializeField] List<ParticleSystemRenderer> particles = new List<ParticleSystemRenderer>();
        [SerializeField] List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

        public void ChangeLayer(bool top, int multiplier = 1)
        {
            for (var i = 0; i < particles.Count; i++)
            {
                if (top)
                    particles[i].sortingOrder += 6 * multiplier;
                else
                    particles[i].sortingOrder -= 6 * multiplier;
            }

            for (var i = 0; i < spriteRenderers.Count; i++)
            {
                if (top)
                    spriteRenderers[i].sortingOrder += 6 * multiplier;
                else
                    spriteRenderers[i].sortingOrder -= 6 * multiplier;
            } 
        }

        public void ChangeSkinState(bool state)
        {
            return;
        }
    }
}