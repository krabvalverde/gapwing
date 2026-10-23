using UnityEngine;

public class Scroller : MonoBehaviour
{
    [SerializeField] float factor = 1f, width = 30f;

    Vector3 start;

    void Awake() => start = transform.position;

    void Update()
    {
        transform.position += Vector3.left * GameManager.Instance.Speed * factor * Time.deltaTime;
        if (start.x - transform.position.x >= width) transform.position += Vector3.right * width;
    }
}
