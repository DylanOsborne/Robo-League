using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    // List of players in the lobby
    private List<PlayerLobby> playersInLobby = new List<PlayerLobby>();

    private void Awake()
    {
        if(instance == null) 
        { 
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayerToLobby(PlayerLobby player)
    {
        playersInLobby.Add(player); 
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Load your actual game scene
    }
}
