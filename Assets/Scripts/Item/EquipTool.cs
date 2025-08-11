using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    [Header("Stats")]
    public float attackRate;
    public float attackDistance;
    public float useStamina;
    public int damage;

    [Header("Funtionality")]
    public bool doesGatherResource;
    public bool doesDealDamage;

    private bool attacking;
    private Animator animator;
    private Camera camera;

    void Awake()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke(nameof(OnCanAttack), attackRate);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, attackDistance))
        {
            //if (doesGatherResource && hit.collider.TryGetComponent(out Resource resource))
            //{
            //    resource.Gather(hit.point, hit.normal);
            //}
            //if (doesDealDamage && hit.collider.TryGetComponent(out IDamageable damageable))
            //{
            //    damageable.TakePhysicalDamage(damage);
            //}
        }
    }
}
