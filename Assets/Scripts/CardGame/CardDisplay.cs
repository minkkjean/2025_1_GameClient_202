using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;   //카드 데이터

    //3D 카드 요소
    public MeshRenderer cardRenderer;               //카드 렌더러 (icon or 일러스트)
    public TextMeshPro nameText;                    //이름 텍스트
    public TextMeshPro costText;                    //비용 텍스트
    public TextMeshPro attackText;                  //공격력/효과 텍스트
    public TextMeshPro descriptionText;             //설명 텍스트

    //카드 데이터 설정

    //카드 데이터 설정
    public void SetupCard(CardData data)
    {
        cardData = data;

        //3D 텍스트 업데이트
        if (nameText != null) nameText.text = data.cardName;
        if (costText != null) costText.text = data.manaCost.ToString();
        if (attackText != null) attackText.text = data.effectAmount.ToString();
        if (descriptionText != null) descriptionText.text = data.description;

        //카드 텍스처 설정
        if (cardRenderer != null && data.artwork != null)
        {
            Material cardMaterial = cardRenderer.material;
            cardMaterial.mainTexture = data.artwork.texture;
        }
    }

}
