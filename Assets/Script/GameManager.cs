using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header ("Play")]
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    [SerializeField] private GameObject startingPanel;
    private float currentTime;
    private bool play = false;

    [Header ("Win")]
    [SerializeField] private GameObject winningPanel;
    [SerializeField] private TMPro.TextMeshProUGUI winText;
    [SerializeField] private TMPro.TextMeshProUGUI recordText;
    private float bestTime;

    [Header ("References")]
    [SerializeField] private List<Vehicle> allVehicles;
    [SerializeField] private List<Station> allStations;
    [SerializeField] private Menu menu;

    private void Start()
    {
        Time.timeScale = 0;
        play = false;
        currentTime = 0f;
        startingPanel.SetActive(true);
        winningPanel.SetActive(false);
        bestTime = PlayerPrefs.GetFloat("BestTime", 9999.0f);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (play)
        {
            currentTime += Time.deltaTime;
            timerText.text = Mathf.Floor(currentTime / 60) + ":" + Mathf.RoundToInt(currentTime % 60);
        }
            
    }

    public void StartTimer()
    {
        Time.timeScale = 1;
        play = true;
        startingPanel.SetActive(false);
    }

    public void PauseGame()
    {
        play = false;
        Time.timeScale = 0;
    }

    public void Continue()
    {
        play = true;
        Time.timeScale = 1;
    }

    public void CheckWin()
    {
        play = false;
        Time.timeScale = 0;
        winningPanel.SetActive(true);
        if(currentTime >= 120.0f)
        {
            winText.text = "You're late! Well we couldn't fired you anyway\n Your time is " + Mathf.Floor(currentTime / 60) + ":" + Mathf.RoundToInt(currentTime % 60);

        }
        else if(currentTime >= 60.0f)
        {
            winText.text = "You're on time! Well, I'll keep my eyes on you..\n Your time is " + Mathf.Floor(currentTime / 60) + ":" + Mathf.RoundToInt(currentTime % 60);
        }
        else if(currentTime >= 30.0f)
        {
            winText.text = "Oh! You're quite fast, huh? Unfortunately, we don't give bonus for that\n Your time is " + Mathf.Floor(currentTime / 60) + ":" + Mathf.RoundToInt(currentTime % 60);
        }
        else
        {
            winText.text = "What! How did you.. Can't believe you arrived here first\n Your time is " + Mathf.Floor(currentTime / 60) + ":" + Mathf.RoundToInt(currentTime % 60);
        }
        if (currentTime <= bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }
        recordText.text = "Your best time is " + +Mathf.Floor(bestTime / 60) + ":" + Mathf.RoundToInt(bestTime % 60);

    }

    public void Restart()
    {
        PlayerController.playerInstance.ResetPosition();
        foreach(Vehicle v in allVehicles)
        {
            v.ResetPosition();
        }
        foreach(Station s in allStations)
        {
            s.ReloadVehicle();
        }
        menu.CloseTutorial();
        Start();
        
    }
}
