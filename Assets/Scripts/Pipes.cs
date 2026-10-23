using UnityEngine;

public class Pipes : MonoBehaviour
{
    [SerializeField] Transform top, bottom;
    [SerializeField] float destroyX = -17f;

    public void SetGap(float gap)
    {
        top.localPosition = Vector3.up * gap / 2f;
        bottom.localPosition = Vector3.down * gap / 2f;
    }

    void Update()
    {
        transform.position += Vector3.left * GameManager.Instance.Speed * Time.deltaTime;
        if (transform.position.x < destroyX) Destroy(gameObject);
    }
}
