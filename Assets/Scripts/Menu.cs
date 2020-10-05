using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Dropdown player1;
    public Dropdown player2;

    // Use this for initialization
    public void UpdatePlayer()
    {
        Settings.player1 = (Settings.PLAYERTYPE)player1.value;
        Settings.player2 = (Settings.PLAYERTYPE)player2.value;
    }

    public void Play()
    {
        UpdatePlayer();
        Settings.turn = 0;
        Settings.pass = 0;
        SceneManager.LoadScene("Game");
    }

}
