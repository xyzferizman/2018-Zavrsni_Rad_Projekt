using UnityEngine;

public class PauseGame : MonoBehaviour {
    
    [SerializeField] private GameObject pauseButton;
    bool gamePaused;

    void Start()
    {
        gamePaused = false;
        pauseButton.SetActive(false);
    }
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !gamePaused)
        {
            gamePaused = true;
            Debug.Log("PauseGame script : key press detected");
            PauseTheGame();
        }

        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && gamePaused)
        {
            ContinueGame();
            gamePaused = false;
        }
    }

    private void PauseTheGame()
    {
        Debug.Log("PauseTheGame called");
        Time.timeScale = 0;
        pauseButton.SetActive(true);
        //Disable scripts that still work while timescale is set to 0

        // cini se da nema potrebe za gašenjem PlayerMovement i PlayerCollision skripti
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(false);
        //enable the scripts again
    }
}
