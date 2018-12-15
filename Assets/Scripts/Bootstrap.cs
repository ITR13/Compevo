using System;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField]
    private float _fireRate = 1;
    [SerializeField]
    EntityPrefab _entityPrefab;
    [SerializeField]
    private Mesh _mesh;
    [SerializeField]
    private Material _material;

    public void Start()
    {
        BulletSpawnerSystem.Mesh = _mesh;
        BulletSpawnerSystem.Material = _material;

        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        var entity = entityManager.CreateEntity(
            ComponentType.Create<BulletSpawner>(),
            ComponentType.Create<Position>(),
            ComponentType.Create<Rotation>()
        );
        var bullet =
            entityManager.CreateArchetype(
                ComponentType.Create<Position>(),
                ComponentType.Create<Rotation>(),
                ComponentType.Create<RadialTranslate>(),
                ComponentType.Create<Circler>(),
                ComponentType.Create<MeshInstanceRenderer>(),
                ComponentType.Create<LocalToWorld>()
            );
        _entityPrefab.Archetype = bullet;
        entityManager.SetComponentData(
            entity,
            new BulletSpawner
            {
                Prefab = _entityPrefab,
                FireRate = _fireRate,
            }
        );
        entityManager.SetComponentData(
            entity,
            new Position
            {
                Value = Vector3.zero
            }
        );
        entityManager.SetComponentData(
            entity,
            new Rotation
            {
                Value = Quaternion.identity
            }
        );
    }
}

[Serializable]
public struct EntityPrefab
{
    public EntityArchetype Archetype;
    public RadialTranslate RadialTranslate;
    public Circler Circler;
}