using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class gameController : MonoBehaviour
{
    public GameObject ground1, ground2, enemyBlood;
    public List<GameObject> enemyList;
    public GameObject bg_01, bg_02, gameOver,
    gameStart, player
    , apple, goldApple, liveTimeText;
    player playerObj;
    public int enemyCnt;
    float timeCnt = 0, apple_TimeCount, goldApple_TimeCount, interApple, interGoldApple;
    [HideInInspector]
    float moveSpeed;
    [HideInInspector]
    public bool hurtMove, arrowCreate, meteoCreate;
    public int minutes, seconds;
    public Text recordText, pointText;
    List<float> eTimeList = Enumerable.Repeat(0f, 32).ToList();
    public Dictionary<int, (
        float posX,
        float inter,
        float timeStart,
        float timeEnd,
        float randomTimeStart,
        float randomTimeEnd
        )> enemyDic = new Dictionary<int, (
        float posX,
        float inter,
        float timeStart,
        float timeEnd,
        float randomTimeStart,
        float randomTimeEnd)>();
    public class enemy
    {
        public float posX { get; set; }
        public float inter { get; set; }
        public float timeStart { get; set; }
        public float timeEnd { get; set; }
        public float randomTimeStart { get; set; }
        public float randomTimeEnd { get; set; }
    }
    void Start()
    {
        playerObj = player.GetComponent<player>();
        moveSpeed = 3;
        string skillJsonFilePath = Path.Combine(Application.streamingAssetsPath, "textFiles/enemyData.json");
        string skillJson_Dese = System.IO.File.ReadAllText(skillJsonFilePath);
        var jsonFriendlySkill_Dese = JsonConvert.DeserializeObject<Dictionary<int, enemy>>(skillJson_Dese);
        foreach (var kvp in jsonFriendlySkill_Dese)
        {
            enemyDic[kvp.Key] = (
                kvp.Value.posX,
                kvp.Value.inter,
                kvp.Value.timeStart,
                kvp.Value.timeEnd,
                kvp.Value.randomTimeStart,
                kvp.Value.randomTimeEnd
            );
        }
        //敵属性の初期化
        for (int i = 1; i < 32; i++)
        {
            if (enemyDic.ContainsKey(i))
            {
                float posX = Random.Range(3f, 8f);
                float inter = Random.Range(enemyDic[i].randomTimeStart, enemyDic[i].randomTimeEnd);
                var newValue = (
                    posX,
                    inter,
                    enemyDic[i].timeStart,
                    enemyDic[i].timeEnd,
                    enemyDic[i].randomTimeStart,
                    enemyDic[i].randomTimeEnd);
                enemyDic[i] = newValue;
            }
        }
        //リンゴ初期化
        player player_Obj = player.GetComponent<player>();
        interApple = Random.Range(35f * player_Obj.appleRate, 55f * playerObj.appleRate);
        interGoldApple = Random.Range(30f * player_Obj.appleRate, 60f * playerObj.appleRate);
    }

    void Update()
    {
        player player_Obj = player.GetComponent<player>();
        apple_TimeCount += Time.deltaTime;
        goldApple_TimeCount += Time.deltaTime;
        if (apple_TimeCount >= interApple)
        {
            float posX = Random.Range(-0.56f, 8.85f);
            Instantiate(apple, new Vector2(posX, 6.25f), Quaternion.identity);
            apple_TimeCount = 0;
        }
        if (goldApple_TimeCount >= interGoldApple)
        {
            float posX = Random.Range(-0.56f, 8.85f);
            Instantiate(goldApple, new Vector2(posX, 6.25f), Quaternion.identity);
            goldApple_TimeCount = 0;
        }
        //生存時間
        minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad) / 60;
        seconds = Mathf.FloorToInt(Time.timeSinceLevelLoad) % 60;
        if (!player.GetComponent<player>().isDead)
        {
            liveTimeText.GetComponent<Text>().text = $"TIME:   {minutes:D2}:{seconds:D2}";
        }
        if (Time.timeScale == 0)
        {
            return;
        }
        timeCnt += Time.deltaTime;

        //敵現れる時間
        for (int i = 1; i < 32; i++)
        {
            if (enemyDic.ContainsKey(i))
            {
                if (timeCnt >= enemyDic[i].timeStart && timeCnt <= enemyDic[i].timeEnd)
                {
                    eTimeList[i] += Time.deltaTime;
                }
            }
        }

        //敵を生成
        for (int i = 0; i < 32; i++)
        {
            if (enemyDic.ContainsKey(i))
            {
                enemyCreate(i, enemyDic[i].posX, enemyDic[i].inter, enemyDic[i].timeStart,
                               enemyDic[i].timeEnd, enemyDic[i].randomTimeStart,
                               enemyDic[i].randomTimeEnd);
            }
        }
        void enemyCreate(int enemyNum, float posX, float inter, float timeStart, float timeEnd, float randomTime1, float randomTime2)
        {
            GameObject enemys = GameObject.Find("enemys");
            if (eTimeList[enemyNum] >= enemyDic[enemyNum].inter)
            {
                enemyCnt++;
                GameObject enemy = Instantiate(enemyList[enemyNum - 1], new Vector3(Camera.main.transform.position.x + 9.2f + posX, -4.11f, enemyCnt * 0.01f), Quaternion.identity);
                enemy.transform.SetParent(enemys.transform, true);
                posX = Random.Range(3f, 8f);
                inter = Random.Range(randomTime1, randomTime2);
                eTimeList[enemyNum] = 0;
                var newValue = (
                posX,
                inter,
                enemyDic[enemyNum].timeStart,
                enemyDic[enemyNum].timeEnd,
                enemyDic[enemyNum].randomTimeStart,
                enemyDic[enemyNum].randomTimeEnd); ;
                enemyDic[enemyNum] = newValue;
            }
        }

        //プレーヤー死亡
        if (playerObj.hp <= 0)
        {
            int record = PlayerPrefs.GetInt("Record", 0);
            int livetime = GameObject.Find("ui").GetComponent<UI>().walkRecord;
            if (livetime >= record)
            {
                PlayerPrefs.SetInt("Record", livetime);
            }
            SceneManager.LoadScene("overScene");
        }

    }
    private void FixedUpdate()
    {
        GameObject bg_01 = GameObject.Find("map/bg_01");
        GameObject bg_02 = GameObject.Find("map/bg_02");
        GameObject bg_left = bg_01.transform.position.x > bg_02.transform.position.x ? bg_02 : bg_01;
        GameObject bg_right = bg_01.transform.position.x > bg_02.transform.position.x ? bg_01 : bg_02;
        GameObject g_left = ground1.transform.position.x > ground2.transform.position.x ? ground2 : ground1;
        GameObject g_right = ground1.transform.position.x > ground2.transform.position.x ? ground1 : ground2;
        GameObject enemys = GameObject.Find("enemys");
        float bgWidth = bg_left.GetComponent<SpriteRenderer>().bounds.size.x;
        //バックエンドスクロール
        if (bg_left.transform.localPosition.x < Camera.main.transform.position.x - 19.2f)
        {
            bg_left.transform.localPosition = new Vector2(bg_right.transform.localPosition.x + bgWidth, 0.5f);
        }
        //tileMapスクロール
        if (g_left.transform.localPosition.x < Camera.main.transform.position.x - 19.2f)
        {
            g_left.transform.localPosition = new Vector2(g_right.transform.localPosition.x + bgWidth, 0);
        }
        if (!player.GetComponent<player>().isDead && !player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Attack"))
        {
            bg_01.transform.Translate(Vector2.left * moveSpeed / 2 * Time.deltaTime, Space.World);
            bg_02.transform.Translate(Vector2.left * moveSpeed / 2 * Time.deltaTime, Space.World);
            g_left.transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);
            g_right.transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);
            enemys.transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    public void game_Start()
    {

    }
    public void game_Over()
    {

    }
}
