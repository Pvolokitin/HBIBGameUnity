using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorOfForce : MonoBehaviour
{
    public Scrollbar bar;
    public float ballSpeed;
    private BallController _ballController;
    // Start is called before the first frame update
    void Start()
    {
        _ballController = GameObject.Find("Ball").GetComponent<BallController>();
        

    }

    // Update is called once per frame
    void Update()
    {
        ballSpeed = _ballController.scrollForceBar;
        //Debug.Log(ballSpeed);
        bar.size = ballSpeed;
    }
}
