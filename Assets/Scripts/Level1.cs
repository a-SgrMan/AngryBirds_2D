using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

enum State
{
    Normal,
    Preparation,
    Shooting
}

public class Level1 : MonoBehaviour
{
    public RectTransform leftLine;
    public RectTransform rigntLine;
    public Transform centerPoint;
    public Transform shootPoint;

    private readonly float maxDistance = 200;

    private float distance;
    private State state = State.Normal;

    private Vector3 shootPos;

    private void Update()
    {
        if (state == State.Normal)
        {
            if (Input.GetMouseButtonDown(0))
            {
                state = State.Preparation;
            }
        }
        else if (state == State.Preparation)
        {
            PreparationStage();

            if (Input.GetMouseButtonUp(0))
            {
                state = State.Shooting;
                shootPos = shootPoint.position;
            }
        }
        else if (state == State.Shooting)
        {
            shootPoint
                .DOMove(centerPoint.position * 1.5f - shootPos * 0.5f, 0.1f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    state = State.Normal;
                    leftLine.sizeDelta = new Vector2(leftLine.sizeDelta.x, 0);
                    rigntLine.sizeDelta = new Vector2(rigntLine.sizeDelta.x, 0);
                });
            SetLine();
        }
    }

    /// <summary>
    /// 准备阶段
    /// </summary>
    private void PreparationStage()
    {
        distance = Vector2.Distance(Input.mousePosition, centerPoint.position);

        shootPoint.position
            = distance <= maxDistance ?
            Input.mousePosition :
            centerPoint.position + (Input.mousePosition - centerPoint.position).normalized * maxDistance;

        SetLine();
    }

    private void SetLine()
    {
        leftLine.sizeDelta = new Vector2(leftLine.sizeDelta.x, Vector2.Distance(shootPoint.position, leftLine.position));
        leftLine.eulerAngles = new Vector3(0, 0, GetAngle(leftLine.position - shootPoint.position, Vector2.up));
        rigntLine.sizeDelta = new Vector2(rigntLine.sizeDelta.x, Vector2.Distance(shootPoint.position, rigntLine.position));
        rigntLine.eulerAngles = new Vector3(0, 0, GetAngle(rigntLine.position - shootPoint.position, Vector2.up));
    }

    private float GetAngle(Vector2 fromVector, Vector2 toVector )
    {
        float angle = Vector2.Angle(fromVector, toVector); //求出两向量之间的夹角
        angle *= fromVector.x - toVector.x > 0 ? -1 : 1;
        return angle;
    }
}
