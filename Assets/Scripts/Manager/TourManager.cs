using DungeonRush.Field;
using TMPro;
using UnityEngine;

namespace DungeonRush
{
    namespace Managers
    {
        public class TourManager : MonoBehaviour
        {
            public TextMeshPro textMeshTour;
            public int tourNumber;
            public int oldTourNumber;

            private void Start()
            {
                textMeshTour.text = tourNumber.ToString();
                tourNumber = 0;
                oldTourNumber = 0;
            }

            public bool IsTourNumbersEqual()
            {
                return tourNumber == oldTourNumber ? true : false;
            }

            public void IncreaseTourNumber()
            {
                tourNumber++;
                textMeshTour.text = tourNumber.ToString();
            }

            public void FinishTour(bool isCardDisPlaced)
            {
                if(!isCardDisPlaced)
                    IncreaseTourNumber();
                oldTourNumber = tourNumber;
                Board.touched = false;
            }
        }
    }
}
