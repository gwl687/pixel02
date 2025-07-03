using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class enemy1 : MonoBehaviour
{
    player playerObj;
    Animator myAni;
    BoxCollider2D myBox;
    public float moveSpeed;
    public int attack_Power, hp, money, exp;
    gameController gameCon;
    float timeCnt;
    [HideInInspector]
    public bool getMoney, isfreeze, changeColor, startDead, isRepeled, isStorageAttack;
    void Start()
    {
        isfreeze = false;
        playerObj = GameObject.Find("player").GetComponent<player>();
        gameCon = GameObject.Find("controllers/gameController").GetComponent<gameController>();
        myAni = GetComponent<Animator>();
        myBox = GetComponent<BoxCollider2D>();
        //��ȡ����ʱ��
        RuntimeAnimatorController ac = myAni.runtimeAnimatorController;
        AnimationClip[] clips = ac.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "enemy1_attack")
            {
                break;
            }
        }
        if (transform.localScale.y >= 6)
        {
            myAni.speed = 3 / transform.localScale.y;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //��Ϸ��ͣʱ����
        if (Time.timeScale == 0)
        {
            return;
        }
        //����
        if (myAni.GetCurrentAnimatorClipInfo(0)[0].clip.name == "DEATH" && !startDead)
        {
            myAni.SetBool("isDeath", true);
            AnimatorStateInfo stateInfo = myAni.GetCurrentAnimatorStateInfo(0);
            myBox.enabled = false;
            StartCoroutine(desAfterDie(stateInfo.length));
        }
        //�ƶ�
        if (myAni.GetCurrentAnimatorClipInfo(0)[0].clip.name == "MOVE" && !isfreeze && !isRepeled)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);
        }
        //������
        if (isRepeled)
        {
            timeCnt += Time.deltaTime;
            transform.Translate(Vector2.right * 6 * Time.deltaTime, Space.World);
            if (timeCnt > 0.2)
            {
                timeCnt = 0;
                isRepeled = false;
            }
        }
        //������
        if (isfreeze && !changeColor)
        {
            Color iceColor = new Color(71 / 255f, 46 / 255f, 193 / 255f);
            ChangeColorRecursively(transform, iceColor);
            changeColor = true;
            GetComponent<Animator>().speed = 0;
        }
        if (!isfreeze && changeColor)
        {
            Color normalColor = Color.white;
            ChangeColorRecursively(transform, normalColor);
            changeColor = false;
            GetComponent<Animator>().speed = 1;
        }
    }
    //����������������ʧ
    IEnumerator desAfterDie(float time)
    {
        startDead = true;
        yield return new WaitForSeconds(time + 0.5f);
        if (!getMoney)
        {
            int totalMoney = PlayerPrefs.GetInt("money", 0);
            PlayerPrefs.SetInt("money", money + totalMoney);
            playerObj.GetComponent<player>().realExp += exp;
            getMoney = true;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.ContainsInsensitive("attack") && myAni.GetCurrentAnimatorClipInfo(0)[0].clip.name != "DEATH")
        {
            //ͷ�����������ı�
            float damageTextPosY = transform.position.y + GetComponent<BoxCollider2D>().bounds.size.y;
            float damageTextPosX = transform.position.x - GetComponent<BoxCollider2D>().bounds.size.x / 2;
            Vector2 damageTextPos = new Vector2(damageTextPosX, damageTextPosY);
            Vector2 criDamageTextPos = new Vector2(damageTextPosX, damageTextPosY);
            GameObject uiObj = GameObject.Find("ui");
            //��ȡ�˺�ֵ
            foreach (var component in collision.gameObject.GetComponents<Component>())
            {
                if (component is MonoBehaviour)
                {
                    System.Type scriptType = component.GetType();
                    FieldInfo attckPowerField = scriptType.GetField("attackPower");
                    FieldInfo isRepelAttack = scriptType.GetField("isRepelAttack");
                    FieldInfo isStorageAttackField = scriptType.GetField("isStorageAttack");
                    FieldInfo isCriticalAttackField = scriptType.GetField("isCriticalAttack");
                    if (isStorageAttackField != null && (bool)isStorageAttackField.GetValue(collision.gameObject.GetComponent(scriptType)))
                    {
                        isStorageAttack = true;
                    }
                    if (collision.gameObject.name.Contains("playerAttack") && (bool)isRepelAttack.GetValue(collision.gameObject.GetComponent(scriptType)))
                    {
                        isRepeled = true;
                    }
                    int damage = (int)attckPowerField.GetValue(collision.gameObject.GetComponent(scriptType));
                    hp -= damage;
                    if (isCriticalAttackField != null && (bool)isCriticalAttackField.GetValue(collision.gameObject.GetComponent(scriptType)))
                    {
                        uiObj.GetComponent<UI>().excuteHpReduceCritical(criDamageTextPos, damage);
                    }
                    else
                    {
                        uiObj.GetComponent<UI>().excuteHpReduce(damageTextPos, damage);
                    }
                }
                //���˺�Ϊ�չ�
                if ((collision.name == "playerAttack" || collision.name.Contains("sword")) && !isStorageAttack && collision.tag == "attack")
                {
                    collision.gameObject.tag = "noUse";
                }
                //����Ѫ
                float bloodPosY = transform.position.y + GetComponent<BoxCollider2D>().bounds.size.y / 2;
                float bloodPosX = transform.position.x - GetComponent<BoxCollider2D>().bounds.size.x / 2;
                Vector2 bloodPos = new Vector2(bloodPosX, bloodPosY);
                GameObject enemyBlood = Resources.Load<GameObject>("effects/enemyBlood");
                Instantiate(enemyBlood, bloodPos, Quaternion.identity);
            }
            if (hp <= 0)
            {
                myAni.SetTrigger("4_Death");
            }
            else if (!myAni.GetBool("isDeath"))
            {
                myAni.SetTrigger("3_Damaged");
            }
        }
    }

    //��Ѫ��������һ�κ�����
    IEnumerator blood()
    {
        yield return null;
    }

    //����������ɫ�仯
    void ChangeColorRecursively(Transform parent, Color iceColor)
    {
        // ����ÿ��������
        foreach (Transform child in parent)
        {
            // ����������� Renderer ���
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                //������ɫ
                renderer.material.color = iceColor;
            }

            // �����ݹ�����������������
            ChangeColorRecursively(child, iceColor);
        }
    }
}
