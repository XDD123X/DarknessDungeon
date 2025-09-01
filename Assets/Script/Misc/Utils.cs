using UnityEngine;

public class Utils
{
    public static GameObject CreateVisualCircle(GameObject target, float radius, Material material = null)
    {
        // Create a new empty GameObject to represent the circle
        GameObject circleObject = new GameObject("VisualCircle");

        // Add a SpriteRenderer component for visualization
        SpriteRenderer circleSpriteRenderer = circleObject.AddComponent<SpriteRenderer>();

        // Set the initial position of the circle based on the target's position
        circleObject.transform.position = target.transform.position;

        // Create a circle texture (optional, adjust as needed)
        Texture2D circleTexture = CreateCircleTexture(radius, Color.white);

        // Set the texture of the SpriteRenderer
        circleSpriteRenderer.sprite = Sprite.Create(circleTexture, new Rect(0, 0, circleTexture.width, circleTexture.height), new Vector2(0.5f, 0.5f));

        // Set the material if provided, otherwise use default material
        if (material != null)
        {
            circleSpriteRenderer.material = material;
        }

        // Adjust sorting layer and order if necessary (optional)

        // Return the created circle GameObject
        return circleObject;
    }

    private static Texture2D CreateCircleTexture(float radius, Color color)
    {
        int width = Mathf.RoundToInt(radius * 2);
        int height = width;
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point; // Avoid blurring on scaling

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(width / 2f, height / 2f));
                texture.SetPixel(x, y, distance <= radius ? color : Color.clear);
            }
        }

        texture.Apply();
        return texture;
    }
}
