using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platforms : MonoBehaviour
{
    public float speed = -2.5f;
    private float leftEdge;
    public Player player;
    public Collect collect;
    private float delta;

    private void Start()

    {
        player = Object.FindObjectOfType<Player>();
        collect = player.GetComponent<Collect>();

        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 20;
    
    }

    private void Update()
    {

        
        if (player.isSwinging)
        {
            delta = (speed - collect.levelSpeed) * player.swingSpeed * Time.deltaTime;
        }
        else
        {
            delta = (speed - collect.levelSpeed) * Time.deltaTime;

        }

        transform.position += new Vector3(delta, 0f, 0f);

        if(transform.position.x < leftEdge)
        {
            Destroy(gameObject);

        }

    }

}
