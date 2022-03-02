using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorOfForce : MonoBehaviour
{
    public Scrollbar bar;
    public float ballSpeed;
    private BallController _ballController;
    
    void Start()
    {
        _ballController = GameObject.Find("Ball").GetComponent<BallController>();
    }

    
    void Update()
    {
        ballSpeed = _ballController.scrollForceBar;
        bar.size = ballSpeed;           // Change size bar of throw force from values
    }
}
