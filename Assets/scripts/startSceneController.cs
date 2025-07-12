using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startSceneController : MonoBehaviour
{
    public Text record;
    public Text attackPowerMoneyText, totalMoneyText;
    public GameObject strenPadel;
    [HideInInspector]
    public Dictionary<string, Dictionary<string, int>> strData;
    void Start()
    {
        //��ʼ�����ܵȼ�
        init_Skill_Property_Level();
        //��߼�¼
        int minutes = Mathf.FloorToInt(PlayerPrefs.GetFloat("RECORD", 0)) / 60;
        int seconds = Mathf.FloorToInt(PlayerPrefs.GetFloat("RECORD", 0)) % 60;
        record.text = $"RECORD:   {minutes:D2}:{seconds:D2}";
    }

    private void Update()
    {
        //�ܽ�Ǯ
        totalMoneyText.text = PlayerPrefs.GetInt("money", 0).ToString();
        //  attackPowerMoneyText.text = PlayerPrefs.GetInt("attackPowerMoney", 100).ToString();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void game_start()
    {
        SceneManager.LoadScene("gameScene");
    }
    public void game_quit()
    {
        Application.Quit();
    }
    //�ر�ǿ����
    public void cancelstren()
    {
        strenPadel.SetActive(false);
    }
    //��ǿ����
    public void openStren()
    {
        strenPadel.SetActive(true);
    }

    //��ʼ����弼�ܵȼ�(������)
    void init_Skill_Property_Level()
    {
        //��ʼ����Ǯ
    //    PlayerPrefs.SetInt("money",0);
        init_StrenPadle();
    }
    void init_StrenPadle()
    {
        string strJsonFilePath = Path.Combine(Application.streamingAssetsPath, "textFiles/strData.json");
        // �����л� 
        string strJson = System.IO.File.ReadAllText(strJsonFilePath);
        // �����л�Ϊ JSON �Ѻõĸ�ʽ
        strData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(strJson);
        foreach (string key in strData.Keys.ToList())
        {
            PlayerPrefs.SetInt(key + "_Level", strData[key]["level"]);
            PlayerPrefs.SetInt(key + "_Money", strData[key]["money"]);
            PlayerPrefs.SetInt(key + "_MaxLevel", strData[key]["maxLevel"]);
        }
    }
}
