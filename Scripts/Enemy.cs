using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject<Enemy>
{
    public Rigidbody2D rb;
    Player player;
    float hp;
    float speed;
    float atk;
    Pool<Enemy> pool;
    // Start is called before the first frame update

    public void Init(Vector2 pos, float hp, float speed, float atk)
    {
        rb.position = pos;
        this.hp = hp;
        this.speed = speed;
        this.atk = atk;
    }

    public void Inject(Player player) => this.player = player;
    public void InjectPool(Pool<Enemy> pool)
    {
        this.pool = pool;
    }

    public void FixedUpdate()
    {
        Vector2 dir = player.GetComponent<Rigidbody2D>().position - rb.position;
        rb.MovePosition(rb.position + dir.normalized * speed * Time.fixedDeltaTime);
    }

    public void GetHurt(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            NotificationCenter.Notify("EnemyDown");
            pool.Return(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            NotificationCenter.Notify("EnemyDown");
            other.GetComponent<Player>().GetHurt(atk);
            pool.Return(this);
        }
    }

}
