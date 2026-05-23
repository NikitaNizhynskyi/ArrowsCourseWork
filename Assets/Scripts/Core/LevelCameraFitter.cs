using UnityEngine;

public class LevelCameraFitter : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    [Header("World Padding")]
    [SerializeField] private float worldPadding = 0.5f;

    [Header("Screen Margins In Pixels")]
    [SerializeField] private float leftMargin = 80f;
    [SerializeField] private float rightMargin = 80f;
    [SerializeField] private float topMargin = 120f;
    [SerializeField] private float bottomMargin = 220f;

    public void FitToLevel(Vector2Int gridSize, float cellSize, Vector2 gridOrigin)
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera == null)
        {
            Debug.LogWarning("Camera not found");
            return;
        }

        targetCamera.orthographic = true;

        float levelWidth = gridSize.x * cellSize;
        float levelHeight = gridSize.y * cellSize;

        Vector3 levelCenter = new Vector3(
            gridOrigin.x + levelWidth / 2f - cellSize / 2f,
            gridOrigin.y + levelHeight / 2f - cellSize / 2f,
            -10f
        );

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float safeWidth = Mathf.Max(1f, screenWidth - leftMargin - rightMargin);
        float safeHeight = Mathf.Max(1f, screenHeight - topMargin - bottomMargin);

        float screenAspect = screenWidth / screenHeight;

        float safeWidthPercent = safeWidth / screenWidth;
        float safeHeightPercent = safeHeight / screenHeight;

        float wantedWorldWidth = levelWidth + worldPadding * 2f;
        float wantedWorldHeight = levelHeight + worldPadding * 2f;

        float sizeByHeight = wantedWorldHeight / (2f * safeHeightPercent);
        float sizeByWidth = wantedWorldWidth / (2f * screenAspect * safeWidthPercent);

        float finalOrthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);

        targetCamera.orthographicSize = finalOrthographicSize;

        float visibleWorldHeight = finalOrthographicSize * 2f;
        float visibleWorldWidth = visibleWorldHeight * screenAspect;

        float safeCenterX = (leftMargin + safeWidth / 2f) / screenWidth;
        float safeCenterY = (bottomMargin + safeHeight / 2f) / screenHeight;

        float offsetX = (safeCenterX - 0.5f) * visibleWorldWidth;
        float offsetY = (safeCenterY - 0.5f) * visibleWorldHeight;

        targetCamera.transform.position = new Vector3(
            levelCenter.x - offsetX,
            levelCenter.y - offsetY,
            -10f
        );
    }
}