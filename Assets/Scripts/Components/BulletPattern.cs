using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public abstract class BulletPattern<T> : ComponentSystem
    where T : BulletInfo
{
    public struct Components
    {
        public T Component;
        public Transform transform;
    }
}

public abstract class BulletInfo : MonoBehaviour
{
    [HideInInspector]
    public Vector3 Origin;

    public abstract void Reset();
}