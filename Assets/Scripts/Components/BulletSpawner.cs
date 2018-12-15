using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletSpawnerSystem : ComponentSystem
{
    private class BulletSpawnerBarrier : BarrierSystem { };
    [Inject] BulletSpawnerBarrier barrier = null;
    public static Mesh Mesh;
    public static Material Material;
    
    private struct BulletSpawnerComponents
    {
        public unsafe BulletSpawner* BulletSpawner;
    }

    protected unsafe override void OnUpdate()
    {
        var entities = GetEntities<BulletSpawnerComponents>();
        var DeltaTime = Time.deltaTime;
        var CommandBuffer = barrier.CreateCommandBuffer();
        foreach (var entity in entities)
        {
            var bs = entity.BulletSpawner;
            (*bs).Time += DeltaTime * (*bs).FireRate;
            if ((*bs).Time >= 1)
            {
                (*bs).Time -= 1;
                CommandBuffer.CreateEntity(
                    (*bs).Prefab.Archetype
                );
                CommandBuffer.SetComponent(
                    (*bs).Prefab.RadialTranslate
                );
                CommandBuffer.SetComponent(
                    (*bs).Prefab.Circler
                );
                CommandBuffer.SetSharedComponent(
                    new MeshInstanceRenderer
                    {
                        mesh = Mesh,
                        material = Material,
                    }
                );
            }
        }
    }
}

public struct BulletSpawner : IComponentData
{
    public float FireRate;
    public EntityPrefab Prefab;
    public int MeshId;
    [HideInInspector]
    public float Time;
}