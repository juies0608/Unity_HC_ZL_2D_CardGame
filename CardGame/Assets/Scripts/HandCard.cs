﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 手牌:選取後拖拉
/// </summary>
/// IBeginDragHandler 開始拖拉
/// IDragHandler        拖拉中 
/// IEndDragHandler     拖拉結束
public class HandCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 卡牌座標資訊
    /// </summary>
    private RectTransform rect;

    /// <summary>
    /// 場地
    /// </summary>
    public Transform scene;


    /// <summary>
    ///原始做標
    /// </summary>
    private Vector3 original;

    /// <summary>
    /// 是否在場上
    /// </summary>
    private bool inScene;

    /// <summary>
    /// 水晶消耗
    /// </summary>
    private int crystalCost;

    /// <summary>
    /// 場景名稱
    /// </summary>
    public string sceneName;
    /// <summary>
    /// 手牌脫拉進場地位置
    /// </summary>
    public float pos;

    private void Start()
    {
        rect = GetComponent<RectTransform>();

        //123.ToString();      數值轉字串
        //int.Parse("123");     字串轉數值
        crystalCost = int.Parse(transform.Find("消耗").GetComponent<Text>().text);

        //取得場地物件
        scene = GameObject.Find(sceneName).transform;

    }

    /// <summary>
    /// 開始拖拉
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //原始做標 = 卡牌.座標
        original = transform.position;
    }
    /// <summary>
    /// 拖拉中
    /// </summary>
    /// <param name="eventData">拖拉資訊</param>
    public void OnDrag(PointerEventData eventData)
    {
        if (inScene) return;            //如果 在場內 就跳出
        //此卡牌.座標 = 拖拉.座標
        transform.position = eventData.position;
    }

    /// <summary>
    /// 結束拖拉
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        bool con;
        if (sceneName.Contains("NPC")) con = rect.anchoredPosition.y <= pos;
        else con = rect.anchoredPosition.y >= pos;
        
        if (rect.anchoredPosition.y >= 30)   //如果開放時 y 軸 >=30
        {
            checkCrystal();
        }
        else
        {
            transform.position = original;  //卡牌.座標 = 原始座標
        }
    }

    /// <summary>
    /// 檢查水晶數量
    /// </summary>
    private void checkCrystal()
    {
        if (crystalCost <= BattleManager.instance.crystal)
        {
            inScene = true;                 //是否在場景上 = 是
            transform.SetParent(scene);     //父物建設為:場地
            BattleManager.instance.crystal -= crystalCost;      //扣掉水晶
            BattleManager.instance.UpdateCrystal();     //更新水晶
            BattleManager.instance.handCardCount--;     //手牌數量減一
        }
        else
        {
            transform.position = original;  //卡牌.座標 = 原始座標
        }
    }
}
