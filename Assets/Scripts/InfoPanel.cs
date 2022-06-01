using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public TextMeshPro unitNameTMP;
    public SpriteRenderer unitHealthSprite;
    private Unit infoUnit;
    [SerializeField] Vector3 infoOffset;
    Quaternion defaultRotation;


    private void Awake()
    {
        defaultRotation = transform.rotation;
        UpdateInfo();
        
        transform.position += infoOffset;
        //if (Camera.main != null) transform.LookAt(Camera.main.transform);
    }

    private void Update()
    {
        transform.rotation = defaultRotation;
        //if (Camera.main != null) transform.LookAt(Camera.main.transform);
    }

    public void UpdateInfo()
    {
        if (infoUnit == null)
        {
            infoUnit = transform.parent.GetComponent<Unit>();
        }

        if (infoUnit != null)
        {
            unitNameTMP.text = infoUnit.name;

            if (infoOffset == Vector3.zero)
            {
                infoOffset.y = infoUnit.transform.localScale.y;
            }
        }

    }
}
