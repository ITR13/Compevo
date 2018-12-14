using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField]
    private float _fireRate = 1;
    [SerializeField]
    private RadialTranslate _radialTranslate;
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
            ComponentType.Create<MeshInstanceRenderer>(),
            ComponentType.Create<Position>(),
            ComponentType.Create<Rotation>(),
            ComponentType.Create<Scale>()
        );
        var bullet =
            entityManager.CreateArchetype(
                ComponentType.Create<Position>(),
                ComponentType.Create<Rotation>(),
                ComponentType.Create<RadialTranslate>(),
                ComponentType.Create<MeshInstanceRenderer>(),
                ComponentType.Create<LocalToWorld>()
            );
        entityManager.SetComponentData(
            entity,
            new BulletSpawner
            {
                Prefab = new EntityPrefab
                {
                    Archetype = bullet,
                    RadialTranslate = _radialTranslate,
                },
                FireRate = _fireRate,
            }
        );
        entityManager.SetSharedComponentData(
            entity,
            new MeshInstanceRenderer
            {
                mesh = _mesh,
                material = _material
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
        entityManager.SetComponentData(
            entity,
            new Scale
            {
                Value = Vector3.one
            }
        );
    }
}

public struct EntityPrefab
{
    public EntityArchetype Archetype;
    public RadialTranslate RadialTranslate;
}