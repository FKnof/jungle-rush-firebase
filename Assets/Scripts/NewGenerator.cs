using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class NewGenerator : MonoBehaviour
{
    public GameObject[] platformPrefabs;
    public GameObject collectorPrefab;
    private GameObject lastSpawned;
    private float randomSpawnValue;
    private float randomXposition;
    private float randomYposition;
    private Vector2 closestPoint;
    private Vector3 collectorPosition;
    public float platformYposition;
    [SerializeField] Player player;
    [SerializeField] Collect collect;

    private int randomIndex;

    public float spawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 1f;
    public int minXdistance = 2;
    public int maxXdistance = 7;
    public float minYdistance = 1f;
    public float maxYdistance = 2f;

    

    private void Start()
    {
        //Speicher die erste Plattform als "lastSpawned"
        lastSpawned = GameObject.Find("Platform 1");

        //definiert den Abstand der ersten gespawnten Plattform
        randomSpawnValue = Random.Range(minXdistance, maxXdistance);

    }

    private void Update()
    {
        //Ermittle den rechten Rand der letzten Plattform
        Collider2D lastSpawnedCollider = lastSpawned.GetComponent<Collider2D>();
        float rightCorner = lastSpawned.transform.position.x + lastSpawnedCollider.bounds.extents.x;

        //Ermittle den Abstand der letzten Plattform und dem Spawner
        float distance = transform.position.x - rightCorner;


        //Wenn der Abstand größer ist als der definierte Wert kann der Plattform gespawnt werden
        //Überschneidungen werden somit vermieden
        if (distance > randomSpawnValue)
        {
            //Zufällige Plattform wird ausgesucht (randomIndex)
            randomIndex = Random.Range(0, platformPrefabs.Length);
            
            //Plattform wird instanziiert
            GameObject platform = Instantiate(platformPrefabs[randomIndex], transform.position, Quaternion.identity);
            platform.GetComponent<Platforms>().player = player;
            platform.GetComponent<Platforms>().collect = collect;
            Collider2D platformCollider = platform.GetComponent<Collider2D>();
          
            //gespawnte Plattform wird um die Hälfte der größe nach rechts verschoben
            translatePlatform(platform, platformCollider);
            lastSpawned = platform;
            randomSpawnValue = Random.Range(minXdistance, maxXdistance);
            
            spawnCollector(platformCollider);
            
        }





    }

    //gespawnte Plattform wird um die Hälfte der größe nach rechts verschoben
    private void translatePlatform(GameObject platform, Collider2D platformCollider)
    {
        switch (platform.tag)
        {
            case "Small Ground":
                platformYposition = Random.Range(minHeight, maxHeight);
                platform.transform.position += new Vector3(platformCollider.bounds.extents.x, platformYposition);
                break;
            case "Big Ground_1":
                platformYposition = -0.5f;
                platform.transform.position += new Vector3(platformCollider.bounds.extents.x, platformYposition);

                break;
            case "Big Ground_2":
                platformYposition = -0.9f;
                platform.transform.position += new Vector3(platformCollider.bounds.extents.x, platformYposition);
                break;

            default:
                break;
        }
    }




    private void spawnCollector(Collider2D platformCollider)
    {
        //Ermittle random X Position  (Anfang des Colliders bis Ende + randomSpawnValue)
        randomXposition = Random.Range(transform.position.x, transform.position.x + platformCollider.bounds.size.x + randomSpawnValue);



        //Ermittle random Y Position
        //closestPoint = platformCollider.ClosestPoint(new Vector2(randomXposition, transform.position.y + 1));
        //randomYposition = Random.Range(minYdistance, maxYdistance);

        randomYposition = (platformCollider.bounds.max.y + platformYposition) + Random.Range(minYdistance, maxYdistance);


        collectorPosition = new Vector3(randomXposition, randomYposition, 0);
        GameObject collectorObject = Instantiate(collectorPrefab, collectorPosition, Quaternion.identity);
        collectorObject.GetComponent<Platforms>().player = player;
        collectorObject.GetComponent<Platforms>().collect = collect;

        //Debug.DrawLine(new Vector3(randomXposition, transform.position.y + 1, 0), new Vector3(closestPoint.x, closestPoint.y, 0));


    }
}