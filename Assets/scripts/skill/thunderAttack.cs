using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunderAttack : MonoBehaviour
{
    skillUse skill_Use;
    float timeCnt,aniTime;
    [HideInInspector]
    public int attackPower;
    void Start()
    {
    //  GetComponent<AudioSource>().Play();
        skill_Use = GameObject.Find("player").GetComponent<skillUse>();
        attackPower = skill_Use.skill["thunderAttack"].attackPower;
        AnimationClip[] clips = GetComponent<Animator>().runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "blood")
            {
                aniTime = clip.length;
            }
        }
    }
    void Update()
    {
        timeCnt += Time.deltaTime;
        if (timeCnt > aniTime)
        {
            Destroy(gameObject);
        }
    }
}
