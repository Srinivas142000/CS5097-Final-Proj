using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

// public class CustomWallMaker : MonoBehaviour
// {
//     public GameObject objectPrefab;
//     public float spawnDistance = 30f;
//     public float wallHeight = 2.5f;  // Height of the wall in meters
//     public Material wallMaterial;     // Material to apply to the wall

//     private bool buildModeActive = false;
//     private float buttonPressTime = 0f;
//     private const float longPressDuration = 2f;

//     private int pressCount = 0;
//     private float lastPressTime = 0f;
//     private List<Vector3> initialPositions = new List<Vector3>();


//     // Limit number of points
//     private const int MAX_POINTS = 10;

//     // Move static counter to class level
//     private int pointCounter = 1;

//     // Track spawned objects
//     private List<GameObject> spawnedPoints = new List<GameObject>();
//     private GameObject generatedWall;

//     private GameObject buildModeIndicator;

//     private void Start()
//     {
//         // Create the build mode indicator
//         buildModeIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//         buildModeIndicator.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f); // Small dot
//         buildModeIndicator.GetComponent<Renderer>().material = new Material(wallMaterial);
//         buildModeIndicator.GetComponent<Renderer>().material.color = Color.red; // Set initial color
//         buildModeIndicator.GetComponent<Collider>().enabled = false; // Disable collider
//         buildModeIndicator.SetActive(false); // Initially hidden
//     }

//     private void Update()
//     {
//         // Long press logic for toggling build mode and spawning points.
//         if (OVRInput.Get(OVRInput.Button.One))
//         {
//             buttonPressTime += Time.deltaTime;

//             // Check if we've reached the long press threshold
//             if (buttonPressTime >= longPressDuration)
//             {
//                 // Toggle build mode
//                 buildModeActive = !buildModeActive;
//                 buttonPressTime = 0f; // Reset timer after toggling
//                 Debug.Log($"Build Mode: {buildModeActive}");

//                 // Update indicator color and visibility
//                 buildModeIndicator.SetActive(true);
//                 buildModeIndicator.GetComponent<Renderer>().material.color = buildModeActive ? Color.green : Color.red;

//                 // Automatically generate wall when switching out of build mode if there are more than 1 point
//                 if (!buildModeActive && spawnedPoints.Count > 1)
//                 {
//                     GenerateWall();

//                     // Update initial positions since a new wall has been generated
//                     UpdateInitialPositions();
//                 }
//             }
//         }
//         else
//         {
//             // If button is released before threshold
//             if (buttonPressTime > 0f && buttonPressTime < longPressDuration && buildModeActive)
//             {
//                 // Only spawn if below max points
//                 const int MAX_POINTS = 10;  // Replace with your max value if different.
//                 if (spawnedPoints.Count < MAX_POINTS)
//                 {
//                     Debug.Log("Short press detected in build mode. Spawning object.");
//                     SpawnObject();

//                     // Update initial positions for the new point
//                     if (spawnedPoints.Count > initialPositions.Count)
//                     {
//                         initialPositions.Add(spawnedPoints[spawnedPoints.Count - 1].transform.position);
//                     }

//                     // Generate wall when all points have been placed
//                     if (spawnedPoints.Count == MAX_POINTS)
//                     {
//                         GenerateWall();
//                         UpdateInitialPositions();
//                     }
//                 }
//                 else
//                 {
//                     Debug.Log($"Maximum number of points reached ({MAX_POINTS}). Cannot spawn more.");
//                 }
//             }

//             // Reset button press time when button is released
//             buttonPressTime = 0f;
//         }

//         // Triple-click to clear points and wall (only if not in build mode)
//         if (!buildModeActive)
//         {
//             const float pressInterval = 0.3f; // Maximum interval between presses

//             if (OVRInput.GetDown(OVRInput.Button.One))
//             {
//                 if (Time.time - lastPressTime <= pressInterval)
//                 {
//                     pressCount++;
//                 }
//                 else
//                 {
//                     pressCount = 1; // Reset count if interval exceeded
//                 }
//                 lastPressTime = Time.time;

//                 if (pressCount == 3)
//                 {
//                     Debug.Log("Triple press detected. Clearing all points and wall.");
//                     ClearAllPointsAndWall();
//                     pressCount = 0; // Reset count after action

//                     // Also clear the stored initial positions
//                     initialPositions.Clear();
//                 }
//             }
//         }

//         // Automatically generate wall if not in build mode and any point positions have changed
//         if (!buildModeActive && spawnedPoints.Count > 1)
//         {
//             bool positionsChanged = false;
//             for (int i = 0; i < spawnedPoints.Count; i++)
//             {
//                 Vector3 currentPosition = spawnedPoints[i].transform.position;
//                 // Only compare if we have an initial position stored for this point.
//                 if (i < initialPositions.Count)
//                 {
//                     if (!Mathf.Approximately(Vector3.Distance(currentPosition, initialPositions[i]), 0f))
//                     {
//                         Debug.Log($"Position of point {i} changed from {initialPositions[i]} to {currentPosition}");
//                         positionsChanged = true;
//                         break;
//                     }
//                 }
//                 else
//                 {
//                     // If no stored initial position, add the current position.
//                     initialPositions.Add(currentPosition);
//                 }
//             }

//             if (positionsChanged)
//             {
//                 Debug.Log("Positions have changed. Generating wall...");
//                 GenerateWall();
//                 UpdateInitialPositions();
//             }
//             else
//             {
//                 Debug.Log("No positions have changed. Wall generation skipped.");
//             }
//         }

//         // Update the position and pulsing effect of the indicator
//         if (buildModeIndicator.activeSelf)
//         {
//             Transform cameraTransform = Camera.main.transform;
//             buildModeIndicator.transform.position = cameraTransform.position + cameraTransform.forward * 0.8f + cameraTransform.right * 0.25f + cameraTransform.up * -0.5f;
//             float pulse = Mathf.Abs(Mathf.Sin(Time.time * 2f)) * 0.5f + 0.5f; // Pulsing effect
//             buildModeIndicator.transform.localScale = new Vector3(0.09f, 0.09f, 0.09f) * pulse;
//         }
//     }

//     // Helper method to update the stored initial positions for each spawned point
//     private void UpdateInitialPositions()
//     {
//         initialPositions.Clear();
//         foreach (GameObject point in spawnedPoints)
//         {
//             initialPositions.Add(point.transform.position);
//         }
//     }

//     private void SpawnObject()
//     {
//         Transform centerEye = GameObject.Find("CenterEyeAnchor")?.transform;
//         if (centerEye == null || objectPrefab == null)
//         {
//             Debug.LogWarning("Right hand controller or object prefab not found");
//             return;
//         }

//         // Check point limit again (safety check)
//         if (pointCounter > MAX_POINTS)
//         {
//             Debug.Log($"Maximum number of points reached ({MAX_POINTS}). Cannot spawn more.");
//             return;
//         }

//         Vector3 spawnPosition = centerEye.position + centerEye.forward * spawnDistance;

//         // Ensure no overlap by checking minimum distance
//         Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f);
//         while (colliders.Length > 0)
//         {
//             spawnPosition += new Vector3(centerEye.forward.x, 0, centerEye.forward.z).normalized * 0.1f;
//             colliders = Physics.OverlapSphere(spawnPosition, 0.1f);
//         }

//         // First instantiate the object
//         GameObject newObject = Instantiate(objectPrefab, spawnPosition, centerEye.rotation);
//         newObject.SetActive(true);

//         // Track spawned object
//         spawnedPoints.Add(newObject);

//         // Update text label with point number
//         Transform nameChild = newObject.transform.Find("Name");
//         if (nameChild != null)
//         {
//             TextMeshProUGUI textComponent = nameChild.GetComponentInChildren<TextMeshProUGUI>();
//             if (textComponent != null)
//             {
//                 textComponent.text = $"Point {pointCounter}";
//                 pointCounter++; // Increment the counter after assigning the number
//             }
//         }

//         Debug.Log($"Object spawned at: {spawnPosition} with Point #{pointCounter-1} | {spawnedPoints.Count}/{MAX_POINTS}");
//         Debug.Log($"Center Eye Position: {centerEye.position}");
//     }

//     private void GenerateWall()
//     {
//         // 1. Get point positions
//         Vector3[] pointPositions = spawnedPoints.Select(p => p.transform.position).ToArray();

//         // 2. Find best fit vertical plane
//         (Vector3 planeNormal, Vector3 planePoint) = FindBestFitVerticalPlane(pointPositions);

//         // 3. Project points onto the plane and sort them in clockwise order
//         Vector3[] projectedPoints = ProjectPointsOntoPlane(pointPositions, planeNormal, planePoint);
//         Vector3[] sortedPoints = SortPointsClockwise(projectedPoints, planeNormal);

//         // 4. Update the positions of the spawned points to align with the new plane
//         for (int i = 0; i < spawnedPoints.Count; i++)
//         {
//             spawnedPoints[i].transform.position = projectedPoints[i];
//         }

//         // 5. Generate wall mesh
//         CreateWallMesh(sortedPoints, planeNormal);

//         Debug.Log("Wall generated successfully!");
//     }

//     public void ApplyMaterial(Material newMaterial)
//     {
//         if (generatedWall != null && newMaterial != null)
//         {
//             generatedWall.GetComponent<Renderer>().material = newMaterial;
//         }
//     }
//     // Computes a best-fit vertical plane from the provided points.
//     private (Vector3 normal, Vector3 point) FindBestFitVerticalPlane(Vector3[] points)
//     {
//         // Compute centroid.
//         Vector3 centroid = Vector3.zero;
//         foreach (Vector3 point in points)
//         {
//             centroid += point;
//         }
//         centroid /= points.Length;

//         // Only consider x and z for best-fit vertical plane.
//         float sumXX = 0, sumXZ = 0, sumZZ = 0;
//         foreach (Vector3 point in points)
//         {
//             Vector3 centered = point - centroid;
//             sumXX += centered.x * centered.x;
//             sumXZ += centered.x * centered.z;
//             sumZZ += centered.z * centered.z;
//         }

//         // Direction of least variance using PCA in the XZ plane.
//         float angle = 0.5f * Mathf.Atan2(2 * sumXZ, sumXX - sumZZ);

//         // The best-fit normal in the XZ plane is perpendicular to that direction.
//         Vector3 normal = new Vector3(Mathf.Cos(angle + Mathf.PI / 2), 0, Mathf.Sin(angle + Mathf.PI / 2));
//         normal.Normalize();

//         return (normal, centroid);
//     }

//     // Projects the points onto the plane defined by planeNormal and planePoint.
//     private Vector3[] ProjectPointsOntoPlane(Vector3[] points, Vector3 planeNormal, Vector3 planePoint)
//     {
//         Vector3[] projectedPoints = new Vector3[points.Length];
//         for (int i = 0; i < points.Length; i++)
//         {
//             Vector3 v = points[i] - planePoint;
//             float distance = Vector3.Dot(v, planeNormal);
//             projectedPoints[i] = points[i] - distance * planeNormal;
//         }
//         return projectedPoints;
//     }

//     // Sorts the points clockwise around their centroid using the provided normal as the axis.
//     private Vector3[] SortPointsClockwise(Vector3[] points, Vector3 normal)
//     {
//         // Compute the center of the points.
//         Vector3 center = Vector3.zero;
//         foreach (Vector3 point in points)
//         {
//             center += point;
//         }
//         center /= points.Length;

//         // Define a reference vector on the plane. Choose one that's not nearly parallel to the normal.
//         Vector3 refVector = Vector3.right;
//         if (Mathf.Abs(Vector3.Dot(normal, refVector)) > 0.9f)
//             refVector = Vector3.forward;

//         // Find the tangent and bitangent vectors.
//         Vector3 tangent = Vector3.Cross(normal, Vector3.Cross(refVector, normal)).normalized;
//         Vector3 bitangent = Vector3.Cross(normal, tangent);

//         // Sort the points by angle relative to the tangent-bitangent axes.
//         return points.OrderBy(point =>
//         {
//             Vector3 dir = point - center;
//             float angle = Mathf.Atan2(Vector3.Dot(dir, bitangent), Vector3.Dot(dir, tangent));
//             return angle;
//         }).ToArray();
//     }

//     // Triangulates a polygon using an ear clipping algorithm.
//     private void TriangulateFrontFace(Vector3[] polyPoints, Vector3 normal, List<int> triangles)
//     {
//         // Create a list of vertex indices.
//         List<int> indices = new List<int>();
//         for (int i = 0; i < polyPoints.Length; i++)
//         {
//             indices.Add(i);
//         }

//         // Continue clipping ears until only one triangle remains.
//         while (indices.Count > 3)
//         {
//             bool earFound = false;
//             for (int i = 0; i < indices.Count; i++)
//             {
//                 int prevIndex = indices[(i - 1 + indices.Count) % indices.Count];
//                 int currIndex = indices[i];
//                 int nextIndex = indices[(i + 1) % indices.Count];

//                 Vector3 a = polyPoints[prevIndex];
//                 Vector3 b = polyPoints[currIndex];
//                 Vector3 c = polyPoints[nextIndex];

//                 // Ensure the angle at b is convex.
//                 Vector3 ab = b - a;
//                 Vector3 bc = c - b;
//                 Vector3 cross = Vector3.Cross(ab, bc);
//                 if (Vector3.Dot(cross, normal) < 0)
//                     continue; // Concave, not an ear.

//                 // Check if any other point lies inside the triangle.
//                 bool isEar = true;
//                 for (int j = 0; j < indices.Count; j++)
//                 {
//                     if (j == (i - 1 + indices.Count) % indices.Count || j == i || j == (i + 1) % indices.Count)
//                         continue;
//                     int testIndex = indices[j];
//                     if (PointInTriangle(polyPoints[testIndex], a, b, c))
//                     {
//                         isEar = false;
//                         break;
//                     }
//                 }

//                 if (isEar)
//                 {
//                     // Append ear triangle.
//                     triangles.Add(prevIndex);
//                     triangles.Add(currIndex);
//                     triangles.Add(nextIndex);
//                     // Remove the ear tip.
//                     indices.RemoveAt(i);
//                     earFound = true;
//                     break;
//                 }
//             }
//             if (!earFound)
//             {
//                 // Fallback: if ear clipping fails, use a triangle fan.
//                 for (int i = 1; i < indices.Count - 1; i++)
//                 {
//                     triangles.Add(indices[0]);
//                     triangles.Add(indices[i]);
//                     triangles.Add(indices[i + 1]);
//                 }
//                 return;
//             }
//         }
//         // Add the last remaining triangle.
//         if (indices.Count == 3)
//         {
//             triangles.Add(indices[0]);
//             triangles.Add(indices[1]);
//             triangles.Add(indices[2]);
//         }
//     }

//     // Helper method: Determines if point p is inside the triangle defined by a, b, c.
//     private bool PointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
//     {
//         // Compute vectors.
//         Vector3 v0 = c - a;
//         Vector3 v1 = b - a;
//         Vector3 v2 = p - a;

//         // Compute dot products.
//         float dot00 = Vector3.Dot(v0, v0);
//         float dot01 = Vector3.Dot(v0, v1);
//         float dot02 = Vector3.Dot(v0, v2);
//         float dot11 = Vector3.Dot(v1, v1);
//         float dot12 = Vector3.Dot(v1, v2);

//         // Compute barycentric coordinates.
//         float invDenom = 1f / (dot00 * dot11 - dot01 * dot01);
//         float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
//         float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

//         // Check if point is in triangle.
//         return (u >= 0) && (v >= 0) && (u + v < 1);
//     }

//     // The main method that creates the wall mesh from an arbitrary set of points.
//     private void CreateWallMesh(Vector3[] originalPoints, Vector3 extrudeNormal)
//     {
//         // Step 1: Find the best-fit vertical plane.
//         (Vector3 planeNormal, Vector3 planeCenter) = FindBestFitVerticalPlane(originalPoints);

//         // Step 2: Project the original points onto this plane.
//         Vector3[] projectedPoints = ProjectPointsOntoPlane(originalPoints, planeNormal, planeCenter);

//         // Step 3: Sort the projected points clockwise so they define a proper polygon.
//         Vector3[] sortedPoints = SortPointsClockwise(projectedPoints, planeNormal);

//         // Create a new game object for the wall.
//         if (generatedWall != null)
//         {
//             Destroy(generatedWall);
//         }
//         generatedWall = new GameObject("GeneratedWall");

//         // Add mesh components.
//         MeshFilter meshFilter = generatedWall.AddComponent<MeshFilter>();
//         MeshRenderer meshRenderer = generatedWall.AddComponent<MeshRenderer>();
//         MeshCollider meshCollider = generatedWall.AddComponent<MeshCollider>();

//         // Create a new mesh.
//         Mesh mesh = new Mesh();

//         // Define wall thickness.
//         float thickness = 0.05f;

//         // Generate vertices for front and back faces.
//         int vertexCount = sortedPoints.Length * 2;
//         Vector3[] vertices = new Vector3[vertexCount];
//         Vector2[] uv = new Vector2[vertexCount];

//         // Front face vertices.
//         for (int i = 0; i < sortedPoints.Length; i++)
//         {
//             vertices[i] = sortedPoints[i];
//         }

//         // Back face vertices: offset by the extrusion normal.
//         for (int i = 0; i < sortedPoints.Length; i++)
//         {
//             vertices[i + sortedPoints.Length] = sortedPoints[i] - extrudeNormal * thickness;
//         }

//         // UV calculation (projected onto XZ for simplicity).
//         Vector3 min = vertices[0];
//         Vector3 max = vertices[0];
//         foreach (Vector3 v in vertices)
//         {
//             min = Vector3.Min(min, v);
//             max = Vector3.Max(max, v);
//         }
//         for (int i = 0; i < vertices.Length; i++)
//         {
//             float u = Mathf.InverseLerp(min.x, max.x, vertices[i].x);
//             float vCoord = Mathf.InverseLerp(min.z, max.z, vertices[i].z);
//             uv[i] = new Vector2(u, vCoord);
//         }

//         // Create triangles list.
//         List<int> triangles = new List<int>();

//         // Use the improved constrained triangulation for the front face
//         TriangulateWithBruteForce(sortedPoints, planeNormal, triangles);

//         // Triangulate the back face with reversed winding order.
//         List<int> backTriangles = new List<int>();
//         TriangulateWithBruteForce(sortedPoints, -planeNormal, backTriangles);

//         for (int i = 0; i < backTriangles.Count; i += 3)
//         {
//             // Reverse winding order and offset indices.
//             triangles.Add(backTriangles[i] + sortedPoints.Length);
//             triangles.Add(backTriangles[i + 2] + sortedPoints.Length);
//             triangles.Add(backTriangles[i + 1] + sortedPoints.Length);
//         }

//         // Build side faces that connect front and back.
//         for (int i = 0; i < sortedPoints.Length; i++)
//         {
//             int nextI = (i + 1) % sortedPoints.Length;

//             // First triangle for the side quad.
//             triangles.Add(i);
//             triangles.Add(i + sortedPoints.Length);
//             triangles.Add(nextI);

//             // Second triangle for the side quad.
//             triangles.Add(nextI);
//             triangles.Add(i + sortedPoints.Length);
//             triangles.Add(nextI + sortedPoints.Length);
//         }

//         // Assign mesh data.
//         mesh.vertices = vertices;
//         mesh.triangles = triangles.ToArray();
//         mesh.uv = uv;

//         mesh.RecalculateNormals();
//         mesh.RecalculateBounds();

//         // Assign the mesh to components.
//         meshFilter.mesh = mesh;
//         meshCollider.sharedMesh = mesh;
//         if (wallMaterial != null)
//         {
//             meshRenderer.material = wallMaterial;
//         }
//         else
//         {
//             meshRenderer.material = new Material(Shader.Find("Standard"));
//             Debug.LogWarning("Wall material not assigned! Using default material.");
//         }
//     }

//     private void TriangulateWithBruteForce(Vector3[] points, Vector3 normal, List<int> triangles)
//     {
//         int n = points.Length;
//         if (n < 3) return;

//         // Step 1: Define main edges (the polygon boundary)
//         List<(int, int)> mainEdges = new List<(int, int)>();
//         for (int i = 0; i < n; i++)
//         {
//             mainEdges.Add((i, (i + 1) % n));
//         }

//         // Step 2 & 3: Create triangles by connecting each vertex to previous vertices
//         for (int i = 2; i < n; i++) // Start from the third vertex
//         {
//             for (int j = 0; j < i - 1; j++) // Connect to all vertices except the adjacent one
//             {
//                 // Skip adjacent vertex (already connected by main edge)
//                 if (j == i - 1) continue;

//                 // Check if this potential interior edge is valid
//                 if (IsValidInteriorEdge(points, i, j, mainEdges, normal))
//                 {
//                     // Add triangle (i, i-1, j)
//                     triangles.Add(i);
//                     triangles.Add(i - 1);
//                     triangles.Add(j);
//                 }
//             }
//         }

//         // If no triangles were created, fall back to ear clipping
//         if (triangles.Count == 0)
//         {
//             Debug.LogWarning("Brute force triangulation failed. Falling back to ear clipping.");
//             TriangulateFrontFace(points, normal, triangles);
//         }
//     }

//     private bool IsValidInteriorEdge(Vector3[] points, int i, int j, List<(int, int)> mainEdges, Vector3 normal)
//     {
//         // Rule 3.1: Check if the interior edge intersects any main edge
//         for (int k = 0; k < mainEdges.Count; k++)
//         {
//             var edge = mainEdges[k];

//             // Skip if the main edge shares a vertex with our interior edge
//             if (edge.Item1 == i || edge.Item2 == i || edge.Item1 == j || edge.Item2 == j)
//                 continue;

//             // Check for intersection
//             if (EdgesIntersect(points[i], points[j], points[edge.Item1], points[edge.Item2]))
//                 return false;
//         }

//         // Rule 3.2: Check if the edge goes through exterior space
//         // We can check if the midpoint of the edge is inside the polygon
//         Vector3 midpoint = (points[i] + points[j]) / 2f;
//         if (!PointInPolygon(midpoint, points, normal))
//             return false;

//         // Additional check: Ensure the triangle has correct winding order
//         Vector3 a = points[i];
//         Vector3 b = points[i - 1]; // Adjacent vertex to i
//         Vector3 c = points[j];

//         Vector3 triangleNormal = Vector3.Cross(b - a, c - a).normalized;
//         return Vector3.Dot(triangleNormal, normal) > 0;
//     }

//     // New constrained triangulation method that respects boundaries
//     private void TriangulateWithConstraints(Vector3[] points, Vector3 normal, List<int> triangles)
//     {
//         int n = points.Length;
//         if (n < 3) return;

//         // Create a list to track which potential diagonals have been checked
//         bool[,] checkedDiagonals = new bool[n, n];

//         // Try to create triangles using each pair of points
//         for (int i = 0; i < n; i++)
//         {
//             for (int j = 0; j < n; j++)
//             {
//                 // Skip if already checked or same/adjacent points
//                 if (checkedDiagonals[i, j] || i == j || (i + 1) % n == j || (j + 1) % n == i)
//                     continue;

//                 checkedDiagonals[i, j] = true;
//                 checkedDiagonals[j, i] = true;

//                 // For each potential diagonal i-j, try to form triangles with other points
//                 for (int k = 0; k < n; k++)
//                 {
//                     // Skip if same as i or j, or if already checked
//                     if (k == i || k == j)
//                         continue;

//                     // Check if triangle i-j-k is valid (doesn't intersect boundaries and is inside polygon)
//                     if (IsValidTriangle(points, i, j, k, normal))
//                     {
//                         triangles.Add(i);
//                         triangles.Add(j);
//                         triangles.Add(k);
//                     }
//                 }
//             }
//         }

//         // If no triangles were created, fall back to ear clipping
//         if (triangles.Count == 0)
//         {
//             Debug.LogWarning("Constrained triangulation failed. Falling back to ear clipping.");
//             TriangulateFrontFace(points, normal, triangles);
//         }
//     }

//     // Check if a triangle is valid (doesn't intersect any boundary edges and is inside the polygon)
//     private bool IsValidTriangle(Vector3[] points, int i, int j, int k, Vector3 normal)
//     {
//         int n = points.Length;
//         Vector3 a = points[i];
//         Vector3 b = points[j];
//         Vector3 c = points[k];

//         // Check if the triangle has the correct winding order (is facing in the direction of the normal)
//         Vector3 triangleNormal = Vector3.Cross(b - a, c - a).normalized;
//         if (Vector3.Dot(triangleNormal, normal) <= 0)
//             return false;

//         // Check if any of the triangle's edges intersect with boundary edges
//         for (int m = 0; m < n; m++)
//         {
//             int nextM = (m + 1) % n;

//             // Skip if the boundary edge is part of the triangle
//             if ((m == i && nextM == j) || (m == j && nextM == i) ||
//                 (m == j && nextM == k) || (m == k && nextM == j) ||
//                 (m == k && nextM == i) || (m == i && nextM == k))
//                 continue;

//             // Check if any edge of the triangle intersects this boundary edge
//             if (EdgesIntersect(a, b, points[m], points[nextM]) ||
//                 EdgesIntersect(b, c, points[m], points[nextM]) ||
//                 EdgesIntersect(c, a, points[m], points[nextM]))
//                 return false;
//         }

//         // Check if the triangle is inside the polygon by checking its centroid
//         Vector3 centroid = (a + b + c) / 3f;
//         return PointInPolygon(centroid, points, normal);
//     }

//     // Check if two edges intersect (in 2D, projected onto the plane)
//     private bool EdgesIntersect(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
//     {
//         // Project to 2D (using X and Z coordinates for a vertical plane)
//         Vector2 a2d = new Vector2(a.x, a.z);
//         Vector2 b2d = new Vector2(b.x, b.z);
//         Vector2 c2d = new Vector2(c.x, c.z);
//         Vector2 d2d = new Vector2(d.x, d.z);

//         return LineSegmentsIntersect(a2d, b2d, c2d, d2d);
//     }

//     // Check if two line segments intersect in 2D
//     private bool LineSegmentsIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
//     {
//         // Calculate the direction vectors
//         Vector2 ab = b - a;
//         Vector2 cd = d - c;

//         // Calculate the determinant
//         float det = ab.x * cd.y - ab.y * cd.x;

//         // If determinant is zero, lines are parallel
//         if (Mathf.Approximately(det, 0f))
//             return false;

//         // Calculate the parameters
//         Vector2 ac = c - a;
//         float t = (ac.x * cd.y - ac.y * cd.x) / det;
//         float u = (ac.x * ab.y - ac.y * ab.x) / det;

//         // Check if the intersection point is within both line segments
//         return t >= 0f && t <= 1f && u >= 0f && u <= 1f;
//     }

//     // Check if a point is inside a polygon
//     private bool PointInPolygon(Vector3 point, Vector3[] polygon, Vector3 normal)
//     {
//         // Create a coordinate system on the plane
//         Vector3 refVector = Vector3.right;
//         if (Mathf.Abs(Vector3.Dot(normal, refVector)) > 0.9f)
//             refVector = Vector3.forward;

//         Vector3 tangent = Vector3.Cross(normal, Vector3.Cross(refVector, normal)).normalized;
//         Vector3 bitangent = Vector3.Cross(normal, tangent);

//         // Project the point and polygon onto the plane
//         Vector2 point2D = new Vector2(Vector3.Dot(point, tangent), Vector3.Dot(point, bitangent));
//         Vector2[] polygon2D = new Vector2[polygon.Length];

//         for (int i = 0; i < polygon.Length; i++)
//         {
//             polygon2D[i] = new Vector2(
//                 Vector3.Dot(polygon[i], tangent),
//                 Vector3.Dot(polygon[i], bitangent)
//             );
//         }

//         // Ray casting algorithm to determine if point is inside polygon
//         bool inside = false;
//         for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
//         {
//             if (((polygon2D[i].y > point2D.y) != (polygon2D[j].y > point2D.y)) &&
//                 (point2D.x < (polygon2D[j].x - polygon2D[i].x) * (point2D.y - polygon2D[i].y) / 
//                 (polygon2D[j].y - polygon2D[i].y) + polygon2D[i].x))
//             {
//                 inside = !inside;
//             }
//         }

//         return inside;
//     }

//     private void ClearAllPointsAndWall()
//     {
//         // Destroy all spawned points
//         foreach (GameObject point in spawnedPoints)
//         {
//             Destroy(point);
//         }
//         spawnedPoints.Clear();

//         // Reset point counter
//         pointCounter = 1;

//         // Destroy the generated wall if it exists
//         if (generatedWall != null)
//         {
//             Destroy(generatedWall);
//             generatedWall = null;
//         }

//         Debug.Log("All points and wall cleared.");
//     }
// }

using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class CustomWallMaker : MonoBehaviour
{
    public GameObject objectPrefab;
    public float spawnDistance = 30f;
    public float wallHeight = 2.5f; // Height of the wall in meters
    public Material wallMaterial; // Material to apply to the wall

    private bool buildModeActive = false;
    private float buttonPressTime = 0f;
    private const float longPressDuration = 2f;
    private int pressCount = 0;
    private float lastPressTime = 0f;
    private List<Vector3> initialPositions = new List<Vector3>();

    // Limit number of points
    private const int MAX_POINTS = 10;

    // Move static counter to class level
    private int pointCounter = 1;

    // Track spawned objects
    private List<GameObject> spawnedPoints = new List<GameObject>();
    private GameObject generatedWall;
    private GameObject buildModeIndicator;

    private void Start()
    {
        // Create the build mode indicator
        buildModeIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        buildModeIndicator.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f); // Small dot
        buildModeIndicator.GetComponent<Renderer>().material = new Material(wallMaterial);
        buildModeIndicator.GetComponent<Renderer>().material.color = Color.red; // Set initial color
        buildModeIndicator.GetComponent<Collider>().enabled = false; // Disable collider
        buildModeIndicator.SetActive(false); // Initially hidden
    }

    private void Update()
    {
        // Long press logic for toggling build mode and spawning points.
        if (OVRInput.Get(OVRInput.Button.One))
        {
            buttonPressTime += Time.deltaTime;
            // Check if we've reached the long press threshold
            if (buttonPressTime >= longPressDuration)
            {
                // Toggle build mode
                buildModeActive = !buildModeActive;
                buttonPressTime = 0f; // Reset timer after toggling
                Debug.Log($"Build Mode: {buildModeActive}");
                // Update indicator color and visibility
                buildModeIndicator.SetActive(true);
                buildModeIndicator.GetComponent<Renderer>().material.color = buildModeActive ? Color.green : Color.red;
                // Automatically generate wall when switching out of build mode if there are more than 1 point
                if (!buildModeActive && spawnedPoints.Count > 1)
                {
                    GenerateWall();
                    // Update initial positions since a new wall has been generated
                    UpdateInitialPositions();
                }
            }
        }
        else
        {
            // If button is released before threshold
            if (buttonPressTime > 0f && buttonPressTime < longPressDuration && buildModeActive)
            {
                // Only spawn if below max points
                if (spawnedPoints.Count < MAX_POINTS)
                {
                    Debug.Log("Short press detected in build mode. Spawning object.");
                    SpawnObject();
                    // Update initial positions for the new point
                    if (spawnedPoints.Count > initialPositions.Count)
                    {
                        initialPositions.Add(spawnedPoints[spawnedPoints.Count - 1].transform.position);
                    }

                    // Generate wall when all points have been placed
                    if (spawnedPoints.Count == MAX_POINTS)
                    {
                        GenerateWall();
                        UpdateInitialPositions();
                    }
                }
                else
                {
                    Debug.Log($"Maximum number of points reached ({MAX_POINTS}). Cannot spawn more.");
                }
            }
            // Reset button press time when button is released
            buttonPressTime = 0f;
        }

        // Triple-click to clear points and wall (only if not in build mode)
        if (!buildModeActive)
        {
            const float pressInterval = 0.3f; // Maximum interval between presses
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                if (Time.time - lastPressTime <= pressInterval)
                {
                    pressCount++;
                }
                else
                {
                    pressCount = 1; // Reset count if interval exceeded
                }
                lastPressTime = Time.time;
                if (pressCount == 3)
                {
                    Debug.Log("Triple press detected. Clearing all points and wall.");
                    ClearAllPointsAndWall();
                    pressCount = 0; // Reset count after action
                    // Also clear the stored initial positions
                    initialPositions.Clear();
                }
            }
        }

        // Automatically generate wall if not in build mode and any point positions have changed
        if (!buildModeActive && spawnedPoints.Count > 1)
        {
            bool positionsChanged = false;
            for (int i = 0; i < spawnedPoints.Count; i++)
            {
                Vector3 currentPosition = spawnedPoints[i].transform.position;
                // Only compare if we have an initial position stored for this point.
                if (i < initialPositions.Count)
                {
                    if (!Mathf.Approximately(Vector3.Distance(currentPosition, initialPositions[i]), 0f))
                    {
                        Debug.Log($"Position of point {i} changed from {initialPositions[i]} to {currentPosition}");
                        positionsChanged = true;
                        break;
                    }
                }
                else
                {
                    // If no stored initial position, add the current position.
                    initialPositions.Add(currentPosition);
                }
            }

            if (positionsChanged)
            {
                Debug.Log("Positions have changed. Generating wall...");
                GenerateWall();
                UpdateInitialPositions();
            }
            else
            {
                Debug.Log("No positions have changed. Wall generation skipped.");
            }
        }

        // Update the position and pulsing effect of the indicator
        if (buildModeIndicator.activeSelf)
        {
            Transform cameraTransform = Camera.main.transform;
            buildModeIndicator.transform.position = cameraTransform.position + cameraTransform.forward * 0.8f + cameraTransform.right * 0.25f + cameraTransform.up * -0.5f;
            float pulse = Mathf.Abs(Mathf.Sin(Time.time * 2f)) * 0.5f + 0.5f; // Pulsing effect
            buildModeIndicator.transform.localScale = new Vector3(0.09f, 0.09f, 0.09f) * pulse;
        }
    }

    // Helper method to update the stored initial positions for each spawned point
    private void UpdateInitialPositions()
    {
        initialPositions.Clear();
        foreach (GameObject point in spawnedPoints)
        {
            initialPositions.Add(point.transform.position);
        }
    }

    private void SpawnObject()
    {
        Transform centerEye = GameObject.Find("CenterEyeAnchor")?.transform;
        if (centerEye == null || objectPrefab == null)
        {
            Debug.LogWarning("Right hand controller or object prefab not found");
            return;
        }

        // Check point limit again (safety check)
        if (pointCounter > MAX_POINTS)
        {
            Debug.Log($"Maximum number of points reached ({MAX_POINTS}). Cannot spawn more.");
            return;
        }

        Vector3 spawnPosition = centerEye.position + centerEye.forward * spawnDistance;
        // Ensure no overlap by checking minimum distance
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f);
        while (colliders.Length > 0)
        {
            spawnPosition += new Vector3(centerEye.forward.x, 0, centerEye.forward.z).normalized * 0.1f;
            colliders = Physics.OverlapSphere(spawnPosition, 0.1f);
        }

        // First instantiate the object
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, centerEye.rotation);
        newObject.SetActive(true);
        // Track spawned object
        spawnedPoints.Add(newObject);
        // Update text label with point number
        Transform nameChild = newObject.transform.Find("Name");
        if (nameChild != null)
        {
            TextMeshProUGUI textComponent = nameChild.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = $"Point {pointCounter}";
                pointCounter++; // Increment the counter after assigning the number
            }
        }
        Debug.Log($"Object spawned at: {spawnPosition} with Point #{pointCounter - 1} | {spawnedPoints.Count}/{MAX_POINTS}");
        Debug.Log($"Center Eye Position: {centerEye.position}");
    }

    private void GenerateWall()
    {
        // 1. Get point positions
        Vector3[] pointPositions = spawnedPoints.Select(p => p.transform.position).ToArray();
        // 2. Find best fit vertical plane
        (Vector3 planeNormal, Vector3 planePoint) = FindBestFitVerticalPlane(pointPositions);
        // 3. Project points onto the plane and sort them in clockwise order
        Vector3[] projectedPoints = ProjectPointsOntoPlane(pointPositions, planeNormal, planePoint);
        Vector3[] sortedPoints = SortPointsClockwise(projectedPoints, planeNormal);
        // 4. Update the positions of the spawned points to align with the new plane
        for (int i = 0; i < spawnedPoints.Count; i++)
        {
            spawnedPoints[i].transform.position = projectedPoints[i];
        }

        // 5. Generate wall mesh
        CreateWallMesh(sortedPoints, planeNormal);
        Debug.Log("Wall generated successfully!");
    }

    public void ApplyMaterial(Material newMaterial)
    {
        if (generatedWall != null && newMaterial != null)
        {
            generatedWall.GetComponent<Renderer>().material = newMaterial;
        }
    }
    // Computes a best-fit vertical plane from the provided points.
    private (Vector3 normal, Vector3 point) FindBestFitVerticalPlane(Vector3[] points)
    {
        // Compute centroid.
        Vector3 centroid = Vector3.zero;
        foreach (Vector3 point in points)
        {
            centroid += point;
        }
        centroid /= points.Length;

        // Only consider x and z for best-fit vertical plane.
        float sumXX = 0, sumXZ = 0, sumZZ = 0;
        foreach (Vector3 point in points)
        {
            Vector3 centered = point - centroid;
            sumXX += centered.x * centered.x;
            sumXZ += centered.x * centered.z;
            sumZZ += centered.z * centered.z;
        }

        // Direction of least variance using PCA in the XZ plane.
        float angle = 0.5f * Mathf.Atan2(2 * sumXZ, sumXX - sumZZ);
        // The best-fit normal in the XZ plane is perpendicular to that direction.
        Vector3 normal = new Vector3(Mathf.Cos(angle + Mathf.PI / 2), 0, Mathf.Sin(angle + Mathf.PI / 2));
        normal.Normalize();
        return (normal, centroid);
    }

    // Projects the points onto the plane defined by planeNormal and planePoint.
    private Vector3[] ProjectPointsOntoPlane(Vector3[] points, Vector3 planeNormal, Vector3 planePoint)
    {
        Vector3[] projectedPoints = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 v = points[i] - planePoint;
            float distance = Vector3.Dot(v, planeNormal);
            projectedPoints[i] = points[i] - distance * planeNormal;
        }
        return projectedPoints;
    }

    // Sorts the points clockwise around their centroid using the provided normal as the axis.
    private Vector3[] SortPointsClockwise(Vector3[] points, Vector3 normal)
    {
        // Compute the center of the points.
        Vector3 center = Vector3.zero;
        foreach (Vector3 point in points)
        {
            center += point;
        }
        center /= points.Length;

        // Define a reference vector on the plane. Choose one that's not nearly parallel to the normal.
        Vector3 refVector = Vector3.right;
        if (Mathf.Abs(Vector3.Dot(normal, refVector)) > 0.9f)
            refVector = Vector3.forward;

        // Find the tangent and bitangent vectors.
        Vector3 tangent = Vector3.Cross(normal, Vector3.Cross(refVector, normal)).normalized;
        Vector3 bitangent = Vector3.Cross(normal, tangent);

        // Sort the points by angle relative to the tangent-bitangent axes.
        return points.OrderBy(point =>
        {
            Vector3 dir = point - center;
            float angle = Mathf.Atan2(Vector3.Dot(dir, bitangent), Vector3.Dot(dir, tangent));
            return angle;
        }).ToArray();
    }

    // Triangulates a polygon using an ear clipping algorithm.
    private void TriangulateFrontFace(Vector3[] polyPoints, Vector3 normal, List<int> triangles)
    {
        // Create a list of vertex indices.
        List<int> indices = new List<int>();
        for (int i = 0; i < polyPoints.Length; i++)
        {
            indices.Add(i);
        }

        // Continue clipping ears until only one triangle remains.
        while (indices.Count > 3)
        {
            bool earFound = false;
            for (int i = 0; i < indices.Count; i++)
            {
                int prevIndex = indices[(i - 1 + indices.Count) % indices.Count];
                int currIndex = indices[i];
                int nextIndex = indices[(i + 1) % indices.Count];
                Vector3 a = polyPoints[prevIndex];
                Vector3 b = polyPoints[currIndex];
                Vector3 c = polyPoints[nextIndex];

                // Ensure the angle at b is convex.
                Vector3 ab = b - a;
                Vector3 bc = c - b;
                Vector3 cross = Vector3.Cross(ab, bc);
                if (Vector3.Dot(cross, normal) < 0)
                    continue; // Concave, not an ear.

                // Check if any other point lies inside the triangle.
                bool isEar = true;
                for (int j = 0; j < indices.Count; j++)
                {
                    if (j == (i - 1 + indices.Count) % indices.Count || j == i || j == (i + 1) % indices.Count)
                        continue;
                    int testIndex = indices[j];
                    if (PointInTriangle(polyPoints[testIndex], a, b, c))
                    {
                        isEar = false;
                        break;
                    }
                }

                if (isEar)
                {
                    // Append ear triangle.
                    triangles.Add(prevIndex);
                    triangles.Add(currIndex);
                    triangles.Add(nextIndex);
                    // Remove the ear tip.
                    indices.RemoveAt(i);
                    earFound = true;
                    break;
                }
            }

            if (!earFound)
            {
                // Fallback: if ear clipping fails, use a triangle fan.
                for (int i = 1; i < indices.Count - 1; i++)
                {
                    triangles.Add(indices[0]);
                    triangles.Add(indices[i]);
                    triangles.Add(indices[i + 1]);
                }
                return;
            }
        }

        // Add the last remaining triangle.
        if (indices.Count == 3)
        {
            triangles.Add(indices[0]);
            triangles.Add(indices[1]);
            triangles.Add(indices[2]);
        }
    }

    // Helper method: Determines if point p is inside the triangle defined by a, b, c.
    private bool PointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        // Compute vectors.
        Vector3 v0 = c - a;
        Vector3 v1 = b - a;
        Vector3 v2 = p - a;

        // Compute dot products.
        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        // Compute barycentric coordinates.
        float invDenom = 1f / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        // Check if point is in triangle.
        return (u >= 0) && (v >= 0) && (u + v < 1);
    }

    private void CreateWallMesh(Vector3[] originalPoints, Vector3 extrudeNormal)
    {
        // Step 1: Find the best-fit vertical plane.
        (Vector3 planeNormal, Vector3 planeCenter) = FindBestFitVerticalPlane(originalPoints);

        // Step 2: Project the original points onto this plane.
        Vector3[] projectedPoints = ProjectPointsOntoPlane(originalPoints, planeNormal, planeCenter);

        // Step 3: Sort the projected points clockwise so they define a proper polygon.
        Vector3[] sortedPoints = SortPointsClockwise(projectedPoints, planeNormal);

        // Create a new game object for the wall.
        if (generatedWall != null)
        {
            Destroy(generatedWall);
        }
        generatedWall = new GameObject("GeneratedWall");

        // Add mesh components.
        MeshFilter meshFilter = generatedWall.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = generatedWall.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = generatedWall.AddComponent<MeshCollider>();

        // Create a new mesh.
        Mesh mesh = new Mesh();

        // Define wall thickness - increased for better visibility
        float thickness = 0.15f;

        // Generate vertices for front and back faces.
        int vertexCount = sortedPoints.Length * 2;
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];
        Vector3[] normals = new Vector3[vertexCount]; // Add explicit normals

        // Front face vertices.
        for (int i = 0; i < sortedPoints.Length; i++)
        {
            vertices[i] = sortedPoints[i];
            normals[i] = planeNormal; // Set front face normals
        }

        // Back face vertices: offset by the extrusion normal.
        for (int i = 0; i < sortedPoints.Length; i++)
        {
            vertices[i + sortedPoints.Length] = sortedPoints[i] - extrudeNormal * thickness;
            normals[i + sortedPoints.Length] = -planeNormal; // Set back face normals (opposite direction)
        }

        // UV calculation (projected onto XZ for simplicity).
        Vector3 min = vertices[0];
        Vector3 max = vertices[0];
        foreach (Vector3 v in vertices)
        {
            min = Vector3.Min(min, v);
            max = Vector3.Max(max, v);
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            float u = Mathf.InverseLerp(min.x, max.x, vertices[i].x);
            float vCoord = Mathf.InverseLerp(min.z, max.z, vertices[i].z);
            uv[i] = new Vector2(u, vCoord);
        }

        // Create triangles list.
        List<int> triangles = new List<int>();

        // Use the improved brute force triangulation for the front face
        TriangulateWithBruteForce(sortedPoints, planeNormal, triangles);

        // Triangulate the back face with reversed winding order.
        List<int> backTriangles = new List<int>();
        TriangulateWithBruteForce(sortedPoints, -planeNormal, backTriangles);
        for (int i = 0; i < backTriangles.Count; i += 3)
        {
            // Reverse winding order and offset indices.
            triangles.Add(backTriangles[i] + sortedPoints.Length);
            triangles.Add(backTriangles[i + 2] + sortedPoints.Length);
            triangles.Add(backTriangles[i + 1] + sortedPoints.Length);
        }

        // Build side faces that connect front and back.
        for (int i = 0; i < sortedPoints.Length; i++)
        {
            int nextI = (i + 1) % sortedPoints.Length;

            // Calculate the side face normal
            Vector3 sideNormal = Vector3.Cross(vertices[nextI] - vertices[i], vertices[i + sortedPoints.Length] - vertices[i]).normalized;

            // First triangle for the side quad.
            triangles.Add(i);
            triangles.Add(i + sortedPoints.Length);
            triangles.Add(nextI);

            // Second triangle for the side quad.
            triangles.Add(nextI);
            triangles.Add(i + sortedPoints.Length);
            triangles.Add(nextI + sortedPoints.Length);
        }

        // Assign mesh data.
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv;
        mesh.normals = normals; // Assign explicit normals
        mesh.RecalculateBounds();

        // No need to call RecalculateNormals() since we're setting them explicitly

        // Assign the mesh to components.
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        if (wallMaterial != null)
        {
            meshRenderer.material = wallMaterial;
            // Make sure material renders both sides
            meshRenderer.material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        }
        else
        {
            Material defaultMaterial = new Material(Shader.Find("Standard"));
            defaultMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            meshRenderer.material = defaultMaterial;
            Debug.LogWarning("Wall material not assigned! Using default material.");
        }
    }


    private void TriangulateWithBruteForce(Vector3[] points, Vector3 normal, List<int> triangles)
    {
        int n = points.Length;
        if (n < 3) return;

        // Step 1: Define main edges (the polygon boundary)
        List<(int, int)> mainEdges = new List<(int, int)>();
        for (int i = 0; i < n; i++)
        {
            mainEdges.Add((i, (i + 1) % n));
        }

        // Step 2 & 3: Create triangles by connecting each vertex to previous vertices
        for (int i = 2; i < n; i++) // Start from the third vertex
        {
            for (int j = 0; j < i - 1; j++) // Connect to all vertices except the adjacent one
            {
                // Skip adjacent vertex (already connected by main edge)
                if (j == i - 1) continue;

                // Check if this potential interior edge is valid
                if (IsValidInteriorEdge(points, i, j, mainEdges, normal))
                {
                    // Add triangle (i, i-1, j)
                    triangles.Add(i);
                    triangles.Add(i - 1);
                    triangles.Add(j);
                }
            }
        }

        // If no triangles were created, fall back to ear clipping
        if (triangles.Count == 0)
        {
            Debug.LogWarning("Brute force triangulation failed. Falling back to ear clipping.");
            TriangulateFrontFace(points, normal, triangles);
        }
    }

    private bool IsValidInteriorEdge(Vector3[] points, int i, int j, List<(int, int)> mainEdges, Vector3 normal)
    {
        // Rule 3.1: Check if the interior edge intersects any main edge
        for (int k = 0; k < mainEdges.Count; k++)
        {
            var edge = mainEdges[k];

            // Skip if the main edge shares a vertex with our interior edge
            if (edge.Item1 == i || edge.Item2 == i || edge.Item1 == j || edge.Item2 == j)
                continue;

            // Check for intersection
            if (EdgesIntersect(points[i], points[j], points[edge.Item1], points[edge.Item2]))
                return false;
        }

        // Rule 3.2: Check if the edge goes through exterior space
        // We can check if the midpoint of the edge is inside the polygon
        Vector3 midpoint = (points[i] + points[j]) / 2f;
        if (!PointInPolygon(midpoint, points, normal))
            return false;

        // Additional check: Ensure the triangle has correct winding order
        Vector3 a = points[i];
        Vector3 b = points[i - 1]; // Adjacent vertex to i
        Vector3 c = points[j];

        Vector3 triangleNormal = Vector3.Cross(b - a, c - a).normalized;
        return Vector3.Dot(triangleNormal, normal) > 0;
    }

    // Check if two edges intersect (in 2D, projected onto the plane)
    private bool EdgesIntersect(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        // Project to 2D (using X and Z coordinates for a vertical plane)
        Vector2 a2d = new Vector2(a.x, a.z);
        Vector2 b2d = new Vector2(b.x, b.z);
        Vector2 c2d = new Vector2(c.x, c.z);
        Vector2 d2d = new Vector2(d.x, d.z);

        return LineSegmentsIntersect(a2d, b2d, c2d, d2d);
    }

    // Check if two line segments intersect in 2D
    private bool LineSegmentsIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        // Calculate the direction vectors
        Vector2 ab = b - a;
        Vector2 cd = d - c;

        // Calculate the determinant
        float det = ab.x * cd.y - ab.y * cd.x;

        // If determinant is zero, lines are parallel
        if (Mathf.Approximately(det, 0f))
            return false;

        // Calculate the parameters
        Vector2 ac = c - a;
        float t = (ac.x * cd.y - ac.y * cd.x) / det;
        float u = (ac.x * ab.y - ac.y * ab.x) / det;

        // Check if the intersection point is within both line segments
        return t >= 0f && t <= 1f && u >= 0f && u <= 1f;
    }

    // Check if a point is inside a polygon
    private bool PointInPolygon(Vector3 point, Vector3[] polygon, Vector3 normal)
    {
        // Create a coordinate system on the plane
        Vector3 refVector = Vector3.right;
        if (Mathf.Abs(Vector3.Dot(normal, refVector)) > 0.9f)
            refVector = Vector3.forward;

        Vector3 tangent = Vector3.Cross(normal, Vector3.Cross(refVector, normal)).normalized;
        Vector3 bitangent = Vector3.Cross(normal, tangent);

        // Project the point and polygon onto the plane
        Vector2 point2D = new Vector2(Vector3.Dot(point, tangent), Vector3.Dot(point, bitangent));
        Vector2[] polygon2D = new Vector2[polygon.Length];

        for (int i = 0; i < polygon.Length; i++)
        {
            polygon2D[i] = new Vector2(
                Vector3.Dot(polygon[i], tangent),
                Vector3.Dot(polygon[i], bitangent)
            );
        }

        // Ray casting algorithm to determine if point is inside polygon
        bool inside = false;
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if (((polygon2D[i].y > point2D.y) != (polygon2D[j].y > point2D.y)) &&
                (point2D.x < (polygon2D[j].x - polygon2D[i].x) * (point2D.y - polygon2D[i].y) /
                (polygon2D[j].y - polygon2D[i].y) + polygon2D[i].x))
            {
                inside = !inside;
            }
        }

        return inside;
    }

    private void ClearAllPointsAndWall()
    {
        // Destroy all spawned points
        foreach (GameObject point in spawnedPoints)
        {
            Destroy(point);
        }

        spawnedPoints.Clear();
        // Reset point counter
        pointCounter = 1;
        // Destroy the generated wall if it exists
        if (generatedWall != null)
        {
            Destroy(generatedWall);
            generatedWall = null;
        }

        Debug.Log("All points and wall cleared.");
    }
}
