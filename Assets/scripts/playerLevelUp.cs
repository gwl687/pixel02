using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class playerLevelUp : MonoBehaviour
{
    UI ui;
    player player;
    public GameObject strBar;
    public List<GameObject> skillButtonList;
    skillUse skillUse;
    public int leftEnableSkillCount;
    void Start()
    {
        ui = GameObject.Find("ui").GetComponent<UI>();
        player = GameObject.Find("player").GetComponent<player>();
        skillUse = GameObject.Find("player").GetComponent<skillUse>();
        leftEnableSkillCount = 12;
    }

    void Update()
    {

    }

    public void LevelUp()
    {
        List<GameObject> SkillListInStrBar = new List<GameObject>();
        GameObject strBarObj = Instantiate(strBar, new Vector3(0, 0, 0), Quaternion.identity);
        strBarObj.SetActive(true);
        strBarObj.transform.SetParent(ui.transform, true);
        while (true)
        {
            if (strBarObj.transform.childCount >= 3)
            {
                break;
            }
            int skillNum = Random.Range(0, skillButtonList.Count);
            if (skillButtonList[skillNum] != null && !SkillListInStrBar.Contains(skillButtonList[skillNum]))
            {
                SkillListInStrBar.Add(skillButtonList[skillNum]);
            }
            if (SkillListInStrBar.Count == 3 || SkillListInStrBar.Count == leftEnableSkillCount)
            {
                for (int i = 0; i < (SkillListInStrBar.Count == leftEnableSkillCount ? leftEnableSkillCount : 3); i++)
                {
                    GameObject skillButton = Instantiate(SkillListInStrBar[i]);
                    skillButton.name = SkillListInStrBar[i].name;
                    skillButton.transform.SetParent(strBarObj.transform, false);
                    skillButton.transform.localPosition = new Vector2(-4.88f, 1.9f - 1.9f * i);
                    Transform skillButtonText = skillButton.transform.Find($"{skillButton.name}Text");
                    if (skillButtonText != null)
                    {
                        if (skillUse.skill.ContainsKey(skillButton.name))
                        {
                            //已学习，显示text0,否则显示text1及以上
                            if (skillUse.skill[skillButton.name].isLearned == false)
                            {
                                skillButtonText.gameObject.GetComponent<Text>().text = skillUse.skill[skillButton.name].skillText[0];
                            }
                            else
                            {
                                int textNum = Random.Range(1, skillUse.skill[skillButton.name].skillText.Length);
                                skillButtonText.gameObject.GetComponent<Text>().text = skillUse.skill[skillButton.name].skillText[textNum];
                            }
                        }
                    }
                    //  skillButtonText.gameObject.GetComponent<Text>().text = "text";
                    var method = strBarObj.GetComponent<skillStr>().GetType().GetMethod($"{SkillListInStrBar[i].name}");
                    if (method != null)
                    {
                        strBarObj.GetComponent<skillStr>().addButtonFunc(skillButton);
                    }
                }
            }
        }
        Time.timeScale = 0;
    }
}

