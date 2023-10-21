using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject<Bullet>
{
    public float atk;
    public Vector2 dir;
    public float speed;

    Pool<Bullet> pool;

    // void Start(){
    //     rb.velocity = dir * speed;
    // }
    public void Init(Vector2 pos, Vector2 dir, float atk, float speed)
    {
        transform.position = pos;
        this.dir = dir;
        this.atk = atk;
        this.speed = speed;
        StartCoroutine(Move());
    }

    public void InjectPool(Pool<Bullet> pool)
    {
        this.pool = pool;
    }

    IEnumerator Move()
    {
        while (true)
        {
            transform.position = transform.position + new Vector3(dir.x, dir.y, 0) * speed * Time.deltaTime;
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy") other.GetComponent<Enemy>().GetHurt(atk);
        pool.Return(this);
    }
}
