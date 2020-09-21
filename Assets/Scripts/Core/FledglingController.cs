using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FledglingController : MonoBehaviour
{
    [Tooltip("The fledgling will track the position of this GameObject")]
    public GameObject birdToFollow;

    [Tooltip("The speed at which the fledgling moves")]
    public float speed = 5.0f;

    [Tooltip("The fledgling will track a random position within this "
             + "radius of the Bird to Follow")]
    public float followRadius = 1.0f;

    [Tooltip("Amount of time the fledgling waits to find a new position "
             + "after getting to an old one [s]")]
    public float stallTime = 0.5f;


    private bool _wayPointIsSelected;
    private bool _isStalling;
    private float _waypointArriveTime;
    private Vector3 _waypoint;
    private Rigidbody2D _rigidbody;
    private const float DISTANCE_THRESHOLD = 0.1f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!_isStalling)
        {
            MoveToWayPoint();
        }
        else if (Time.fixedTime - _waypointArriveTime > stallTime)
        {
            _waypoint = ChooseWayPoint();
        }
    }

    private Vector3 ChooseWayPoint()
    {
        _isStalling = false;
        float distance = Random.Range(0, followRadius);
        float angle = Random.Range(0, 2*Mathf.PI);
        Vector3 displacement = new Vector3(distance*Mathf.Cos(angle),
                                           distance*Mathf.Sin(angle),
                                           0);
        return birdToFollow.transform.position + displacement;
    }

    private void MoveToWayPoint()
    {
        if (Vector3.Distance(transform.position, _waypoint)
            > DISTANCE_THRESHOLD)
        {
            Vector2 velocity = speed 
                * ((Vector2) (_waypoint - transform.position)).normalized;
            Debug.Log(velocity);
            _rigidbody.AddForce(velocity);
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
            _waypointArriveTime = Time.fixedTime;
            _isStalling = true;
        }
    }
}
