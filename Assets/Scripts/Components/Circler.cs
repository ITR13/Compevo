using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclerSystem : BulletPattern<Circler>
{
    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Components>())
        {
            var t = entity.transform;
            var c = entity.Component;

            if (c.Period <= 0 || c.Amplitude == 0)
            {
                continue;
            }

            c.Time += 2 * Mathf.PI * Time.deltaTime / c.Period;

            var offset = new Vector3(
                Mathf.Cos(c.Time),
                Mathf.Sin(c.Time),
                0
            ) * c.Amplitude;

            t.Translate(
                offset - c.Offset,
                Space.Self
            );

            c.Offset = offset;
        }
    }
}

public class Circler : BulletInfo
{
    public float Period, Amplitude;
    [HideInInspector]
    public float Time;
    [HideInInspector]
    public Vector3 Offset;

    public override void Reset()
    {
        Time = 0;
        Offset = Vector3.right * Amplitude;
    }
}