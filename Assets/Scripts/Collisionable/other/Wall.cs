using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Wall : MonoBehaviour, ICollisionable
{
    public IBaseCollisionData BaseData { get=>_boxData; }
    private BoxData _boxData;
    public CheckCollisionMode CheckCollisionMode { get=>_collisionMode; }
    private CheckCollisionMode _collisionMode = CheckCollisionMode.collisionable;
    public LayerMask CollisionableLayer { get=>_layerMask; }
    [SerializeField]
    private LayerMask _layerMask = default;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = this.GetComponent<BoxCollider>();
        Vector2 origin = new Vector2(transform.position.x - transform.lossyScale.x / 2,transform.position.z);
        Vector2 end = new Vector2(transform.position.x + transform.lossyScale.x / 2, transform.position.z);
        _boxData = new(origin,end,transform.lossyScale.z);
    }
    private void Start()
    {
        
    }
    public void OnCollisionEvent(ICollisionable collisionable)
    {
        
    }
    
}
