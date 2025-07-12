using System;

using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class skillStr : MonoBehaviour
{
    public Text textBarText;
    public GameObject textBar, sword_01;
    playerLevelUp pLu;
    public List<GameObject> skillList;
    public List<GameObject> skillButtonLeftList;
    Dictionary<int, Vector2> skillPos;
    GameObject UI, player;
    skillUse skill_Use;
    int skillLeftNum;

    void Start()
    {
        UI = GameObject.Find("ui");
        player = GameObject.Find("player");
        skillPos = new Dictionary<int, Vector2>
        {  { 0,new Vector2(-5.65f,2.56f)},
           { 1,new Vector2(-3.58f,2.56f)}
        };
        skill_Use = GameObject.Find("player").GetComponent<skillUse>();
        pLu = GameObject.Find("ui/textSet/playerLevelText").GetComponent<playerLevelUp>();
    }

    void Update()
    {

    }
    //陨石
    public void meteoriteAttack()
    {
        int skillLv = skill_Use.skill["meteoriteAttack"].level;
        int skillCnt = skill_Use.skill["meteoriteAttack"].count;
        if (skillLv == 4)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("meteoriteAttack");
        }
        //已学习
        var skillTmpValue = (
            skill_Use.skill["meteoriteAttack"].skillText,
            skillLv > 0 ? skill_Use.skill["meteoriteAttack"].startTime : skill_Use.timeCnt,
            skill_Use.skill["meteoriteAttack"].cd * 0.92f,
            skill_Use.skill["meteoriteAttack"].duration,
            true,
            skill_Use.skill["meteoriteAttack"].isFlashing,
            (int)Math.Ceiling(skill_Use.skill["meteoriteAttack"].attackPower * 1.1f),
            skill_Use.skill["meteoriteAttack"].count + 2,
            skillLv + 1
            );
        skill_Use.skill["meteoriteAttack"] = skillTmpValue;
        //生成左边的skillButton
        if (skillLv == 0)
        {
            addButtonToLeft("meteoriteAttack");
        }
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //黑洞
    public void blackhole()
    {
        int skillLv = skill_Use.skill["blackhole"].level;
        int skillCnt = skill_Use.skill["blackhole"].count;
        if (skillLv == 4)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("blackhole");
        }
        //已学习
        var skillTmpValue = (
            skill_Use.skill["blackhole"].skillText,
            skillLv > 0 ? skill_Use.skill["blackhole"].startTime : skill_Use.timeCnt,
            skill_Use.skill["blackhole"].cd * 0.92f,
            skill_Use.skill["blackhole"].duration + 0.8f,
            true,
            skill_Use.skill["blackhole"].isFlashing,
            skill_Use.skill["blackhole"].attackPower,
            skill_Use.skill["blackhole"].count,
            skillLv + 1
            );
        skill_Use.skill["blackhole"] = skillTmpValue;
        //生成左边的skillButton
        if (skillLv == 0)
        {
            addButtonToLeft("blackhole");
        }
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //击退
    public void repelAttack()
    {
        int skillLv = skill_Use.skill["repelAttack"].level;
        int skillCnt = skill_Use.skill["repelAttack"].count;
        if (skillLv == 4)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("repelAttack");
        }
        //已学习
        var skillTmpValue = (
            skill_Use.skill["repelAttack"].skillText,
            skillLv > 0 ? skill_Use.skill["repelAttack"].startTime : skill_Use.timeCnt,
            skill_Use.skill["repelAttack"].cd * 0.92f,
            skill_Use.skill["repelAttack"].duration,
            true,
            skill_Use.skill["repelAttack"].isFlashing,
            (int)(Math.Ceiling(skill_Use.skill["repelAttack"].attackPower * 1.1f)),
            skill_Use.skill["repelAttack"].count,
            skillLv + 1
            );
        skill_Use.skill["repelAttack"] = skillTmpValue;
        //生成左边的skillButton
        if (skillLv == 0)
        {
            addButtonToLeft("repelAttack");
            Debug.Log("生成左边");
        }
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //闪电
    public void thunderAttack()
    {
        int skillLv = skill_Use.skill["thunderAttack"].level;
        int skillCnt = skill_Use.skill["thunderAttack"].count;
        if (skillLv == 4)
        {
            pLu.leftEnableSkillCount -= 1;

        }
        //已学习
        var skillTmpValue = (
            skill_Use.skill["thunderAttack"].skillText,
            skillLv > 0 ? skill_Use.skill["thunderAttack"].startTime : skill_Use.timeCnt,
            skill_Use.skill["thunderAttack"].cd * 0.92f,
            skill_Use.skill["thunderAttack"].duration,
            true,
            skill_Use.skill["thunderAttack"].isFlashing,
            (int)(Math.Ceiling(skill_Use.skill["thunderAttack"].attackPower * 1.1f)),
            skill_Use.skill["thunderAttack"].count,
            skillLv + 1
            );
        skill_Use.skill["thunderAttack"] = skillTmpValue;
        //生成左边的skillButton
        if (skillLv == 0)
        {
            addButtonToLeft("thunderAttack");
        }
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //火剑
    public void swordFire()
    {
        int skillLv = skill_Use.skill["swordFire"].level;
        int skillCnt = skill_Use.skill["swordFire"].count;
        if (skillLv == 4)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("swordFire");
        }
        //已学习
        var skillTmpValue = (
            skill_Use.skill["swordFire"].skillText,
            skillLv > 0 ? skill_Use.skill["swordFire"].startTime : skill_Use.timeCnt,
            skill_Use.skill["swordFire"].cd * 0.92f,
            skill_Use.skill["swordFire"].duration,
            true,
            skill_Use.skill["swordFire"].isFlashing,
            (int)(Math.Ceiling(skill_Use.skill["swordFire"].attackPower * 1.2f)),
            skill_Use.skill["swordFire"].count,
            skillLv + 1
            );
        skill_Use.skill["swordFire"] = skillTmpValue;
        //生成左边的skillButton
        if (skillLv == 0)
        {
            addButtonToLeft("swordFire");
        }
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //冰冻_skill2
    public void iceAttack()
    {
        //学习或强化
        int skillLv = skill_Use.skill["iceAttack"].level;
        int skillCnt = skill_Use.skill["iceAttack"].count;
        if (skillLv == 4)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("iceAttack");
        }
        var skillTmpValue = (
            skill_Use.skill["iceAttack"].skillText,
            skillLv > 0 ? skill_Use.skill["iceAttack"].startTime : skill_Use.timeCnt,
            skill_Use.skill["iceAttack"].cd,
            skill_Use.skill["iceAttack"].duration + 1f,
            true,
            skill_Use.skill["meteoriteAttack"].isFlashing,
            skill_Use.skill["iceAttack"].attackPower,
            skill_Use.skill["iceAttack"].count,
            skillLv + 1
            );
        skill_Use.skill["iceAttack"] = skillTmpValue;
        //生成左边的skillButton
        if (skillLv == 0)
        {
            addButtonToLeft("iceAttack");
        }
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //护卫剑
    public void guardianSword()
    {
        //学习或强化
        int skillLv = skill_Use.skill["guardianSword"].level;
        int skillCnt = skill_Use.skill["guardianSword"].count;
        if (skillLv == 4)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("guardianSword");
        }
        var skillTmpValue = (
            skill_Use.skill["guardianSword"].skillText,
            skill_Use.timeCnt,
            skill_Use.skill["guardianSword"].cd * 0.92f,
            skill_Use.skill["guardianSword"].duration,
            true,
            skill_Use.skill["meteoriteAttack"].isFlashing,
            (int)(skill_Use.skill["guardianSword"].attackPower * 1.2f),
            skillCnt >= 3 ? skillCnt : skillCnt + 1,
            skillLv + 1
        );
        skill_Use.skill["guardianSword"] = skillTmpValue;
        //生成左边的skillButton
        if (skillLv == 0)
        {
            addButtonToLeft("guardianSword");
        }
        //生成护卫剑
        skill_Use.use_GudianSword();
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //startAttack
    public void starAttack()
    {
        //已学习
        int skillLv = skill_Use.skill["starAttack"].level;
        int skillCnt = skill_Use.skill["starAttack"].count;
        if (skillLv == 4)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("starAttack");
        }
        var skillTmpValue = (
            skill_Use.skill["starAttack"].skillText,
            skillLv > 0 ? skill_Use.skill["starAttack"].startTime : skill_Use.timeCnt,
            skill_Use.skill["starAttack"].cd,
            skill_Use.skill["starAttack"].duration,
            true,
            skill_Use.skill["meteoriteAttack"].isFlashing,
            (int)(skill_Use.skill["starAttack"].attackPower * Mathf.Pow(1.2f, skillLv)),
            skillCnt + 1,
            skillLv + 1
        );
        skill_Use.skill["starAttack"] = skillTmpValue;
        //生成左边的skillButton
        if (skillLv == 0)
        {
            addButtonToLeft("starAttack");
        }
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //强化攻击力
    public void attackPowerStren()
    {
        int skillLv = skill_Use.skill["attackPowerStren"].level;
        if (skillLv == 2)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("attackPowerStren");
        }
        addButtonToTop("attackPowerStren");
        player.GetComponent<player>().attack_Power = Mathf.CeilToInt(player.GetComponent<player>().attack_Power * 1.2f);
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //强化能量
    public void energyStren()
    {
        int skillLv = skill_Use.skill["energyStren"].level;
        if (skillLv == 2)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("energyStren");
        }
        addButtonToTop("energyStren");
        player.GetComponent<player>().Energy = player.GetComponent<player>().Energy * 1.2f;
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //强化防御力
    public void guardPowerStren()
    {
        int skillLv = skill_Use.skill["guardPowerStren"].level;
        if (skillLv == 2)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("guardPowerStren");
        }
        addButtonToTop("guardPowerStren");
        player.GetComponent<player>().defence_Power = Mathf.CeilToInt(player.GetComponent<player>().defence_Power * 1.1f);
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //强化血量
    public void hpStren()
    {
        int skillLv = skill_Use.skill["hpStren"].level;
        if (skillLv == 2)
        {
            pLu.leftEnableSkillCount -= 1;
            clearStrList("hpStren");
        }
        addButtonToTop("hpStren");
        player.GetComponent<player>().hp = player.GetComponent<player>().hp * 1.2f;
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    //给强化栏的技能button加上方法
    public void addButtonFunc(GameObject skillButton)
    {
        MethodInfo methodInfo = GetType().GetMethod(skillButton.name);
        UnityAction action = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), this, methodInfo);
        skillButton.GetComponent<Button>().onClick.AddListener(action);
    }
    //添加到左边技能栏
    public void addButtonToLeft(string skillName)
    {
        //生成左边的skillButton
        string ButtonPath = $"prefabs/skillButton/{skillName}_Left";
        GameObject skillLeftButton = Resources.Load<GameObject>(ButtonPath);
        GameObject skill_Bar = GameObject.Find("ui/skill_Bar");
        if (skill_Bar.transform.Find(skillName + "_LeftButton") == null)
        {
            GameObject leftButton = Instantiate(skillLeftButton);
            leftButton.name = skillName + "_LeftButton";
            leftButton.transform.SetParent(skill_Bar.transform);
            leftButton.transform.localPosition = new Vector2(-0.457f + (skill_Use.skillNum % 2) * 0.914f, 1.371f - 0.914f * (skill_Use.skillNum / 2));
            skill_Use.skillNum++;
        }
        else
        {
            Text[] lvText = skill_Bar.transform.Find(skillName + "_LeftButton").gameObject.GetComponentsInChildren<Text>();
            lvText[0].text = "Lv." + skill_Use.skill[skillName].level;
        }
    }
    public void addButtonToTop(string skillName)
    {
        //生成左边的skillButton
        string ButtonPath = $"prefabs/skillButton/{skillName}_Top";
        GameObject skillTopButton = Resources.Load<GameObject>(ButtonPath);
        GameObject ui = GameObject.Find("ui");
        if (ui.transform.Find(skillName + "_TopButton") == null)
        {
            GameObject topButton = Instantiate(skillTopButton);
            topButton.name = skillName + "_TopButton";
            topButton.transform.SetParent(ui.transform);
            topButton.transform.localPosition = new Vector2(-466 + 50 * skill_Use.strenSkillNum, 397);
            //等级TEXT
            //Text[] lvText = leftButton.GetComponentsInChildren<Text>();
            //lvText[0].text = "Lv." + skill_Use.skill[skillName].level;
            skill_Use.strenSkillNum++;
        }
    }

    public void cancal_padel()
    {
        gameObject.SetActive(false);
    }
    public void cancal_TextBar()
    {
        textBar.SetActive(false);
    }
    //升级满时从生成强化技能里排除
    public void clearStrList(string skillName)
    {
        for (int i = 0; i < pLu.skillButtonList.Count; i++)
        {
            if (pLu.skillButtonList[i] != null && pLu.skillButtonList[i].name == skillName)
            {
                pLu.skillButtonList[i] = null;
            }
        }
    }
}
