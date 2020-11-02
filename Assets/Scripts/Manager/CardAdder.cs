using DungeonRush.Cards;
using DungeonRush.Field;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DungeonRush.Managers
{
    [ExecuteInEditMode]
    public class CardAdder : MonoBehaviour
    {
        private Vector2 defaultCoordinate = new Vector2(-1, -1);

        [SerializeField] BoardCreator boardCreator = null;

        [TextArea(0, 5)]
        [Tooltip("Doesn't do anything. Just comments shown in inspector")]
        public string Notes = "";
        [Header("Card Initialize Menu")]
        [SerializeField] Card card = null;
        [SerializeField] Vector2 coordinate = new Vector2(-1, -1);
        [SerializeField] bool create = false;

        private void Update()
        {
            if (create)
            {
                create = false;
                InitializeCard();
            }

        }

        private void InitializeCard()
        {
            int rL = boardCreator.rowLength;

            if (coordinate == defaultCoordinate || card == null) return;

            foreach (var item in FindObjectsOfType<Card>())
            {
                if (item.transform.position == (Vector3)coordinate)
                    return;
            }

            if (coordinate.x >= 0 && coordinate.x < rL && coordinate.y >= 0 && coordinate.y < rL)
                AddCard(card, coordinate);

            Reset();
        }

        private void AddCard(Card piece, Vector3 pos)
        {
            Card c = PrefabUtility.InstantiatePrefab(piece) as Card;
            c.transform.position = pos;
        }

        private void Reset()
        {
            coordinate = defaultCoordinate;
            card = null;
        }
    }
}
