using UnityEngine;

public class PlayerLobby : MonoBehaviour
{
    void Start()
    {
        // Register the player in the lobby when instantiated
        NetworkManager.instance.AddPlayerToLobby(this);
    }
}