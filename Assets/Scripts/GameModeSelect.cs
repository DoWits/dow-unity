using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelect : MonoBehaviour
{

    public static string GameMode = "PvP";

    public static int Level = 1;
    // Start is called before the first frame update
    public void StartPvP()
    {
        GameMode = "PvP";
        SceneManager.LoadScene("Board Set");
    }

    public void StartPvA1()
    {
        GameMode = "PvA";
        Level = 1;
        SceneManager.LoadScene("Board Set");
    }

    public void StartPvA2()
    {
        GameMode = "PvA";
        Level = 2;
        SceneManager.LoadScene("Board Set");
    }
    public void StartPvA3()
    {
        GameMode = "PvA";
        Level = 3;
        SceneManager.LoadScene("Board Set");
    }
    public void StartPvA4()
    {
        GameMode = "PvA";
        Level = 4;
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
