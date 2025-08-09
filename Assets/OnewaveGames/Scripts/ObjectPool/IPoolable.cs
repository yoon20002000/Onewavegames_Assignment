public interface IPoolable
{
    // 풀에서 오브젝트가 생성될 때 호출.
    void OnCreate();
    
    // 풀에서 오브젝트를 가져올 때 호출.
    void OnGet();
    
    // 풀에 오브젝트를 반환할 때 호출.
    void OnRelease();
    
    // 풀 오브젝트가 파괴될 때 호출.
    void OnDestroy();
}