using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public GameObject textBarText;
    void Start()
    {
        StartCoroutine(TypeText("������"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void testMethod()
    {
        Debug.Log("�����÷���");
    }

    IEnumerator TypeText(string text)
    {
        GetComponent<Text>().text = "";
        for (int i = 0; i < text.Length; i++)
        {
            GetComponent<Text>().text += text[i];
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}
