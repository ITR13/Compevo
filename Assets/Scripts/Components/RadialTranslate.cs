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
                entity.BulletInfo.Origin,
                Vector3.forward,
                360 * entity.Component.RadialSpeed * Time.deltaTime
            );
            t.Translate(
                entity.Component.RadiusSpeed * Vector3.right * Time.deltaTime,
                Space.Self
            );
        }
    }
}

public class RadialTranslate : MonoBehaviour, IResetable
{
    public float RadialSpeed, RadiusSpeed;

    public void Disable() { }

    public void Enable() { }
}
