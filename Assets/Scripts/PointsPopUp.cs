using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;




public class PointsPopUp : MonoBehaviour
{
    private TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();

    }


    public void Setup(int points)
    {
        textMesh.SetText(points.ToString());

    }

    private void Update()
    {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
