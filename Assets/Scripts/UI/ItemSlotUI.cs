using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outLine;

    public int index; // 자신의 인덱스만 기억
    public InventoryUI inventoryUI; // 자신을 관리하는 상위 UI 매니저

    private void Awake()
    {
        outLine = GetComponent<Outline>();
        // 버튼 클릭 시 OnClickButton 함수가 호출되도록 리스너 추가
        button.onClick.AddListener(OnClickButton);
    }

    public void Set(InventorySlot slotData)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = slotData.item.icon;
        quantityText.text = slotData.quantity > 1 ? slotData.quantity.ToString() : "";
        if (outLine != null)
        {
            outLine.enabled = slotData.equipped;
        }
    }

    // 슬롯을 비우는 역할만 함
    public void Clear()
    {
        icon.gameObject.SetActive(false);
        quantityText.text = "";

        if (outLine != null)
        {
            outLine.enabled = false;
        }
    }

    // 버튼이 클릭되면, 자신을 관리하는 InventoryUI에게 "나(index) 눌렸어!" 라고 알리기만 함
    public void OnClickButton()
    {
        if (inventoryUI != null)
        {
            inventoryUI.OnSlotClicked(index);
        }
    }
}
