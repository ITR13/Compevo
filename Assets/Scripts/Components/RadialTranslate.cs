using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialTranslateSystem : BulletPattern<RadialTranslate>
{
    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Components>())
        {
            var t = entity.transform;
            t.RotateAround(
                entity.Component.Origin,
                Vector3.forward,
                360 * entity.Component.RadialSpeed * Time.deltaTime
            );
            var direction = (t.position - entity.Component.Origin).normalized;
            t.Translate(
                entity.Component.RadiusSpeed * Vector3.right * Time.deltaTime, 
                Space.Self
            );
        }
    }
}

public class RadialTranslate : BulletInfo
{
    public float RadialSpeed, RadiusSpeed;

    public override void Reset(){}
}
