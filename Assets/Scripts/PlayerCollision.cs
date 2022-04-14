using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerCollision : MonoBehaviour
{
    GameManager gameManager;
    DatabaseManagment dbManagment;
    EndTrigger endTrigger;
    int currentBuildIndex;

    void Start()
    {
        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentBuildIndex == 5) endTrigger = null;

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.Log("ERROR : PlayerCollision.cs couldn't get gameManager.");
            return;
        }

        dbManagment = FindObjectOfType<DatabaseManagment>();
        if (dbManagment == null)
        {
            Debug.Log("ERROR : PlayerCollision.cs couldn't get dbManagment.");
            return;
        }

        if ( currentBuildIndex != 5)
        {
            endTrigger = FindObjectOfType<EndTrigger>();
            if (endTrigger == null)
            {
                Debug.Log("ERROR : PlayerCollision.cs couldn't get endTrigger.");
                throw new Exception("ERROR : PlayerCollision.cs couldn't get endTrigger.");
                
            }
        }
        
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        //Debug.Log(collisionInfo.gameObject.name);

        string tag = collisionInfo.gameObject.tag;

        if (tag == "Obstacle" || tag == "Ball")
        {
            if(currentBuildIndex != 5 )
            {
                endTrigger.gameOver = true;
                dbManagment.gameWon = false;
            }
            
            dbManagment.causeOfDefeat = tag;
            dbManagment.gameOver = true;
            gameManager.EndGameLose();
            
        }
    }
}
