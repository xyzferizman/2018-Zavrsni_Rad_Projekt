using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour {

    GameObject player;
    PlayerMovement playerMovement;
    Transform playerTransform;
    public float distanceFromPlayerForDestroy = 25f;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.Log("ERROR : SelfDestroy.cs couldn't get playerMovement.");
            return;
        }
        player = playerMovement.gameObject;
    }

    // Update is called once per frame
    void Update () {
        if (this.gameObject.transform.position.z < (player.transform.position.z - distanceFromPlayerForDestroy))
        {
            Destroy(this.gameObject);
        }
    }
}
