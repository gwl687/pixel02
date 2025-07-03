using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System;
using UnityEngine.Events;
using UnityEngine.Windows;
using System.Reflection;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine.Rendering;
using Unity.VisualScripting;



public class SkillData
{
    public string[] skillText { get; set; }
    public float startTime { get; set; }
    public float cd { get; set; }
    public float duration { get; set; }
    public bool isLearned { get; set; }
    public bool isFlashing { get; set; }
    public int attackPower { get; set; }
    public int count { get; set; }
    public int level { get; set; }
}

public class skillUse : MonoBehaviour
{
    public GameObject star, sword_01, blackhole, thunderAttack, meteoAttack, iceAttack;
    [HideInInspector]
    public float timeCnt;
    [HideInInspector]
    public int skillNum, strenSkillNum;
    GameObject UI;
    bool startMeteo;
    public Dictionary<string, Coroutine> skillCorou = new Dictionary<string, Coroutine>();
    //��ʼ�������ֵ䡣ֵΪ�ı������ܿ�ʼʱ�䣬cd���Ƿ�ѧϰ,������,���������ܵȼ�)
    public Dictionary<string, (string[] skillText,
        float startTime,
        float cd,
        float duration,
        bool isLearned,
        bool isFlashing,
        int attackPower,
        int count,
        int level)> skill = new Dictionary<string, (
            string[] skillText,
            float startTime,
            float cd,
            float duration,
            bool isLearned,
            bool isFlashing,
            int attackPower,
            int count,
            int level)>();
    void Start()
    {
        UI = GameObject.Find("ui");
        //skill�ļ�·��
        string skillJsonFilePath = Path.Combine(Application.streamingAssetsPath, "textFiles/skillData.json");
        // �����л� 
        string skillJson_Dese = System.IO.File.ReadAllText(skillJsonFilePath);
        // �����л�Ϊ JSON �Ѻõĸ�ʽ
        var jsonFriendlySkill_Dese = JsonConvert.DeserializeObject<Dictionary<string, SkillData>>(skillJson_Dese);
        // ת����ԭʼ�ֵ��ʽ
        foreach (var kvp in jsonFriendlySkill_Dese)
        {
            skill[kvp.Key] = (
                kvp.Value.skillText,
                kvp.Value.startTime,
                kvp.Value.cd,
                kvp.Value.duration,
                kvp.Value.isLearned,
                kvp.Value.isFlashing,
                kvp.Value.attackPower,
                kvp.Value.count,
                kvp.Value.level
            );
        }
    }
    void Update()
    {
        timeCnt += Time.deltaTime;
        GameObject skill_Bar = GameObject.Find("ui/skillBar");
        foreach (var key in skill.Keys.ToList())
        {
            var value = skill[key];
            GameObject leftButton = GameObject.Find($"ui/skill_Bar/{key}_LeftButton");
            if (leftButton != null)
            {
                Text[] lvText = leftButton.GetComponentsInChildren<Text>();
                lvText[0].text = "Lv." + value.level;
            }
            //cd��Ϊ0,cd����ִ��һ��
            if (!value.isFlashing && value.cd != 0 && value.isLearned && timeCnt >= value.startTime + value.cd)
            {
                var newValue = (value.skillText,
                    timeCnt,
                    value.cd,
                    value.duration,
                    value.isLearned,
                    value.isFlashing,
                    value.attackPower,
                    value.count,
                    value.level);
                skill[key] = newValue;
                string skillName = char.ToUpper(key[0]) + key.Substring(1);
                string skillFuncName = "use_" + skillName;
                MethodInfo method = GetType().GetMethod(skillFuncName);
                method.Invoke(this, null);
            }
            //��߼�������cd
            GameObject cdImage = GameObject.Find($"ui/skill_Bar/{key}_LeftButton/{key}_Cd");
            if (cdImage != null && !value.isFlashing)
            {
                cdImage.GetComponent<Image>().fillAmount = (timeCnt - value.startTime) / value.cd;
            }
        }
    }
    //���˹���
    public void use_RepelAttack()
    {
        GetComponent<player>().isRepelAttack = true;
        var value = skill["repelAttack"];
        var newValue = (value.skillText,
                    timeCnt,
                    value.cd,
                    value.duration,
                    value.isLearned,
                    true,
                    value.attackPower,
                    value.count,
                    value.level);
        skill["repelAttack"] = newValue;
        skillCorou["repelAttack"] = StartCoroutine(buttonFlash("repelAttack"));
    }

    //�ڶ�
    public void use_Blackhole()
    {
        Instantiate(blackhole, new Vector2(5.79f, -3.29f), Quaternion.identity);
    }
    //�׵�
    public void use_ThunderAttack()
    {
        Instantiate(thunderAttack, new Vector2(-1f, -4.41f), Quaternion.identity);

    }
    //��ʯ
    public void use_MeteoriteAttack()
    {
        StartCoroutine(meteoCreate());
    }
    IEnumerator meteoCreate()
    {
        int meteoCount = skill["meteoriteAttack"].count;
        for (int i = 0; i < meteoCount; i++)
        {
            float posX = UnityEngine.Random.Range(-4.84f, 6.68f);
            float posY = UnityEngine.Random.Range(6f, 10f);
            Vector3 pos = new Vector3(posX, posY, -5);
            GameObject meteo = Instantiate(meteoAttack, pos, Quaternion.Euler(new Vector3(0, 0, -53.361f)));
            GameObject effects = GameObject.Find("effects");
            meteo.transform.SetParent(effects.transform, true);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    //��
    public void use_IceAttack()
    {
        GameObject enemys = GameObject.Find("enemys");
        foreach (Transform child in enemys.transform)
        {
            if (child.gameObject.name.Contains("enemy"))
            {
                Type scriptType = child.gameObject.GetComponent<enemy1>().GetType();
                scriptType.GetField("isfreeze").SetValue(child.gameObject.GetComponent<enemy1>(), true);
                StartCoroutine(iceStop(child.gameObject));
            }
        }
    }
    IEnumerator iceStop(GameObject enemy1)
    {
        float iceTime = skill["iceAttack"].duration;
        yield return new WaitForSeconds(iceTime);
        if (enemy1.gameObject != null)
        {
            enemy1.GetComponent<enemy1>().isfreeze = false;
        }
    }
    //��
    public void use_SwordFire()
    {
        //  GetComponent<player>().skillName = "playerAttack_swordFire";
        var value = skill["swordFire"];
        var newValue = (value.skillText,
                    timeCnt,
                    value.cd,
                    value.duration,
                    value.isLearned,
                    true,
                    value.attackPower,
                    value.count,
                    value.level);
        skill["swordFire"] = newValue;
        skillCorou["swordFire"] = StartCoroutine(buttonFlash("swordFire"));
    }
    //������
    public void use_GudianSword()
    {
        //���ɽ�֮ǰ��������н�
        GameObject player = GameObject.Find("player");
        foreach (Transform child in player.transform)
        {
            if (child.gameObject.name.Contains("sword_01"))
            {
                Destroy(child.gameObject);
            }
        }
        //���ɽ�
        int swordNum = skill["guardianSword"].count;
        for (int i = 0; i < swordNum; i++)
        {
            Vector2 playerPos = player.transform.position;
            float playerWidth = player.GetComponent<SpriteRenderer>().bounds.size.x;
            float playerHeight = player.GetComponent<SpriteRenderer>().bounds.size.y;
            Vector2 playerCenterPos = new Vector2(playerPos.x + playerWidth / 2, playerPos.y + playerHeight / 2);
            float swordAngle = (float)(-360 * i / swordNum);
            GameObject sword_01_Obj = Instantiate(sword_01);
            sword_01_Obj.transform.SetParent(player.transform, true);
            sword_01_Obj.GetComponent<sword_01>().angle = swordAngle;
        }
    }

    //����
    public void use_StarAttack()
    {
        int count = skill["starAttack"].count;
        for (int i = 0; i < count; i++)
        {
            GameObject star_obj = Instantiate(star);
            star_obj.name = "star";
            GameObject effects = GameObject.Find("effects");
            star_obj.transform.SetParent(effects.transform, false);
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)); Vector2 leftBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            float randomX = UnityEngine.Random.Range(-screenBounds.x, screenBounds.x); // X �����Ұ��
            float randomY = UnityEngine.Random.Range(-screenBounds.y, screenBounds.y); // Y �����·�Χ
            star_obj.transform.position = new Vector2(randomX, randomY);
        }
    }
    //���ͼ����˸
    public IEnumerator buttonFlash(string skillName)
    {
        float timeCnt = 0;
        timeCnt += Time.deltaTime;
        Image leftCdImg = GameObject.Find($"ui/skill_Bar/{skillName}_LeftButton/{skillName}_Cd").GetComponent<Image>();
        Color cdColor = new Color(80f / 255f, 80f / 255f, 80f / 255f);
        Color imgColor = leftCdImg.color;
        while (true)
        {
            timeCnt += Time.deltaTime;
            float t = Mathf.PingPong(timeCnt / 0.5f, 1);
            leftCdImg.color = Color.Lerp(imgColor, cdColor, t);
            yield return null;
        }
    }

    public void stopCorou(string skillName)
    {
        StopCoroutine(skillCorou[skillName]);
        skillCorou[skillName] = null;
        Image leftCdImg = GameObject.Find($"ui/skill_Bar/{skillName}_LeftButton/{skillName}_Cd").GetComponent<Image>();
        leftCdImg.color = Color.white;
    }
}
