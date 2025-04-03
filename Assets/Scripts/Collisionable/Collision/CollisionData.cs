using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// àÍäáÇ≈ä«óùÇ∑ÇÈÇΩÇﬂ
/// </summary>
public interface IBaseCollisionData
{
    public bool CheckCollision(IBaseCollisionData collisionData);
    
   
}

public class CapsuleData:IBaseCollisionData
{
    public Vector2 endPoint;
    public Vector2 originPoint;
    public float radius;
    private LineData lineData;
    public CapsuleData()
    {
        lineData = new LineData(originPoint,endPoint);
    }
    public CapsuleData(Vector2 origin, Vector2 end, float radius)
    {
        this.endPoint = end;
        this.originPoint = origin;
        this.radius = radius;
        lineData = new LineData(originPoint, endPoint);
    }
    public void SetData(Vector2 origin, Vector2 end, float radius)
    {
        this.endPoint = end;
        this.originPoint = origin;
        this.radius = radius;
        lineData = new LineData(originPoint, endPoint);
    }
    public LineData ToLine()
    {
        lineData.SetData(originPoint, endPoint);
        return lineData;
    }
    public bool CheckCollision(IBaseCollisionData collisionData)
    {
        return this.CheckCollision2D(collisionData);
    }
}
public class CircleData:IBaseCollisionData
{
    public Vector2 position;
    public float radius;
    public CircleData(Vector2 position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }
    public void SetData(Vector2 position,float radius)
    {
        this.position = position;
        this.radius = radius;
    }
    public bool CheckCollision(IBaseCollisionData collisionData)
    {
        return this.CheckCollision2D(collisionData);
    }
}
public class LineData:IBaseCollisionData
{
    public Vector2 endPoint;
    public Vector2 originPoint;
    public LineData(Vector2 origin, Vector2 end)
    {
        this.endPoint = end;
        this.originPoint = origin;
    }
    public void SetData(Vector2 origin, Vector2 end)
    {
        this.endPoint = end;
        this.originPoint = origin;
    }
    public bool CheckCollision(IBaseCollisionData collisionData)
    {
        return this.CheckCollision2D(collisionData);
    }
}
public class BoxData : IBaseCollisionData
{
    public Vector2 originPoint;
    public Vector2 endPoint;
    public float boxWidth;
    private LineData lineData;
    public BoxData(Vector2 originPoint,Vector2 endPoint,float boxWidth)
    {
        this.boxWidth = boxWidth;
        this.originPoint = originPoint;
        this.endPoint = endPoint;
        lineData = new LineData(originPoint, endPoint);
    }
    public void SetData(Vector2 originPoint,Vector2 endPoint,float boxWidth)
    {
        this.boxWidth = boxWidth;
        this.originPoint = originPoint;
        this.endPoint = endPoint;
    }
    public LineData ToLine()
    {
        lineData.SetData(originPoint, endPoint);
        return lineData;
    }
    public bool CheckCollision(IBaseCollisionData collisionData)
    {
        return this.CheckCollision2D(collisionData);
    }
}


