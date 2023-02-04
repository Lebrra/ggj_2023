using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameStateManager : MonoBehaviour
{
    public static Action OnGameOver;  //You can ignore this for now - we will talk about Actions a bit later in this course.
    [field: SerializeField]
    public float TimeForTheGame { get; set; }
    [SerializeField]
    public GameObject m_Sunlight;
    [SerializeField]
    public float m_MaxSunRotation;
    [SerializeField]
    public GameObject gameOverScreen;
    [SerializeField]
    public GameObject winText;
    [SerializeField]
    public GameObject loseText;

    public static float CurrentNormalizedGameTime { get; private set; }

    private static GameStateManager _instance; //This class is a Singleton - We will also discuss this pattern later in this class.
    private static GAME_STATE currentGameState;

    private static float m_elapsedTime;

    public enum GAME_STATE
    {
        PLAYING,
        PAUSED,
        GAMEOVER
    };

    public void SwitchGameState(GAME_STATE newState)
    {
        currentGameState = newState;
        switch(currentGameState)
        {
            case GAME_STATE.PLAYING:
                Time.timeScale = 1;
                break;
            case GAME_STATE.PAUSED:
                Time.timeScale = 0;
                break;
            case GAME_STATE.GAMEOVER:
                break;
            default:
                Debug.LogError("UNKNOWN GAMESTATE!");
                break;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        //Setup for making this class a Singlton - Don't modify this part of the code.
        if (_instance == null)
        {
            _instance = this;

           // DontDestroyOnLoad(_instance);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        currentGameState = GAME_STATE.PLAYING;
        m_elapsedTime = 0;
        Time.timeScale = 1;


    }

    public void Update()
    {
        if (m_elapsedTime >= TimeForTheGame)
        {
            GameOver(true);
            return;
        }

        if(currentGameState == GAME_STATE.GAMEOVER)
        {
            return;
        }


        m_elapsedTime += Time.deltaTime;
        CurrentNormalizedGameTime = m_elapsedTime / TimeForTheGame;
        m_Sunlight.transform.localEulerAngles = new Vector3(CurrentNormalizedGameTime * m_MaxSunRotation, 0, 0);

      
    }


    //These two methods help up to handle the Game being over and restarting. 
    public static void GameOver(bool win)
    {
        _instance.SwitchGameState(GAME_STATE.GAMEOVER);
        //Add any logic that you would want to do when the game ends here

        //This invokes the game over screen - here we are calling all the methods that subscribed to this action.
        OnGameOver?.Invoke();
        Time.timeScale = 0;
        _instance.gameOverScreen.SetActive(true);
        if (win) _instance.winText.SetActive(true);
        else _instance.loseText.SetActive(true);
        
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public static void Restart()
    {

        Time.timeScale = 1;
        currentGameState = GAME_STATE.PLAYING;
        SceneManager.LoadScene("MainGameScene");
    }

}
