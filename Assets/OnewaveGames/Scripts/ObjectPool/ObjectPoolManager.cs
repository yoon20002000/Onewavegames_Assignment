using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    #region Singleton region

    private static ObjectPoolManager instance;

    public static ObjectPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<ObjectPoolManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("ObjectPoolManager");
                    instance = go.AddComponent<ObjectPoolManager>();
                    DontDestroyOnLoad(go);
                }
            }

            return instance;
        }
    }

    #endregion

    #region Pool Configuration region

    [System.Serializable]
    public class PoolConfig
    {
        [Header("풀 설정")] 
        [SerializeField] 
        private string poolName;
        public string PoolName => poolName;
        
        [SerializeField] 
        private GameObject prefab;
        public GameObject Prefab => prefab;
        
        [SerializeField] 
        private int defaultCapacity = 10;
        public int DefaultCapacity => defaultCapacity;
        
        [SerializeField] 
        private int maxSize = 100;
        public int MaxSize => maxSize;
        
        [SerializeField] 
        private bool collectionCheck = true;
        public bool CollectionCheck => collectionCheck;
    }

    [Header("풀 설정")] 
    [SerializeField] 
    private List<PoolConfig> poolConfigs = new List<PoolConfig>();

    [Header("디버그 설정")] 
    [SerializeField] 
    private bool bEnableDebugLog = true;

    #endregion

    #region Main Container region

    private readonly Dictionary<string, IObjectPool<GameObject>> pools =
        new Dictionary<string, IObjectPool<GameObject>>();

    private readonly Dictionary<string, GameObject> poolParents = new Dictionary<string, GameObject>();

    #endregion

    #region Unity Method region

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            initializePools();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            ClearAllPools();
        }
    }

    #endregion

    #region Pool Initialization region

    // 설정된 모든 풀을 초기화함
    private void initializePools()
    {
        foreach (var config in poolConfigs)
        {
            if (config.Prefab != null && !string.IsNullOrEmpty(config.PoolName))
            {
                createPool(config);
            }
            else
            {
                logWarning($"Invalid pool config: {config.PoolName}");
            }
        }
    }

    // 새로운 풀을 생성함
    private void createPool(PoolConfig config)
    {
        if (pools.ContainsKey(config.PoolName))
        {
            logWarning($"Pool {config.PoolName} already exists!");
            return;
        }

        // 풀 부모 오브젝트 생성
        GameObject poolParent = new GameObject($"Pool_{config.PoolName}");
        poolParent.transform.SetParent(transform);
        poolParents[config.PoolName] = poolParent;

        // ObjectPool 생성
        var pool = new ObjectPool<GameObject>(
            createFunc: () => createPooledItem(config.Prefab, poolParent),
            actionOnGet: onTakeFromPool,
            actionOnRelease: onReturnToPool,
            actionOnDestroy: onDestroyPoolObject,
            collectionCheck: config.CollectionCheck,
            defaultCapacity: config.DefaultCapacity,
            maxSize: config.MaxSize
        );

        pools[config.PoolName] = pool;
        logInfo($"Pool {config.PoolName} created with capacity: {config.DefaultCapacity}, maxSize: {config.MaxSize}");
    }

    #endregion

    #region Pool Operations region

    // 풀에서 오브젝트를 가져옴
    public GameObject Get(string poolName, Vector3 position = default, Quaternion rotation = default)
    {
        if (!pools.TryGetValue(poolName, out var pool))
        {
            logError($"Pool {poolName} not found!");
            return null;
        }

        GameObject obj = pool.Get();
        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            logDebug($"Get object from pool {poolName}: {obj.name}");
        }

        return obj;
    }

    // 오브젝트를 풀에 반환함
    public void Release(GameObject obj, string poolName = null)
    {
        if (obj == null) return;

        // poolName이 지정되지 않은 경우 자동으로 찾기
        if (string.IsNullOrEmpty(poolName))
        {
            poolName = findPoolNameByObject(obj);
        }

        if (string.IsNullOrEmpty(poolName))
        {
            logWarning($"Cannot find pool for object: {obj.name}");
            Destroy(obj);
            return;
        }

        if (pools.TryGetValue(poolName, out var pool))
        {
            pool.Release(obj);
            logDebug($"Release object to pool {poolName}: {obj.name}");
        }
        else
        {
            logError($"Pool {poolName} not found for object: {obj.name}");
            Destroy(obj);
        }
    }

    // 오브젝트가 속한 풀 이름을 찾음
    private string findPoolNameByObject(GameObject obj)
    {
        foreach (var kvp in pools)
        {
            if (obj.transform.IsChildOf(poolParents[kvp.Key].transform))
            {
                return kvp.Key;
            }
        }

        return null;
    }

    // 풀의 활성 오브젝트 수를 반환함
    public int GetActiveCount(string poolName)
    {
        if (poolParents.TryGetValue(poolName, out var poolParent))
        {
            int activeCount = 0;
            foreach (Transform child in poolParent.transform)
            {
                if (child.gameObject.activeInHierarchy)
                {
                    activeCount++;
                }
            }

            return activeCount;
        }

        return 0;
    }

    // 풀의 비활성 오브젝트 수를 반환함
    public int GetInactiveCount(string poolName)
    {
        if (poolParents.TryGetValue(poolName, out var poolParent))
        {
            int inactiveCount = 0;
            foreach (Transform child in poolParent.transform)
            {
                if (!child.gameObject.activeInHierarchy)
                {
                    inactiveCount++;
                }
            }

            return inactiveCount;
        }

        return 0;
    }

    // 풀이 존재하는지 확인함
    public bool PoolExists(string poolName)
    {
        return pools.ContainsKey(poolName);
    }

    // 특정 풀을 정리함
    public void ClearPool(string poolName)
    {
        if (pools.TryGetValue(poolName, out var pool))
        {
            // 모든 활성 오브젝트를 비활성화
            var activeObjects = new List<GameObject>();
            foreach (Transform child in poolParents[poolName].transform)
            {
                if (child.gameObject.activeInHierarchy)
                {
                    activeObjects.Add(child.gameObject);
                }
            }

            foreach (var obj in activeObjects)
            {
                pool.Release(obj);
            }

            int activeCount = GetActiveCount(poolName);
            int inactiveCount = GetInactiveCount(poolName);
            logInfo($"Pool {poolName} cleared. Active: {activeCount}, Inactive: {inactiveCount}");
        }
    }

    // 모든 풀을 정리함
    public void ClearAllPools()
    {
        foreach (var poolName in pools.Keys)
        {
            ClearPool(poolName);
        }

        logInfo("All pools cleared");
    }

    #endregion

    #region Pool Callbacks region

    // 풀에서 오브젝트를 생성할 때 호출됨
    private GameObject createPooledItem(GameObject prefab, GameObject parent)
    {
        GameObject obj = Instantiate(prefab, parent.transform);
        obj.name = $"{prefab.name}_Pooled";

        // IPoolable 인터페이스가 구현되어 있다면 초기화
        var poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnCreate();
        }

        logDebug($"Created pooled item: {obj.name}");
        return obj;
    }

    // 풀에서 오브젝트를 가져올 때 호출됨
    private void onTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);

        // IPoolable 인터페이스가 구현되어 있다면 활성화 처리
        var poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnGet();
        }
    }

    // 풀에 오브젝트를 반환할 때 호출됨
    private void onReturnToPool(GameObject obj)
    {
        // IPoolable 인터페이스가 구현되어 있다면 비활성화 처리
        var poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnRelease();
        }

        obj.SetActive(false);
        obj.transform.SetParent(getPoolParent(obj));
    }

    // 풀 오브젝트를 파괴할 때 호출됨
    private void onDestroyPoolObject(GameObject obj)
    {
        // IPoolable 인터페이스가 구현되어 있다면 파괴 처리
        var poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnDestroy();
        }

        logDebug($"Destroyed pooled item: {obj.name}");
    }

    // 오브젝트가 속한 풀의 부모를 찾음
    private Transform getPoolParent(GameObject obj)
    {
        foreach (var kvp in poolParents)
        {
            if (obj.transform.IsChildOf(kvp.Value.transform))
            {
                return kvp.Value.transform;
            }
        }

        return transform;
    }

    #endregion

    #region Runtime Pool Creation region

    // 런타임에 새로운 풀을 생성함
    public void CreateRuntimePool(string poolName, GameObject prefab, int defaultCapacity = 10, int maxSize = 100)
    {
        if (pools.ContainsKey(poolName))
        {
            logWarning($"Pool {poolName} already exists!");
            return;
        }

        // 풀 부모 오브젝트 생성
        GameObject poolParent = new GameObject($"Pool_{poolName}");
        poolParent.transform.SetParent(transform);
        poolParents[poolName] = poolParent;

        // ObjectPool 생성
        var pool = new ObjectPool<GameObject>(
            createFunc: () => createPooledItem(prefab, poolParent),
            actionOnGet: onTakeFromPool,
            actionOnRelease: onReturnToPool,
            actionOnDestroy: onDestroyPoolObject,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );

        pools[poolName] = pool;
        logInfo($"Runtime pool {poolName} created with capacity: {defaultCapacity}, maxSize: {maxSize}");
    }

    #endregion

    #region Debug and Logging region

    private void logInfo(string message)
    {
        if (bEnableDebugLog)
        {
            Debug.Log($"[ObjectPoolManager] {message}");
        }
    }

    private void logWarning(string message)
    {
        if (bEnableDebugLog)
        {
            Debug.LogWarning($"[ObjectPoolManager] {message}");
        }
    }

    private void logError(string message)
    {
        if (bEnableDebugLog)
        {
            Debug.LogError($"[ObjectPoolManager] {message}");
        }
    }

    private void logDebug(string message)
    {
        if (bEnableDebugLog)
        {
            Debug.Log($"[ObjectPoolManager] {message}");
        }
    }

    // 모든 풀의 상태를 로그로 출력함
    [ContextMenu("Log Pool Status")]
    public void LogPoolStatus()
    {
        logInfo("=== Pool Status ===");
        foreach (var kvp in pools)
        {
            string poolName = kvp.Key;
            int activeCount = GetActiveCount(poolName);
            int inactiveCount = GetInactiveCount(poolName);
            logInfo($"Pool: {poolName} | Active: {activeCount} | Inactive: {inactiveCount}");
        }
    }

    #endregion
}