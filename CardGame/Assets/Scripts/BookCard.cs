﻿using UnityEngine;
using UnityEngine.UI;

public class BookCard : MonoBehaviour
{
    /// <summary>
    /// 卡牌圖鑑的編號
    /// </summary>
    public int index;


    //原本寫法:透過尋找
    //public deckManager deckManager;
    private void Start()
    {
        //原本寫法:透過尋找
        //deckManager = FindObjectOfType<DeckManager>();

        //取得按鈕.點擊.添加監聽器(方法)
        GetComponent<Button>().onClick.AddListener(ChooseBookCard);
    }

    /// <summary>
    /// 選擇圖鑑內的卡牌
    /// </summary>
    private void ChooseBookCard()
    {
        print("選取的圖鑑編號為:" + index);

        DeckManager.instance.AddCard(index);
    }
}
