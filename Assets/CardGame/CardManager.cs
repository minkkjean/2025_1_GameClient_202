using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<CardData> deckCards = new List<CardData>();          //���� �ִ� ī��
    public List<CardData> handCards = new List<CardData>();         //�տ��ִ� ī��
    public List<CardData> discardCards = new List<CardData>();

    public GameObject cardPrefab;           //ī�� ������
    public Transform deckPosition;         //�� ��ġ
    public Transform handPosition;         //�� �߾� ��ġ
    public Transform discardPosition;      //���� ī�� ���� ��ġ

    public List<GameObject> card0bjects = new List<GameObject>();           //���� ī�� ���� ������Ʈ��
    // Start is called before the first frame update
    void Start()
    {
        ShuffleDeck();                  //���� �� ī�� ����
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))                 //D�� ������ ī�� ��ο�
        {
            DrawCard();

        }
        if (Input.GetKeyDown(KeyCode.F))                //F�� ������ ���� ī�带 ������ �ǵ����� ����
        {
            ReturnDiscardsToDeck();
        }

        ArrangeHand();                                  //���� ��ġ ������Ʈ


    }

    //�� ����
    public void ShuffleDeck()
    {
        //�ӽ� ����Ʈ�� ī�� ����
        List<CardData> tempDeck = new List<CardData>(deckCards);
        deckCards.Clear();


        //�����ϰ� ����
        while (tempDeck.Count > 0)
        {
            int randIndex = Random.Range(0, tempDeck.Count);
            deckCards.Add(tempDeck[randIndex]);
            tempDeck.RemoveAt(randIndex);
        }

        Debug.Log("���� �������ϴ�. : " + deckCards.Count + "��");

    }

    //ī�� ��ο�
    public void DrawCard()
    {
        if(handCards.Count >= 6)                //���а� �̹� 6�� �̻��̸� ��ο� ���� ����
        {
            Debug.Log("���а� ���� á���ϴ�.!(�ִ�6��)");
            return;

        }

        if(deckCards.Count == 0)
        {
            Debug.Log("���� ī�尡 �����ϴ�.");
            return;
        }

        //������ �� �� ī�� ��������
        CardData carData = deckCards[0];
        deckCards.RemoveAt(0);

        //���п� �߰�
        handCards.Add(carData);

        //ī�� ���� ������Ʈ ����
        GameObject card0bj = Instantiate(cardPrefab, deckPosition.position, Quaternion.identity);

        //ī�� ���� ����
        CardDisplay cardDisplay = card0bj.GetComponent<CardDisplay>();

        if(cardDisplay != null)
        {
            cardDisplay.SetupCard(carData);
            cardDisplay.cardIndex = handCards.Count - 1;
            card0bjects.Add(card0bj);
        }

        //���� ��ġ ������Ʈ
        ArrangeHand();

        Debug.Log("ī�带 ��ο� �߽��ϴ�. :" + carData.cardName + "(���� : " + handCards.Count + "/6");
    }

    public void ArrangeHand()           //�տ� �ִ� ī�� ������
    {
        if (handCards.Count == 0) return;

        //���� ��ġ�� ���� ����

        float cardWidth = 1.2f;
        float spacing = cardWidth + 1.8f;
        float totalWidth = (handCards.Count - 1) * spacing;
        float startX = -totalWidth / 2f;

        //ī�� ��ġ ����
        for(int i = 0; i < card0bjects.Count; i++)
        {
            if (card0bjects[i] != null)
            {
                //�巡������ ī��� �ǳʶ��
                CardDisplay display = card0bjects[i].GetComponent<CardDisplay>();
                if (display != null && display.isDragging)
                    continue;

                //��ǥ ��ġ ���
                Vector3 tarPosition = handPosition.position + new Vector3(startX + (i * spacing), 0, 0);

                //�ε巯�� �̵�
                card0bjects[i].transform.position = Vector3.Lerp(card0bjects[i].transform.position, tarPosition, Time.deltaTime * 10f);




            }
        }
    }

    public void DiscardCard(int handIndex) //ī�� ������(��ī��)
    {
        if(handIndex < 0 || handIndex >= handCards.Count)
        {
            Debug.Log("��ȿ���� �ʴ� ī�� �ε����Դϴ�.");
            return;

        }
        
        //���п��� ī�� ��������
        CardData cardData = handCards[handIndex];
        handCards.RemoveAt(handIndex);

        //���� ī�� ���̿� �߰�
        discardCards.Add(cardData);

        //�ش� ī�� ���� ������Ʈ ����
        if(handIndex < card0bjects.Count)
        {
            Destroy(card0bjects[handIndex]);
            card0bjects.RemoveAt(handIndex);
        }

        //ī�� �ε��� �缳��
        for(int i = 0; i < card0bjects.Count; ++i)
        {
            CardDisplay display = card0bjects[i].GetComponent<CardDisplay>();
            if (display != null) display.cardIndex = i;
        }


        ArrangeHand();
        Debug.Log("ī�带 ���Ƚ��ϴ�. " + cardData.cardName);

    }

    //���� ī�� ������ �ǵ����� ����

    public void ReturnDiscardsToDeck()
    {
        if(discardCards.Count == 0)
        {
            Debug.Log("���� ī�� ���̰� �� �ֽ��ϴ�.");
            return;
        }

        deckCards.AddRange(discardCards);               //���� ī�带 ��� ���� �߰�
        discardCards.Clear();                           //���� ī�� ���� ����
        ShuffleDeck();                                  //�� ����

        Debug.Log("���� ī��" + deckCards.Count + "���� ������ �ǵ����� �������ϴ�. ");
    }
}
