using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool gameOver = true;
    public bool isCoopMode = false;
    public int deadPlayer = 0;


    //Player (Single player Mode)
    [SerializeField]
    private Player _player1Prefab;
    //Players (Coop Mode)
    [SerializeField]
    private Player _player2Prefab;
    //UI Manager
    private UIManager _uiManager;
    //Spawn Manager
    private SpawnManager _spawnManager;

    // Use this for initialization
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }

    }

    public void StartGame()
    {
       
        //set gameOver to be false
        gameOver = false;
        deadPlayer = 0;
        Time.timeScale = 1;

        //hide the Title Screen and display player's lives
        _uiManager.HideTitleScreen();

        //reset and display the Score
        _uiManager.score = 0;
        _uiManager.scoreText.text = "Score: " + _uiManager.score;


        if (isCoopMode == false)
        {
            //Add the player with 3 lives
            Instantiate(_player1Prefab, transform.position = new Vector3(0, -2.5f, 0), Quaternion.identity);
            //display the number of lives
            _uiManager.UpdatePlayer1Lives(_player1Prefab.lives);

            //display the Best Score if there's a previous best one
            _uiManager.bestScore = PlayerPrefs.GetInt("BestScore");
            if (_uiManager.bestScore > 0)
            {
                _uiManager.bestScoreText.text = "Best Score: " + _uiManager.bestScore;
            }
        }
        else
        {
            //Add the player 1 with 3 lives
            Instantiate(_player1Prefab, transform.position = new Vector3(4, -2.5f, 0), Quaternion.identity);
            //display the number of lives
            _uiManager.UpdatePlayer1Lives(_player1Prefab.lives);
            //Add the player 2 with 3 lives
            Instantiate(_player2Prefab, transform.position = new Vector3(-4, -2.5f, 0), Quaternion.identity);
            //display the number of lives
            _uiManager.UpdatePlayer2Lives(_player2Prefab.lives);
        }

        if (_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutines();
        }
    }

    //Pauses the Game
    public void PauseGame()
    {
        _uiManager.EnablePauseMenu();
        Animator pauseMenu = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        if(pauseMenu != null)
        {
            pauseMenu.updateMode = AnimatorUpdateMode.UnscaledTime;
            pauseMenu.SetBool("isPaused", true);
        }
        Time.timeScale = 0;
    }

    //Resumes a paused Game
    public void ResumeGame()
    {
        Animator pauseMenu = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        if (pauseMenu != null)
        {
            pauseMenu.SetBool("isPaused", false);
        }
        _uiManager.DisablePauseMenu();
        Time.timeScale = 1;
    }

    //Quit Game
    public void QuitGame()
    {
        GameOver();
        SceneManager.LoadScene("Main_Menu");
    }


    //Ends the Game
    public void GameOver()
    {
        if (isCoopMode == false)
        {
            //Check if we need to update the best score
            _uiManager.CheckBestScore();
        }

        //destroy all the instantiated clones
        var clones = GameObject.FindGameObjectsWithTag("Clone");
        foreach (var item in clones)
        {
            Destroy(item);
        }

        //stop spawning enemies and powerups immediately
        _spawnManager.StopSpawnRoutines();

        //set the gameOver state to true after all animations are over
        StartCoroutine(EndGame());
    }

    //Delays the gameOver state and the Title Screen
    //Gives time to all animations to be over and all instantiated objects to be destroyed
    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3.0f);
        gameOver = true;
        _uiManager.ShowTitleScreen();

    }


}