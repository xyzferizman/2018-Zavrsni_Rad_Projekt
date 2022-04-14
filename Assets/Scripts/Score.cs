using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Transform player;
    public Text scoreText;
    public int prethodniHighscore;

    private void Start()
    {
        prethodniHighscore = 0;
    }

    // Update is called once per frame
    void Update () {
        
        scoreText.text = (prethodniHighscore + player.position.z + 6).ToString("0");

    }
}
