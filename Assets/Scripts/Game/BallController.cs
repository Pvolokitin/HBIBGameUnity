using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;


    private float speedUp = 0.0f;                       //      ��������� �������� ������ �����
    private float maxSpeedUp = 60.0f;                   //      ������������ �������� ������ �����
    private float speedForward = 0.0f;                  //      ��������� �������� ������ ������
    private float maxSpeedForward = 30.0f;              //      ������������ �������� ������ ������

    private float tSF = 0.0f, tSU = 0.0f;               
 
    public bool isOnGround;     //  �������� �� ����� �� ���
    public int pointValue;      //  ���������� ���������� �� ����

    
    public GameObject BasketRing;       // ������� ������ �������
    private GameManager gameManager;


    public ParticleSystem accessBasket;
    private AudioSource ballAudio;
    private Rigidbody BallRb;

    public AudioClip checkColBasket;
    public AudioClip dropBall;

    public float scrollForceBar;

    void Start()
    {
        BallRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        ballAudio = GetComponent<AudioSource>();
        
    }
  
    void Update()
    {
        BallJump();
        //Debug.Log($"�������� ������ - {speedForward}, �������� ��� ��������� - {scrollForceBar}");
    }

    void BallJump()     // ���������� �������
    {
        if (gameManager.isGameActive && isOnGround == true)
        {

            BallRb.isKinematic = true;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);         //  ���������� ����� �� ���������� �����
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y + 2, raycastHit.point.z);
            }
            if (Input.GetMouseButton(0))        // ���������� ��� ��� �������� ������
            {
                tSU += Time.deltaTime * 30;
                tSF += Time.deltaTime * 15;
                speedUp = Mathf.PingPong(tSU, maxSpeedUp);
                speedForward = Mathf.PingPong(tSF, maxSpeedForward);
                scrollForceBar = speedForward / 30;

            }
            if (Input.GetMouseButtonUp(0))      // ��������� ��� ��� ���������� ������ � ������� �������
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

    private void OnCollisionEnter(Collision other)      //  ���������� � �������� ���� Ground
    {
       if (other.gameObject.CompareTag("Ground"))
       {
            isOnGround = true;                          //  ��������� ������ ������ ��� �������� �� ����� �� ���
       }
    }

    private void OnTriggerEnter(Collider other)         //  �������� ������� � �������� ���� Basket (������ � ������)
    {
        if (other.gameObject.CompareTag("Basket"))
        {
            isOnGround = true;                          //  ��������� ������ ������ ��� �������� �� ����� �� ���
            ballAudio.PlayOneShot(checkColBasket);
            accessBasket.Play();
            pointValue += 2;                            // ��������� 2 ���� �� ���������
        }
            
    }


    Vector3 RingPosition()      // ����������� ���� � ������� �� ������
    {
        Vector3 ringPosition = (BasketRing.transform.position - transform.position).normalized;
        return ringPosition;
    }

}
