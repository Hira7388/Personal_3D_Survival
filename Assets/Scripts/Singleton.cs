using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            // �ν��Ͻ� ���� ��
            if(instance == null)
            {
                // �ν��Ͻ��� ã�ƺ���. ������ �־��ش�.
                instance = FindObjectOfType<T>(); 
                // ã�ƺ��� ������ ���� �����Ѵ�.
                if(instance == null) 
                {
                    GameObject singletonObject = new(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                    (instance as Singleton<T>).Initialize();
                }
            }
            return instance;
        }
    }

    // �ʱ�ȭ �޼��� Awake���� �����ϴϱ� ���⼭ �ʱ�ȭ�ص� �ȴ�.
    protected virtual void Initialize() { }

    protected virtual void Awake()
    {
        // �ش� �Ŵ����� �ְ�, �� �Ŵ����� ���� �ƴϸ�(�ߺ�)
        if(instance != null && instance != this)
        {
            // ���� �ı��ؼ� �ߺ� �����Ѵ�.
            Destroy(gameObject);
        }
        else
        {
            // �׳� this�� MonoBehaviour�̴�. ���� as T�� Singleton���� �� ��ȯ �Ѵ�.
            instance = this as T;
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
    }
}
