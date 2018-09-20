using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenManager : MonoBehaviour {
    
    public Text scoreField;
    int targetScore;
    int currentDisplayScore = 0;


    private void Awake() {
        targetScore = GameManager.instance.GetScore();
    }

    void Start() {
        if(targetScore > 0){
            StartCoroutine(CountUpToTarget());
        }
    }

    IEnumerator CountUpToTarget() {
        while (currentDisplayScore < targetScore) {
            currentDisplayScore += (int)(targetScore/(GlobalConstants.SCORE_COUNT_DURATION/Time.deltaTime)); // or whatever to get the speed you like
            currentDisplayScore = Mathf.Clamp(currentDisplayScore, 0, targetScore);
            scoreField.text = currentDisplayScore + "";
            yield return null;
        }
    }    
	
    public void Next() {
        GameManager.instance.GameOver();
    }

	// Update is called once per frame
	void Update () {}
}
