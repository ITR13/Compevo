using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BulletSpawnerSystem : ComponentSystem
{
    private struct Entity
    {
        public BulletSpawner BulletSpawner;
        public Transform Transform;
    }

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Entity>())
        {
            var bs = entity.BulletSpawner;
            bs.Time += Time.deltaTime;
            if (bs.Time >= bs.FireRate)
            {
                bs.Time -= bs.FireRate;
                var bullet = bs.Cache.Get();
                bullet.Obj.SetValues(
                    entity.Transform.position,
                    entity.Transform.rotation
                );
                bs.Bullets.Enqueue(bullet);
                if (bs.Bullets.Count > 200)
                {
                    bs.Bullets.Dequeue().Put();
                }


                bullet.Obj.Enable();
            }
        }
    }
}

public class BulletSpawner : MonoBehaviour
{
    public GameObject Prefab;
    public float FireRate;
    [HideInInspector]
    public float Time;
    [HideInInspector]
    public Cache<Bullet> Cache;
    [HideInInspector]
    public Queue<Cache<Bullet>.CachedObject> Bullets;

    private void Awake()
    {
        Bullets = new Queue<Cache<Bullet>.CachedObject>();
        Cache = Cache<Bullet>.GetCache(Prefab);
    }

    public void Init(GameObject prefab)
    {
        Prefab = prefab;
        foreach (var bullet in Bullets)
        {
            bullet.Put();
        }
        Bullets.Clear();

        if (prefab)
        {
            Cache = Cache<Bullet>.GetCache(Prefab);
        }
        else
        {
            Cache = null;
        }
    }
}

public struct Bullet : IInitializable, IResetable, IDestroyable
{
    private GameObject GameObject;
    private BulletInfo BulletInfo;
    private IResetable[] Resetables;

    public void SetValues(Vector3 origin, Quaternion rotation)
    {
        BulletInfo.Origin = origin;
        GameObject.transform.position = origin;
        GameObject.transform.rotation = rotation;
    }

    public void Disable()
    {
        for (int i = 0; i < Resetables.Length; i++)
        {
            Resetables[i].Disable();
        }
        GameObject.SetActive(false);
    }

    public void Enable()
    {
        GameObject.SetActive(true);
        for (int i = 0; i < Resetables.Length; i++)
        {
            Resetables[i].Enable();
        }
    }

    public void Init(GameObject gameObject)
    {
        GameObject = gameObject;
        BulletInfo = gameObject.GetComponent<BulletInfo>();
        Resetables = gameObject.GetComponents<IResetable>();
    }

    public void Destroy()
    {
        Object.Destroy(GameObject);
        BulletInfo = null;
    }
}