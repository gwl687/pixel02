using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Windows;

public class apple : MonoBehaviour
{
    float gravity = -8f; // 重力加速度
    private Vector3 velocity;      // 记录速度
    UI ui;
    player player;
    public GameObject strBar;
    public List<GameObject> skillList;

    void Start()
    {
        ui = GameObject.Find("ui").GetComponent<UI>();
        player = GameObject.Find("player").GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y += gravity * Time.deltaTime;

        // 更新物体位置
        transform.position += velocity * Time.deltaTime;

        //销毁
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("attack"))
        {
            if (name.Contains("gold"))
            {
                int money = PlayerPrefs.GetInt("money", 0);
                PlayerPrefs.SetInt("money", money + 10);
            }
            else
            {
                GameObject heal = GameObject.Find("effects/Healing");
                heal.GetComponent<ParticleSystem>().Play();
                player.realHp += 50;
            }
            Destroy(gameObject);
        }
    }
    void InvokeMethod(System.Reflection.MethodInfo method)
    {
        method.Invoke(this, null);
    }
    public void testMethod()
    {
        Debug.Log("测试用方法");
    }
}
