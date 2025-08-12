using UnityEngine;

public class JumpPlatform : MonoBehaviour, ILauncher
{
    [SerializeField]
    [Tooltip("플레이어를 위로 쏘아 올릴 힘의 크기")]
    private float jumpPower = 15f;

    public void Jump(Rigidbody target)
    {
        target.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            Jump(collision.rigidbody);
        }
    }
}