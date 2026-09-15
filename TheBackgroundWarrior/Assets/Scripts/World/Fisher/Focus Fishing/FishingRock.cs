using UnityEngine;

public class FishingRock : MonoBehaviour
{
    [SerializeField] GameObject _destroyVFX;

    private float _moveSpeed;
    private float _despawnX;
    private bool _isInitialized;

    public void Initialize(float speed, float despawnPositionX)
    {
        _moveSpeed = speed;
        _despawnX = despawnPositionX;
        _isInitialized = true;
    }

    private void Update()
    {
        if (!_isInitialized) return;

        transform.position += Vector3.left * _moveSpeed * Time.deltaTime;

        if (transform.position.x <= _despawnX)
        {
            Destroy(gameObject);
        }
    }

    public void StartDestroy()
    {
        GameObject vfx = Instantiate(_destroyVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = null;
        Destroy(gameObject);
    }
}
