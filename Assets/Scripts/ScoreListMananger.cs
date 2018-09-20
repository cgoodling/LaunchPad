using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreListMananger : MonoBehaviour {
    
    public GameObject scoreEntryPrefab;
    HighScoreManager scoreManager;
    int lastChangeCounter;


	// Use this for initialization
	void Start () {

        //scoreManager = FindObjectOfType<HighScoreManager>();
        scoreManager = GameObject.FindObjectOfType<HighScoreManager>();

        //Load Scores from Disk

        lastChangeCounter = scoreManager.GetChangeCounter();

        List<PlayerScore> scores = scoreManager.GetPlayerScores();
        RenderScores(scores);

	}
	
	// Update is called once per frame
	void Update () {
        if (scoreManager == null) {
            Debug.LogError("You forgot to add the HighScoreManager script to respectve GameObject in scene.");
            return;
        }

        if(lastChangeCounter == scoreManager.GetChangeCounter()) {
            return;
        }

        lastChangeCounter = scoreManager.GetChangeCounter();

        //REFETCH DATA
        RenderScores(scoreManager.GetPlayerScores());
	}


    void RenderScores(List<PlayerScore> scores) {

        // DESTROY PREVIOUS ENTRY GAMEOBJECTS
        while (this.transform.childCount > 0) {
            Transform c = this.transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }

        // RENDER NEW ONES
        foreach (PlayerScore score in scores) {
            GameObject go = (GameObject)Instantiate(scoreEntryPrefab);
            go.transform.SetParent(this.transform);
            go.transform.Find("User").GetComponent<Text>().text = score.GetName();
            go.transform.Find("Score").GetComponent<Text>().text = score.GetScore().ToString();
        }
    }
}
