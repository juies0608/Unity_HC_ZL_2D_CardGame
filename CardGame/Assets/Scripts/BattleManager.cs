using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// 對戰管理器實體物件
    /// </summary>
    public static BattleManager instance;

    [Header("金幣")]
    public Rigidbody coin;
    [Header("遊戲畫面")]
    public GameObject gameView;
    [Header("畫布")]
    public Transform canvas;
    [Header("手牌區域")]
    public Transform handArea;

    /// <summary>
    /// 先後攻
    /// true 先
    /// false 後
    /// </summary>
    private bool firstAttack;

    [Header("手排卡牌資訊")]
    /// <summary>
    /// 對戰用牌組:手牌
    /// </summary>
    public List<CardData> battleDeck = new List<CardData>();
    [Header("手牌卡牌遊戲物件")]
    public List<GameObject> handGameObject = new List<GameObject>();

    private void Start()
    {
        instance = this;
    }
    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartBattle()
    {
        gameView.SetActive(true);       //顯示遊戲畫面

        ThrowCoin();
    }

    /// <summary>
    /// 擲金幣
    /// </summary>
    private void ThrowCoin()
    {
        coin.AddForce(0, Random.Range(100, 300), 0);     //推力
        coin.AddTorque(Random.Range(30, 120), 0, 0);    //旋轉

        Invoke("CheckCoin", 3);                         //延遲呼叫檢查方法
    }

    /// <summary>
    /// 檢查金幣正反面
    /// rotation.x 為-1 -背面
    /// rotation.x 為 0 -正面
    /// </summary>
    private void CheckCoin()
    {
        //三元運算子
        //先後攻 = 布林運算? 成立 : 不成立
        print(coin.transform.GetChild(0).position.y);

        firstAttack = coin.transform.GetChild(0).position.y > 0.25 ?  true: false;

        print("先後攻:" + firstAttack);

        /** if (coin.rotation.x<0)
         {
             firstAttack = false;
         }
         else
         {
             firstAttack = true;
         }*/
        StartCoroutine(GetCard(3));
    }

    /// <summary>
    /// 抽牌組卡牌到手牌組
    /// </summary>
    private IEnumerator GetCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
        //抽牌組第一張 放到 手牌 第一張 
        battleDeck.Add(DeckManager.instance.deck[0]);
        //刪除 牌組第一張
        DeckManager.instance.deck.RemoveAt(0);
        //抽牌組第一張物件 放到 手牌 第一張
        handGameObject.Add(DeckManager.instance.deckGameObject[0]);
        //刪除 牌組第一張遊戲物件
        DeckManager.instance.deckGameObject.RemoveAt(0);

            //等待協程執行結束
       yield return StartCoroutine(MoveCard());
        }
        
    }

   

    /// <summary>
    /// 顯示卡牌在移動到手上
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCard()
    {   
        RectTransform card = handGameObject[handGameObject.Count - 1].GetComponent<RectTransform>();        //取得手排最後一張[數量-1]

        card.SetParent(canvas);                     //將父物件設為畫布
        card.anchorMin = Vector2.one * 0.5f;       //設定中心點
        card.anchorMax = Vector2.one * 0.5f;       //設定中心點
      

        while (card.anchoredPosition.x > 501)       //當 x >500 執行移動
        {
            card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(500, 2), 0.5f * Time.deltaTime * 50);       //取得手排第一張[0]

            yield return null;                  //等待一個影格
        }

        yield return new WaitForSeconds(0.35f);

        card.localScale = Vector3.one * 0.5f;       //縮小

        while (card.anchoredPosition.y > -274)       //當 y >-27 執行移動
        {
            card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(0, -275), 0.5f * Time.deltaTime * 50);

            yield return null;                  //等待一個影格

        }
        card.SetParent(handArea);                           //受定父物件為手牌區域
        card.gameObject.AddComponent<HandCard>();           //添加手牌腳本 - 可拖拉
    }
}
