using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    public Image iconImage;
    public Image durationFillImage; // DurationFill 이미지

    private ActiveBuff currentBuff;

    // BuffUI로부터 ActiveBuff 데이터를 받아서 초기화하는 함수
    public void Initialize(ActiveBuff buff)
    {
        currentBuff = buff;
    }

    private void Update()
    {
        if (currentBuff != null)
        {
            // 남은 시간의 비율을 계산합니다. (0과 1 사이의 값)
            float fillAmount = 1f - (currentBuff.RemainingTime / currentBuff.Data.duration);
            // DurationFill 이미지의 fillAmount 값을 업데이트합니다.
            durationFillImage.fillAmount = fillAmount;
        }
    }
}
