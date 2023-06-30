using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private Player player;




    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        lr = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()

    {

        for (int i = 0; i < player.linePoints.Length; i++)
        {
            lr.SetPosition(i, player.linePoints[i]);

        }

    }

    public void SetUpLine(Vector3[] points)
    {
        lr.positionCount = points.Length;
        //this.linePoints = points;

    }

}
