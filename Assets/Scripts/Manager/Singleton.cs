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
            // 인스턴스 없을 때
            if(instance == null)
            {
                // 인스턴스를 찾아본다. 있으면 넣어준다.
                instance = FindObjectOfType<T>(); 
                // 찾아봐도 없으면 새로 생성한다.
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

    // 초기화 메서드 Awake에서 실행하니까 여기서 초기화해도 된다.
    protected virtual void Initialize() { }

    protected virtual void Awake()
    {
        // 해당 매니저가 있고, 그 매니저가 내가 아니면(중복)
        if(instance != null && instance != this)
        {
            // 나를 파괴해서 중복 방지한다.
            Destroy(gameObject);
        }
        else
        {
            // 그냥 this는 MonoBehaviour이다. 따라서 as T로 Singleton으로 형 변환 한다.
            instance = this as T;
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
    }
}
