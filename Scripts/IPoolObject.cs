using UnityEngine;
public interface IPoolObject<T> where T : MonoBehaviour,IPoolObject<T>{
    void InjectPool(Pool<T> pool);
}