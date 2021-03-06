using System;
using System.Collections;
using System.Collections.Generic;
using RegicideProtocol;
using UnityEngine;

class GameOnlineMgr:Singleton<GameOnlineMgr>
{
    #region 协议
    public void Init()
    {
        GameClient.Instance.RegActionHandle((int)ActionCode.Attack, AttackRes);
        GameClient.Instance.RegActionHandle((int)ActionCode.Skill, SkillRes);
        GameClient.Instance.RegActionHandle((int)ActionCode.Damage, DamageRes);
        GameClient.Instance.RegActionHandle((int)ActionCode.Hurt, HurtRes);
    }

    private void HurtRes(MainPack mainPack)
    {
        throw new NotImplementedException();
    }

    private void DamageRes(MainPack mainPack)
    {
        throw new NotImplementedException();
    }

    private void SkillRes(MainPack mainPack)
    {
        throw new NotImplementedException();
    }

    #region Attack
    public void AttackReq()
    {
        MainPack mainPack = ProtoUtil.BuildMainPack(RequestCode.Game, ActionCode.Attack);

        GameClient.Instance.SendCSMsg(mainPack);
    }

    private void AttackRes(MainPack mainPack)
    {
        throw new NotImplementedException();
    }
    #endregion

    #endregion



    public int MyGameIndex { private set; get; }
    public int PlayerNum { private set; get; }
    public uint GameId { private set; get; }
    public uint GameIndex { private set; get; }

    #region List
    public const int TotalCardNum = 54;
    private List<CardData> m_totalList = new List<CardData>(TotalCardNum);  //总牌堆
    private List<CardData> m_myList = new List<CardData>(TotalCardNum);     //可抽卡
    private List<CardData> m_useList = new List<CardData>(TotalCardNum);    //墓地
    private List<CardData> m_bossList = new List<CardData>();                       //boss堆

    #endregion

    private List<PlayerActor> m_players = new List<PlayerActor>();

    public void Attack(int actorIndex)
    {
        var actor = m_players[actorIndex];

        ActorEventHelper.Send(actor,"Attack");
    }

    public void Hurt(int actorIndex)
    {
        var actor = m_players[actorIndex];

        ActorEventHelper.Send(actor, "Hurt");
    }
}