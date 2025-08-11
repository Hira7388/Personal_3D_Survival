using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;


[System.Serializable] // 인스펙터 창에 보이게 하기 위한 어트리뷰트
public class InventorySlot
{
    public ItemData item;   // 어떤 아이템인가
    public int quantity;    // 몇 개를 가지고 있는가
    public bool equipped;   // 장착 아이템이면 장착한 상태인가

    public InventorySlot(ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
        this.equipped = false;
    }
}
public class Inventory : MonoBehaviour
{
    // 인벤토리나 선택 아이템이 변경될 때 UI에 알릴 델리게이트
    public event Action OnInventoryChanged;
    // 인벤토리의 모든 슬롯을 담을 리스트
    public List<InventorySlot> slots = new List<InventorySlot>();
    // 보통 최대 개수를 정해놓고 잠궈둔 다음 해금하는 방식을 많이 사용한다고 알고 있다
    public int maxSlotCount = 20;
    // 현재 선택한 슬롯
    public InventorySlot selectedSlot { get; private set; }
    public int curEquipIndex { get; private set; } = -1;


    private PlayerCondition condition;
    private PlayerEquip equip;
    [SerializeField] private Transform dropPosition;

    private void Awake()
    {
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<PlayerEquip>();
    }

    // 아이템을 추가하는 핵심 함수
    public void AddItem(ItemData newItem)
    {
        // 1. 스택 가능한 아이템인가? 그리고 이미 인벤토리에 같은 아이템이 있는가?
        if (newItem.canStack)
        {
            InventorySlot existingSlot = GetItemStack(newItem);

            if (existingSlot != null)
            {
                // 같은 아이템이 있다면 수량만 증가
                existingSlot.quantity++;
                Debug.Log($"{newItem.displayName}의 개수가 {existingSlot.quantity}가 되었습니다.");
                OnInventoryChanged?.Invoke();
                return; // 함수 종료
            }
        }

        // 2. 스택이 불가능한 아이템, 처음 들어오는 아이템이라면 새로운 슬롯에 추가
        if (slots.Count < maxSlotCount)
        {
            slots.Add(new InventorySlot(newItem, 1));
            Debug.Log($"{newItem.displayName}을(를) 인벤토리에 추가했습니다.");
            OnInventoryChanged?.Invoke();
        }
        else
        {
            ThrowItem(newItem);
        }
    }

    public void SelectItem(int index)
    {
        selectedSlot = (index < 0 || index >= slots.Count) ? null : slots[index];
        OnInventoryChanged?.Invoke();
    }

    public void UseSelectedItem()
    {
        if (selectedSlot == null || selectedSlot.item.type != ItemType.Consumable) return;

        foreach (var consumable in selectedSlot.item.consumables)
        {
            switch (consumable.type)
            {
                case ConsumableType.Health: condition.Heal(consumable.value); break;
                case ConsumableType.Hunger: condition.Eat(consumable.value); break;
            }
        }
        RemoveItem(selectedSlot, 1);
    }

    public void EquipSelectedItem()
    {
        if (selectedSlot == null || selectedSlot.item.type != ItemType.Equipable) return;

        if (curEquipIndex != -1) UnEquip(curEquipIndex);

        selectedSlot.equipped = true;
        curEquipIndex = slots.IndexOf(selectedSlot);
        equip.EquipNew(selectedSlot.item);
        OnInventoryChanged?.Invoke();
    }

    public void UnEquipSelectedItem()
    {
        if (selectedSlot == null || !selectedSlot.equipped) return;
        UnEquip(slots.IndexOf(selectedSlot));
    }

    public void DropSelectedItem()
    {
        if (selectedSlot == null) return;
        ThrowItem(selectedSlot.item);
        RemoveItem(selectedSlot, 1);
    }

    private void RemoveItem(InventorySlot slotToRemove, int quantity)
    {
        if (slotToRemove == null) return;
        slotToRemove.quantity -= quantity;

        if (slotToRemove.quantity <= 0)
        {
            bool wasSelected = (slotToRemove == selectedSlot);
            if (slotToRemove.equipped) UnEquip(slots.IndexOf(slotToRemove));
            slots.Remove(slotToRemove);
            if (wasSelected) SelectItem(-1); // 선택 해제
        }
        OnInventoryChanged?.Invoke();
    }

    private void UnEquip(int index)
    {
        if (index < 0 || index >= slots.Count || !slots[index].equipped) return;

        slots[index].equipped = false;
        equip.UnEquip();
        if (index == curEquipIndex) curEquipIndex = -1;
        OnInventoryChanged?.Invoke();
    }

    private InventorySlot GetItemStack(ItemData data)
    {
        return slots.Find(s => s.item == data && s.quantity < data.maxStackAmount);
    }
    private void ThrowItem(ItemData data)
    { 
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.identity); 
    }
}

