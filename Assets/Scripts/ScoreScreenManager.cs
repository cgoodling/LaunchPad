using UnityEngine;

public class ScoreScreenManager : MonoBehaviour {

    [SerializeField] GameObject enterNamePanel;
    HighScoreManager scoreManager;
    private string userInput;
    private int userScore;

    private void Awake() {
        enterNamePanel.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        scoreManager = GameObject.FindObjectOfType<HighScoreManager>();
        userInput = "";

        userScore = GameManager.instance.GetScore();
        if(userScore > 0 && (scoreManager.GetPlayerScoreCount() < 10 || scoreManager.IsHighScore(userScore))) {
            enterNamePanel.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (scoreManager == null) {
            Debug.LogError("You forgot to add the HighScoreManager script to respectve GameObject in scene.");
            return;
        }
	}

    public void Submit() {
        if(userInput.Length > 0) {
            if(scoreManager.AddScore(userInput, userScore)) {
                scoreManager.SaveScoresToDisk();
            }
            // SAVE SCORES TO DISK
            enterNamePanel.SetActive(false);
            userInput = "";

            // Since we should be in GAME OVER status, RESET GAME VALUES
            if(GameManager.instance.IsGameOver()){
                GameManager.instance.ResetGame();
            }

        } else {
            Debug.Log("USER NAME CANNOT BE EMPTY STRING");
        }
    } 

    public void OnInputComplete(string name) {
        userInput = name;
    }
}
