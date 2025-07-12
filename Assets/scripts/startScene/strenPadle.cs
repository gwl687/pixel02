
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class strenPadle : MonoBehaviour
{
    int totalMoney;
    startSceneController sSc;
    void Start()
    {
        sSc = GameObject.Find("controllers/startSceneController").GetComponent<startSceneController>();
        init_StrenPadleCircle();
    }

    void Update()
    {

    }
    //强化
    public void strengthen(string strName)
    {
        totalMoney = PlayerPrefs.GetInt("money", 0);
        int level = PlayerPrefs.GetInt(strName + "_Level", 0);
        int strMoney = PlayerPrefs.GetInt(strName + "_Money", 0);
        if (totalMoney >= strMoney)
        {
            totalMoney -= strMoney;
            PlayerPrefs.SetInt("money", totalMoney);
            PlayerPrefs.SetInt(strName + "_Level", level + 1);
            PlayerPrefs.SetInt(strName + "_Money", Mathf.CeilToInt(strMoney * 1.5f));

            sSc.strData[strName]["level"] = level + 1;
            sSc.strData[strName]["money"] = Mathf.CeilToInt(strMoney * 1.5f);
            string strJsonFilePath = Path.Combine(Application.streamingAssetsPath, "textFiles/strData.json");
            string json = JsonConvert.SerializeObject(sSc.strData, Formatting.Indented);
            File.WriteAllText(strJsonFilePath, json);
            init_StrenPadleCircle();
        }
        else
        {
            Debug.Log("not enough money");
        }
    }
    //サークルアイコンを生成
    void init_StrenPadleCircle()
    {
        //
        foreach (Transform child in transform)
        {
            if (sSc.strData.ContainsKey(child.name))
            {
                for (int i = 0; i < PlayerPrefs.GetInt(child.name + "_Level", 0); i++)
                {
                    GameObject levelCircle = GameObject.Find($"ui/strenPadle/{child.name}/levelCircle{i}");
                    levelCircle.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ui/littleCircle2");
                }
            }
        }
        //強化のお金
        foreach (Transform child in transform)
        {
            if (sSc.strData.ContainsKey(child.name))
            {
                GameObject coinIcon = GameObject.Find($"ui/strenPadle/{child.name}/coinIcon");
                GameObject moneyText = GameObject.Find($"ui/strenPadle/{child.name}/coinIcon/moneyText");
                //PlayerPrefs.SetInt(child.name + "_Money", Mathf.CeilToInt(sSc.strData[child.name]["money"] * 1.5f));
                int money = PlayerPrefs.GetInt(child.name + "_Money");
                int level = PlayerPrefs.GetInt(child.name + "_Level");
                int maxLevel = PlayerPrefs.GetInt(child.name + "_MaxLevel");
                if (level >= maxLevel)
                {
                    Destroy(coinIcon);
                }
                else
                {
                    moneyText.GetComponent<Text>().text = "       " + money.ToString();
                }
            }
        }
    }
}
