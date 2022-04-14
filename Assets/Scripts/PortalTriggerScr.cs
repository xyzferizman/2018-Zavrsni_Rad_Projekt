using UnityEngine;
using System;

public class PortalTriggerScr : MonoBehaviour {

    GameObject player;
    Rigidbody playerRigidbody;
    //float maxBrzina = 10f;    // PROMIJENITI KASNIJE , OVO MORA BIT MAX DOPUSTENA BRZINA KOJA CE SE DATI TELEPORTIRANOM IGRACU NAKON PROLASKA KROZ PORTAL
    //float pocetnaBrzina = 1000000f; // PROMIJENITI KASNIJE
    Vector3 pocetnaPozicija = new Vector3(0,1,-6);
    public float teleportDelay = 1.5f;
    

    GameManager gameManager;
    Score scoreScript;
    PlayerMovement playerMovementScript;
    FollowPlayer followPlayerScript;
    PlayerCollision playerCollisionScript;
    ProceduralGeneration procGenScr;

    Quaternion rotation;

    private void Start()
    {
        

        procGenScr = FindObjectOfType<ProceduralGeneration>();
        if (procGenScr == null)
        {
            Debug.Log("ERROR : PortalTriggerScr.cs couldn't get procGenScr.");
            return;
        }

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.Log("ERROR : PortalTriggerScr.cs couldn't get gameManager.");
            return;
        }

        playerMovementScript = FindObjectOfType<PlayerMovement>();
        if (playerMovementScript == null)
        {
            Debug.Log("ERROR : PortalTriggerScr.cs couldn't get playerMovementScript.");
            return;
        }
        player = playerMovementScript.gameObject;   // PRETPOSTAVKA : playerMovementScript ima samo Player objekt
        playerRigidbody = player.GetComponent<Rigidbody>();

        scoreScript = FindObjectOfType<Score>();
        if (scoreScript == null)
        {
            Debug.Log("ERROR : PortalTriggerScr.cs couldn't get scoreScript.");
            return;
        }

        followPlayerScript = FindObjectOfType<FollowPlayer>();
        if (followPlayerScript == null)
        {
            Debug.Log("ERROR : GameManager.cs couldn't get followPlayerScript.");
            return;
        }
        
        playerCollisionScript = FindObjectOfType<PlayerCollision>();
        if (playerCollisionScript == null)
        {
            Debug.Log("ERROR : GameManager.cs couldn't get playerCollisionScript.");
            return;
        }

        rotation = player.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ako je to player a ne neki od obstaclea
        if ( other.tag == "Player" && !gameManager.gameHasEnded )
        {
            //disable skripte
            //pricekaj 2 sekunde
            //enable skripte
            Debug.Log("teleportacija aktivirana");

            // TODO
            followPlayerScript.enabled = false; // neka kamera ostane tu gdje je na kraju levela
            playerMovementScript.enabled = false;   // ugasi kretanje igrača
            scoreScript.enabled = false;
            playerCollisionScript.enabled = false;

            Invoke("Teleport", teleportDelay);

            //spasi prethodni highscore
            scoreScript.prethodniHighscore = Int32.Parse(scoreScript.scoreText.text);
        }
    }

    void Teleport()
    {
        // enable skripte
        followPlayerScript.enabled = true; // neka kamera ostane tu gdje je na kraju levela
        playerMovementScript.enabled = true;   // ugasi kretanje igrača
        scoreScript.enabled = true;
        playerCollisionScript.enabled = true;
        
        //teleportiraj
        player.transform.position = pocetnaPozicija;     //onda teleportiraj igraca
        player.transform.rotation = rotation;
        playerRigidbody.velocity = new Vector3(0f, 0f, playerRigidbody.velocity.z); ;  // postavljanje brzine teleportiranog igrača
        playerRigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
        Debug.Log("proc gen enabled... : " + procGenScr.enabled);
    }
}
