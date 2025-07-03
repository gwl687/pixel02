using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class attack_01 : MonoBehaviour
{
    [HideInInspector]
    public int attackPower;
    public bool isRepelAttack, isCriticalAttack, isStorageAttack;
    skillUse skill_Use;
    player player;
    void Start()
    {
        player = GameObject.Find("player").GetComponent<player>();
        skill_Use = GameObject.Find("player").GetComponent<skillUse>();
        //»÷·É
        if (player.isRepelAttack)
        {
            isRepelAttack = true;
            attackPower = skill_Use.skill["repelAttack"].attackPower + player.attack_Power;
        }
        else
        {
            attackPower = player.attack_Power;
        }
        //±©»÷
        if (player.isCritical)
        {
            isCriticalAttack = true;
            attackPower *= 2;
        }
        //ÐîÁ¦¹¥»÷
        if (player.storageEnergy > 0)
        {
            isStorageAttack = true;
            attackPower += Mathf.CeilToInt(player.storageEnergy * 0.1f);
            player.storageEnergy = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            if (isRepelAttack)
            {
                player.GetComponent<player>().isRepelAttack = false;
            }
            if (isCriticalAttack)
            {
                player.GetComponent<player>().isCritical = false;
                attackPower /= 2;
            }
            Destroy(gameObject);
        }
    }
}
