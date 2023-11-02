using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 1f;

    private Vector3 nextPosition;
    private GameObject player;
    private Vector3 lastPlatformPosition;

    void Start()
    {
        nextPosition = pointB.position;
        lastPlatformPosition = transform.position;
    }

    void Update()
    {
        if (transform.position == pointA.position)
        {
            nextPosition = pointB.position;
        }
        else if (transform.position == pointB.position)
        {
            nextPosition = pointA.position;
        }

        Move();
    }

    void Move()
    {
        Vector3 platformMoveVector = transform.position - lastPlatformPosition;
        lastPlatformPosition = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);

        if (player != null)
        {
            player.transform.position += platformMoveVector;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }
}
