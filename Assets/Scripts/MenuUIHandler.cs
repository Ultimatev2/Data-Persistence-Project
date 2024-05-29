using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]

public class MenuUIHandler : MonoBehaviour
{
    public string playerName;
    public TMP_InputField InputField;
    public static MenuUIHandler Instance;
    public TMP_Text highScoreText;
    private static string _highScoreName;
    private static int _HighScore;
    private void Awake()
    {
        // start of new code
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code
        LoadHighScore();
        highScoreText.text = GetHighScoreText();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GetNameFromInput()
    {
        playerName = InputField.text;
    }

    public string GetPlayerName()
    {
        return playerName;
    }
    
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Exit()
    {
    #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
    #else
        Application.Quit(); // original code to quit Unity player
    #endif
    }
    
    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int HighScore;
    }

    public static void SaveHighScore(string playerName, int HighScore)
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.HighScore = HighScore;
        string json = JsonUtility.ToJson(data);
        
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            _highScoreName = data.playerName;
            _HighScore = data.HighScore;
            highScoreText.text = GetHighScoreText();
        }
    }

    public string GetHighScoreText()
    {
        return $"Best Score : {_highScoreName} : {_HighScore}"; 
    }

    public void SetNewHigh(string nameOfGod, int godScore)
    {
        _highScoreName = nameOfGod;
        _HighScore = godScore;
    }
}