using UnityEngine;

public class BackgroundView : MonoBehaviour
{
    [SerializeField] Vector2 _tiling;
    public Vector2 Tiling => _tiling;

    public void MoveTo(Vector2 position)
    {
        transform.position = new Vector3(
            Mathf.RoundToInt(position.x / Tiling.x) * Tiling.x,
            Mathf.RoundToInt(position.y / Tiling.y) * Tiling.y,
            0);
    }
}
