using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    [SerializeField] Text recordText;
    private void Start()
    {
        int lastRunScore = PlayerPrefs.GetInt("lastRunScore");
        int recordScore = PlayerPrefs.GetInt("recordScore");

        if (lastRunScore > recordScore)
        {
            recordScore = lastRunScore;
            PlayerPrefs.SetInt("recordScore", recordScore);
            recordText.text = recordScore.ToString();
        }
        else
        {
            recordText.text = recordScore.ToString();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}
