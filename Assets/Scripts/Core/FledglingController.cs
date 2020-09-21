using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Add this component to a GameObject to make it automatically follow
/// the position of another GameObject birdToFollow.  It selects a
/// position to go to that's within a radius followRadius of
/// birdToFollow, then travels with constant acceleration to that point.
/// It then stalls for stallTime seconds.  If it's unable to get to the
/// target point, it'll select a new target after timeoutTime seconds.
/// </summary>
class FledglingController : MonoBehaviour
{
    [Tooltip("The fledgling will track the position of this GameObject")]
    public GameObject birdToFollow;

    [Tooltip("The acceleration with which the fledgling moves")]
    public float acceleration = 5.0f;

    [Tooltip("The fledgling will track a random position within this "
             + "radius of the Bird to Follow")]
    public float followRadius = 1.0f;

    [Tooltip("Amount of time the fledgling waits to find a new position "
             + "after getting to an old one [s]")]
    public float stallTime = 0.5f;

    [Tooltip("If the GameObject hasn't reached the target position "
             + "within this amount of time, then it will select a new "
             + "target position [s]")]
    public float timeoutTime = 10.0f;

    [Tooltip("Check this if you'd like for the fledgling to only follow "
             + "if birdToFollow is within a trigger attached to this "
             + "gameObject")]
    public bool useTrigger = false;


    private bool _isStalling;
    private float _waypointSelectedTime;
    private float _waypointArriveTime;
    private Vector3 _homePosition;
    private Vector3 _waypoint;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private const float DISTANCE_THRESHOLD = 0.1f;

    void Start()
    {
        _homePosition = transform.position;
        _waypoint = _homePosition;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!_isStalling)
        {
            MoveToWayPoint();
        }
        else if (Time.fixedTime > _waypointArriveTime + stallTime)
        {
            _waypoint = ChooseWayPoint();
        }
        else if (Time.fixedTime > _waypointSelectedTime + timeoutTime)
        {
            _waypoint = ChooseWayPoint();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (useTrigger)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.tag == "WarmBody")
            {
                birdToFollow = other.gameObject;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == birdToFollow)
        {
            birdToFollow = null;
        }
    }

    private Vector3 ChooseWayPoint()
    {
        if (birdToFollow != null)
        {
            _waypointSelectedTime = Time.fixedTime;
            _isStalling = false;
            float distance = Random.Range(0, followRadius);
            float angle = Random.Range(0, 2*Mathf.PI);
            Vector3 displacement = new Vector3(distance*Mathf.Cos(angle),
                                               distance*Mathf.Sin(angle),
                                               0);
            return birdToFollow.transform.position + displacement;
        }
        else
        {
            Debug.Log("null");
            _isStalling = true;
            _waypointArriveTime = Time.fixedTime;
            return _homePosition;
        }
    }

    private void MoveToWayPoint()
    {
        if (Vector3.Distance(transform.position, _waypoint)
            > DISTANCE_THRESHOLD)
        {
            Vector2 force = acceleration 
                * ((Vector2) (_waypoint - transform.position)).normalized;
            _rigidbody.AddForce(force);
            if (_rigidbody.velocity.x > 0)
            {

            }
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
            _waypointArriveTime = Time.fixedTime;
            _isStalling = true;
        }
    }
}
