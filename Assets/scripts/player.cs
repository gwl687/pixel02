using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class player : MonoBehaviour
{
    public GameObject attack_01, ground, bg_01, textBar,
    bg_02, skill2, skill3, cdText, playerBlood, fadeImage,
    ice, playerLvText;
    [HideInInspector]
    public bool cantStorage, cantGuard, isDead, isStorageColor, isHurting, isGuarding, isStoraging, startHurt, isStorageAttack, isStrongAttack, isRepelAttack, isCritical, revive;
    [HideInInspector]
    public float hp, realHp, Energy, realEnergy, exp, realExp, skillPower, storageEnergy;
    [HideInInspector]
    public int attack_Power, defence_Power, money, hurtNum;
    [HideInInspector]
    public Animator myAni;
    [HideInInspector]
    public float attackTime, hurtTime, guardTimeCnt, storageTimeCnt, criticalRate, skillCd, appleRate, attackSpeed;
    [HideInInspector]
    public int playerLevel, defense;
    [HideInInspector]
    public string skillName;
    Coroutine storageFlashCorou;
    public string skill;
    public Text moneyText;
    [HideInInspector]
    bool isAttacking, isEnterEndScened, isChargedAttack;
    public AudioClip[] audioClips;
    public AudioSource myAudio;

    private void Awake()
    {
        //��ʼ����ɫ����
        initProperty();

    }
    void Start()
    {

        myAni = GetComponent<Animator>();
        isHurting = false;
        skill = "normal";
        AnimationClip[] clips = myAni.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "player_Attack1")
            {
                attackTime = clip.length;
            }
            if (clip.name == "player_Hurt")
            {
                hurtTime = clip.length;
            }
        }
        //    myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //
        if (realHp > hp)
        {
            realHp = hp;
        }
        //����
        if (cantGuard)
        {
            guardTimeCnt += Time.deltaTime;
        }
        if (guardTimeCnt > 2)
        {
            cantGuard = false;
            guardTimeCnt = 0;
        }
        //��������
        if (cantStorage)
        {
            storageTimeCnt += Time.deltaTime;
        }
        if (storageTimeCnt > 2)
        {
            cantStorage = false;
            storageTimeCnt = 0;
        }
        if (realEnergy < 0)
        {
            realEnergy = 0;
        }
        //�ȼ�
        playerLvText.GetComponent<Text>().text = "Lv." + playerLevel.ToString();
        exp = (float)(playerLevel * 50);
        if (realExp >= exp && playerLevel < 49 && realHp > 0)
        {
            realExp = 0;
            playerLevel++;
            playerLvText.GetComponent<playerLevelUp>().LevelUp();
        }
        //Ǯ
        moneyText.text = PlayerPrefs.GetInt("money", 0).ToString();
        //�����ָ�
        if (realEnergy < Energy)
        {
            realEnergy += Time.deltaTime * 15;
        }
        if (realEnergy > Energy)
        {
            realEnergy = Energy;
        }
        //ǿ��
        //����������   
        AnimatorStateInfo stateInfo = myAni.GetCurrentAnimatorStateInfo(0);
        if (UnityEngine.Input.GetMouseButtonDown(0) && !isDead && !stateInfo.IsName("player_Attack1"))
        {
            // ����Ƿ����� UI Ԫ��
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = UnityEngine.Input.mousePosition
                };

                // ����һ���б����洢���߼��Ľ��
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                // ��������Ƿ��� Button
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.GetComponent<Button>() != null)
                    {
                        return;
                    }
                }
            }
            //û�㵽button,player����
            if (realEnergy < 20)
            {
                return;
            }
            //������һ�¸�����û��playerAttack,�оͲ�������
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                if (obj.name.Contains("playerAttack"))
                {
                    isAttacking = true;
                    return;
                }
                else
                {
                    isAttacking = false;
                }
            }
            if (!isAttacking)
            {
                realEnergy -= 20;
                float Critical = UnityEngine.Random.Range(0, 100f);
                if (Critical <= criticalRate)
                {
                    isCritical = true;
                }
                if (!GetComponent<skillUse>().skill["repelAttack"].isFlashing)
                {
                    myAni.SetTrigger("attack1");
                }
                else
                {
                    myAni.SetTrigger("attack2");
                    isRepelAttack = true;
                }
            }
        }
        //�����Ҽ�����
        if (UnityEngine.Input.GetMouseButton(1))
        {
            if (realEnergy > 1 && !cantGuard)
            {
                isGuarding = true;
                GetComponent<SpriteRenderer>().color = new Color(69f / 255f, 69f / 255f, 69f / 255f);
                realEnergy -= 40 * Time.deltaTime;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                cantGuard = true;
                isGuarding = false;
            }
        }
        if (UnityEngine.Input.GetMouseButtonUp(1))
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            isGuarding = false;
        }
        //���������������
        if (UnityEngine.Input.GetMouseButton(0))
        {
            if (stateInfo.IsName("player_Attack1"))
            {
                if (attackTime * stateInfo.normalizedTime >= 0.07f)
                {
                    myAni.SetBool("attackStay", true);
                }
            }
            if (stateInfo.IsName("player_Attack1_Stay"))
            {
                if (stateInfo.length * stateInfo.normalizedTime > 0.5f)
                {
                    if (realEnergy > 1 && !cantStorage)
                    {
                        isChargedAttack = true;
                        realEnergy -= 40 * Time.deltaTime;
                        isStorageAttack = true;
                        storageEnergy += 40 * Time.deltaTime;
                        if (!isStorageColor)
                        {
                            storageFlashCorou = StartCoroutine(storageFlash());
                            isStorageColor = true;
                        }
                    }
                    else
                    {
                        myAni.SetBool("attackStay", false);
                        StopCoroutine(storageFlashCorou);
                        isStorageColor = false;
                        GetComponent<SpriteRenderer>().color = Color.white;
                        cantStorage = true;
                    }
                }
            }
        }
        //�ſ����
        if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            if (stateInfo.IsName("player_Attack1_Stay"))
            {
                myAni.SetBool("attackStay", false);
                isStorageAttack = false;
            }
            if (isStorageColor)
            {
                StopCoroutine(storageFlashCorou);
                isStorageColor = false;
                GetComponent<SpriteRenderer>().color = Color.white;

            }
            //if (stateInfo.IsName("player_Attack1"))
            //{
            //    realEnergy -= 20;
            //}
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //����
        hurtNum++;
        if (collision.gameObject.tag == "enemy" && !isHurting && isDead)
        {
            isHurting = true;
            //��Ѫ����
            Vector2 bloodPos = new Vector2(-2.3f, -2.6f);
            GameObject playerBloodObj = Instantiate(playerBlood, bloodPos, Quaternion.identity);
            playerBloodObj.name = "playerBloodObj";
            playerBloodObj.GetComponent<Animator>().speed = 0.5f;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy" && !isDead && !collision.GetComponent<enemy1>().isfreeze)
        {
            isHurting = true;
            GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 117f / 255f, 117f / 255f);
            int damage = collision.gameObject.GetComponent<enemy1>().attack_Power;
            realHp -= (isGuarding ? (damage - defence_Power) : damage) * Time.deltaTime;
            if (realHp <= 0)
            {
                if (revive)
                {
                    realHp = hp;
                    revive = false;
                }
                else
                {
                    myAni.SetTrigger("die");
                    isDead = true;
                    //��Ѫ������ʧ
                    //�������и�����
                    GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                    foreach (GameObject rootObject in rootObjects)
                    {
                        if (rootObject.name.Contains("Blood"))
                        {
                            Destroy(rootObject);
                        }
                    }
                    PlayerPrefs.SetFloat("survivedTime", Time.timeSinceLevelLoad);
                    float survivedTime = PlayerPrefs.GetFloat("survivedTime", 0);
                    if (survivedTime > PlayerPrefs.GetFloat("RECORD", 0))
                    {
                        PlayerPrefs.SetFloat("RECORD", survivedTime);
                    }
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        hurtNum--;
        if (hurtNum == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            myAni.SetTrigger("run");
            //��Ѫ������ʧ
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            //�������и�����
            foreach (GameObject rootObject in rootObjects)
            {
                if (rootObject.name.Contains("Blood"))
                {
                    Destroy(rootObject);
                }
            }
            isHurting = false;
        }
    }



    //���ⲿ���ã�������button��ӷ���
    public void addButtonFunc(int skillNum, GameObject skillButton)
    {
        var methodName = $"use_skill{skillNum}";
        MethodInfo methodInfo = GetType().GetMethod(methodName);
        UnityAction action = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), this, methodInfo);
        skillButton.GetComponent<Button>().onClick.AddListener(action);
    }

    //���ɹ�����Ч
    public void attack()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        if (!GetComponent<skillUse>().skill["swordFire"].isFlashing)
        {
            Vector3 attackPos = mousePosition;
            GameObject attackPrefab = Resources.Load<GameObject>("prefabs/skillEffects/playerAttack");
            GameObject attack = Instantiate(attackPrefab, attackPos, Quaternion.identity);
            if (!isChargedAttack)
            {
                attack.transform.tag = "attack";
            }
            else
            {
                attack.transform.tag = "CharagedAttack";
                isChargedAttack = false;
            }
                attack.name = "playerAttack";
        }
        else
        {
            Vector3 attackPos = new Vector3(mousePosition.x - 1.5f, mousePosition.y, -5);
            GameObject attackPrefab = Resources.Load<GameObject>("prefabs/skillEffects/playerAttack_swordFire");
            GameObject attack = Instantiate(attackPrefab, attackPos, Quaternion.Euler(-65.579f, 90, 90));
            attack.name = "playerAttack_swordFire";
        }
        if (GetComponent<skillUse>().skill["repelAttack"].isFlashing)
        {
            isRepelAttack = true;
            var value = GetComponent<skillUse>().skill["repelAttack"];
            var newValue = (value.skillText,
                        GetComponent<skillUse>().timeCnt,
                        value.cd,
                        value.duration,
                        value.isLearned,
                        false,
                        value.attackPower,
                        value.count,
                        value.level);
            GetComponent<skillUse>().skill["repelAttack"] = newValue;
            if (GetComponent<skillUse>().skillCorou["repelAttack"] != null)
            {
                GetComponent<skillUse>().stopCorou("repelAttack");
            }
        }
        if (GetComponent<skillUse>().skill["swordFire"].isFlashing)
        {
            var value = GetComponent<skillUse>().skill["swordFire"];
            var newValue = (value.skillText,
                        GetComponent<skillUse>().timeCnt,
                        value.cd,
                        value.duration,
                        value.isLearned,
                        false,
                        value.attackPower,
                        value.count,
                        value.level);
            GetComponent<skillUse>().skill["swordFire"] = newValue;
            if (GetComponent<skillUse>().skillCorou["swordFire"] != null)
            {
                GetComponent<skillUse>().stopCorou("swordFire");
            }
        }
    }

    public void playerRepelMusic()
    {
        //    myAudio.clip = audioClips[1];
        //    myAudio.Play();
    }
    //��Ϸ�ڳ�ʼ����ɫ����
    public IEnumerator storageFlash()
    {
        float timeCnt = 0;
        timeCnt += Time.deltaTime;
        Color storageColor = new Color(167f / 255f, 191f / 255f, 255f / 255f);
        Color myColor = GetComponent<SpriteRenderer>().color;
        while (true)
        {
            timeCnt += Time.deltaTime;
            float t = Mathf.PingPong(timeCnt / 0.2f, 1);
            GetComponent<SpriteRenderer>().color = Color.Lerp(myColor, storageColor, t);
            yield return null;
        }
    }
    void initProperty()
    {
        playerLevel = 1;
        exp = 50;
        attack_Power = (int)(10 * Math.Pow(1.2, PlayerPrefs.GetInt("attackPower_Level", 0)));
        hp = (int)(100 * Math.Pow(1.1, PlayerPrefs.GetInt("hp_Level", 0)));
        Energy = (int)(100 * Math.Pow(1.1, PlayerPrefs.GetInt("energy_Level", 0)));
        skillCd = (float)(Math.Pow(0.95, PlayerPrefs.GetFloat("skillCd_Level", 0)));
        defence_Power = 5 * Mathf.CeilToInt((float)Math.Pow(1.1, PlayerPrefs.GetInt("defencePower_Level", 0)));
        criticalRate = (float)(15 * Math.Pow(1.2, PlayerPrefs.GetFloat("criticalRate_Level", 0)));
        revive = PlayerPrefs.GetInt("revive_Level") == 1 ? true : false;
        appleRate = (float)(Math.Pow(0.95, PlayerPrefs.GetInt("appleRate_Level")));
        skillPower = (float)(Math.Pow(1.2, PlayerPrefs.GetInt("attackSpeed_Level")));
        //
        realHp = hp;
        realEnergy = Energy;
        realExp = 0;
    }

    public void StartSceneTransition()
    {
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeOutAndLoad()
    {
        float timer = 0f;
        while (timer < 2)
        {
            timer += Time.deltaTime;
            float alpha = timer / 2; // ����͸����
            fadeImage.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene("overScene"); // �л�����
    }
}
