using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Dialogue_", menuName = "Dialogues/New Dialogue", order = 0)]
    public class Dialogues : ScriptableObject
    {
        [TextArea]
        public List<string> list = new();
    }
}