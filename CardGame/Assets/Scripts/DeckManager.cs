﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;       //系統.集合.一般

/// <summary>
/// 牌組管理器
/// </summary>
public class DeckManager : MonoBehaviour
{
    //清單<要存放的類型> 清單名稱 = 新增 清單物件
    /// <summary>
    /// 牌組內卡牌資料
    /// </summary>
    public List<CardData> deck = new List<CardData>();
    /// <summary>
    /// 牌組內卡牌遊戲物件
    /// </summary>
    public List<GameObject> deckGameObject = new List<GameObject>();

    [Header("牌組卡牌資訊")]
    public GameObject DeckObject;
    [Header("牌組內容")]
    public Transform contentDeck;
    [Header("牌組卡牌數量")]
    public Text textDeckCount;
    [Header("開始遊戲按鈕")]
    public Button btnStart;
    [Header("洗牌後牌組")]
    public Transform tranShuffle;
    [Header("金幣")]
    public Rigidbody coin;
    [Header("遊戲畫面")]
    public GameObject gameView;

  
    

    /// <summary>
    /// 牌組管理器實體物件
    /// </summary>
    public static DeckManager instance;

    private void Awake()
    {
        //牌組趕理器實體物件 = 此腳本
        instance = this;
        //取消開始遊戲按鈕 互動
        btnStart.interactable = false;
        //添加監聽器(開始遊戲)
        btnStart.onClick.AddListener(StartBattle);
    }

    private void Update()
    {
        Choose30Card();
    }

    /// <summary>
    /// 自選 30 張
    /// </summary>
    private void Choose30Card()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j <= 15; j++)
                {
                    AddCard(j);
                }
            }
        }
    }
    /// <summary>
    /// 添加卡牌至牌組內
    /// </summary>
    /// <param name="index">要添加到牌組的卡牌編號</param>

    public void AddCard(int index)
    {
        //如果 牌組.數量 < 30 - List 長度 Count
        if (deck.Count < 30)
        {
            //取得卡牌資訊
            CardData card = GetCard.instance.cards[index - 1];
            //牌組.尋找全部(卡牌 => 卡牌.等於(選取卡牌))
            List<CardData> sameCard = deck.FindAll(c => c.Equals(card));

            //如果 相同卡牌 <2 才能新增
            if (sameCard.Count < 2)
            {

            //牌組.增加(去德卡牌.實體物件.卡牌資料[編號])
            deck.Add(GetCard.instance.cards[index -1]);
            

            Transform temp;

                if (sameCard.Count <1)
                {
                    //生成 牌組卡牌資訊物件 到 牌組內容
                    temp =  Instantiate(DeckObject, contentDeck).transform;
                    //添加牌組物件腳本，讓按鈕有功能
                    temp.gameObject.AddComponent<DeckObject>().index = card.index;
                    temp.name = "牌組卡牌資訊:" + card.name;
                }
                else
                {
                    temp = GameObject.Find("牌組卡牌資訊:" + card.name).transform;
                }
            
           

            //更新卡牌數量
            textDeckCount.text = "套牌數量:" + deck.Count + " / 30 ";
            
            temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
            temp.Find("名稱").GetComponent<Text>().text = card.name;
            temp.Find("數量").GetComponent<Text>().text = (sameCard.Count+1).ToString();
            }
            }
        //如果卡牌  等於 30 張 啟動開始遊戲按鈕 互動
        if (deck.Count == 30) btnStart.interactable = true;
    }

    /// <summary>
    /// 刪除牌組的卡牌
    /// </summary>
    /// <param name="index">要從牌組內刪除的卡牌編號</param>
    public void DeleteCard(int index)
    {
        //取得卡牌資訊
        CardData card = GetCard.instance.cards[index - 1];
        //牌組.尋找全部(卡牌 => 卡牌.等於(選取卡牌))
        List<CardData> sameCard = deck.FindAll(c => c.Equals(card));

        //牌組.刪除(卡牌)
        deck.Remove(card);

        //牌組物件
        Transform temp = GameObject.Find("牌組卡牌資訊:" + card.name).transform;

        //相同卡牌 > 1
        if (sameCard.Count >1)
        {
            //更新 牌組物件 數量
            temp.Find("數量").GetComponent<Text>().text = (sameCard.Count - 1).ToString();
        }
        //相同卡牌 < 1
        else
        {
            //刪除 牌組物件
            Destroy(temp.gameObject);
        }
        textDeckCount.text = "牌組數量:" + deck.Count + " / 30";

        //取消開始遊戲按鈕 互動
        btnStart.interactable = false;
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    private void Shuffle()
    {
        //執行 30 次
        for (int i = 0; i < deck.Count; i++)
        {

            //儲存目前卡牌
            CardData original = deck[i];

            //取得隨機編號 0 - 30
            int r = Random.Range(0, deck.Count);

            //目前 = 隨機卡牌
            deck[i] = deck[r];


            //隨機卡牌 = 目前
            deck[r] = original;
        }

        CreateCard();
    }
    /// <summary>
    /// 建立卡牌物件
    /// </summary>
    private void CreateCard()
    {
        //迴圈執行 0 -13
        for (int i = 0; i < deck.Count; i++)
        {
            //變形 = 生成(物件,父物件).變形
            Transform temp = Instantiate(GetCard.instance.cardObject,tranShuffle).transform;
            //卡片資料
            CardData card = deck[i];
            //尋找子物件並更新文字
            temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
            temp.Find("攻擊").GetComponent<Text>().text = card.attack.ToString();
            temp.Find("血量").GetComponent<Text>().text = card.hp.ToString();
            temp.Find("名稱").GetComponent<Text>().text = card.name;
            temp.Find("描述").GetComponent<Text>().text = card.description;
            //尋找圖片子物件.圖片  = 來源.載入<圖片>(檔案名稱)
            temp.Find("遮色片").Find("圖片").GetComponent<Image>().sprite = Resources.Load<Sprite>(card.file);

            //添加元件<圖鑑卡牌> 編號 = 卡牌.編號
            temp.gameObject.AddComponent<BookCard>().index = card.index;
            //將生出來的卡牌物件放到清單內
            deckGameObject.Add(temp.gameObject);
        }
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    private void StartBattle()
    {
        gameView.SetActive(true);       //顯示遊戲畫面

        Shuffle();
        BattleManager.instance.StartBattle();
    }
    
}
