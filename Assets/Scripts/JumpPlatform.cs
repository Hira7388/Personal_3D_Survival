using UnityEngine;

// 이 스크립트는 플레이어와 직접 상호작용하여 점프를 발동시키므로
// ILauncher 인터페이스를 구현(implement)합니다.
public class JumpPlatform : MonoBehaviour, ILauncher
{
    [SerializeField]
    [Tooltip("플레이어를 위로 쏘아 올릴 힘의 크기")]
    private float jumpPower = 15f;

    // ILauncher 인터페이스의 요구사항을 만족시키는 Jump 메서드.
    // 이 메서드는 반드시 public이어야 합니다.
    public void Jump(Rigidbody target)
    {
        // 위쪽 방향으로 순간적인 힘을 가합니다.
        target.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    // 다른 오브젝트와 물리적 충돌이 시작될 때 호출됩니다.
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인합니다.
        if (collision.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            // 이 스크립트가 가진 Jump 메서드를 직접 호출합니다.
            // 인자로는 충돌한 플레이어의 Rigidbody를 넘겨줍니다.
            Jump(collision.rigidbody);
        }
    }
}