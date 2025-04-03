using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMove:MonoBehaviour
{
    public virtual void CharacterMove(float moveSpped, Vector2 vector)
    {
        Vector2 nomalizedVector = vector.normalized;
        Vector3 moveVector = new Vector3(nomalizedVector.x, 0, nomalizedVector.y);
        this.transform.position += moveVector * Time.deltaTime * moveSpped;

    }
}
