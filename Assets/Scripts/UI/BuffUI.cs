using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUI : MonoBehaviour
{
    [Header("버프 프리팹")]
    public GameObject buffIconPrefab;

    private void OnEnable()
    {
        // BuffManager 인스턴스가 존재할 때만 이벤트를 구독합니다.
        if (BuffManager.Instance != null)
        {
            // 1. UpdateBuffIcons 뒤의 괄호()를 제거하고, 올바른 이벤트 이름(OnBuffsChanged)으로 수정했습니다.
            BuffManager.Instance.OnBuffsChanged += UpdateBuffIcons;
        }
    }

    private void OnDisable()
    {
        // BuffManager 인스턴스가 존재할 때만 이벤트 구독을 해제합니다.
        if (BuffManager.Instance != null)
        {
            BuffManager.Instance.OnBuffsChanged -= UpdateBuffIcons;
        }
    }

    private void UpdateBuffIcons(List<ActiveBuff> currentBuffs)
    {
        // 새로운 아이콘을 만들기 전에, 기존에 있던 모든 자식 아이콘 삭제
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 전달받은 활성 버프 목록으로 새로운 버프 UI를 생성한다.
        foreach (ActiveBuff buff in currentBuffs)
        {
            GameObject iconGO = Instantiate(buffIconPrefab, transform);
            iconGO.GetComponent<Buff>().Initialize(buff);
        }
    }
}
