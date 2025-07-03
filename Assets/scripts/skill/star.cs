using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class star : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 startPoint;     // 贝塞尔曲线起点
    private Vector2 controlPoint1; // 第一个控制点
    private Vector2 controlPoint2; // 第二个控制点
    private Vector2 endPoint;      // 贝塞尔曲线终点
    private int segments = 100;    // 分段数
    private float t = 0f;          // 贝塞尔曲线参数 0 ~ 1
    private float moveSpeed = 2.0f; // 移动速度（单位：世界坐标/秒）
    private float[] arcLengths;    // 存储弧长表
    private float totalLength;     // 总弧长
    float timeCnt;
    skillUse skill_Use;
    [HideInInspector]
    public int attackPower;

    void Start()
    {
        mainCamera = Camera.main;
        GenerateRandomBezierCurve(); // 初始化第一条曲线
        skill_Use = GameObject.Find("player").GetComponent<skillUse>();
    }

    void Update()
    {
        //持续时间
        timeCnt += Time.deltaTime;
        if (timeCnt >= skill_Use.skill["starAttack"].duration)
        {
            Destroy(gameObject);
        }
        attackPower = skill_Use.skill["starAttack"].attackPower;
        // 增加移动距离（基于时间和速度）
        //  float distanceTravelled = Time.deltaTime * moveSpeed;

        // 找到目标弧长对应的 t 值
        //   t = GetTFromArcLength(t, distanceTravelled);
        t += Time.deltaTime * moveSpeed;
        // 如果 t 超过 1，生成新曲线
        if (t >= 1f)
        {
            t = 0f;
            GenerateRandomBezierCurve();
        }

        // 根据贝塞尔曲线公式计算当前位置
        transform.position = CalculateBezierPoint(t, startPoint, controlPoint1, controlPoint2, endPoint);
    }

    // 随机生成贝塞尔曲线的点
    private void GenerateRandomBezierCurve()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 leftBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height));

        // 起点
        startPoint = transform.position;

        // 控制点1
        controlPoint1 = GetRandomPointInRightHalf(leftBounds, screenBounds);
        // 控制点2
        controlPoint2 = GetRandomPointInRightHalf(leftBounds, screenBounds);

        // 终点
        endPoint = GetRandomPointInRightHalf(leftBounds, screenBounds);

        // 预计算弧长表
        PrecomputeArcLengths();
    }

    // 获取屏幕右半部分的随机点
    private Vector2 GetRandomPointInRightHalf(Vector2 leftBounds, Vector2 screenBounds)
    {
        float randomX = Random.Range(leftBounds.x, screenBounds.x); // X 轴在右半侧
        float randomY = Random.Range(-screenBounds.y, screenBounds.y); // Y 轴上下范围
        return new Vector2(randomX, randomY);
    }

    // 计算三次贝塞尔曲线上的点
    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 point = uuu * p0; // 起点部分
        point += 3 * uu * t * p1; // 控制点1部分
        point += 3 * u * tt * p2; // 控制点2部分
        point += ttt * p3; // 终点部分
        return point;
    }

    // 预计算弧长表
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

    // 根据目标弧长找到对应的 t 值
    private float GetTFromArcLength(float currentT, float distanceTravelled)
    {
        float targetLength = distanceTravelled + currentT * totalLength;

        // 二分查找对应的弧长区间
        int low = 0, high = segments;
        while (low < high)
        {
            int mid = (low + high) / 2;
            if (arcLengths[mid] < targetLength)
                low = mid + 1;
            else
                high = mid;
        }

        // 线性插值计算更精确的 t 值
        int index = Mathf.Clamp(low - 1, 0, segments - 1);
        float segmentLength = arcLengths[index + 1] - arcLengths[index];
        float segmentT = (targetLength - arcLengths[index]) / segmentLength;

        return (index + segmentT) / segments;
    }
}
