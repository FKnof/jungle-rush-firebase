using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Collect : MonoBehaviour
{
    public int collectorCounter;
    static public int score;
    public int level;
    private AudioManager aManager;
    [SerializeField] private Text scoreText;
    [SerializeField] private Transform pointsPrefab;
    private AudioClip levelchange;
    Player player;
    public GameObject levelChangeText;
    public float durationSpeed;
    public float timeElapsed = 0f;

    public float levelSpeed;





    private void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();

        score = 1;
        collectorCounter = 1;
        aManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        aManager.Fade("Intro", "Level 1");
        level = 1;
        ShowLevelChangeText();

        foreach (Sound s in aManager.sounds)
        {
            if(s.name == "Level Change")
            {
                levelchange = s.source.clip;
            }
        }
    }

    
    private void Update()
       
    {
        Debug.Log(Time.timeSinceLevelLoad);
        if (Time.timeSinceLevelLoad > 63 && level == 1 && !player.hasFallen)
        {
            Destroy(GameObject.Find("Score_Canvas/LevelChange(Clone)"));
            level = 2;
            ShowLevelChangeText();


            aManager.StopRunning();
            aManager.Play("Level Change");
            //parallax.levelSpeed = durationSpeed;
            //platforms.levelSpeed = durationSpeed;

            Invoke("SongLevel3", levelchange.length);

            
        }
        else if (Time.timeSinceLevelLoad > 126 && level == 2 && !player.hasFallen)
        {
            Destroy(GameObject.Find("Score_Canvas/LevelChange(Clone)"));
            level = 3;
            ShowLevelChangeText();

            aManager.StopRunning();
            aManager.Play("Level Change");

            Invoke("SongLevel4", levelchange.length);
           
        }
        else if (Time.timeSinceLevelLoad > 223 && level == 3 && !player.hasFallen)
        {
            Destroy(GameObject.Find("Score_Canvas/LevelChange(Clone)"));
            level = 4;
            ShowLevelChangeText();

            aManager.StopRunning();
            aManager.Play("Level Change");
            Invoke("SongLevel5", levelchange.length);
          
        }
        //else if (Time.timeSinceLevelLoad > 310 && level == 4 && !player.hasFallen)
        //{
     //ggf. Song Level 2
        //}

    }
    private void LateUpdate()
    {
        

        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 6f && durationSpeed < 4.46f)
        {
            durationSpeed += 0.1f;
            timeElapsed = 0f;
        }
    }

    void ShowLevelChangeText()
    {

        Instantiate(levelChangeText, GameObject.FindGameObjectWithTag("Score_Canvas").transform, false);
        //Instantiate(levelChangeText, new Vector3(320, 287.7f, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Score_Canvas").transform); 
    }

    void SongLevel2()
    {
        aManager.Play("Level 2");

    }

    void SongLevel3()
    {
        if (!player.hasFallen)
        {
            aManager.Play("Level 3");

            levelSpeed = durationSpeed;
        }

    }

    void SongLevel4()
    {
        if (!player.hasFallen)
        {
            aManager.Play("Level 4");
            levelSpeed = durationSpeed;
        }
    }
    void SongLevel5()
    {
        if (!player.hasFallen)
        {
            aManager.Play("Boss");
            levelSpeed = durationSpeed;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Banana"))
        {
            aManager.Play("Banana");
            Destroy(collision.gameObject);


            collectorCounter++;
            score += collectorCounter;

            scoreText.text = score.ToString();

            CreatePopUp(pointsPrefab, collision.transform.position, collectorCounter);
        }
    }


    public static PointsPopUp CreatePopUp(Transform pointsPrefab, Vector3 collector, int points)
    {
        Transform pointsPopUpTransform = Instantiate(pointsPrefab, collector, Quaternion.identity);
        PointsPopUp pointsPopUp = pointsPopUpTransform.GetComponent<PointsPopUp>();
        pointsPopUp.Setup(points);

        return pointsPopUp;

    }


}
