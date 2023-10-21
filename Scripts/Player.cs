using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameController controller;
    public float hp;
    public float atk;
    public float speed;
    public float guard;
    private float fireTime;
    public float fireDuration;
    private float rushSpeed = 1f;
    private float rushTime;
    private float rushDuration = 2f;
    private float shield = 0f;
    private float shieldState = 0f;
    private float shieldTime = 0f;
    private float shieldDuration = 30f;

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Tick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if(Input.GetKey(KeyCode.E)){
            if (Time.time - shieldTime < shieldDuration) return;
            shieldState = shield;
            shieldTime = Time.time;
        }
        shieldState -= Time.deltaTime;
    }

    public void FixedTick()
    {
        Vector2 vec = new Vector2();
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        dir = new Vector3(dir.x, dir.y, 0);
        //移动
        if (Input.GetKey(KeyCode.W))
        {
            vec += new Vector2(0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            vec += new Vector2(0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            vec += new Vector2(-1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            vec += new Vector2(1, 0);
        }
        vec.Normalize();
        if(Input.GetKey(KeyCode.Space)){
            if (Time.time - rushTime < rushDuration) return;
            vec += new Vector2(rushSpeed,rushSpeed);
            rushTime = Time.time;
        }
        if (vec != Vector2.zero)
        {
            rb.MovePosition(rb.position + vec * speed * Time.fixedDeltaTime);
        }
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Shoot()
    {
        if (Time.time - fireTime < fireDuration) return;
        Bullet bullet = controller.GenBullet();
        bullet.Init(transform.position, transform.up, this.atk, 30);
        fireTime = Time.time;
    }
    public void GetHurt(float damage)
    {
        if(shieldState > 0) return;
        Debug.Log("GetDamage:"+damage);
        hp -= damage-guard;
        if (hp < 0)
        {
            NotificationCenter.Notify("PlayerDead");
        }
    }

    public void AddHp(float v) => hp += v;
    public void AddAtk(float v) => atk += v;
    public void AddSpeed(float v) => speed += v;
    public void AddGuard(float v) => guard += v;
    public void AddFire(float v) => fireDuration += v;
    public void AddShield(float v) => shield += v;
    public void AddRush(float v) => rushSpeed += v;
}
