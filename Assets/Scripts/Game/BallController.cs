using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;


    private float speedUp = 0.0f;                       //      Начальная скорость полета вверх
    private float maxSpeedUp = 60.0f;                   //      Максимальная скорость полета вверх
    private float speedForward = 0.0f;                  //      Начальная скорость полета вперед
    private float maxSpeedForward = 30.0f;              //      Максимальная скорость полета вперед

    private float tSF = 0.0f, tSU = 0.0f;               
 
    public bool isOnGround;     //  проверка на земле ли мяч
    public int pointValue;      //  переменная отвечающая за очки

    
    public GameObject BasketRing;       // Игровой объект корзина
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
        //Debug.Log($"Скорость вперед - {speedForward}, Скорость для скролБара - {scrollForceBar}");
    }

    void BallJump()     // Управление мячиком
    {
        if (gameManager.isGameActive && isOnGround == true)
        {

            BallRb.isKinematic = true;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);         //  Перемещаем мячик за указателем мышки
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y + 2, raycastHit.point.z);
            }
            if (Input.GetMouseButton(0))        // Удерживаем ЛКМ для усиления броска
            {
                tSU += Time.deltaTime * 30;
                tSF += Time.deltaTime * 15;
                speedUp = Mathf.PingPong(tSU, maxSpeedUp);
                speedForward = Mathf.PingPong(tSF, maxSpeedForward);
                scrollForceBar = speedForward / 30;

            }
            if (Input.GetMouseButtonUp(0))      // Отпускаем ЛКМ для применения броска в сторону корзины
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

    private void OnCollisionEnter(Collision other)      //  Столкнулся с объектом тега Ground
    {
       if (other.gameObject.CompareTag("Ground"))
       {
            isOnGround = true;                          //  Присвоили булево правда для проверки на земле ли мяч
       }
    }

    private void OnTriggerEnter(Collider other)         //  сработал триггер с объектом тега Basket (забили в кольцо)
    {
        if (other.gameObject.CompareTag("Basket"))
        {
            isOnGround = true;                          //  Присвоили булево правда для проверки на земле ли мяч
            ballAudio.PlayOneShot(checkColBasket);
            accessBasket.Play();
            pointValue += 2;                            // Добавляем 2 очка за попадание
        }
            
    }


    Vector3 RingPosition()      // Направление мяча в воздухе до кольца
    {
        Vector3 ringPosition = (BasketRing.transform.position - transform.position).normalized;
        return ringPosition;
    }

}
