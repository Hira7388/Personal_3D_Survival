using UnityEngine;

// 코루틴을 더 이상 사용하지 않으므로 using System.Collections;는 없어도 됩니다.
public class SimpleLauncher : MonoBehaviour
{
    public enum TriggerType { KeyPress, Timed }

    [Header("발사 설정")]
    public TriggerType triggerType = TriggerType.Timed;
    public Vector3 launchDirection = new Vector3(0, 1, 1);
    public float launchPower = 20f;

    [Header("키 입력 설정")]
    public KeyCode launchKey = KeyCode.F;

    [Header("타이머 설정")]
    public float launchCooldown = 3.0f;

    // playerControlDisableTime 변수는 더 이상 필요 없습니다.

    private Rigidbody targetRigidbody;
    private float elapsedTime = 0.0f;

    private void Update()
    {
        if (triggerType == TriggerType.Timed)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= launchCooldown)
            {
                Launch();
                elapsedTime = 0.0f;
            }
        }
        else if (triggerType == TriggerType.KeyPress)
        {
            if (Input.GetKeyDown(launchKey))
            {
                Launch();
            }
        }
    }

    private void Launch()
    {
        if (targetRigidbody == null) return;

        targetRigidbody.AddForce(launchDirection.normalized * launchPower, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetRigidbody = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, launchDirection.normalized * 5f);
        Gizmos.DrawSphere(transform.position + launchDirection.normalized * 5f, 0.25f);
    }
}