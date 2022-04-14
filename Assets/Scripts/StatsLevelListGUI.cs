using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsLevelListGUI : MonoBehaviour {

	public void returnToMenu()
    {
        Debug.Log("returnToMenu()");
        SceneManager.LoadScene(11);
    }

    public void load1GUI()
    {
        Debug.Log("1GUI");
        SceneManager.LoadScene(9);
    }

    public void load2GUI()
    {
        Debug.Log("2GUI");
        SceneManager.LoadScene(12);
    }

    public void load3GUI()
    {
        Debug.Log("3GUI");
        SceneManager.LoadScene(13);
    }

    public void load4GUI()
    {
        Debug.Log("4GUI");
        SceneManager.LoadScene(14);
    }

    public void load5GUI()
    {
        Debug.Log("5GUI");
        SceneManager.LoadScene(10);
    }
}
