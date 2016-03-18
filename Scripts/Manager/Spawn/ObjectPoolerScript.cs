using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolerScript : MonoBehaviour {

    public static ObjectPoolerScript m_Pooler;
    public int m_PooledAmount = 30;
    public bool willGrow = true;
    public GameObject m_Object;

    public List<GameObject> m_PooledObjects;

    void Awake()
    {
        m_Pooler = this;
    }

    void Start()
    {
        m_PooledObjects = new List<GameObject>();

        for(int i = 0; i < m_PooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(m_Object);
            obj.SetActive(false);
            m_PooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < m_PooledObjects.Count; i++)
        {
            if(!m_PooledObjects[i].activeInHierarchy)
            {
                return m_PooledObjects[i];
            }
        }

        if(willGrow)
        {
            GameObject obj = (GameObject)Instantiate(m_Object);
            m_PooledObjects.Add(obj);
            return obj;
        }

        return null;

    }
}
