using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameIzbornikScr : MonoBehaviour {

    public GameObject endGameGUI;
    //BehindPlayerObjDestroy destroyScr;

    private void Start()
    {
        //destroyScr = FindObjectOfType<BehindPlayerObjDestroy>();
        //if (destroyScr == null)
        //{
        //    Debug.Log("ERROR : GameManager.cs couldn't get destroyScr.");
        //}
    }

    public void povratakUIzbornik()
    {
        // izbornik index
        endGameGUI.SetActive(false);
        // da se nebi pojavljivali nakon 2. ili daljnijh iteracija igre
        GameObject[] objekti = FindObjectsOfType<GameObject>();
        foreach (GameObject o in objekti)
        {
            if (o.tag == "Obstacle" || o.tag == "Ball") Destroy(o);
        }
        SceneManager.LoadScene(11);
    }

    public void resetirajIgru()
    {   // buildIndex levela 5
        endGameGUI.SetActive(false);
        SceneManager.LoadScene(5);
        GameObject[] objekti = FindObjectsOfType<GameObject>();
        // ako kojim slučajem preostao objekt iz prošle iteracije levela 5
        foreach(GameObject o in objekti)
        {
            if (o.tag == "Obstacle" || o.tag == "Ball") Destroy(o);
        }
        
        //destroyScr.enabled = true;  // aktiviraj skriptu za uništavanje prođenih objekata
    }
}
