using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory; // 플레이어의 Inventory 컴포넌트를 연결

    [Header("UI Elements")]
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public GameObject itemSlotPrefab;
    private ItemSlotUI[] slots;


    [Header("Selected Item Window")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public Button useButton;
    public Button equipButton;
    public Button unequipButton;
    public Button dropButton;

    private void Start()
    {
        // 1. 슬롯을 먼저 '생성'하고 'slots' 배열에 채웁니다.
        GenerateSlots();

        // 2. 그 다음에 Inventory의 이벤트를 '구독'합니다.
        inventory.OnInventoryChanged += UpdateUI;

        // 3. 마지막으로 창을 닫고 UI를 초기 상태로 한번 그려줍니다.
        inventoryWindow.SetActive(false);
        UpdateUI();
    }

    void OnDestroy()
    {
        inventory.OnInventoryChanged -= UpdateUI;
    }

    // 설정한 슬롯 개수에 맞게 자동으로 생성하는 메서드
    private void GenerateSlots()
    {
        foreach (Transform child in slotPanel)
        {
            Destroy(child.gameObject);
        }

        // inventory에 설정된 maxSlotCount 만큼 슬롯을 동적으로 생성
        slots = new ItemSlotUI[inventory.maxSlotCount];
        for (int i = 0; i < inventory.maxSlotCount; i++)
        {
            // itemSlotPrefab을 slotPanel의 자식으로 생성
            GameObject slotObject = Instantiate(itemSlotPrefab, slotPanel);
            slots[i] = slotObject.GetComponent<ItemSlotUI>();
            slots[i].index = i;
            slots[i].inventoryUI = this;
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.slots.Count)
            {
                slots[i].Set(inventory.slots[i]);
            }
            else
            {
                slots[i].Clear();
            }
        }
        UpdateSelectedItemWindow();
    }

    private void UpdateSelectedItemWindow()
    {
        if (inventory.selectedSlot != null)
        {
            ItemData item = inventory.selectedSlot.item;
            selectedItemName.text = item.displayName;
            selectedItemDescription.text = item.description;

            string statNames = "";
            string statValues = "";

            if (item.type == ItemType.Consumable)
            {
                foreach (var consumable in item.consumables)
                {
                    statNames += consumable.type.ToString() + "\n";
                    statValues += consumable.value.ToString() + "\n";
                }
            }
            else if (item.type == ItemType.Equipable)
            {
                if (item.equipPrefab != null && item.equipPrefab.TryGetComponent(out EquipTool tool))
                {
                    statNames += "Damage\nAttack Rate";
                    statValues += $"{tool.damage}\n{tool.attackRate}";
                }
            }

            selectedStatName.text = statNames;
            selectedStatValue.text = statValues;

            useButton.gameObject.SetActive(item.type == ItemType.Consumable);
            equipButton.gameObject.SetActive(item.type == ItemType.Equipable && !inventory.selectedSlot.equipped);
            unequipButton.gameObject.SetActive(item.type == ItemType.Equipable && inventory.selectedSlot.equipped);
            dropButton.gameObject.SetActive(true);
        }
        else
        {
            ClearSelectedItemWindow();
        }
    }

    private void ClearSelectedItemWindow()
    {
        selectedItemName.text = "";
        selectedItemDescription.text = "";
        selectedStatName.text = "";
        selectedStatValue.text = "";
        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }
    public void Toggle()
    {
        //창의 활성 상태를 반대로 변경한다.
        bool isActive = !inventoryWindow.activeSelf;
        inventoryWindow.SetActive(isActive);

        // 창이 켜졌다면 (isActive == true)
        if (isActive)
        {
            // 커서를 보이고 잠금을 해제
            Cursor.lockState = CursorLockMode.None;
        }
        else // 창이 꺼졌다면 (isActive == false)
        {
            // 커서를 숨기고 화면 중앙에 잠금
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // 슬롯을 클릭했을 때
    public void OnSlotClicked(int index) => inventory.SelectItem(index);
    // 사용 버튼을 눌렀을 때
    public void OnUseButton() => inventory.UseSelectedItem();
    // 장착 버튼을 눌렀을 때
    public void OnEquipButton() => inventory.EquipSelectedItem();
    // 해제 버튼을 눌렀을 때
    public void OnUnEquipButton() => inventory.UnEquipSelectedItem();
    // 버리기 버튼을 눌렀을 때
    public void OnDropButton() => inventory.DropSelectedItem();
    // 인벤토리 껏다 켜기
}
