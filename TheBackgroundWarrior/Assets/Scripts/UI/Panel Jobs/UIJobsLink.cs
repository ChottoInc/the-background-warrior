using UnityEngine;

[ExecuteInEditMode]
public class UIJobsLink : MonoBehaviour
{
    [SerializeField] RectTransform from;
    [SerializeField] RectTransform to;
    [SerializeField] RectTransform line;


    private void Update()
    {
        if (!from || !to || !line) return;

        Vector3 a = from.position;
        Vector3 b = to.position;

        // Midpoint
        line.position = (a + b) * 0.5f;

        // Direction
        Vector3 dir = b - a;

        // Rotation
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        line.rotation = Quaternion.Euler(0, 0, angle);

        // Length
        float distance = dir.magnitude;
        line.sizeDelta = new Vector2(distance, line.sizeDelta.y);
    }
}
