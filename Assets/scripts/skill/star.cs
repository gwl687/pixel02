using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class star : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 startPoint;     // �������������
    private Vector2 controlPoint1; // ��һ�����Ƶ�
    private Vector2 controlPoint2; // �ڶ������Ƶ�
    private Vector2 endPoint;      // �����������յ�
    private int segments = 100;    // �ֶ���
    private float t = 0f;          // ���������߲��� 0 ~ 1
    private float moveSpeed = 2.0f; // �ƶ��ٶȣ���λ����������/�룩
    private float[] arcLengths;    // �洢������
    private float totalLength;     // �ܻ���
    float timeCnt;
    skillUse skill_Use;
    [HideInInspector]
    public int attackPower;

    void Start()
    {
        mainCamera = Camera.main;
        GenerateRandomBezierCurve(); // ��ʼ����һ������
        skill_Use = GameObject.Find("player").GetComponent<skillUse>();
    }

    void Update()
    {
        //����ʱ��
        timeCnt += Time.deltaTime;
        if (timeCnt >= skill_Use.skill["starAttack"].duration)
        {
            Destroy(gameObject);
        }
        attackPower = skill_Use.skill["starAttack"].attackPower;
        // �����ƶ����루����ʱ����ٶȣ�
        //  float distanceTravelled = Time.deltaTime * moveSpeed;

        // �ҵ�Ŀ�껡����Ӧ�� t ֵ
        //   t = GetTFromArcLength(t, distanceTravelled);
        t += Time.deltaTime * moveSpeed;
        // ��� t ���� 1������������
        if (t >= 1f)
        {
            t = 0f;
            GenerateRandomBezierCurve();
        }

        // ���ݱ��������߹�ʽ���㵱ǰλ��
        transform.position = CalculateBezierPoint(t, startPoint, controlPoint1, controlPoint2, endPoint);
    }

    // ������ɱ��������ߵĵ�
    private void GenerateRandomBezierCurve()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 leftBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height));

        // ���
        startPoint = transform.position;

        // ���Ƶ�1
        controlPoint1 = GetRandomPointInRightHalf(leftBounds, screenBounds);
        // ���Ƶ�2
        controlPoint2 = GetRandomPointInRightHalf(leftBounds, screenBounds);

        // �յ�
        endPoint = GetRandomPointInRightHalf(leftBounds, screenBounds);

        // Ԥ���㻡����
        PrecomputeArcLengths();
    }

    // ��ȡ��Ļ�Ұ벿�ֵ������
    private Vector2 GetRandomPointInRightHalf(Vector2 leftBounds, Vector2 screenBounds)
    {
        float randomX = Random.Range(leftBounds.x, screenBounds.x); // X �����Ұ��
        float randomY = Random.Range(-screenBounds.y, screenBounds.y); // Y �����·�Χ
        return new Vector2(randomX, randomY);
    }

    // �������α����������ϵĵ�
    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 point = uuu * p0; // ��㲿��
        point += 3 * uu * t * p1; // ���Ƶ�1����
        point += 3 * u * tt * p2; // ���Ƶ�2����
        point += ttt * p3; // �յ㲿��
        return point;
    }

    // Ԥ���㻡����
    private void PrecomputeArcLengths()
    {
        arcLengths = new float[segments + 1];
        arcLengths[0] = 0f;
        Vector2 prevPoint = CalculateBezierPoint(0f, startPoint, controlPoint1, controlPoint2, endPoint);

        totalLength = 0f;
        for (int i = 1; i <= segments; i++)
        {
            float t = (float)i / segments;
            Vector2 currentPoint = CalculateBezierPoint(t, startPoint, controlPoint1, controlPoint2, endPoint);
            totalLength += Vector2.Distance(prevPoint, currentPoint);
            arcLengths[i] = totalLength;
            prevPoint = currentPoint;
        }
    }

    // ����Ŀ�껡���ҵ���Ӧ�� t ֵ
    private float GetTFromArcLength(float currentT, float distanceTravelled)
    {
        float targetLength = distanceTravelled + currentT * totalLength;

        // ���ֲ��Ҷ�Ӧ�Ļ�������
        int low = 0, high = segments;
        while (low < high)
        {
            int mid = (low + high) / 2;
            if (arcLengths[mid] < targetLength)
                low = mid + 1;
            else
                high = mid;
        }

        // ���Բ�ֵ�������ȷ�� t ֵ
        int index = Mathf.Clamp(low - 1, 0, segments - 1);
        float segmentLength = arcLengths[index + 1] - arcLengths[index];
        float segmentT = (targetLength - arcLengths[index]) / segmentLength;

        return (index + segmentT) / segments;
    }
}
