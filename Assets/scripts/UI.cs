using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI : MonoBehaviour
{
    public GameObject player, liveText, damageText, criDamageText;
    public Text recordText, textBarText;
    public UnityEngine.UI.Image blood, energyBar, expBar;
    public float attackTime, liveTime;
    public int walkRecord;
    [HideInInspector]
    public int leftSkillCount;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //Ѫ��
        float realHp = player.GetComponent<player>().realHp;
        blood.fillAmount = (float)(realHp / player.GetComponent<player>().hp);
        //����
        energyBar.fillAmount = (float)(player.GetComponent<player>().realEnergy / player.GetComponent<player>().Energy);
        //����
        expBar.fillAmount = (float)(player.GetComponent<player>().realExp / player.GetComponent<player>().exp);
        //�����Ļ�öԻ�����ʧ
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = UnityEngine.Input.mousePosition
                };

                // ����һ���б����洢���߼��Ľ��
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                // ��������Ƿ��� Button
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.GetComponent<Button>() != null)
                    {
                        return;
                    }
                }
            }
            //GameObject textBar = GameObject.Find("ui/textBar");
            //if (textBar != null && textBar.activeSelf)
            //{
            //    textBar.SetActive(false);
            //    Time.timeScale = 1;
            //}
        }
    }
    //���ִ���
    IEnumerator TypeText(string text)
    {
        Text textBarText;
        GameObject textBarTextObj = GameObject.Find("ui/textBar/textBarText");
        textBarTextObj.GetComponent<Text>().text = "";
        for (int i = 0; i < text.Length; i++)
        {
            textBarTextObj.GetComponent<Text>().text += text[i];
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void startType(string text)
    {
        StartCoroutine(TypeText(text));
    }

    //��Ѫ��Ч
    public IEnumerator hpReduce(GameObject damageText, Vector2 pos, int damage)
    {
        float time = 0;
        damageText.GetComponent<Text>().text = $"-{damage}";
        Color c1 = damageText.GetComponent<Text>().color;
        while (time >= 0 && time < 0.6f)
        {
            time += Time.deltaTime;
            if (time < 0.3f)
            {
                c1.a += 1f / 0.3f * Time.deltaTime;
                damageText.GetComponent<Text>().color = c1;
                damageText.transform.localScale = new Vector3(time / 0.3f, time / 0.3f, 0);
            }
            else
            {
                c1.a -= 1f / 0.3f * Time.deltaTime;
                damageText.GetComponent<Text>().color = c1;
                damageText.transform.localScale = new Vector3((0.6f - time) / 0.3f, (0.6f - time) / 0.3f, 0);
            }
            yield return null;
        }
        Destroy(damageText);
    }
    //ִ�м�Ѫ������Ч
    public void excuteHpReduce(Vector2 pos, int damage)
    {
        GameObject damageTextObj = Instantiate(damageText, pos, Quaternion.identity);
        damageTextObj.transform.SetParent(transform, true);
        StartCoroutine(hpReduce(damageTextObj, pos, damage));
    }
    public void excuteHpReduceCritical(Vector2 pos, int damage)
    {
        GameObject damageTextObj = Instantiate(criDamageText, pos, Quaternion.identity);
        damageTextObj.transform.SetParent(transform, true);
        StartCoroutine(hpReduce(damageTextObj, pos, damage));
    }
}
