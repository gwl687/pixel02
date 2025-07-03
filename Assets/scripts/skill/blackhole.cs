using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class blackhole : MonoBehaviour
{
    float timeCnt;
    skillUse skill_Use;
    void Start()
    {
        skill_Use = GameObject.Find("player").GetComponent<skillUse>();
    }


    void Update()
    {
        GameObject player = GameObject.Find("player");
        timeCnt += Time.deltaTime;
        GameObject enemys = GameObject.Find("enemys");
        foreach (Transform child in enemys.transform)
        {
            Vector3 holePos = new Vector3(5.79f, -4.11f);
            Vector3 targetPosition = holePos - child.transform.position;
            if (child.transform.position.x >= player.transform.position.x + 0.5f && child.gameObject.GetComponent<enemy1>().hp > 0)
            {
                child.transform.position = Vector2.MoveTowards(child.transform.position, holePos, 2.5f * Time.deltaTime);
            }
        }
        float duaration = skill_Use.skill["blackhole"].duration;
        if (timeCnt >= duaration)
        {
            Destroy(gameObject);
        }
    }
}
