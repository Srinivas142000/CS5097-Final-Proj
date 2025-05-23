using System.Collections.Generic;
using UnityEngine;

public class WallCreator : MonoBehaviour
{
    public GameObject origin; // First corner point (diagonal)
    public GameObject topright; // Opposite diagonal corner point

    public Material defaultWallMaterial; // Default material to apply
    
    public GameObject createdWall; // Reference to the created wall
    public float wallThickness = 0.005f; // Thickness of the wall
    
    // Generate wall using the current points
    public void CreateWall()
    {
        if (origin == null || topright == null)
        {
            Debug.LogError("Both points (origin and topright) must be assigned to create a wall");
            return;
        }
        
        if (createdWall != null)
        {
            Destroy(createdWall);
        }
        
        // Create a new cube for the wall
        createdWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        createdWall.name = "GeneratedWall";
        
        // Apply default material if provided
        if (defaultWallMaterial != null)
        {
            createdWall.GetComponent<Renderer>().material = defaultWallMaterial;
        }
        
        // world positions of the two corner points that define the wall
        Vector3 posA = origin.transform.position;
        Vector3 posB = topright.transform.position;
        
        // direction vector from origin to topright in the XZ plane
        Vector3 directionAB = new Vector3(posB.x - posA.x, 0, posB.z - posA.z).normalized;
        
        // Calculate the wall's forward direction (perpendicular to the wall face)
        Vector3 wallForward = Vector3.Cross(directionAB, Vector3.up).normalized;
        
        // Handle case where points might be aligned vertically
        if (wallForward.magnitude < 0.1f)
        {
            // Default to a direction based on the relative positions
            float xDiff = Mathf.Abs(posB.x - posA.x);
            float zDiff = Mathf.Abs(posB.z - posA.z);
            wallForward = xDiff <= zDiff ? Vector3.right : Vector3.forward;
        }
        
        // minimum and maximum bounds of the wall
        float minX = Mathf.Min(posA.x, posB.x);
        float maxX = Mathf.Max(posA.x, posB.x);
        float minY = Mathf.Min(posA.y, posB.y);
        float maxY = Mathf.Max(posA.y, posB.y);
        float minZ = Mathf.Min(posA.z, posB.z);
        float maxZ = Mathf.Max(posA.z, posB.z);
        
        // center point and dimensions of the wall
        Vector3 faceCenter = new Vector3(
            (minX + maxX) / 2f,
            (minY + maxY) / 2f,
            (minZ + maxZ) / 2f
        );
        
        // Calculate the wall's width and height
        float wallWidth = Vector3.Distance(
            new Vector3(minX, 0, minZ),
            new Vector3(maxX, 0, maxZ)
        );
        float height = Mathf.Abs(maxY - minY);
        
        Vector3 cubeCenter = faceCenter + wallForward * (wallThickness / 2f);
        
        Quaternion rotation = Quaternion.LookRotation(wallForward, Vector3.up);
        Vector3 newScale = new Vector3(wallWidth, height, wallThickness);
        Debug.Log($"Wall Scale: Width={newScale.x}, Height={newScale.y}, Thickness={newScale.z}");
        
        createdWall.transform.position = cubeCenter;
        createdWall.transform.localScale = newScale;
        createdWall.transform.rotation = rotation;
        createdWall.transform.SetParent(this.transform);
    }
    
    // Assign points from other scripts
    public void SetWallPoints(GameObject firstPoint, GameObject secondPoint)
    {
        origin = firstPoint;
        topright = secondPoint;
    }
    
    // Apply a new material to the wall
    public void ApplyMaterial(Material newMaterial)
    {
        if (createdWall != null && newMaterial != null)
        {
            createdWall.GetComponent<Renderer>().material = newMaterial;
        }
    }

    public void ApplyTexture(Texture newTexture)
    {
        if (createdWall != null && newTexture != null)
        {
            Renderer renderer = createdWall.GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                renderer.material.mainTexture = newTexture;
                Debug.Log($"Texture {newTexture.name} applied to wall.");
            }
        }
    }


    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One)) // Y button
        {
            CreateWall();
        }
    }
}