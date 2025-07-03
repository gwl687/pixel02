using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class overSceneController : MonoBehaviour
{
    public GameObject liveText;
    void Start()
    {
        int minutes = Mathf.FloorToInt(PlayerPrefs.GetFloat("survivedTime", 0)) / 60;
        int seconds = Mathf.FloorToInt(PlayerPrefs.GetFloat("survivedTime", 0)) % 60;
        liveText.GetComponent<Text>().text = $"YOU SURVIVED:   {minutes:D2}:{seconds:D2}";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void game_restart()
    {
        SceneManager.LoadScene("gameScene");
    }
    public void backToMenu()
    {
        SceneManager.LoadScene("startScene");
    }
    public void game_quit()
    {
        Application.Quit();
    }
}
