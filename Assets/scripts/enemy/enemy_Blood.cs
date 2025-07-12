using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_Blood : MonoBehaviour
{
    float cnt, bloodTime;
    void Start()
    {
        AnimationClip[] clips = GetComponent<Animator>().runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "enemyBlood")
            {
                bloodTime = clip.length;
            }
        }
    }
    void Update()
    {
        cnt += Time.deltaTime;
        if (cnt > bloodTime)
        {
            Destroy(gameObject);
        }
    }
}
