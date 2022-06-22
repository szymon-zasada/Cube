using UnityEngine;

public class Enemy : ScriptableObject
{

    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] int cashValue;
    [SerializeField] bool hasCrystal;

    public float Health => health;
    public float MaxHealth => maxHealth;
    public float Speed => speed;
    public float Damage => damage;
    public int CashValue => cashValue;
    public bool HasCrystal => hasCrystal;


    public void TakeDamage(float damageValue) => health -= damageValue;


    public static Enemy CreateInstance(float maxHealth, float speed, float damage, int cashValue, float crystalChance)
    {
        var c = ScriptableObject.CreateInstance<Enemy>();
        c.maxHealth = maxHealth;
        c.health = c.maxHealth;
        c.speed = speed;
        c.damage = damage;
        c.cashValue = cashValue;

        float crystalProbability = Random.Range(0.5f * GameManager.Instance.Difficulty, 0.5f + (GameManager.Instance.Difficulty));
        if(crystalProbability < crystalChance)
            c.hasCrystal = true;
        else
            c.hasCrystal = false;
        

        return c;
    }


}
