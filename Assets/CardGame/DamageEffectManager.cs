using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;  // UI 관련 기능 사용을 위한 네임스페이스 추가

public class DamageEffectManager : MonoBehaviour
{

    [SerializeField] private GameObject textPrefab; // 텍스트 프리팹
    [SerializeField] private Canvas uiCanvas;       // UI 캔버스 참조

    public static DamageEffectManager Instance { get; private set; }

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }   

        if(uiCanvas == null)
        {
            uiCanvas = FindObjectOfType<Canvas>();
            if (uiCanvas == null)
            {
                Debug.LogError("UI 컨버스를 찾을 수 없습니다.");
            }
        }
    }

    public void ShowDamageText(Vector3 positien, string text, Color color, bool isCritical = false, bool isStatusEffect = false)
    {
        if (textPrefab == null || uiCanvas == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(positien);       //월드 좌표를 스크린 좌표로 전환

        if (screenPos.z < 0) return;                                        //UI가 카메라 뒤에 있는 경우 표시 하지 않음

        GameObject damageText = Instantiate(textPrefab, uiCanvas.transform);   //데머지 텍스트 UI 생성

        RectTransform rectTransform = damageText.GetComponent<RectTransform>();    //스크린 위치 설정
        if (rectTransform != null)
        {
            rectTransform.position = screenPos;
        }


        TextMeshProUGUI tmp = damageText.GetComponent<TextMeshProUGUI>();       //텍스트 컴포넌트 설정
        if (tmp != null)
        {
            tmp.text = text;                                                    //텍스트 설정
            tmp.color = color;                                                  //색상 설정
            tmp.outlineColor = new Color(
                Mathf.Clamp01(color.r - 0.3f),
                Mathf.Clamp01(color.g - 0.3f),
                Mathf.Clamp01(color.b - 0.3f),
                color.a

            );

            float scale = 1.0f;             //크기 설정

            int numbericValue;
            if (int.TryParse(text.Replace("+", "").Replace("CRIT!", "").Replace("HEAL CRIT", ""), out numbericValue))
            {
                scale = Mathf.Clamp(numbericValue / 15f, 0.8f, 2.5f);

            }

            if (isCritical) scale = 1.4f;
            if (isStatusEffect) scale = 0.8f;

            damageText.transform.localScale = new Vector3(scale, scale, scale);
        }

        DamageTextEffect effect = damageText.GetComponent<DamageTextEffect>();
        if (effect != null)
        {
            effect.Initialized(isCritical, isStatusEffect);
            if (isStatusEffect)
            {
                effect.SetVerticalMovement();
            }
        }
    }


    public void ShowDamage(Vector3 position, int amount, bool isCritical = false)
    {
        string text = amount.ToString();
        Color color = isCritical ? new Color(1.0f, 0.8f, 0.0f) : new Color(1.0f, 0.3f, 0.03f);

        if (isCritical)
        {
            text = "CRIT!\n" + text;
        }

        ShowDamageText(position, text, color, isCritical);
    }
    public void ShowHeal(Vector3 position, int amount, bool isCritical = false)
    {
        string text = amount.ToString();
        Color color = isCritical ? new Color(1.0f, 0.8f, 0.0f) : new Color(1.0f, 0.3f, 0.03f);

        if (isCritical)
        {
            text = "HEAL CRIT!\n" + text;
        }

        ShowDamageText(position, text, color, isCritical);
    }
    public void ShowMiss(Vector3 position)
    {
        ShowDamageText(position, "MISS", Color.gray, false);
    }

    public void ShowStatusEffect(Vector3 position, string effectName) // 상태 효과 함수
    {
        Color color;

        switch (effectName.ToLower()) // 상태 효과에 따른 색상 설정
        {
            case "poison":
                color = new Color(0.5f, 0.1f, 0.5f); // 보라색
                break;
            case "burn":
                color = new Color(1.0f, 0.4f, 0.0f); // 주황색
                break;
            case "freeze":
                color = new Color(0.5f, 0.8f, 1.0f); // 하늘색
                break;
            case "stun":
                color = new Color(1.0f, 1.0f, 0.0f); // 노란색
                break;
            default:
                color = new Color(1.0f, 1.0f, 1.0f); // 기본 흰색
                break;
        }

        ShowDamageText(position, effectName.ToLower(), color, false, true);
    }

}
