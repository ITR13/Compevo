using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CirclerSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new CirclerJob
        {
            DeltaTime = Time.deltaTime
        };
        return job.Schedule(this, inputDeps);
    }

    private struct CirclerJob : IJobProcessComponentData<
        Circler,
        Position
    >
    {
        [ReadOnly]
        public float DeltaTime;

        public void Execute(
            ref Circler circler,
            ref Position position
        )
        {
            circler.Time += DeltaTime * circler.Frequency * Mathf.PI * 2;

            var pos = new float3(
                math.cos(circler.Time),
                math.sin(circler.Time),
                0
            ) * circler.Amplitude;

            position.Value += pos - circler.Position;
            circler.Position = pos;
        }
    }
}

[Serializable]
public struct Circler : IComponentData
{
    public float Frequency, Amplitude;
    [HideInInspector]
    public float Time;
    [HideInInspector]
    public float3 Position;

    public void Enable() { }
}