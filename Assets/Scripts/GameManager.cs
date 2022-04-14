using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool gameHasEnded = false;
    int currentBuildIndex;

    FollowPlayer followPlayerScript;
    PlayerMovement playerMovementScript;
    PlayerCollision playerCollisionScript;
    DatabaseManagment dbManagment;
    Score scoreScript;
    ProceduralGeneration proceduralScript;

    public float UIDelay = 1.5f;
    public float restartDelay = 3f;
    public GameObject completeLevelUI;
    public GameObject lvl5EndGUI;

    void Start()
    {

        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;


        if ( currentBuildIndex == 5)
        {
            proceduralScript = FindObjectOfType<ProceduralGeneration>();
            if (proceduralScript == null)
            {
                Debug.Log("ERROR : GameManager.cs couldn't get proceduralScript.");
            }

            scoreScript = FindObjectOfType<Score>();
            if (scoreScript == null)
            {
                Debug.Log("ERROR : GameManager.cs couldn't get scoreScript.");
            }
        }

        dbManagment = FindObjectOfType<DatabaseManagment>();
        if (dbManagment == null)
        {
            Debug.Log("ERROR : GameManager.cs couldn't get dbManagmentScript.");
        }

        followPlayerScript = FindObjectOfType<FollowPlayer>();
        if (followPlayerScript == null)
        {
            Debug.Log("ERROR : GameManager.cs couldn't get followPlayerScript.");
        }

        playerMovementScript = FindObjectOfType<PlayerMovement>();
        if (playerMovementScript == null)
        {
            Debug.Log("ERROR : GameManager.cs couldn't get playerMovementScript.");
        }

        playerCollisionScript = FindObjectOfType<PlayerCollision>();
        if (playerCollisionScript == null)
        {
            Debug.Log("ERROR : GameManager.cs couldn't get playerCollisionScript.");
        }
    }

    public void EndGameLose()
    {
        if ( !gameHasEnded )
        {
            gameHasEnded = true;
            Debug.Log("EndGameLose");
            
            // disable movement
            playerMovementScript.enabled = false;
            // disable score
                                    
            if (currentBuildIndex == 5)
            {
                scoreScript.enabled = false;
                // aktiviraj endgamescreen ,deaktivacija nakon stiska tipke
                lvl5EndGUI.SetActive(true);
                proceduralScript.enabled = false;
                //throw new NotImplementedException();
            }
            else
            {
                Invoke("Restart", restartDelay);
            }
        }
    }

    public void EndGameWin()
    {
        if ( !gameHasEnded )
        {
            gameHasEnded = true;
            Debug.Log("usao u EndGameWin");
            //Debug.Log("LEVEL COMPLETE!");

            //destroyScr.enabled = false;
            followPlayerScript.enabled = false; // neka kamera ostane tu gdje je na kraju levela
            playerMovementScript.enabled = false;   // ugasi kretanje igrača
            //scoreScript.enabled = false;
            playerCollisionScript.enabled = false;

            Invoke("ActivateCompleteLevelUI", UIDelay);
            currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
            Invoke("LoadNext", restartDelay);
        }
    }

    void ActivateCompleteLevelUI()
    {
        completeLevelUI.SetActive(true);
    }

    void LoadNext()
    {
        Debug.Log("usao u LoadNext , currentBuildIndex = " + currentBuildIndex);
        SceneManager.LoadScene(currentBuildIndex + 1);  // nikad se ne zove na lvlu 5 jer ništa ne triggera EndGameWin koji jedini poziva LoadNext
    }

    void Restart()
    {
        SceneManager.LoadScene(currentBuildIndex);  // prije bilo SceneManager.getactivescene.name
    }

    

}
