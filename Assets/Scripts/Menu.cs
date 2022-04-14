using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public void StartGame()
    {
       // pretpostavka : Level1 ima build index = 1
        SceneManager.LoadScene(1);
    }

    public void Logout()
    {
        Debug.Log("Logout");
        SceneManager.LoadScene(0);
    }
    
    public void OpenStatisticsScene()
    {
        SceneManager.LoadScene(6);
    }
}
