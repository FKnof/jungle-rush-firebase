using System;
using System.Collections;
using System.Diagnostics;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player : MonoBehaviour
{

    [SerializeField] private LayerMask platformLayerMask;
    
    
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public Animator animator;
    private float leftEdge;
    public GameManager gameManager;


    public float gravityChange = 1.0f;
    public float jumpForceUp = 8f;
    public float jumpForceRight;
    public float jumpTime = 0.3f;

 
    //public Rigidbody2D connectedBody;
    public DistanceJoint2D hingeJoint;
    public bool isSwinging;
    private float initialZRotation;
    public Collider2D roof;
    public float swingSpeed = 1.3f;
    private LineController lineController;
    public Vector3[] linePoints;
    [SerializeField] private GameObject line;




    [SerializeField] public GameObject waterLayer;
    private float jumpTimeCounter;
    private float jumpForceCurrent;

    private bool isJumping;
    public float jumpReductionFactor = 0.97f;

    private Touch touch;


    public float resistance = 0f;
    private Vector3 constantVelocity;

    //Variablen     für ScreenBoundries
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    public bool hasFallen;
    private AudioManager audioManager;
    private Parallax waterSpeedComponent;



    private void Start()
    {
      
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        waterSpeedComponent = waterLayer.GetComponent<Parallax>();
        boxCollider = GetComponent<BoxCollider2D>();
        Physics.gravity *= gravityChange;
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 5;
        linePoints = new Vector3[2];

    hasFallen = false;
        isSwinging = false;
        jumpTimeCounter = jumpTime;
        
        // Screen Boundries
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

    }


    private void Update()
    {



        ////////
        ///////
        /////------------Keyboard Controls---------------------------------------------------------------------------------



        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {

            jumpForceCurrent = jumpForceUp;
            audioManager.Play("Jump");
            rb.AddForce(new Vector2(0, 9f), ForceMode2D.Impulse);
            isJumping = true;
        }
        //  Sprunghöhe durch Haltezeit bestimmen
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpForceCurrent > 0)
            {

                jumpForceCurrent *= jumpReductionFactor;
                rb.AddForce(new Vector2(0, jumpForceCurrent), ForceMode2D.Impulse);

            }
        }
        else
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isSwinging == false && !IsGrounded() && !hasFallen)
        {
            Vector2 distance = roof.ClosestPoint(transform.position);
            float yDistance = distance.y - transform.position.y;
            audioManager.Play("Swing");
            hingeJoint = gameObject.AddComponent<DistanceJoint2D>();
            hingeJoint.autoConfigureConnectedAnchor = false;
            hingeJoint.connectedAnchor = new Vector2(transform.position.x + 2, transform.position.y + yDistance);
            hingeJoint.anchor = new Vector2(0, 0);
            hingeJoint.enableCollision = true;
            hingeJoint.autoConfigureDistance = false;

            linePoints[0] = transform.position;
            linePoints[1] = hingeJoint.connectedAnchor;

            Instantiate(line);
            lineController = GameObject.Find("LineRenderer(Clone)").GetComponent<LineController>();
            lineController.SetUpLine(linePoints);
            isJumping = false;
            isSwinging = true;

        }
        else if (Input.GetKey(KeyCode.Space) && isSwinging == true && hingeJoint != null)
        {

            hingeJoint.connectedAnchor += Vector2.left * 7f * Time.deltaTime;
            linePoints[0] = transform.position;
            linePoints[1] = hingeJoint.connectedAnchor;
            lineController.SetUpLine(linePoints);
            if (hingeJoint.connectedAnchor.x < -7 && hingeJoint != null)
            {
                Destroy(hingeJoint);
                Destroy(GameObject.Find("LineRenderer(Clone)"));
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space) && isSwinging == true)
        {
            Destroy(hingeJoint);
            Destroy(GameObject.Find("LineRenderer(Clone)"));
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, initialZRotation);
        }
        //-------

        /////////------------Touch Controls---------------------------------------------------------------------------------
        ///

        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && IsGrounded())
        //{
        //    Touch touch = Input.GetTouch(0);
        //    UnityEngine.Debug.Log("Jump");
        //    jumpForceCurrent = jumpForceUp;
        //    audioManager.Play("Jump");
        //    rb.AddForce(new Vector2(0, 9f), ForceMode2D.Impulse);
        //    isJumping = true;
        //}
        ////  Sprunghöhe durch Haltezeit bestimmen
        //if (Input.touchCount > 0 && isJumping == true)
        //{
        //    if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
        //    {

        //        if (jumpForceCurrent > 0)
        //        {
        //            jumpForceCurrent *= jumpReductionFactor;
        //            rb.AddForce(new Vector2(0, jumpForceCurrent), ForceMode2D.Impulse);
        //        }
        //    }
        //}
        //if (touch.phase == TouchPhase.Ended && isJumping == true)

        //{
        //    isJumping = false;
        //}

        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && isSwinging == false && !IsGrounded() && !hasFallen)
        //{
        //    Vector2 distance = roof.ClosestPoint(transform.position);
        //    float yDistance = distance.y - transform.position.y;
        //    audioManager.Play("Swing");
        //    hingeJoint = gameObject.AddComponent<DistanceJoint2D>();
        //    hingeJoint.autoConfigureConnectedAnchor = false;
        //    hingeJoint.connectedAnchor = new Vector2(transform.position.x + 2, transform.position.y + yDistance);
        //    hingeJoint.anchor = new Vector2(0, 0);
        //    hingeJoint.enableCollision = true;
        //    hingeJoint.autoConfigureDistance = false;

        //    linePoints[0] = transform.position;
        //    linePoints[1] = hingeJoint.connectedAnchor;

        //    Instantiate(line);
        //    lineController = GameObject.Find("LineRenderer(Clone)").GetComponent<LineController>();
        //    lineController.SetUpLine(linePoints);
        //    isJumping = false;
        //    isSwinging = true;

        //}
        //else if (Input.touchCount > 0 && isSwinging && hingeJoint != null)
        //{
        //    if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Stationary)
        //    {
        //        hingeJoint.connectedAnchor += Vector2.left * 7f * Time.deltaTime;
        //        linePoints[0] = transform.position;
        //        linePoints[1] = hingeJoint.connectedAnchor;
        //        lineController.SetUpLine(linePoints);
        //        if (hingeJoint.connectedAnchor.x < -7 && hingeJoint != null)
        //        {
        //            Destroy(hingeJoint);
        //            Destroy(GameObject.Find("LineRenderer(Clone)"));
        //        }
        //    }
        //}
        //else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && isSwinging == true)
        //{
        //    Destroy(hingeJoint);
        //    Destroy(GameObject.Find("LineRenderer(Clone)"));
        //    gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, initialZRotation);
        //}

        ////////////
        ///////////
        /////////------------Touch Controls---------------------------------------------------------------------------------
        ///

        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Began && IsGrounded())
        //    {

        //        UnityEngine.Debug.Log("Jump");
        //        jumpForceCurrent = jumpForceUp;
        //        audioManager.Play("Jump");
        //        rb.AddForce(new Vector2(0, 9f), ForceMode2D.Impulse);
        //        isJumping = true;
        //    }
        //    //  Sprunghöhe durch Haltezeit bestimmen

        //    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        //    {
        //        if (isJumping == true && jumpForceCurrent > 0)
        //        {
        //            jumpForceCurrent *= jumpReductionFactor;
        //            rb.AddForce(new Vector2(0, jumpForceCurrent), ForceMode2D.Impulse);
        //        }

        //    }
        //    if (touch.phase == TouchPhase.Ended && isJumping == true)

        //    {
        //        isJumping = false;
        //    }

        //    if (touch.phase == TouchPhase.Began && isSwinging == false && !IsGrounded() && !hasFallen)
        //    {
        //        Vector2 distance = roof.ClosestPoint(transform.position);
        //        float yDistance = distance.y - transform.position.y;
        //        audioManager.Play("Swing");
        //        hingeJoint = gameObject.AddComponent<DistanceJoint2D>();
        //        hingeJoint.autoConfigureConnectedAnchor = false;
        //        hingeJoint.connectedAnchor = new Vector2(transform.position.x + 2, transform.position.y + yDistance);
        //        hingeJoint.anchor = new Vector2(0, 0);
        //        hingeJoint.enableCollision = true;
        //        hingeJoint.autoConfigureDistance = false;

        //        linePoints[0] = transform.position;
        //        linePoints[1] = hingeJoint.connectedAnchor;

        //        Instantiate(line);
        //        lineController = GameObject.Find("LineRenderer(Clone)").GetComponent<LineController>();
        //        lineController.SetUpLine(linePoints);
        //        isJumping = false;
        //        isSwinging = true;

        //    }
        //    else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        //    {
        //        if (isSwinging && hingeJoint != null)
        //        {
        //            hingeJoint.connectedAnchor += Vector2.left * 7f * Time.deltaTime;
        //            linePoints[0] = transform.position;
        //            linePoints[1] = hingeJoint.connectedAnchor;
        //            lineController.SetUpLine(linePoints);
        //            if (hingeJoint.connectedAnchor.x < -7 && hingeJoint != null)
        //            {
        //                Destroy(hingeJoint);
        //                Destroy(GameObject.Find("LineRenderer(Clone)"));
        //            }
        //        }

        //    }
        //    else if (touch.phase == TouchPhase.Ended && isSwinging == true)
        //    {
        //        Destroy(hingeJoint);
        //        Destroy(GameObject.Find("LineRenderer(Clone)"));
        //        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, initialZRotation);
        //    }
        //}

        if (IsGrounded())
            {
                animator.SetBool("grounded", IsGrounded());
                isSwinging = false;

                //transform.position += constantVelocity * Time.deltaTime;

            }




            //Transition jump to run animation

            animator.SetBool("grounded", IsGrounded());
        }

        private void LateUpdate()
        {

            if (hasFallen)
            {
                rb.velocity = new Vector2(waterSpeedComponent.moveSpeed, 0);
                if (transform.position.x < leftEdge)
                {
                    Destroy(gameObject);

                }
            }
            else {

                ////--- Player im Screen halten

                Vector3 viewPos = transform.position;
                viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
                viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
                transform.position = viewPos;


            }


        }



        ////-- Funktion ermittelt Bodenkontakt des Players
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Big Ground_1") || collision.gameObject.CompareTag("Big Ground_2") || collision.gameObject.CompareTag("Small Ground"))
            {
                audioManager.Play("Impact");
                jumpTimeCounter = jumpTime;

                jumpForceCurrent = jumpForceUp;

                isJumping = false;

            }

            if (hingeJoint != null)
            {
                Destroy(hingeJoint);
                Destroy(GameObject.Find("LineRenderer(Clone)"));
            }

            if (collision.gameObject.CompareTag("Water") && !hasFallen)
            {
                hasFallen = true;

                animator.SetTrigger("waterTouched");

                //rb.AddForce(Vector2.left * 2, ForceMode2D.Force);

                //UnityEngine.Debug.Log("Water touched");
                isSwinging = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
                if (hingeJoint != null)
                {
                    Destroy(hingeJoint);
                    Destroy(GameObject.Find("LineRenderer(Clone)"));
                }

                gameManager.GameOver();
            }

        }
    


    private bool IsGrounded()
    {
        float extraHeight = 0.025f;
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeight, platformLayerMask);

        Color rayColor;
        if (hit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        UnityEngine.Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraHeight), rayColor);
        UnityEngine.Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraHeight), rayColor);
        UnityEngine.Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y + extraHeight), Vector2.right * (boxCollider.bounds.extents.x), rayColor);


        return hit.collider != null;
          }
}