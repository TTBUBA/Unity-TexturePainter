using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(MeshCollider))]
public class DrawSystem : MonoBehaviour
{
    [SerializeField] private Camera cam;
   // [SerializeField] private Texture2D texture2D;
    [SerializeField] private int brushSize = 10;
    [SerializeField] private Renderer targetRenderer;  
    [SerializeField] private int textureSize = 1024;

    private Texture2D texture2D;
    private Vector2Int previousPosition;
    private bool hasPreviousPosition;

    private Color[] brushPixels;

    private void Awake()
    {
        //create a new texture when the object is active
        texture2D = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);

        Color[] background = new Color[textureSize * textureSize];

        for (int i = 0; i < background.Length; i++)
            background[i] = Color.black;

        texture2D.SetPixels(background);
        texture2D.Apply();

        targetRenderer.material.mainTexture = texture2D;


        brushPixels = new Color[brushSize * brushSize];

        for (int i = 0; i < brushPixels.Length; i++)
            brushPixels[i] = Color.red;
    }

    void Update()
    {
        if(!Mouse.current.leftButton.isPressed)
        {
            hasPreviousPosition = false;

            return;
        }

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.value);

        if (!Physics.Raycast(ray, out RaycastHit rayHit))
        {
            hasPreviousPosition = false;
            return;
        }

        Vector2Int currentPosition = new Vector2Int(
            Mathf.RoundToInt(rayHit.textureCoord.x * (texture2D.width - 1)),
            Mathf.RoundToInt(rayHit.textureCoord.y * (texture2D.height - 1))
        );

        //Debug.Log($"Hit: {rayHit.collider.name} | UV: {rayHit.textureCoord} | Pixel: {currentPosition}");

        if (hasPreviousPosition)
        {
            //drawLine
            DrawLine(previousPosition, currentPosition);
        }
        else
        {
            //draw brush
            DrawBrush(currentPosition);
        }

        previousPosition = currentPosition;
        hasPreviousPosition = true;

        texture2D.Apply(false, false);
    }

    //algorithme Bresenham  
    private void DrawLine(Vector2Int start, Vector2Int end)
    {
        int deltaX = Mathf.Abs(end.x - start.x);
        int deltaY = Mathf.Abs(end.y - start.y);

        int stepX = start.x < end.x ? 1 : -1; //direction of march
        int stepY = start.y < end.y ? 1 : -1;

        int error = deltaX - deltaY; //choose if move in horizzonatl or vetical 

        Vector2Int point = start;

        while (true)
        {
            DrawBrush(point);

            if (point == end) break;

            int DoubleError = 2 * error;

            if(DoubleError > -deltaY) //We are too "behind" horizontally
            {
                error -= deltaY;
                point.x += stepX;
            }
            if (DoubleError < deltaX) //We are too "behind" vertical
            {
                error += deltaX;
                point.y += stepY;
            }

        }

    }

    private void DrawBrush(Vector2Int center)
    {
        Vector2Int corner = GetBrushCorner(center);

        texture2D.SetPixels(corner.x, corner.y, brushSize, brushSize, brushPixels);
    }

    private Vector2Int GetBrushCorner(Vector2Int center)
    {
        int halfBrush = brushSize / 2;

        int maxX = texture2D.width - brushSize;
        int maxY = texture2D.height - brushSize;

        return new Vector2Int(
            Mathf.Clamp(center.x - halfBrush, 0, maxX),
            Mathf.Clamp(center.y - halfBrush, 0, maxY)
        );
    }

    public void ClearTexture()
    {
        int sizeTexture = texture2D.width;
        int sizeHeight = texture2D.height;

        Debug.Log($"Size Texture: {sizeTexture}x{sizeHeight}");

        texture2D.SetPixels(0, 0, sizeTexture, sizeHeight, new Color[sizeTexture * sizeHeight]);
        texture2D.Apply();
    }

    private void DebugTexture()
    {
        Debug.Log($"Format: {texture2D.format}");
        Debug.Log($"Is Readable: {texture2D.isReadable}");
        Debug.Log($"Width: {texture2D.width}");
        Debug.Log($"Height: {texture2D.height}");
        Debug.Log($"Graphics Format: {texture2D.graphicsFormat}");
    }
}
