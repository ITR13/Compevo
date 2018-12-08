using System;
using System.Collections;
using System.Collections.Generic;
using GameObject = UnityEngine.GameObject;

public class Cache<T> where T : IInitializable, IResetable, IDestroyable, new()
{
    private GameObject _prefab;
    private Stack<T> _objects = new Stack<T>();

    private Cache(GameObject prefab)
    {
        _prefab = prefab;
    }

    private static Dictionary<GameObject, Cache<T>> _prefabs =
        new Dictionary<GameObject, Cache<T>>();
    public static Cache<T> GetCache(GameObject prefab)
    {
        if (!_prefabs.ContainsKey(prefab))
        {
            _prefabs.Add(prefab, new Cache<T>(prefab));
        }
        return _prefabs[prefab];
    }

    public CachedObject Get()
    {
        return new CachedObject(
            this,
            GetObject()
        );
    }

    private T GetObject()
    {
        T obj;
        if (_objects.Count != 0)
        {
            obj = _objects.Pop();
            obj.Enable();
            return obj;
        }
        var prefab = UnityEngine.Object.Instantiate(_prefab);
        obj = new T();
        obj.Init(prefab);
        obj.Enable();
        return obj;
    }

    private void Set(T obj)
    {
        obj.Disable();
        _objects.Push(obj);
    }

    public void Clear()
    {
        foreach(var obj in _objects)
        {
            obj.Destroy();
        }
        _objects.Clear();
    }

    public struct CachedObject
    {
        public T obj { get; private set; }
        private Cache<T> _cache;

        public CachedObject(Cache<T> cache, T obj)
        {
            _cache = cache;
            this.obj = obj;
        }

        public void Put()
        {
            _cache.Set(obj);
            obj = default(T);
        }
    }
}

public interface IInitializable
{
    void Init(GameObject gameObject);
}

public interface IResetable
{
    void Enable();
    void Disable();
}

public interface IDestroyable
{
    void Destroy();
}