using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour {

    GameManager gameManager;
    DatabaseManagment dbManagment;
    public bool gameOver;
    Score scoreScript;

    void Start()
    {
        gameOver = false;

        //scoreScript = FindObjectOfType<Score>();
        //if (scoreScript == null)
        //{
        //    Debug.Log("ERROR : EndTrigger.cs couldn't find scoreScript object.");
        //}

        gameManager = FindObjectOfType<GameManager>();
        if ( gameManager == null )
        {
            Debug.Log("ERROR : EndTrigger.cs couldn't find GameManager object.");
        }

        // TO CORRECT INTO DB
        dbManagment = FindObjectOfType<DatabaseManagment>();
        if (dbManagment == null)
        {
            Debug.Log("ERROR : EndTrigger.cs couldn't find dbManagment object.");
        }
    }

    // this gets called when ANY object enters the trigger
    // if we want the code in below function to execute only
    // when Player enters the trigger , surround entire code in 
    // if-statement
	void OnTriggerEnter()
    {
        if ( !gameOver )
        {
            Debug.Log("EndTrigger : OnTriggerEnter");
            gameManager.EndGameWin();
            dbManagment.gameWon = true;
            dbManagment.gameOver = true;
            //scoreScript.enabled = false;
        }
    }
    
}
