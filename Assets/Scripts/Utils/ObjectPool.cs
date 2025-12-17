using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : IPoolObject
{
    private Queue<T> _pool;
    private GameObject _prefab;
    private Transform _parent;
    public ObjectPool(GameObject prefab, Transform parent)
    {
        _pool = new Queue<T>();
        _prefab = prefab;
        _parent = parent;
    }

    public void InitPool(int capacity, string objectName = default)
    {
        for (int i = 0; i < capacity; i++)
        {
            GameObject go = Object.Instantiate(_prefab);
            if (!string.IsNullOrEmpty(objectName))
                go.name = objectName;
            go.SetActive(false);
            T pool = go.GetComponent<T>();
            pool.Initialize(this);
            _pool.Enqueue(pool);
            go.transform.SetParent(_parent);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
        }
    }

    public T GetObject()
    {
        T ob = default;
        if (_pool.Count > 0)
        {
            ob = _pool.Dequeue();
            ob.GameObject.SetActive(true);
            ob.OnSpawn();
            return ob;
        }
        ob = CreateObject();
        ob.OnSpawn();
        return ob;
    }


    public T GetObject(Vector3 position, Quaternion rotation)
    {
        T ob = default;
        if (_pool.Count > 0)
        {
            ob = _pool.Dequeue();
            ob.GameObject.SetActive(true);
            ob.GameObject.transform.position = position;
            ob.GameObject.transform.rotation = rotation;
            ob.OnSpawn();
            return ob;
        }
        ob = CreateObject();
        ob.OnSpawn();
        return ob;
    }

    private T CreateObject()
    {
        GameObject go = Object.Instantiate(_prefab);
        go.SetActive(true);
        T obj = go.GetComponent<T>();
        obj.Initialize(this);
        return obj;
    }

    public void RecycleObject(T ob)
    {
        ob.GameObject.SetActive(false);
        ob.OnDespawn(_parent);
        ob.GameObject.transform.localPosition = Vector3.zero;
        ob.GameObject.transform.localRotation = Quaternion.identity;
        _pool.Enqueue(ob);
    }
}
