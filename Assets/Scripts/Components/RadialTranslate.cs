using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RadialTranslateSystem : JobComponentSystem
{


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RadialTranslateJob
        {
            DeltaTime = Time.deltaTime
        };
        return job.Schedule(this, inputDeps);
    }

    private struct RadialTranslateJob : IJobProcessComponentData<
        RadialTranslate,
        Position
    >
    {
        [ReadOnly]
        public float DeltaTime;

        public void Execute(
            ref RadialTranslate radialTranslate,
            ref Position position
        )
        {
            radialTranslate.RadialTime +=
                DeltaTime * radialTranslate.RadialSpeed * Mathf.PI * 2;
            radialTranslate.RadiusTime +=
                DeltaTime * radialTranslate.RadiusSpeed;

            var pos =
                new float3(
                    math.cos(radialTranslate.RadialTime),
                    math.sin(radialTranslate.RadialTime),
                    0
                ) * radialTranslate.RadiusTime;

            position.Value += pos - radialTranslate.Position;
            radialTranslate.Position = pos;
        }
    }
}

[Serializable]
public struct RadialTranslate : IComponentData
{
    public float RadialSpeed, RadiusSpeed;
    public float StartingRotation;
    [HideInInspector]
    public float RadialTime, RadiusTime;
    [HideInInspector]
    public float3 Position;
}
