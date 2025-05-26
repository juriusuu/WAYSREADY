using UnityEngine;

public class MovingObstacles : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right; // Direction to move (e.g., Vector3.right, Vector3.forward, etc.)
    public float moveDistance = 3f;               // How far to move from the start position
    public float speed = 2f;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 target;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + moveDirection.normalized * moveDistance;
        target = endPos;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = target == startPos ? endPos : startPos;
        }
    }
}