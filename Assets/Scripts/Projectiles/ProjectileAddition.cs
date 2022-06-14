using UnityEngine;



public class ProjectileAddition : MonoBehaviour
{
    [SerializeField] SpecialEffect spawnedSpecialEffect;
    [SerializeField] float timeTillDestroy = 0.5f;
    public float damage;
    public Unit owner;
    private float timeTillDestroyCurrent;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
        timeTillDestroyCurrent = timeTillDestroy;
    }

    private void OnEnable()
    {
        if (spawnedSpecialEffect != null)
        {
            Instantiate(spawnedSpecialEffect, transform.position, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (timeTillDestroyCurrent > 0)
        {
            timeTillDestroyCurrent -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (damage == 0) return;

        Unit collidedUnit = other.gameObject.GetComponent<Unit>();
        if (collidedUnit == null) return;
        if (collidedUnit == owner) return;


        owner.DealDamage(collidedUnit, damage);

    }
}
