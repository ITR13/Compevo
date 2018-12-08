using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public abstract class BulletPattern<T> : ComponentSystem
    where T : MonoBehaviour, IResetable
{
    public struct Components
    {
        public BulletInfo BulletInfo;
        public T Component;
        public Transform transform;
    }
}
