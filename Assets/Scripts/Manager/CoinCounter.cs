using TMPro;
using UnityEngine;

namespace DungeonRush
{
    namespace Managers
    {
        public class CoinCounter : MonoBehaviour
        {
            public int coin;
            public TextMeshPro textMeshCoin;

            private void Start()
            {
                coin = 0;
                textMeshCoin.text = coin.ToString();
            }

            public void IncreaseCoin(int coin)
            {
                this.coin += coin;
                textMeshCoin.text = this.coin.ToString();
            }
        }
    }
}
