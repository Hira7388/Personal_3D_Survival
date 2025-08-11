using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controller;
    public  PlayerCondition condition { get; set; }
    public Inventory inventory { get; set; }
    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        inventory = GetComponent<Inventory>();
    }
}
