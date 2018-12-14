using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;


public class WaverSystem
{
    /*protected void OnUpdate()
    {
        foreach (var entity in GetEntities<Components>())
        {
            var t = entity.transform;
            var c = entity.Component;

            if (c.Period <= 0 || c.Amplitude == 0)
            {
                continue;
            }

            c.Time += Time.deltaTime / c.Period;
            var offset = Mathf.Sin(Mathf.PI * 2 * c.Time) * c.Amplitude;
            t.position +=
                t.TransformDirection(
                    c.Direction
                ) * (
                    offset - c.Offset
                );
            c.Offset = offset;
        }
    }*/
}

public struct Waver : IComponentData, IResetable
{
    public float Period, Amplitude;
    public Vector3 Direction;
    [HideInInspector]
    public float Time, Offset;

    public void Disable() { }

    public void Enable()
    {
        Time = 0;
        Offset = 0;
    }
}
