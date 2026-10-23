using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] Pipes prefab;
    [SerializeField] float interval = 1.6f, yRange = 2f;

    float timer;

    void Update()
    {
        var game = GameManager.Instance;
        if (game.Current != GameManager.State.Playing) return;
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = interval;
        var pipes = Instantiate(prefab, transform.position + Vector3.up * Random.Range(-yRange, yRange), Quaternion.identity);
        pipes.SetGap(game.Gap);
    }
}
