using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword_01 : MonoBehaviour
{
    float radius; // ��ת�뾶
    float speed;  // ��ת�ٶ�
    float playerWidth, playerHeight;
    [HideInInspector]
    public float angle = 0f; //��ǰ��ת�Ƕ�
    [HideInInspector]
    public int attackPower;

    void Start()
    {
        radius = 1.8f;
        speed = -150f;
        GameObject player = GameObject.Find("player");
        playerWidth = player.GetComponent<SpriteRenderer>().bounds.size.x;
        playerHeight = player.GetComponent<SpriteRenderer>().bounds.size.y;
        attackPower = player.GetComponent<skillUse>().skill["guardianSword"].attackPower;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("player");
        angle += speed * Time.deltaTime;
        angle %= 360f;
        // ������λ��
        float x = player.transform.position.x + playerWidth / 2 + radius * Mathf.Cos((angle + 90) * Mathf.Deg2Rad);
        float y = player.transform.position.y + playerHeight / 2 + radius * Mathf.Sin((angle + 90) * Mathf.Deg2Rad);
        // ����������λ��
        transform.position = new Vector2(x + 0.2f, y - 0.2f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
