using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject loseCanvas;

    private void Start()
    {
        loseCanvas.SetActive(false);
        Play();
    }

    void Update()
    {
        if(_player == null)
        {
            loseCanvas.SetActive(true);
            Pause();
        }
    }

    public void BeginGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Play()
    {
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
