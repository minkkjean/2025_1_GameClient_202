using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<CardData> deckCards = new List<CardData>();          //덱에 있는 카드
    public List<CardData> handCards = new List<CardData>();         //손에있는 카드
    public List<CardData> discardCards = new List<CardData>();

    public GameObject cardPrefab;           //카드 프리팹
    public Transform deckPosition;         //덱 위치
    public Transform handPosition;         //손 중앙 위치
    public Transform discardPosition;      //버린 카드 더미 위치

    public List<GameObject> card0bjects = new List<GameObject>();           //실제 카드 게임 오브젝트들
    // Start is called before the first frame update
    void Start()
    {
        ShuffleDeck();                  //시작 시 카드 섞기
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))                 //D를 누르면 카드 드로우
        {
            DrawCard();

        }
        if (Input.GetKeyDown(KeyCode.F))                //F를 누르면 버린 카드를 덱으로 되돌리고 섞기
        {
            ReturnDiscardsToDeck();
        }

        ArrangeHand();                                  //손패 위치 업데이트


    }

    //덱 섞기
    public void ShuffleDeck()
    {
        //임시 리스트에 카드 복사
        List<CardData> tempDeck = new List<CardData>(deckCards);
        deckCards.Clear();


        //랜덤하게 섞기
        while (tempDeck.Count > 0)
        {
            int randIndex = Random.Range(0, tempDeck.Count);
            deckCards.Add(tempDeck[randIndex]);
            tempDeck.RemoveAt(randIndex);
        }

        Debug.Log("덱을 섞었습니다. : " + deckCards.Count + "장");

    }

    //카드 드로우
    public void DrawCard()
    {
        if(handCards.Count >= 6)                //손패가 이미 6장 이상이면 드로우 하지 않음
        {
            Debug.Log("손패가 가득 찼습니다.!(최대6장)");
            ToastMessage.Instance.ShowMessage("손패가 가득 찼습니다. ! (최대6장) ", ToastMessage.MessageType.Warning);
            return; 

        }

        if(deckCards.Count == 0)
        {
            Debug.Log("덱에 카드가 없습니다.");
            ToastMessage.Instance.ShowMessage("덱에 카드가 없습니다.", ToastMessage.MessageType.Warning);
            return;
        }

        //덱에서 맨 위 카드 가져오기
        CardData carData = deckCards[0];
        deckCards.RemoveAt(0);

        //손패에 추가
        handCards.Add(carData);

        //카드 게임 오브젝트 생성
        GameObject card0bj = Instantiate(cardPrefab, deckPosition.position, Quaternion.identity);

        //카드 정보 설정
        CardDisplay cardDisplay = card0bj.GetComponent<CardDisplay>();

        if(cardDisplay != null)
        {
            cardDisplay.SetupCard(carData);
            cardDisplay.cardIndex = handCards.Count - 1;
            card0bjects.Add(card0bj);
        }

        //손패 위치 업데이트
        ArrangeHand();

        Debug.Log("카드를 드로우 했습니다. :" + carData.cardName + "(손패 : " + handCards.Count + "/6");
        ToastMessage.Instance.ShowMessage("카드를 드로우 했습니다. : " + carData.cardName + " (손패 :" + handCards.Count +  "/6", ToastMessage.MessageType.info); 
    }

    public void ArrangeHand()           //손에 있는 카드 재정렬
    {
        if (handCards.Count == 0) return;

        //손패 배치를 위한 변수

        float cardWidth = 1.2f;
        float spacing = cardWidth + 1.8f;
        float totalWidth = (handCards.Count - 1) * spacing;
        float startX = -totalWidth / 2f;

        //카드 위치 설정
        for(int i = 0; i < card0bjects.Count; i++)
        {
            if (card0bjects[i] != null)
            {
                //드래그중인 카드는 건너띄기
                CardDisplay display = card0bjects[i].GetComponent<CardDisplay>();
                if (display != null && display.isDragging)
                    continue;

                //목표 위치 계산
                Vector3 tarPosition = handPosition.position + new Vector3(startX + (i * spacing), 0, 0);

                //부드러운 이동
                card0bjects[i].transform.position = Vector3.Lerp(card0bjects[i].transform.position, tarPosition, Time.deltaTime * 10f);




            }
        }
    }

    public void DiscardCard(int handIndex) //카드 버리기(디스카드)
    {
        if(handIndex < 0 || handIndex >= handCards.Count)
        {
            Debug.Log("유효하지 않는 카드 인덱스입니다.");
            return;

        }
        
        //손패에서 카드 가져오기
        CardData cardData = handCards[handIndex];
        handCards.RemoveAt(handIndex);

        //버린 카드 더미에 추가
        discardCards.Add(cardData);

        //해당 카드 게임 오브젝트 제거
        if(handIndex < card0bjects.Count)
        {
            Destroy(card0bjects[handIndex]);
            card0bjects.RemoveAt(handIndex);
        }

        //카드 인덱스 재설정
        for(int i = 0; i < card0bjects.Count; ++i)
        {
            CardDisplay display = card0bjects[i].GetComponent<CardDisplay>();
            if (display != null) display.cardIndex = i;
        }


        ArrangeHand();
        Debug.Log("카드를 버렸습니다. " + cardData.cardName);

    }

    //버린 카드 덱으로 되돌리고 섞기

    public void ReturnDiscardsToDeck()
    {
        if(discardCards.Count == 0)
        {
            Debug.Log("버린 카드 더미가 비여 있습니다.");
            return;
        }

        deckCards.AddRange(discardCards);               //버린 카드를 모두 덱에 추가
        discardCards.Clear();                           //버린 카드 더미 비우기
        ShuffleDeck();                                  //덱 섞기

        Debug.Log("버린 카드" + deckCards.Count + "장을 덱으로 되돌리고 섞었습니다. ");
    }
}
