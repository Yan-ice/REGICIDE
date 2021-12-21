﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class CardListUI : UIWindow
{
    #region 脚本工具生成的代码
    private Transform m_tfContent;
    private GameObject m_itemCard;
    private List<ItemCard> cardLilst = new List<ItemCard>();
    protected override void ScriptGenerator()
    {
        m_tfContent = FindChild("ScrollView/Viewport/m_tfContent");
        m_itemCard = FindChild("m_itemCard").gameObject;
    }
    #endregion

    protected override void OnCreate()
    {
        base.OnCreate();
        AdjustIconNum(cardLilst,53,m_tfContent,m_itemCard);
        m_itemCard.gameObject.SetActive(false);
        for (int i = 0; i < cardLilst.Count; i++)
        {
            cardLilst[i].Init(i);
        }
    }

    #region 事件
    #endregion

}

class ItemCard : UIWindowWidget
{
    #region 脚本工具生成的代码
    private Image m_imgIcon;
    protected override void ScriptGenerator()
    {
        m_imgIcon = FindChildComponent<Image>("m_imgIcon");
    }
    #endregion

    #region 事件
    public void Init(int index){
        var sprite = CardMgr.Instance.GetCardSprite("card_"+index);
        Debug.Log(sprite);
        if(sprite!= null){
            m_imgIcon.sprite = sprite;
        }
    }
    #endregion

}
