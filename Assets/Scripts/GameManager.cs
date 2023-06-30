using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    
    public Player player;


    public GameObject gameOverCanvas;
    public TextMeshProUGUI firstScoreText;
    public TextMeshProUGUI secondScoreText;
    public TextMeshProUGUI thirdScoreText;
    private AudioManager audioManager;
    [SerializeField] public Collect collect;
    private string scoreKey;


    //public GameObject gameOverSign;



    public Animator transition;

    private float transitionTime;
    

    private void Awake()
    {
        Application.targetFrameRate = 60;



        //SceneManager.UnloadSceneAsync("Game");
        //Pause();

    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
    }


    public void Play()
    {
        audioManager.Play("Button 1");
        audioManager.Stop("Wind");
        Time.timeScale = 1f;
        StartCoroutine(LoadLevel("Game", 5));
      
        //gameOverCanvas.SetActive(false);
    }

    public void GoToMainMenu()
    {
        audioManager.Play("Button 3");
    
        StartCoroutine(LoadLevel("Main Menu", 0));

    }

    public void BackToMainMenu()
    {
        audioManager.Play("Button 3");
        audioManager.Fade("Intro", "Intro");
        StartCoroutine(LoadLevel("Main Menu", 0));
        audioManager.Play("Wind");

    }

    public void GoToScores()
    {
        audioManager.Play("Button 2");
        StartCoroutine(LoadLevel("Scores", 0));

    }

    public void GameOver()
    {
        StartCoroutine(StopMusic());
        
        audioManager.Play("GameOver");

        CheckHighScore(Collect.score);
        UpdateHighScoreText(thirdScoreText, "ThirdScore");
        UpdateHighScoreText(secondScoreText, "SecondScore");
        UpdateHighScoreText(firstScoreText, "FirstScore");
        gameOverCanvas.SetActive(true);
        Pause();



    }
    IEnumerator StopMusic()
    {
        audioManager.StopRunning();
        yield return new WaitForSeconds(2);


        }

   

    public void Pause()
    {
        //Time.timeScale = 0f;
        //player.enabled = false;

    }
    

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    void CheckHighScore(int score)
    {


        if (score > PlayerPrefs.GetInt("ThirdScore", 0) && score > PlayerPrefs.GetInt("SecondScore", 0) && score > PlayerPrefs.GetInt("FirstScore", 0) )
        {
            PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
            PlayerPrefs.SetInt("SecondScore", PlayerPrefs.GetInt("FirstScore"));
            PlayerPrefs.SetInt("FirstScore", score);

        }
        else if (score > PlayerPrefs.GetInt("ThirdScore", 0) && score > PlayerPrefs.GetInt("SecondScore", 0) && score < PlayerPrefs.GetInt("FirstScore", 0))
        {
            PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
            PlayerPrefs.SetInt("SecondScore", score);

        }
        else if (score > PlayerPrefs.GetInt("ThirdScore", 0) && score < PlayerPrefs.GetInt("SecondScore", 0) && score < PlayerPrefs.GetInt("FirstScore", 0))
        {
            PlayerPrefs.SetInt("ThirdScore", score);
        }

    }


    public static void UpdateHighScoreText(TextMeshProUGUI scoreText, string scoreKey)
    {
        scoreText.text = PlayerPrefs.GetInt(scoreKey, 0).ToString();
    }


    IEnumerator LoadLevel(string scene, int transitionTime)
    {

        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("StartTransition");

        yield return new WaitForSeconds(transition.GetCurrentAnimatorStateInfo(0).length);

        SceneManager.LoadScene(scene);
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }


}
