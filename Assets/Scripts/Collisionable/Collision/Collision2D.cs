using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CapsuleCollision2D
{
    #region �v�Z
    private static Vector2 GetNearestPoint2D(Vector2 origin, Vector2 end, Vector2 position)
    {
        //�J�v�Z���̎n�_����I�_�̃x�N�g���𐳋K����������
        Vector2 vector1 = (end - origin).normalized;
        //�n�_����_�ւ̃x�N�g��
        Vector2 originToPoint = position - origin;
        //�I�_����_�ւ̃x�N�g��
        Vector2 endToPoint = position - end;

        if (0 > Vector2.Dot(vector1, originToPoint))
        {

            return origin;
        }
        else if (0 < Vector2.Dot(vector1, endToPoint))
        {
            return end;
        }

        Vector2 point = origin + vector1 * Vector2.Dot(vector1, originToPoint);

        return point;
    }
    private static Vector2 GetNearestPoint2D(LineData lineData1,LineData lineData2)
    {
        Vector2 originNearPoint = GetNearestPoint2D(lineData1.originPoint, lineData1.endPoint, lineData2.originPoint);
        float originDistance = GetDistance2D(originNearPoint, lineData2.originPoint);
        Vector2 endNearPoint = GetNearestPoint2D(lineData1.originPoint, lineData1.endPoint, lineData2.endPoint);
        float endDistance = GetDistance2D(endNearPoint, lineData2.endPoint);
        if (originDistance <= endDistance)
        {
            return originNearPoint;
        }
        return endNearPoint;
    }
   

    private static float GetDistance2D(Vector2 point1, Vector2 point2)
    {
        
        Vector2 distanceVec = point1 - point2;
        float distance = Mathf.Sqrt(
            Mathf.Pow(Mathf.Abs(distanceVec.x), 2) +
            Mathf.Pow(Mathf.Abs(distanceVec.y), 2)
            );
        return distance;
    }
    private static float GetLinesDistance2D(LineData lineData1,LineData lineData2)
    {
        Vector2 originNearPoint = GetNearestPoint2D(lineData1.originPoint,lineData1.endPoint, lineData2.originPoint);
        float originDistance = GetDistance2D(originNearPoint, lineData2.originPoint);
        Vector2 endNearPoint = GetNearestPoint2D(lineData1.originPoint, lineData1.endPoint, lineData2.endPoint);
        float endDistance = GetDistance2D(endNearPoint, lineData2.endPoint);
        if (originDistance <= endDistance)
        {
            return originDistance;
        }
        return endDistance;
    }
    #endregion
    #region capsule
    public static bool CheckCollision2D(this CapsuleData capsuleData,IBaseCollisionData collisionData)
    {
        switch (collisionData)
        {
            case CircleData circle:
                {
                    return capsuleData.CheckCollision2D(circle) ;
                }
            case CapsuleData capsule:
                {
                    return capsuleData.CheckCollision2D(capsule);
                }
            case LineData line:
                {
                    return capsuleData.CheckCollision2D(line);
                }
        
        }

        return false;
    }
    /// <summary>
    /// point
    /// </summary>
    /// <param name="capsuleData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CapsuleData capsuleData, Vector2 position)
    {
        Vector2 point = GetNearestPoint2D(capsuleData.originPoint, capsuleData.endPoint, position);
        //Debug.Log(point);
        float distance = GetDistance2D(position, point);
        if (distance - capsuleData.radius <= 0)
        {

            return true;
        }
        return false;
    }
    /// <summary>
    /// circle
    /// </summary>
    /// <param name="capsuleData"></param>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CapsuleData capsuleData,CircleData circleData)
    {
        Vector2 point = GetNearestPoint2D(capsuleData.originPoint, capsuleData.endPoint,circleData.position);
        //Debug.Log(point);
        float distance = GetDistance2D(circleData.position, point);
        if (distance - (capsuleData.radius+circleData.radius) <= 0)
        {

            return true;
        }
        return false;
    }
    /// <summary>
    /// line
    /// </summary>
    /// <param name="capsuleData"></param>
    /// <param name="circleData"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CapsuleData capsuleData, LineData lineData)
    {
        
       float distance = GetLinesDistance2D(capsuleData.ToLine(), lineData);
       
        if (distance - (capsuleData.radius + capsuleData.radius) <= 0)
        {

            return true;
        }
        return false;
    }/// <summary>
    /// capsule
    /// </summary>
    /// <param name="capsuleData"></param>
    /// <param name="circleData"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CapsuleData capsuleData1, CapsuleData capsuleData2)
    {
        
       float distance = GetLinesDistance2D(capsuleData1.ToLine(), capsuleData2.ToLine());
       
        if (distance - (capsuleData1.radius + capsuleData2.radius) <= 0)
        {

            return true;
        }
        return false;
    }
    public static bool CheckCollision2D(this CapsuleData capsule, BoxData box)
    {
        Vector2 nearestPoint = GetNearestPoint2D(box.ToLine(), capsule.ToLine());
        if (CheckLineOut(box, nearestPoint, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, nearestPoint) > box.boxWidth + capsule.radius)
        {
            return false;
        }
        return true;
    }
    #endregion
    #region circle
    public static bool CheckCollision2D(this CircleData circleData, IBaseCollisionData collisionData)
    {
        switch (collisionData)
        {
            case CircleData circle:
                {
                    return circleData.CheckCollision2D(circle);
                }
            case CapsuleData capsule:
                {
                    return circleData.CheckCollision2D(capsule);
                }
            case LineData line:
                {
                    return circleData.CheckCollision2D(line);
                }
            case BoxData box:
                {
                    return circleData.CheckCollision2D(box);
                }

        }

        return false;
    }

    /// <summary>
    /// circle
    /// </summary>
    /// <param name="circleData"></param>
    /// <param name="circleData1"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CircleData circleData, CircleData circleData1)
    {

       
        float distance = GetDistance2D(circleData.position,circleData1.position);
        
        if (distance - (circleData.radius + circleData1.radius) <= 0)
        {
           
            return true;
        }
        return false;
    }
    /// <summary>
    /// point
    /// </summary>
    /// <param name="circleData"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CircleData circleData, Vector2 point)
    {

        //Debug.Log(point);
        float distance = GetDistance2D(circleData.position, point);
        if (distance - circleData.radius <= 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// line
    /// </summary>
    /// <param name="circleData"></param>
    /// <param name="lineData"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CircleData circleData,LineData lineData)
    {

        Vector2 point = GetNearestPoint2D(lineData.originPoint,lineData.endPoint, circleData.position);
        float distance = GetDistance2D(circleData.position, point);
        if (distance - circleData.radius <= 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// casule
    /// </summary>
    /// <param name="circleData"></param>
    /// <param name="capsuleData"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CircleData circleData, CapsuleData capsuleData)
    {

        Vector2 point = GetNearestPoint2D(capsuleData.originPoint, capsuleData.endPoint, circleData.position);
        float distance = GetDistance2D(circleData.position, point);
        if (distance - (circleData.radius+capsuleData.radius) <= 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// box
    /// </summary>
    /// <param name="circleData"></param>
    /// <param name="box"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CircleData circle, BoxData box)
    {
        if (!CheckLineOut(box, circle.position, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, circle.position) > box.boxWidth + circle.radius)
        {
            return false;
        }
        return true;
    }
    #endregion
    #region line
    public static bool CheckCollision2D(this LineData lineData, IBaseCollisionData collisionData)
    {
        switch (collisionData)
        {
            case CircleData circle:
                {
                    return lineData.CheckCollision2D(circle);
                }
            case CapsuleData capsule:
                {
                    return lineData.CheckCollision2D(capsule);
                }
            case LineData line:
                {
                    return lineData.CheckCollision2D(line);
                }
            case BoxData box:
                {
                    return lineData.CheckCollision2D(box);
                }

        }

        return false;
    }
    /// <summary>
    /// point
    /// </summary>
    /// <param name="lineData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this LineData lineData, Vector2 position)
    {
        Vector2 point = GetNearestPoint2D(lineData.originPoint, lineData.endPoint, position);
        Debug.Log(point);
        float distance = GetDistance2D(position, point);
        if (distance <= 0)
        {

            return true;
        }
        return false;
    }
    /// <summary>
    /// circle
    /// </summary>
    /// <param name="lineData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this LineData lineData, CircleData circleData)
    {
        Vector2 point = GetNearestPoint2D(lineData.originPoint, lineData.endPoint, circleData.position);
        Debug.Log(point);
        float distance = GetDistance2D(circleData.position, point);
        if (distance-circleData.radius <= 0)
        {

            return true;
        }
        return false;
    }
    /// <summary>
    /// capsule
    /// </summary>
    /// <param name="lineData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this LineData lineData, CapsuleData capsuleData)
    {
        
        float distance = GetLinesDistance2D(lineData,capsuleData.ToLine());
        if (distance-capsuleData.radius <= 0)
        {

            return true;
        }
        return false;
    }/// <summary>
    /// line
    /// </summary>
    /// <param name="lineData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this LineData lineData1,LineData lineData2)
    {
        
        float distance = GetLinesDistance2D(lineData1,lineData2);
        if (distance <= 0)
        {

            return true;
        }
        return false;
    }
    /// <summary>
    /// box
    /// </summary>
    /// <param name="lineData1"></param>
    /// <param name="lineData2"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this LineData lineData,BoxData box)
    {

        Vector2 nearestPoint = GetNearestPoint2D(box.ToLine(), lineData);
        if (CheckLineOut(box, nearestPoint, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, nearestPoint) > box.boxWidth)
        {
            return false;
        }
        return true;
    }
    #endregion
    #region box
    public static bool CheckCollision2D(this BoxData boxData, IBaseCollisionData collisionData)
    {
        switch (collisionData)
        {
            case CircleData circle:
                {
                    return boxData.CheckCollision2D(circle);
                }
            case CapsuleData capsule:
                {
                    return boxData.CheckCollision2D(capsule);
                }
            case LineData line:
                {
                    return boxData.CheckCollision2D(line);
                }
            case BoxData box:
                {
                    return boxData.CheckCollision2D(box);
                }

        }

        return false;
    }
    /// <summary>
    /// point
    /// </summary>
    /// <param name="box"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this BoxData box,Vector2 position)
    {
       
       if(!CheckLineOut(box,position,out Vector2 point))
        {
            return false;
        }
        if(GetDistance2D(point,position)>box.boxWidth)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// line
    /// </summary>
    /// <param name="box"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this BoxData box,LineData line)
    {
        Vector2 nearestPoint = GetNearestPoint2D(box.ToLine(), line);
       if(CheckLineOut(box,nearestPoint,out Vector2 point))
        {
            return false;
        }
        if(GetDistance2D(point,nearestPoint)>box.boxWidth)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// circle
    /// </summary>
    /// <param name="box"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this BoxData box,CircleData circle)
    {
        
        if (!CheckLineOut(box, circle.position, out Vector2 point))
        {
            
            return false;
        }
        if (GetDistance2D(point, circle.position) > box.boxWidth+circle.radius)
        {
            
            return false;
        }
        return true;
    }
    public static bool CheckCollision2D(this BoxData box, CapsuleData capsule)
    {
        Vector2 nearestPoint = GetNearestPoint2D(box.ToLine(), capsule.ToLine());
        if (CheckLineOut(box, nearestPoint, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, nearestPoint) > box.boxWidth+capsule.radius)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// box
    /// </summary>
    /// <param name="box"></param>
    /// <param name="capsule"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this BoxData box1, BoxData box2)
    {
        Vector2 nearestPoint = GetNearestPoint2D(box1.ToLine(), box2.ToLine());
        if (CheckLineOut(box1, nearestPoint, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, nearestPoint) > box1.boxWidth+box2.boxWidth)
        {
            return false;
        }
        return true;
    }
    

    /// <summary>
    /// �_�����C���̊O������
    /// </summary>
    /// <param name="box"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private static bool CheckLineOut(this BoxData box, Vector2 position)
    {
        //�J�v�Z���̎n�_����I�_�̃x�N�g���𐳋K����������
        Vector2 vector1 = (box.endPoint - box.originPoint).normalized;
        //�n�_����_�ւ̃x�N�g��
        Vector2 originToPoint = position - box.originPoint;
        //�I�_����_�ւ̃x�N�g��
        Vector2 endToPoint = position - box.endPoint;

        if (0 > Vector2.Dot(vector1, originToPoint) || 0 < Vector2.Dot(vector1, endToPoint))
        {
            return false;

        }
        return true;
    }
    /// �_�����C���̊O������
    /// </summary>
    /// <param name="box"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private static bool CheckLineOut(this BoxData box, Vector2 position,out Vector2 nearestPoint)
    {
        //�J�v�Z���̎n�_����I�_�̃x�N�g���𐳋K����������
        Vector2 vector1 = (box.endPoint - box.originPoint).normalized;
        //�n�_����_�ւ̃x�N�g��
        Vector2 originToPoint = position - box.originPoint;
        //�I�_����_�ւ̃x�N�g��
        Vector2 endToPoint = position - box.endPoint;

        if (0 > Vector2.Dot(vector1, originToPoint) || 0 < Vector2.Dot(vector1, endToPoint))
        {
            nearestPoint = default;
            return false;
            
        }
        nearestPoint= box.originPoint + vector1 * Vector2.Dot(vector1, originToPoint); 
        return true;
    }
    #endregion

    //vector3��xz��vector2�ɕϊ�����
    public static Vector2 ToVector2XZ(this Vector3 vector3)
    {

        return new Vector2(vector3.x,vector3.z);

    }
    

}
