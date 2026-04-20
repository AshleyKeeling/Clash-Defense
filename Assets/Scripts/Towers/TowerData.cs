using UnityEngine;

public class TowerData : MonoBehaviour
{
    public int buildCost;
    public float viewRadius = 10f;

    [Header("Gun Settings")]
    public float fireRate = 1f;
    public float bulletDamage = 1f;
    public bool canFire = true;

    [Header("Ability Settings")]
    public float abilityFireRate = 1f;
    public float abilityBulletDamage = 1f;
    public bool isBoostAbilityEnabled = false;
    public bool isDamageAbilityEnabled = false;
}
