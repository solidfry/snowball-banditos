using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class NetworkUIManager : MonoBehaviour
    {
        [SerializeField] private Button serverButton, hostButton, clientButton;

        private void Awake()
        {
            serverButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartServer();
            });
            hostButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartHost();
            });
            clientButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
            });
        }
    }
}