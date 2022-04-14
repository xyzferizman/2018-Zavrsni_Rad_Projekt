using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float strafeForce = 500f;
    GameManager gameManager;
    public float velocityOfNoMoreAcceleration = 1000f;
    bool secondTimePointPrinted = false;
    DatabaseManagment dbManagment;
    EndTrigger endTrigger;
    int currentBuildIndex;

    void Start()
    {
        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        if( currentBuildIndex != 5 )
        {
            endTrigger = FindObjectOfType<EndTrigger>();
            if (endTrigger == null)
            {
                Debug.Log("ERROR : PlayerMovement.cs couldn't get endTrigger.");
            }
        }
       
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager==null)
        {
            Debug.Log("ERROR : PlayerMovement.cs couldn't get gameManager.");
            //print(Time.time);
        }

        dbManagment = FindObjectOfType<DatabaseManagment>();
        if (dbManagment == null)
        {
            Debug.Log("ERROR : PlayerMovement.cs couldn't get dbManagment.");
            //print(Time.time);
        }
    }

    // we changed this to FixedUpdate cus we messed with physics
    void FixedUpdate () {

        if ( rb.velocity.z >= velocityOfNoMoreAcceleration && !secondTimePointPrinted )
        {
            secondTimePointPrinted = true;
            //print(Time.time);
        }

        if ( rb.velocity.z < velocityOfNoMoreAcceleration  )
        {
            rb.AddForce(0, 0, forwardForce * Time.deltaTime);
        }
       

        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow ) )   {
            rb.AddForce(strafeForce * Time.deltaTime ,0,0, ForceMode.VelocityChange);
        }

        else if ( Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow ))   {
            rb.AddForce(-strafeForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if ( rb.position.y < -1 )
        {
            if ( currentBuildIndex != 5)
            {
                endTrigger.gameOver = true;
            }
            
            dbManagment.gameWon = false;
            dbManagment.causeOfDefeat = "Fall";
            dbManagment.gameOver = true;
            gameManager.EndGameLose();
        }       
    }

    
}
