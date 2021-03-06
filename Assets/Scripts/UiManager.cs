﻿using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public GameObject winningScreen;
    public Button restartButton;

    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
    }

    public void DisplayWinningScreen()
    {
        winningScreen.SetActive(true);
    }

    private void RestartGame()
    {
        winningScreen.SetActive(false);
        GameManager.Instance.StartNewGame();
    }
}
