using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteo : MonoBehaviour
{
    [HideInInspector]
    public int attackPower;
    void Start()
    {
        GameObject player = GameObject.Find("player");
        attackPower = player.GetComponent<skillUse>().skill["meteoriteAttack"].attackPower;
    }

    void Update()
    {
        float moveSpeed = 1;
        Vector3 direction = new Vector3(1.5f, -1, -5);
        transform.position += new Vector3(1f * Time.deltaTime, -3f * Time.deltaTime, 0);
        //  transform.Translate(moveSpeed * direction * Time.deltaTime, Space.Self);
        if (transform.position.y < -30f)
        {
            Destroy(gameObject);
        }
    }
}
