using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToggleImage : MonoBehaviour
    {
        [SerializeField] private Sprite empty, full;
        [SerializeField] Image image;
        [SerializeField] private bool isFull;

        public bool IsFull
        {
            get => isFull;
            set => isFull = value;
        }

        private void Awake()
        {
            image = this.GetComponent<Image>();
        }

        private void Update() => Toggle();

        void Toggle() => image.sprite = IsFull == true ? full : empty;


    }
}