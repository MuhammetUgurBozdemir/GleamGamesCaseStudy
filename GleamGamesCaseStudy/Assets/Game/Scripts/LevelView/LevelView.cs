using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class LevelView : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    public int LevelIndex => levelIndex;

    [SerializeField, FoldoutGroup("GridSettings")]
    private GridView gridViewPrefab;

    [SerializeField, FoldoutGroup("GridSettings")]
    private int gridWidth;

    [SerializeField, FoldoutGroup("GridSettings")]
    private int gridHeight;


    [SerializeField, FoldoutGroup("GridSettings")]
    private float cellWidth = 2.0f;

    [SerializeField, FoldoutGroup("GridSettings")]
    private float cellHeight = 1.4f;

    private readonly Vector2 startPosition = Vector2.zero;


    [SerializeField] private List<GridView> gridViews;


    #region Injection

    private LevelController levelController;
    private Camera camera;
    DiContainer container;

    [Inject]
    private void Construct(LevelController _levelController
        , [Inject(Id = "mainCam")] Camera _camera,
        DiContainer _container)
    {
        levelController = _levelController;
        camera = _camera;
        container = _container;
    }

    #endregion


    public void Init()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2 position = new Vector2(startPosition.x + (x * cellWidth), startPosition.y - (y * cellHeight));
                var gridView = container.InstantiatePrefabForComponent<GridView>(gridViewPrefab);

                Transform gridViewTransform = gridView.transform;

                gridView.transform.SetParent(transform);
                gridViewTransform.localPosition = position;

                gridViews.Add(gridView);
            }
        }
        
        CenterCamera();
    }
    
    void CenterCamera()
    {
        float gridWidth = this.gridWidth * cellWidth;
        float gridHeight = this.gridHeight * cellHeight;
        
        Vector2 gridCenter = new Vector2(startPosition.x + (gridWidth - cellWidth) / 2, startPosition.y - (gridHeight - cellHeight) / 2);
        
        camera.transform.position = new Vector3(gridCenter.x, gridCenter.y, this.gridWidth*-4);
        
        float average = Mathf.Max(gridWidth / 2f, gridHeight / 2f);
        camera.orthographicSize = average * 0.8f; // 0.8 ile biraz içeri çekebiliriz
    }


    public void Dispose()
    {
        Destroy(gameObject);
    }
}