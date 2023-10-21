using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Property
{
    生命,
    攻击力,
    防御力,
    速度,
    开火间隔,
    冲刺,
    无敌,
    
}
public struct PropertyValue{
    public Property prop;
    public float value;
}
public class GameController : MonoBehaviour
{
    private int gameState = 1;
    public Vector2 hpRange;
    public Vector2 atkRange;
    public Vector2 guardRange;
    public Vector2 speedRange;
    public Vector2 rushSpeed;
    public Vector2 shieldTime;
    public Vector2 fireDuration;


    public int EnemyNum;
    public Vector2Int EnemyAdd;
    public Vector2 EnemyHP;
    public Vector2 EnemyATK;
    public Vector2 EnemySPEED;
    public Vector2 EnemyHPAdd;
    public Vector2 EnemyATKAdd;
    public Vector2 EnemySPEEDAdd;

    public Player player;
    public Canvas UI;
    Pool<Bullet> bulletPool;
    Pool<Enemy> enemyPool;

    public GameObject poolGo;

    private int curEnemyNum;

    void Start()
    {
        NotificationCenter.Register("StartGame", () =>
        {
            gameState = 1;
            GameObject state = Resources.Load<GameObject>("Prefabs/UI_State");
            GameObject go = GameObject.Instantiate<GameObject>(state,UI.transform);
            go.GetComponent<UI_GameState>().player = this.player;
            GenEnemy();
        });
        NotificationCenter.Register("EnemyDown", EnemyDown);
        NotificationCenter.Register("EnterNextLevel", EnterNextLevel);

        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/bullet");
        bulletPool = new Pool<Bullet>(() =>
        {
            return GameObject.Instantiate(bulletPrefab, poolGo.transform).GetComponent<Bullet>();
        });

        GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/enemy");
        enemyPool = new Pool<Enemy>(() =>
        {
            return GameObject.Instantiate(enemyPrefab, poolGo.transform).GetComponent<Enemy>();
        });
        GameObject start = Resources.Load<GameObject>("Prefabs/UI_GameStart");
        GameObject go = GameObject.Instantiate<GameObject>(start,UI.transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != 1) return;
        player.Tick();
    }
    void FixedUpdate()
    {
        if (gameState != 1) return;
        player.FixedTick();
        Camera.main.transform.position = player.transform.position - new Vector3(0, 0, 10);
    }

    public Bullet GenBullet()
    {
        return bulletPool.Take();
    }
    void GenEnemy()
    {
        curEnemyNum = EnemyNum;
        for (int i = 0; i < EnemyNum; ++i)
        {
            float x = Random.Range(-9.0f, 9.0f);
            float y = Random.Range(-9.0f, 9.0f);
            float atk = Random.Range(EnemyATK.x, EnemyATK.y);
            float speed = Random.Range(EnemySPEED.x, EnemySPEED.y);
            float hp = Random.Range(EnemyHP.x, EnemyHP.y);
            Enemy enemy = enemyPool.Take();
            enemy.Init(new Vector2(x, y), hp, speed, atk);
            enemy.Inject(player);
        }
    }

    void EnemyDown()
    {
        --curEnemyNum;
        CheckEnemyAllDown();
    }
    void CheckEnemyAllDown()
    {
        if (curEnemyNum == 0)
        {
            gameState = 2;
            GameObject selectUI = Resources.Load<GameObject>("Prefabs/UI_SelectAward");
            GameObject go = GameObject.Instantiate<GameObject>(selectUI,UI.transform);
            UI_GameSelcetAward com = go.GetComponent<UI_GameSelcetAward>();
            com.Init(GenAward());
            com.selectAwardEvent += SelectAward;
        }
    }
    void EnterNextLevel()
    {
        int numAdd = Random.Range(EnemyAdd.x, EnemyAdd.y);
        float atkAdd = Random.Range(EnemyATKAdd.x, EnemyATKAdd.y);
        float hpAdd = Random.Range(EnemyHPAdd.x, EnemyHPAdd.y);
        float speedAdd = Random.Range(EnemySPEEDAdd.x, EnemySPEEDAdd.y);

        EnemyNum += numAdd;
        EnemyATK += new Vector2(atkAdd, atkAdd);
        EnemyHP += new Vector2(hpAdd, hpAdd);
        EnemySPEED += new Vector2(speedAdd, speedAdd);

        GenEnemy();
        gameState = 1;
    }

    PropertyValue[] GenAward(){
        PropertyValue[] arr = new PropertyValue[3];
        float GetValue(int property){
            switch((Property)property){
                case Property.生命:
                    return Random.Range(hpRange.x,hpRange.y);
                case Property.攻击力:
                    return Random.Range(atkRange.x,atkRange.y);
                case Property.防御力:
                    return Random.Range(guardRange.x,guardRange.y);
                case Property.速度:
                    return Random.Range(speedRange.x,speedRange.y);
                case Property.冲刺:
                    return Random.Range(rushSpeed.x,rushSpeed.y);
                case Property.无敌:
                    return Random.Range(shieldTime.x,shieldTime.y);
                case Property.开火间隔:
                    return Random.Range(fireDuration.x,fireDuration.y);
                default:
                    return 0;
            }
        }
        int idx = Random.Range(0,7);
        arr[0] = new PropertyValue(){prop = (Property)idx, value = GetValue(idx)};
        idx = Random.Range(0,7);
        arr[1] = new PropertyValue(){prop = (Property)idx, value = GetValue(idx)};
        idx = Random.Range(0,7);
        arr[2] = new PropertyValue(){prop = (Property)idx, value = GetValue(idx)};
        return arr;
        
    }
    void SelectAward(PropertyValue propValue){
        switch(propValue.prop){
            case Property.生命:
                player.AddHp(propValue.value);
                break;
            case Property.攻击力:
                player.AddAtk(propValue.value);
                break;
            case Property.防御力:
                player.AddAtk(propValue.value);
                break;
            case Property.速度:
                player.AddSpeed(propValue.value);
                break;
            case Property.冲刺:
                player.AddRush(propValue.value);
                break;
            case Property.无敌:
                player.AddShield(propValue.value);
                break;
            case Property.开火间隔:
                player.AddShield(propValue.value);
                break;
        }

        EnterNextLevel();
    }
}
