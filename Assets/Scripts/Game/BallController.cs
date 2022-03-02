using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;


    private float speedUp = 0.0f;                       //      min speed up
    private float maxSpeedUp = 60.0f;                  
    private float speedForward = 0.0f;                  //      min speed forward
    private float maxSpeedForward = 30.0f;              

    private float tSF = 0.0f, tSU = 0.0f;               
 
    public bool isOnGround;     
    public int pointValue;      // score value

    
    public GameObject BasketRing;       
    private GameManager gameManager;


    public ParticleSystem accessBasket;
    private AudioSource ballAudio;
    private Rigidbody BallRb;

    public AudioClip checkColBasket;
    public AudioClip dropBall;

    public float scrollForceBar;        // force bar value

    void Start()
    {
        BallRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        ballAudio = GetComponent<AudioSource>();
        
    }
  
    void Update()
    {
        BallJump();
    }

    void BallJump()     // Ball Controller
    {
        if (gameManager.isGameActive && isOnGround == true)
        {

            BallRb.isKinematic = true;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);         //  Moving ball towards the mouse cursor
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))     // LayerMask = ground
            {
                transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y + 2, raycastHit.point.z);
            }
            if (Input.GetMouseButton(0))        // Hold LMB to get throw force
            {
                tSU += Time.deltaTime * 30;
                tSF += Time.deltaTime * 15;
                speedUp = Mathf.PingPong(tSU, maxSpeedUp);
                speedForward = Mathf.PingPong(tSF, maxSpeedForward);
                scrollForceBar = speedForward / 30;

            }
            if (Input.GetMouseButtonUp(0))      // Let go of LMB to apply a throw towards the basket
            {
                tSU = 0.0f; tSF = 0.0f;
                isOnGround = false;
                BallRb.isKinematic = false;
                ballAudio.PlayOneShot(dropBall);
                BallRb.AddForce(Vector3.up * speedUp, ForceMode.Impulse);
                BallRb.AddForce(RingPosition() * speedForward, ForceMode.Impulse);
                BallRb.AddTorque(new Vector3(1, 1, 0) * 400);
            }
        }
    }

    private void OnCollisionEnter(Collision other)      
    {
       if (other.gameObject.CompareTag("Ground"))       // if ball on ground => is onGround true
       {
            isOnGround = true;                          
       }
    }

    private void OnTriggerEnter(Collider other)         
    {
        if (other.gameObject.CompareTag("Basket"))      // if ball hit the basket trigger
        {
            isOnGround = true;                          //  ball on ground
            ballAudio.PlayOneShot(checkColBasket);
            accessBasket.Play();
            pointValue += 2;                            // score Update
        }
            
    }


    Vector3 RingPosition()      // Ball movement in the air
    {
        Vector3 ringPosition = (BasketRing.transform.position - transform.position).normalized;
        return ringPosition;
    }

}
