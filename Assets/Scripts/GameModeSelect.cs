using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelect : MonoBehaviour
{

    public static string GameMode = "PvP"; 
    // Start is called before the first frame update
    public void StartPvP()
    {
        GameMode = "PvP";
        SceneManager.LoadScene("Board Set");
    }

    public void StartPvA()
    {
        GameMode = "PvA";
        SceneManager.LoadScene("Board Set");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public string getGameMode()
    { return GameMode; }
}
