using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text coinsText;
    private void Start()
    {
        int coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(0);
    }
}
