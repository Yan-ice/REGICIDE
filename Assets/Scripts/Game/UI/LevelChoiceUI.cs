using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class LevelChoiceUI : UIWindow
{
    private List<ItemLevel> m_list = new List<ItemLevel>();
    #region 脚本工具生成的代码
    private GameObject m_itemLevel;
    private Button m_btnClose;
    private Transform m_tfLevel;
    protected override void ScriptGenerator()
    {
        m_itemLevel = FindChild("m_itemLevel").gameObject;
        m_btnClose = FindChildComponent<Button>("m_btnClose");
        m_tfLevel = FindChild("m_tfLevel");
        m_btnClose.onClick.AddListener(OnClickCloseBtn);
    }
    #endregion

    protected override void OnCreate()
    {
        base.OnCreate();
        AdjustIconNum(m_list,4,m_tfLevel,m_itemLevel);
        for (int i = 0; i < m_list.Count; i++)
        {
            m_list[i].Init(i);
        }
        m_itemLevel.gameObject.SetActive(false);
    }

    #region 事件
    private void OnClickCloseBtn()
    {
        Close();
    }
	#endregion

}


class ItemLevel : UIWindowWidget
{
    #region 脚本工具生成的代码

    private Button m_btn;
    private Transform m_tfContent;
    private Image m_imgIcon;
    private Text m_textRoomInfo;
    private Text m_textNum;
    private Text m_textRoomId;
    protected override void ScriptGenerator()
    {
        m_tfContent = FindChild("m_tfContent");
        m_imgIcon = FindChildComponent<Image>("m_tfContent/m_imgIcon");
        m_textRoomInfo = FindChildComponent<Text>("m_tfContent/m_textRoomInfo");
        m_textNum = FindChildComponent<Text>("m_tfContent/m_textNum");
        m_textRoomId = FindChildComponent<Text>("m_textRoomId");
        m_btn = UnityUtil.GetComponent<Button>(gameObject);
        m_btn.onClick.AddListener(Choice);
    }
    #endregion

    protected override void OnCreate()
    {
        base.OnCreate();
        var rect = m_tfContent as RectTransform;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    #region 事件

    private int m_index;
    public void Init(int index)
    {
        m_index = index;
        switch (index)
        {
            case 0:
            {
                m_textRoomInfo.text = "初级";
                break;
            }
            case 1:
            {
                m_textRoomInfo.text = "中级";
                    break;
            }
            case 2:
            {
                m_textRoomInfo.text = "高级";
                    break;
            }
            case 3:
            {
                m_textRoomInfo.text = "噩梦";
                    break;
            }
        }
        m_textNum.text = ((index + 1) * 4).ToString();
    }

    private void Choice()
    {
        GameMgr.Instance.RestartGame(m_index);
        UISys.Mgr.CloseWindow<LevelChoiceUI>();
    }
    #endregion

}
