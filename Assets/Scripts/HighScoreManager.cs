using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {

    List<PlayerScore> playerScores;

    int changeCounter = 0;

    private void Awake() {
        Init();
    }

    void Start() {}

    void Init() {
        if (playerScores != null)
            return;

        // load from disk
        LoadScoresFromDisk();
    }


    public void Reset() {
        changeCounter++;
        playerScores = null;
        playerScores = new List<PlayerScore>();
    }

    public bool AddScore(string name, int score) {

        if(playerScores.Count < GlobalConstants.SCORE_BOARD_CAP){
            playerScores.Add(new PlayerScore(name, score));
            changeCounter++;
            return true;
        } else if (IsHighScore(score)) {
            //remove lowest and add new score
            RemoveLowestScore();
            playerScores.Add(new PlayerScore(name, score));
            changeCounter++;
            return true;
        }
        return false;
    }


    public bool IsHighScore(int score) {
        for (int i = 0; i < playerScores.Count; i++){
            if (score > playerScores[i].GetScore()) {
                return true;
            }
        }
        return false;
    }


    private void RemoveLowestScore() {

        int min = playerScores[0].GetScore();
        int mIndex = 0;

        for (int i = 1; i < playerScores.Count; i++ ){
            if(playerScores[i].GetScore() < min) {
                min = playerScores[i].GetScore();
                mIndex = i;
            }
        }
        playerScores.RemoveAt(mIndex);
    }


    public List<PlayerScore> GetPlayerScores() {
        playerScores.Sort((x,y) => x.CompareTo(y));
        return playerScores;
    }


    public int GetPlayerScoreCount(){
        if (playerScores != null) {
            return playerScores.Count;
        }
        return 0;
    }


    public int GetChangeCounter() {
        return changeCounter;
    }


    public void SaveScoresToDisk() {
        if (Directory.Exists(Application.dataPath + "/Save/") == false) {
            Directory.CreateDirectory(Application.dataPath + "/Save/");
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/Save/highScores.secure");

        PlayerScore[] data = playerScores.ToArray();

        bf.Serialize(file, data);
        file.Close();
    }


    public void LoadScoresFromDisk() {
        if (File.Exists(Application.dataPath + "/Save/highScores.secure")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/Save/highScores.secure", FileMode.Open);
            PlayerScore[] data = (PlayerScore[])bf.Deserialize(file);
            file.Close();

            playerScores = data.ToList<PlayerScore>();

        } else {
            // IF WE CANT LOAD FROM DISK CREATE NEW LIST
            playerScores = new List<PlayerScore>();
        }
    }

}


[System.Serializable]
public class PlayerScore : IComparable<PlayerScore> {
    
    private string name;
    private int score;

    public PlayerScore(string newName, int newScore) {
        name = newName;
        score = newScore;
    }

    public int GetScore() {
        return score;
    }

    public string GetName() {
        return name;
    }

    public int CompareTo(PlayerScore other) {
        if (other == null)
        {
            return 1;
        }

        return other.score - score;
    }
}
