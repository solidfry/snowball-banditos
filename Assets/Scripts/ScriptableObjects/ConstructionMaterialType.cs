using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ConstructionMaterial_", menuName = "Construction Materials/New Construction Material", order = 0)]
    public class ConstructionMaterialType : ScriptableObject
    {
        public new string name;
        public List<AudioClip> audioClips = new();
    }
}