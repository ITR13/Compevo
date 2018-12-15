using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class WaverSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new WaverJob
        {
            DeltaTime = Time.deltaTime
        };
        return job.Schedule(this, inputDeps);
    }

    private struct WaverJob : IJobProcessComponentData<
        Waver,
        Position
    >
    {
        [ReadOnly]
        public float DeltaTime;

        public void Execute(ref Waver waver, ref Position position)
        {
            waver.Time += DeltaTime * waver.Frequency;

            var distance = math.sin(
                Mathf.PI * 2 * waver.Time
            ) * waver.Amplitude;

            var pos =
                new float3(
                    math.cos(waver.Rotation),
                    math.sin(waver.Rotation),
                    0
                ) * distance;

            position.Value += pos - waver.Position;
            waver.Position = pos;
        }
    }
}

[Serializable]
public struct Waver : IComponentData
{
    public float Frequency, Amplitude;
    [HideInInspector]
    public float Time, Rotation;
    [HideInInspector]
    public float3 Position;
}
