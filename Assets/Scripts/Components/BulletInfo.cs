using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct BulletInfo : IComponentData
{
    [HideInInspector]
    public Vector3 Origin;
}