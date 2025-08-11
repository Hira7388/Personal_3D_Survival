using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionUI : MonoBehaviour
{
    [SerializeField] private Condition health;
    public Condition Health { get => health; }
    [SerializeField] private Condition hunger;
    public Condition Hunger { get => hunger; }
    [SerializeField] private Condition stamina;
    public Condition Stamina { get => stamina; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
