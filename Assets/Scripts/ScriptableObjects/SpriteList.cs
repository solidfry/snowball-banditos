using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "_SpriteList", menuName = "Sprites/New Sprite List", order = 0)]
    public class SpriteList : ScriptableObject
    {
        [SerializeField] private List<Sprite> listOfSprites;

        public List<Sprite> ListOfSprites => listOfSprites;

        public Sprite GetRandomSprite() => ListOfSprites[Random.Range(0, ListOfSprites.Count - 1)];
    }
}