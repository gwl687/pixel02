using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fireSlash : MonoBehaviour
{
    [HideInInspector]
    public int attackPower;
    public bool isRepelAttack;
    float timeCnt;
    skillUse skill_Use;
    player player;
    void Start()
    {
        player = GameObject.Find("player").GetComponent<player>();
        skill_Use = GameObject.Find("player").GetComponent<skillUse>();
        if (player.isRepelAttack)
        {
            isRepelAttack = true;
            attackPower = skill_Use.skill["swordFire"].attackPower + skill_Use.skill["repelAttack"].attackPower + player.attack_Power;     
        }
        else
        {
            attackPower = skill_Use.skill["swordFire"].attackPower + player.attack_Power;
        }
    }

    void Update()
    {
        timeCnt += Time.deltaTime;
        if (timeCnt >= 0.3f)
        {
            if (isRepelAttack)
            {
                player.GetComponent<player>().isRepelAttack = false;
            }
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
