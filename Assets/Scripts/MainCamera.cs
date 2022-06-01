using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] Transform offsetCenterPoint;
    Vector3 offsetDifference;
    float zoomSensitivity = 0.1f;
    float zoomMultiplier = 1.0f;
    Vector2 zoomMultiplierLimit;

    // Start is called before the first frame update
    void Awake()
    {
        zoomMultiplierLimit.x = 0.3f;
        zoomMultiplierLimit.y = 1.2f;
        if (offsetCenterPoint != null)
        {
            offsetDifference = transform.position - offsetCenterPoint.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0.0f)
        {
            zoomMultiplier -= Input.mouseScrollDelta.y * zoomSensitivity;
            zoomMultiplier = Mathf.Clamp(zoomMultiplier,zoomMultiplierLimit.x,zoomMultiplierLimit.y);
        }
        if (GameController.Instance.selectedHero != null)
        {
            transform.position = Vector3.Lerp(transform.position, GameController.Instance.selectedHero.transform.position + offsetDifference * zoomMultiplier, 2f * Time.deltaTime);
        }
    }
}
