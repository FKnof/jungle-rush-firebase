using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{

    //public AnimationClip[] animations;
    private Text text;
    private Animator animation;
    private Collect collect;
    private int level;

    void Start()
    {
        
        collect = GameObject.Find("Player").GetComponent<Collect>();
        level = collect.level;
        text = GetComponent<Text>();
        animation = GetComponent<Animator>();
        text.text = "Level " + level;

        animation.SetInteger("level", level);
    }
}

