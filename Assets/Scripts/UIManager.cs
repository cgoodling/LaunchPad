using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public void LoadByIndex(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    public void StartGame() {
        GameManager.instance.NewGame();
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene(GlobalConstants.MAIN_SCREEN);
    }

    public void GoToHelp() {
        SceneManager.LoadScene(GlobalConstants.HELP_SCREEN);
    }

    public void GoToHighScores() {
        SceneManager.LoadScene(GlobalConstants.HIGHSCORE_SCREEN);
    }

    public void GoToCredits() {
        SceneManager.LoadScene(GlobalConstants.CREDITS_SCREEN);
    }

    public void Quit() {
        Application.Quit();
    }

}

