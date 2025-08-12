using UnityEngine;

enum MovingType
{
    Go,
    Back
}

public class MovingPlatform : MonoBehaviour
{
    [Header("플랫폼 움직임")]
    [SerializeField] private float movingSpeed;
    [SerializeField] private float movingDistance;

    private Vector3 startPos;
    private Vector3 endPos;
    private Transform curPos;
    private MovingType movingType;
    private void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(movingDistance, 0, 0);
        curPos = transform;
        movingType = MovingType.Go;
    }

    private void Update()
    {
        Moving();
    }

    private void Moving()
    {
        if (movingType == MovingType.Go)
        {
            //curPos.position = new Vector3(curPos.position.x + movingSpeed * Time.deltaTime, curPos.position.y, curPos.position.z);
            curPos.position = Vector3.MoveTowards(curPos.position, endPos, movingSpeed * Time.deltaTime);

            if (curPos.position == endPos)
            {
                movingType = MovingType.Back;
            }
        }
        else
        {
            // 반대로 되돌아 가는 로직
            //curPos.position = new Vector3(curPos.position.x - movingSpeed * Time.deltaTime, curPos.position.y, curPos.position.z);
            curPos.position = Vector3.MoveTowards(curPos.position, startPos, movingSpeed * Time.deltaTime);
            if (curPos.position == startPos)
            {
                movingType = MovingType.Go;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.parent = this.transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.parent = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(startPos, new Vector3(movingDistance, 0, 0));
    }
}
