using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [Header("Buff Options")]
    [SerializeField] float cooldownDuration;
    private float cooldownDurationCurrent;
    [SerializeField] bool isAuraBuff;
    [SerializeField] float buffDuration = 1.0f;

    [SerializeField] List<AbilityEffect> effects = new List<AbilityEffect>();

    public Unit owner;

    float buffDurationCurrent;

    protected void Awake()
    {
        owner = transform.parent.GetComponent<Unit>();
        buffDurationCurrent = buffDuration;
        //cooldownDurationCurrent = cooldownDuration;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!isAuraBuff)
        {
            if (buffDurationCurrent > 0)
            {
                buffDurationCurrent -= Time.deltaTime;
            }
            else
            {
                BuffExpire();
            }
        }

        if (cooldownDuration > 0)
        {
            if (cooldownDurationCurrent <= 0)
            {
                BuffApply();
                cooldownDurationCurrent = cooldownDuration;
            }
            else
            {
                cooldownDurationCurrent -= Time.deltaTime;
            }
        }
    }

    public void BuffExpire()
    {
        Destroy(gameObject);
    }

    protected virtual void BuffApply()
    {
        for (int x = 0; x < effects.Count; x++)
        {
            effects[x].ApplyEffect(this);
        }
    }
}
