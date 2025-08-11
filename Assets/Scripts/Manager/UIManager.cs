using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public InventoryUI inventoryUI;

    protected override void Initialize()
    {
        base.Initialize();

        // 만약 인스펙터에서 inventoryUI가 수동으로 연결되지 않다면
        // 씬에 있는 InventoryUI를 직접 찾아서 자동으로 연결해준다.
        if (inventoryUI == null)
        {
            inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
            {
                Debug.Log("InventoryUI를 씬에서 찾아 UIManager에 연결했습니다.");
            }
            else
            {
                // 씬에 InventoryUI가 없을 경우 경고를 표시합니다.
                Debug.LogWarning("UIManager가 씬에서 InventoryUI를 찾을 수 없습니다!");
            }
        }
    }
}
