using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadScores : MonoBehaviour
{
    public TextMeshProUGUI firstScoreText;
    public TextMeshProUGUI secondScoreText;
    public TextMeshProUGUI thirdScoreText;

    private void Awake()
    {
        GameManager.UpdateHighScoreText(thirdScoreText, "ThirdScore");
        GameManager.UpdateHighScoreText(secondScoreText, "SecondScore");
        GameManager.UpdateHighScoreText(firstScoreText, "FirstScore");
    }


}
