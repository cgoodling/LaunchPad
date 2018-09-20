using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    //current active camera
    private int cameraPosition;

    // Game level load delay
    [SerializeField] float levelLoadDelay = GlobalConstants.LEVEL_LOAD_DELAY;

    // Player starting lives
    [SerializeField] int startingLives = GlobalConstants.PLAYER_STARTING_LIVES;

    // Player current lives
    private int lives;

    // Player score
    private int score;

    // Game Over Flag
    private bool gameOver = false;

	//Awake is always called before any Start functions
	void Awake() {
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		//Call the InitGame function to initialize our variables 
		InitGame();
	}

	//Initializes the game for each level.
	void InitGame() {
        //If needed here is where we would initialize the game
        cameraPosition = GlobalConstants.CAMERA_MAIN;
		lives = startingLives;
        score = 0;
        gameOver = false;
        Debug.Log("GAME INITIALIZED");
	}

    public int GetLives() {
        return lives;   
    } 

	public int IncreaseLives() {
		return ++lives;
	}

	public int DecreaseLives() {
		if(lives == 0){
			return 0;
		}
		return --lives;
	}

    public int GetScore() {
        return score;
    }

    public void IncreaseScore(int newScore){
        score += newScore;
    }

    //Update is called every frame.
    void Update() {}


    // LEVEL MANAGEMENT SECTION // 

    public void RestartLevel(){
        Invoke("LoadCurrentLevel", levelLoadDelay); //parametize this time
    }

    private void LoadCurrentLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel() {
        Invoke("LoadNextLevel", levelLoadDelay); //parametize this time
    }

    private void LoadNextLevel() {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;


        // MAY NOT NEED THIS ANYMORE
        if (nextSceneIndex == (SceneManager.sceneCountInBuildSettings)) {
            nextSceneIndex = GlobalConstants.MAIN_SCREEN;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    public void NewGame() {
        if(gameOver) {
            InitGame();
        }
        SceneManager.LoadScene(GlobalConstants.STARTING_STAGE);
    }

    public void GameOver() {
        gameOver = true;
        Invoke("EndGame", levelLoadDelay);
    }

    private void EndGame() {
        SceneManager.LoadScene(GlobalConstants.HIGHSCORE_SCREEN);
    }

    public void QuitToMenu() {
        gameOver = true;
        SceneManager.LoadScene(GlobalConstants.MAIN_SCREEN);
    }

    public void ResetGame() {
        InitGame();
    }

    public bool IsGameOver() {
        return gameOver;
    }


    // CAMERA POSITION MANAGEMENT

    public int ChangeCameraPosition() {
        if (cameraPosition == GlobalConstants.CAMERA_MAIN){
            cameraPosition = GlobalConstants.CAMERA_FOLLOW;
            return cameraPosition;
        }
        cameraPosition = GlobalConstants.CAMERA_MAIN;
        return cameraPosition;
    }

    public int GetCameraPosition() {
        return cameraPosition;
    }

}