using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void CreateLobby()
    {
        // Generate a unique room ID (you can use a more robust solution)
        string roomID = GenerateRoomID();

        // Set the room ID in a static class or GameManager for access in the next scene
        GameManager.RoomID = roomID;

        // Load the lobby scene
        SceneManager.LoadScene("LobbyScene");
    }

    public void JoinLobby(string roomID)
    {
        // Set the room ID in a static class or GameManager for access in the next scene
        GameManager.RoomID = roomID;

        // Load the lobby scene
        SceneManager.LoadScene("LobbyScene");
    }

    private string GenerateRoomID()
    {
        // Generate a unique room ID using your preferred method
        return System.Guid.NewGuid().ToString();
    }
}
