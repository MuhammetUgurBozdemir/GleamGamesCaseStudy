using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
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
        CenterCamera();

        foreach (var gridView in gridViews)
        {
            gridView.Init();
        }
    }

#if UNITY_EDITOR
    [Button]
    [PropertyOrder(-1)]
    public void BuildLevel()
    {
        foreach (var grid in gridViews)
        {
            DestroyImmediate(grid.gameObject);
        }

        gridViews.Clear();

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                var obj = PrefabUtility.InstantiatePrefab(gridViewPrefab, transform) as GridView;
                if (obj == null) continue;
                Vector2 position = new Vector2(startPosition.x + (x * cellWidth), startPosition.y - (y * cellHeight));

                obj.transform.position = new Vector3(position.x, position.y, 0);
                obj.transform.SetParent(transform);

                gridViews.Add(obj);
                obj.transform.name = $"Grid_{x}_{y}";
            }
        }
    }
#endif


    void CenterCamera()
    {
        float width = gridWidth * cellWidth;
        float height = gridHeight * cellHeight;

        Vector2 gridCenter = new Vector2(startPosition.x + (width - cellWidth) / 2, startPosition.y - (height - cellHeight) / 2);

        camera.transform.position = new Vector3(gridCenter.x, gridCenter.y, gridWidth * -4);
    }


    public void Dispose()
    {
        Destroy(gameObject);
    }
}