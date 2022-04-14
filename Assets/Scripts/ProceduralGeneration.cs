using UnityEngine;
using System;

public class ProceduralGeneration : MonoBehaviour
{

    GameObject player;
    GameManager gameManager;
    public float generationDelay = 1.5f;
    float firstTimeCheck;
    float secondTimeCheck;
    public float distanceFromPlayer = 100f;
    float playerZ;
    public float ballStartVelocity = 40f;
    public float ballAdditionalDistance = 20f;

    public GameObject boxPrefab;
    public GameObject smallBallPrefab;
    public GameObject bigBallPrefab;

    Quaternion rotation;

    // Use this for initialization
    void Start()
    {
        rotation = boxPrefab.transform.rotation;
        firstTimeCheck = 0f;
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.Log("ERROR : PortalTriggerScr.cs couldn't get gameManager.");
            return;
        }

        player = gameObject;    
    }

    private void Update()
    {
        
        secondTimeCheck = Time.time;
        if ((secondTimeCheck - firstTimeCheck) > generationDelay )
        {
            firstTimeCheck = secondTimeCheck;
            playerZ = player.transform.position.z;

            if ((playerZ + distanceFromPlayer + 50) < 1990)
            {
                //Debug.Log("uvjet prošo");
                
                GenerateObstacles();
            }
        }
    }

    void GenerateObstacles()
    {
        Debug.Log("called generateobstacles");
        System.Random rnd = new System.Random();
        int randomBroj = rnd.Next(1, 11);
        //randombroj od 1 do 10
        //if ...


        if (randomBroj == 1)
        {
            //static prefab 1 , nek bude 25 razlike
            Instantiate(boxPrefab, new Vector3(-4f, 1f, playerZ + distanceFromPlayer), rotation);
            Instantiate(boxPrefab, new Vector3(4f, 1f, playerZ + distanceFromPlayer), rotation);
            Instantiate(boxPrefab, new Vector3(0f, 1f, playerZ + distanceFromPlayer + 25f), rotation);
        }
        else if (randomBroj == 2)
        {
            // static prefab 2
            Instantiate(boxPrefab, new Vector3(-4f, 1f, playerZ + distanceFromPlayer), rotation);
            Instantiate(boxPrefab, new Vector3(-2f, 1f, playerZ + distanceFromPlayer + 12.5f), rotation);
            Instantiate(boxPrefab, new Vector3(-0f, 1f, playerZ + distanceFromPlayer + 25f), rotation);
        }
        else if (randomBroj == 3)
        {
            // static prefab 3
            Instantiate(boxPrefab, new Vector3(4f, 1f, playerZ + distanceFromPlayer), rotation);
            Instantiate(boxPrefab, new Vector3(2f, 1f, playerZ + distanceFromPlayer + 12.5f), rotation);
            Instantiate(boxPrefab, new Vector3(0f, 1f, playerZ + distanceFromPlayer + 25f), rotation);
        }
        else if (randomBroj == 4)
        {
            // static prefab 4
            Instantiate(boxPrefab, new Vector3(-4f, 1f, playerZ + distanceFromPlayer), rotation);
            Instantiate(boxPrefab, new Vector3(0f, 1f, playerZ + distanceFromPlayer +25f), rotation);
            Instantiate(boxPrefab, new Vector3(4f, 1f, playerZ + distanceFromPlayer), rotation);
        }
        else if (randomBroj == 5)
        {
            // static prefab 5
            Instantiate(boxPrefab, new Vector3(-4f, 1f, playerZ + distanceFromPlayer), rotation);
            Instantiate(boxPrefab, new Vector3(-3f, 1f, playerZ + distanceFromPlayer +25f), rotation);
            Instantiate(boxPrefab, new Vector3(3f, 1f, playerZ + distanceFromPlayer + 25f), rotation);
            Instantiate(boxPrefab, new Vector3(4f, 1f, playerZ + distanceFromPlayer), rotation);
        }
        else if (randomBroj == 6)
        {
            // kugla prefab crvena lijevo
            GameObject newObj = Instantiate(smallBallPrefab, new Vector3(-2f, 1f, playerZ + distanceFromPlayer + ballAdditionalDistance), rotation);
            newObj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -ballStartVelocity);
        }
        else if (randomBroj == 7)
        {
            // kugla prefab crvena centar
            GameObject newObj = Instantiate(smallBallPrefab, new Vector3(0f, 1f, playerZ + distanceFromPlayer + ballAdditionalDistance), rotation);
            newObj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -ballStartVelocity);
        }
        else if (randomBroj == 8)
        {
            // kugla prefab crvena desno
            GameObject newObj = Instantiate(smallBallPrefab, new Vector3(2f, 1f, playerZ + distanceFromPlayer + ballAdditionalDistance), rotation);
            newObj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -ballStartVelocity);
        }
        else if (randomBroj == 9)
        {
            // kugla prefab plava lijevo
            GameObject newObj = Instantiate(bigBallPrefab, new Vector3(-2f, 1f, playerZ + distanceFromPlayer + ballAdditionalDistance), rotation);
            newObj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -ballStartVelocity);
        }
        else if (randomBroj == 10)
        {
            // kugla prefab plava desno
            GameObject newObj = Instantiate(bigBallPrefab, new Vector3(2f, 1f, playerZ + distanceFromPlayer + ballAdditionalDistance), rotation);
            newObj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -ballStartVelocity);
        }
        
        //throw new NotImplementedException();
    }
}
