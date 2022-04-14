
using UnityEngine;

public class BallTrigger : MonoBehaviour
{

    public Rigidbody ball;
    public float ballForce = 2000f;
    public float ballStartVelocity = 50f;

    bool triggered = false;
    
    // this gets called when ANY object enters the trigger
    // if we want the code in below function to execute only
    // when Player enters the trigger , surround entire code in 
    // if-statement
    void OnTriggerEnter(Collider collider)
    {
        if ( collider.tag == "Player" )
        {
            Debug.Log("TRIGGERED : " + gameObject.name);
            triggered = true;
            ball.velocity = new Vector3(0,0,-ballStartVelocity);
        }
        
    }

    private void FixedUpdate()
    {
        if ( triggered )
        {
            ball.AddForce(0, 0, -ballForce * Time.deltaTime);
        }
        
    }
}
