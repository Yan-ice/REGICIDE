﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackData
{
    public bool CouldTurnCard;
    public bool CouldDoubleAtk;
    public bool CouldDownBossAtk;
    public bool CouldAddHp;
    public int Damage;
    public bool HadPet;
    public AttackData(List<CardData> list)
    {
        CouldTurnCard = false;
        CouldDoubleAtk = false;
        CouldDownBossAtk = false;
        CouldAddHp = false;
        Damage = 0;
        HadPet = false;

        var count = list.Count;

        foreach (var card in list)
        {
            if (card.IsPet)
            {
                HadPet = true;
            }
            else
            {
                Damage += card.CardValue;
            }

            if (card.cardType == CardType.DIAMOND)
            {
                CouldTurnCard = true;
            }
            else if (card.cardType == CardType.CLUB)
            {
                CouldDoubleAtk = true;
            }
            else if (card.cardType == CardType.SPADE)
            {
                CouldDownBossAtk = true;
            }
            else if (card.cardType == CardType.HEART)
            {
                CouldAddHp = true;
            }
        }

        if (HadPet)
        {
            Damage += 1;
        }
    }
}

public class BattleMgr : Singleton<BattleMgr>
{
    public AttackData GenAttackData(List<CardData> list)
    {
        return new AttackData(list);
    }

    public void ImpactSkill(AttackData attackData)
    {
        Debug.Log(string.Format("attackData: 数值{0},抽卡{1},降低boss攻击{2},回复{3},双倍攻击{4}",attackData.Damage,attackData.CouldTurnCard,attackData.CouldDownBossAtk,attackData.CouldAddHp,attackData.CouldDoubleAtk));

        bool couldDouble = attackData.CouldDoubleAtk;

        int value = couldDouble ? attackData.Damage * 2 : attackData.Damage;

        if (attackData.CouldDownBossAtk)
        {
            EventCenter.Instance.EventTrigger<int>("DownAtk", value);
        }

        if (attackData.CouldTurnCard)
        {
            GameMgr.Instance.TurnCard(value);
        }

        if (attackData.CouldAddHp)
        {
            
        }

        EventCenter.Instance.EventTrigger<int>("AttackBoss", value);
    }
}