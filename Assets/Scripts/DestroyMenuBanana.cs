using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMenuBanana : MonoBehaviour
{
    [SerializeField] private Transform pointsPrefab;
    private Collider2D BananaCollider;
    private AudioManager audioManager;


    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BananaCollider = GetComponent<Collider2D>();
        audioManager.Play("Banana");
        Destroy(gameObject);

        CreatePopUp(pointsPrefab, BananaCollider.transform.position, 1);

    }


    public static Transform CreatePopUp(Transform pointsPrefab, Vector3 collector, int points)
    {
        Transform pointsPopUpTransform = Instantiate(pointsPrefab, collector, Quaternion.identity,GameObject.FindGameObjectWithTag("Main Menu").transform);

        return pointsPopUpTransform;
    }
}
