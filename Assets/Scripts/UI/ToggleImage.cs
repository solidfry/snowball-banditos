using UnityEngine;
using UnityEngine.PlayerLoop;
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
            set
            {
                isFull = value;
                Toggle();
            }
        }
        
        private void Awake()
        {
            image = this.GetComponent<Image>();
        }


        void Toggle()
        {
            if (isFull)
                image.sprite = full;
            else
                image.sprite = empty;
        }
        
        
    }
}