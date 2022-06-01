using UnityEngine;

public class HitZone : MonoBehaviour
{
    [SerializeField] float hitZoneDuration = 0.5f;
    [SerializeField] float hitZoneDurationCurrent;
    public float hitZoneDamage;
    private CapsuleCollider hitZoneCollider;
    public Unit hitZoneOwner;

    [SerializeField] bool isMeleeCalculation;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
        hitZoneCollider = GetComponent<CapsuleCollider>();
        //hitZoneOwner = transform.parent.GetComponent<Unit>();

        hitZoneDurationCurrent = hitZoneDuration;
    }

    private void OnEnable()
    {
        if (isMeleeCalculation)
        {
            Vector3 hitDirectionCalculation = hitZoneOwner.transform.position - GameController.Instance.playerWorldMousePos;
            transform.position = hitZoneOwner.transform.position - (hitDirectionCalculation.normalized * hitZoneOwner.attackRange);
            transform.localScale *= hitZoneOwner.attackRange;

            //hitZoneDamage = hitZoneOwner.
        }

        CharacterController hitZoneOwnerCollider = hitZoneOwner.GetComponent<CharacterController>();
        Physics.IgnoreCollision(hitZoneCollider, hitZoneOwnerCollider);


    }

    // Update is called once per frame
    void Update()
    {
        if (hitZoneDurationCurrent <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            hitZoneDurationCurrent -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit collidedUnit = other.gameObject.GetComponent<Unit>();

        if (collidedUnit != null)
        {
            if (isMeleeCalculation)
            {
                hitZoneOwner.DealDamage(collidedUnit);
            }
            else
            {
                hitZoneOwner.DealDamage(collidedUnit, hitZoneDamage);
            }
            //Debug.Log();
        }
    }
}
