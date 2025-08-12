using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public interface IDamagalbe
{
    void TakePhysicalDamage(int damage);
}
public class PlayerCondition : MonoBehaviour, IDamagalbe
{
    // 각 Condition이 붙어있는 오브젝트를 직접 연결
    [Header("Player Conditions")]
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    [Header("Settings")]
    public float noHungerHealthDecay;
    public event Action OnTakeDamage; // 데미지를 받았을 때 발생할 이벤트를 담을 변수

    private PlayerController controller;

    private void Awake()
    {
        controller = CharacterManager.Instance.Player.controller;
    }

    private void OnEnable()
    {
        controller.OnJumpEvent += DecreaseStaminaOnJump;
    }

    void Update()
    {
        if (hunger.CurValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.CurValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Die()
    {
        Debug.Log("죽었습니다.");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        OnTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.CurValue - amount < 0f)
            return false;

        stamina.Subtract(amount);
        return true;
    }

    public void DecreaseStaminaOnJump()
    {

    }

    private void OnDisable()
    {
        if (controller != null)
        {
            controller.OnJumpEvent -= DecreaseStaminaOnJump;
        }
    }
}
