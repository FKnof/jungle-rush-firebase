using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] bool scrollLeft;
    [SerializeField] Player player;
    public Collect collect;
    private float delta;



    float singleTextureWidth;

    private void Start()
    {

        SetupTexture();

        if (scrollLeft)
        {
            moveSpeed = -moveSpeed;
                }
    }

    void SetupTexture()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;

    }

    void Scroll()
    {


        if (player.isSwinging)
        {
            delta = (moveSpeed - collect.levelSpeed) * player.swingSpeed * Time.deltaTime;
        }
        else
        {
            delta = (moveSpeed - collect.levelSpeed) * Time.deltaTime;
            
        }
        transform.position += new Vector3(delta, 0f, 0f);

    }

    void CheckReset()
    {
        if ((Mathf.Abs(transform.position.x) - singleTextureWidth) > 0)
        {
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);

        }
    }


    private void Update()
    {

        Scroll();
        CheckReset();

    }

}
