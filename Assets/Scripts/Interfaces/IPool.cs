using UnityEngine;

public interface IPool<T> where T : MonoBehaviour
{
    public void Init();
    public T Create();
    public T Get();
    public void Release(T element);
}

public interface IPoolElement<T> where T : MonoBehaviour
{
    public bool IsFree { get; }
    public T Element { get; }
    public void Activate();
    public void Release();

    public void InitInstance();
}
