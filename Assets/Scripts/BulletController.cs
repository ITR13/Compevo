using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private BulletSpawner _spawner;
    [SerializeField]
    private RadialTranslate _spawnRotator;
    [SerializeField]
    private GameObject _prefab;

    private float _rotation;

    private Type[] _bulletInfos = new Type[]
    {
        typeof(RadialTranslate),
        typeof(Waver),
        typeof(Circler),
    };

    private GameObject _spawnedPrefab;
    private Comp[] _comps;

    private void Start()
    {
        _prefab.SetActive(false);
        _spawnedPrefab = Instantiate(_prefab);

        _spawner.Init(_spawnedPrefab);


        _comps = _bulletInfos.Select(
            t => new Comp(t)
        ).ToArray();
    }

    private bool _replace;
    private void Update()
    {
        if (_replace)
        {
            _replace = false;
            ReplacePrefab();
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width / 4, Screen.height));

        if (GUILayout.Button("Replace"))
        {
            _replace = true;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Rotation:", GUILayout.Width(60));
        _rotation = GUILayout.HorizontalSlider(_rotation, 0, 1);
        if (GUILayout.Button(
            _rotation.ToString("0.00"),
            GUILayout.Width(40)
        ))
        {
            _rotation = Mathf.Round(_rotation * 20) / 20;
        }
        else
        {
            _rotation = Mathf.Round(_rotation * 100) / 100;
        }
        GUILayout.EndHorizontal();
        foreach (var field in _comps)
        {
            GUILayout.BeginHorizontal();
            field.Enabled = GUILayout.Toggle(field.Enabled, "");
            GUILayout.Label(field.Title);
            GUILayout.EndHorizontal();
            for (int i = 0; i < field.Names.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(field.Names[i], GUILayout.Width(60));
                var t = field.FieldInfos[i].FieldType;
                if (t == typeof(float))
                {
                    var r = GUILayout.HorizontalSlider(
                        field.Values[i] != null ? (float)field.Values[i] : 0f,
                        -1, 5
                    );
                    if (GUILayout.Button(
                        r.ToString("0.00"),
                        GUILayout.Width(40)
                    ))
                    {
                        r = Mathf.Round(r * 20) / 20;
                    }
                    else
                    {
                        r = Mathf.Round(r * 100) / 100;
                    }
                    field.Values[i] = r;
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndArea();
    }

    private void ReplacePrefab()
    {
        var oldPrefab = _spawnedPrefab;
        _spawnedPrefab = Instantiate(_prefab);

        for (int i = 0; i < _comps.Length; i++)
        {
            var comp = _comps[i];
            if (comp.Enabled)
            {
                var info = _spawnedPrefab.AddComponent(_bulletInfos[i]);
                for (int j = 0; j < comp.FieldInfos.Count; j++)
                {
                    var fieldInfo = comp.FieldInfos[j];
                    fieldInfo.SetValue(info, comp.Values[j]);
                }
            }
        }
        _spawnRotator.RadialSpeed = _rotation;

        _spawner.Init(_spawnedPrefab);

        if (_spawnedPrefab)
        {
            var cache = Cache<Bullet>.GetCache(_spawnedPrefab);
            cache.Clear();
            Destroy(oldPrefab);
        }

    }

    private class Comp
    {
        public string Title;
        public bool Enabled = true;
        public List<string> Names;
        public List<object> Values;
        public List<FieldInfo> FieldInfos;

        public Comp(Type type)
        {
            Title = type.Name;

            var fields = type.GetFields();
            FieldInfos = fields.Where(f =>
                {
                    foreach (var a in f.CustomAttributes)
                    {
                        if (a.AttributeType == typeof(HideInInspector))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            ).ToList();

            Names = FieldInfos.Select(f => f.Name).ToList();
            Values = FieldInfos.Select(f =>
            {
                var t = f.GetType();
                if (t == typeof(float))
                {
                    return 0f;
                }
                else if (t == typeof(Vector3))
                {
                    return (object)Vector3.right;
                }
                return null;
            }
            ).ToList();
        }
    }
}
