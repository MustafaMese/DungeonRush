using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DungeonRush.Field;
using DungeonRush.Cards;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    Grid grid;
    bool showTiles;

    bool decorateMode = false;
    bool configureMode = false;

    Transform boardTransform;

    GameObject selectedObject;

    private void OnEnable() 
    {
        grid = (Grid)target;
        boardTransform = grid.transform;
        SceneView.duringSceneGui += GridUpdate;
    }

    private void OnDisable() 
    {
        SceneView.duringSceneGui -= GridUpdate;
    }

    private void GridUpdate(SceneView sceneView)
    {
        Event e = Event.current;

        Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
        Vector3 mousePos = r.origin;
        Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x + 0.5f),
                            Mathf.Floor(mousePos.y + 0.5f));

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if(configureMode)
                UpdateTiles(aligned);
            else if(decorateMode && selectedObject != null)
                CreateGameObject(aligned, grid.objectList, grid.tileList, selectedObject);
        }
        else if(Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            if(configureMode)
            {
                var isContain = GridStructure<Tile>.Contains(grid.tileList, aligned);
                if(isContain)
                {
                    DeleteObject(aligned, grid.tileList);
                    isContain = GridStructure<Card>.Contains(grid.objectList, aligned);
                    if(isContain)
                        DeleteObject(aligned, grid.objectList);
                }
            }
            else if(decorateMode)
            {
                var isContain = GridStructure<Card>.Contains(grid.objectList, aligned);
                if (isContain)
                    DeleteObject(aligned, grid.objectList);
            }
        }
        else if(e.isKey && e.character == 'd')
        {
            if(configureMode)
            {
                for (int i = 0; i < grid.tileList.Count; i++)
                {
                    GridStructure<Tile> item = grid.tileList[i];
                    DeleteObject(item.position, grid.tileList);
                }
            }
            else if (decorateMode)
            {
                for (int i = 0; i < grid.tileList.Count; i++)
                {
                    GridStructure<Card> item = grid.objectList[i];
                    DeleteObject(item.position, grid.objectList);
                }
            }
        }
        else if(e.isKey && e.character == 'f')
        {
            if(configureMode)
            {
                for (var i = 0; i < grid.tileList.Count; i++)
                    grid.tileList.Remove(grid.tileList[i]);
            }
        }

    }

    private void UpdateTiles(Vector3 aligned)
    {
        CreateTile(aligned, grid.tileList, grid.tilePrefab);

        CreateTile(aligned, new Vector3(0, 1, 0), grid.tileList, grid.topWallPrefab, new List<TileType>() { TileType.TOPLEFT_WALL, TileType.TOPRIGHT_WALL});
        CreateTile(aligned, new Vector3(0, -1, 0), grid.tileList, grid.downWallPrefab, new List<TileType>() { TileType.DOWNLEFT_WALL, TileType.DOWNRIGHT_WALL});
        CreateTile(aligned, new Vector3(1, 0, 0), grid.tileList, grid.rightWallPrefab, new List<TileType>() { TileType.TOPRIGHT_WALL, TileType.DOWNRIGHT_WALL});
        CreateTile(aligned, new Vector3(-1, 0, 0), grid.tileList, grid.leftWallPrefab, new List<TileType>() { TileType.TOPLEFT_WALL, TileType.DOWNLEFT_WALL});

        CreateTile(aligned, new Vector3(-1, 1, 0), grid.tileList, grid.topLeftWallPrefab, grid.downRightOppositeWallPrefab);
        CreateTile(aligned, new Vector3(1, 1, 0), grid.tileList, grid.topRightWallPrefab, grid.downLeftOppositeWallPrefab);
        CreateTile(aligned, new Vector3(1, -1, 0), grid.tileList, grid.downRightWallPrefab, grid.topLeftOppositeWallPrefab);
        CreateTile(aligned, new Vector3(-1, -1, 0), grid.tileList, grid.downLeftWallPrefab, grid.topRightOppositeWallPrefab);
    }

    private void CreateTile(Vector3 aligned, Vector3 direction, List<GridStructure<Tile>> list, GameObject cornerPrefab, GameObject cornerPrefabOppositeVersion)
    {
        Vector3 pos = new Vector3(aligned.x + direction.x, aligned.y + direction.y, 0f);
        if(!GridStructure<Tile>.Contains(list, pos))
            CreatePrefab(pos, cornerPrefab, list);

        Vector3 posOppositeDirectionHorizontal = new Vector3(pos.x - direction.x, pos.y, 0f);
        Vector3 posOppositeDirectionVertical = new Vector3(pos.x, pos.y - direction.y, 0f);

        Tile tile1 = GridStructure<Tile>.GetObject(list, posOppositeDirectionHorizontal);
        Tile tile2 = GridStructure<Tile>.GetObject(list, posOppositeDirectionVertical);

        if(GridStructure<Tile>.Contains(list, posOppositeDirectionHorizontal) && GridStructure<Tile>.Contains(list, posOppositeDirectionVertical)
            && (tile1 != null && tile1.tileType == TileType.TILE) && (tile2 != null && tile2.tileType == TileType.TILE))
        {
            Tile tile = GridStructure<Tile>.GetObject(list, pos);
            if(tile.tileType != TileType.TILE)
            {
                DeleteObject(pos, list);
                CreatePrefab(pos, cornerPrefabOppositeVersion, list);
            }
        }
    }

    private void CreateTile(Vector3 aligned, Vector3 direction, List<GridStructure<Tile>> list, GameObject prefab, List<TileType> excludeTypes)
    {
        Vector3 pos = new Vector3(aligned.x + direction.x, aligned.y + direction.y, 0f);
        bool isContain = GridStructure<Tile>.Contains(list, pos);
        if(!isContain)
            CreatePrefab(pos, prefab, list);
        else
        {
            Tile tile = GridStructure<Tile>.GetObject(list, pos);
            if(tile.tileType != TileType.TILE && (tile.tileType == excludeTypes[0] || tile.tileType == excludeTypes[1]))
            {
                DeleteObject(pos, list);
                CreatePrefab(pos, prefab, list);
            }
        }
    }

    private void CreateTile(Vector3 aligned, List<GridStructure<Tile>> list, GameObject prefab)
    {
        var isContain = GridStructure<Tile>.Contains(list, aligned);
        if (!isContain)
            CreatePrefab(aligned, prefab, list);
        else
        {
            Tile tile = GridStructure<Tile>.GetObject(list, aligned);
            if (tile.tileType != TileType.TILE)
            {
                DeleteObject(aligned, list);
                CreatePrefab(aligned, prefab, list);
            }
        }
    }

    private void CreateGameObject(Vector3 aligned, List<GridStructure<Card>> objectList, List<GridStructure<Tile>> tileList, GameObject prefab)
    {
        var isContain = GridStructure<Tile>.Contains(tileList, aligned);
        if(isContain)
        {
            Tile tile = GridStructure<Tile>.GetObject(tileList, aligned);
            if(tile.tileType == TileType.TILE)
            {
                isContain = GridStructure<Card>.Contains(objectList, aligned);
                if (!isContain)
                    CreatePrefab(aligned, prefab, objectList, false);
                else
                {
                    DeleteObject(aligned, objectList);
                    CreatePrefab(aligned, prefab, objectList, false);
                }
            }
        }
    }

    private void DeleteObject<T>(Vector2 pos, List<GridStructure<T>> list) where T : MonoBehaviour 
    {
        GridStructure<T> gStructure = GridStructure<T>.GetStructure(list, pos);

        if(gStructure == null) return;

        list.Remove(gStructure);
        if (Application.isEditor)
            Object.DestroyImmediate(gStructure.obj.gameObject);
        else
            Object.Destroy(gStructure.obj.gameObject);
    }

    private void AddObjectsToList<T>(List<GridStructure<T>> list) where T : MonoBehaviour
    {
        List<T> objects = new List<T>(FindObjectsOfType<T>());
        foreach (var obj in objects)
        {
            Vector3 pos = obj.transform.position;
            list.Add(new GridStructure<T>(obj, pos));
        }
    }

    private void CreatePrefab<T>(Vector3 pos, GameObject prefab, List<GridStructure<T>> list, bool hasParent = true) where T : MonoBehaviour
    {
        Undo.IncrementCurrentGroup();
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        list.Add(new GridStructure<T>(obj.GetComponent<T>(), pos));
        obj.transform.position = pos;
        if(hasParent)
            obj.transform.SetParent(boardTransform);
        Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        grid.showStartPoint = EditorGUILayout.Toggle(" Show Start Point ", grid.showStartPoint);
        GUILayout.EndHorizontal();

        if(!decorateMode)
        {
            GUILayout.BeginHorizontal();
            configureMode = EditorGUILayout.Toggle(" Configure Mode ", configureMode);
            GUILayout.EndHorizontal();
        }

        if (!configureMode)
        {
            GUILayout.BeginHorizontal();
            decorateMode = EditorGUILayout.Toggle(" Decorate Mode ", decorateMode);
            GUILayout.EndHorizontal();
        }

        if(configureMode)
        {
            

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            FloatField(" Grid Width ", grid.width, 50);
            FloatField(" Grid Height ", grid.height, 50);
            GUILayout.EndHorizontal();

            Vector3Field(" Offset ", grid.offset, 200);

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tilePrefab"), new GUIContent(" Tile Prefab "));
            LabelField(" Wall Prefabs ");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("topWallPrefab"), new GUIContent(" Top Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("downWallPrefab"), new GUIContent(" Down Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rightWallPrefab"), new GUIContent(" Right Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("leftWallPrefab"), new GUIContent(" Left Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("topLeftWallPrefab"), new GUIContent(" Top Left Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("topRightWallPrefab"), new GUIContent(" Top Right Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("downLeftWallPrefab"), new GUIContent(" Down Left Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("downRightWallPrefab"), new GUIContent(" Down Right Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("topRightOppositeWallPrefab"), new GUIContent(" Top Right Opposite Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("downRightOppositeWallPrefab"), new GUIContent(" Down Right Opposite Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("topLeftOppositeWallPrefab"), new GUIContent(" Top Left Opposite Wall Prefab "));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("downLeftOppositeWallPrefab"), new GUIContent(" Down Left Opposite Wall Prefab "));
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            if(grid.tileList.Count < 1)
                GetObjectsButton<Tile>(" Get Tiles ", grid.tileList);
            else
            {
                showTiles = EditorGUILayout.BeginFoldoutHeaderGroup(showTiles, new GUIContent(" List of tiles."));

                if (showTiles)
                {
                    GUILayout.BeginVertical();
                    for (var i = 0; i < grid.tileList.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(" Tile ");
                        var position = EditorGUILayout.Vector2Field("", grid.tileList[i].position, GUILayout.Width(200));
                        var obj = EditorGUILayout.ObjectField(grid.tileList[i].obj, typeof(GameObject), true, GUILayout.Width(200)) as GameObject;
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(" List Count ");
                    var count = EditorGUILayout.IntField(grid.tileList.Count, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }
            }
        }

        if (decorateMode)
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fileNames"));
            serializedObject.ApplyModifiedProperties();

            ObjectField(" Selected Object ", selectedObject);
            ListObjectFiles();

            if(grid.objectList.Count < 1)
                GetObjectsButton<Card>(" Get Objects ", grid.objectList);
            else 
            {
                showTiles = EditorGUILayout.BeginFoldoutHeaderGroup(showTiles, new GUIContent(" List of cards."));

                if (showTiles)
                {
                    GUILayout.BeginVertical();
                    for (var i = 0; i < grid.objectList.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(" Card ");
                        var position = EditorGUILayout.Vector2Field("", grid.objectList[i].position, GUILayout.Width(200));
                        var obj = EditorGUILayout.ObjectField(grid.objectList[i].obj, typeof(GameObject), true, GUILayout.Width(200)) as GameObject;
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(" List Count ");
                    var count = EditorGUILayout.IntField(grid.objectList.Count, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }
            }
        }

        SceneView.RepaintAll();
    }

    private void GetObjectsButton<T>(string label, List<GridStructure<T>> list) where T : MonoBehaviour
    {
        GUILayout.BeginHorizontal();
        var show = GUILayout.Button(label, GUILayout.Height(20), GUILayout.Width(80));
        if (show)
            AddObjectsToList<T>(list);
        GUILayout.EndHorizontal();
    }

    private void ListObjectFiles()
    {
        EditorGUILayout.BeginVertical();
        List<string> names = new List<string>(grid.fileNames);

        for (var i = 0; i < names.Count; i++)
        {
            string path = "Assets/Prefabs/" + names[i];

            LabelField(" " + names[i]);
            var assets = AssetDatabase.FindAssets("t:Object", new[] { path });

            EditorGUILayout.BeginHorizontal();
            foreach (var guid in assets)
            {
                var clip = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid));
                // bool isSelected = GUILayout.Button(new GUIContent(AssetPreview.GetAssetPreview(clip), clip.name), GUILayout.Height(40), GUILayout.Width(40));
                // if (isSelected)
                //     selectedObject = (GameObject)clip;
                if (clip.GetType() == typeof(GameObject))
                {
                    bool isSelected = GUILayout.Button(new GUIContent(AssetPreview.GetAssetPreview(clip), clip.name), GUILayout.Height(40), GUILayout.Width(40));
                    if (isSelected)
                        selectedObject = (GameObject)clip;
                }
                else
                    continue;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    private int IntField(string label ,int value)
    {
        GUILayout.BeginHorizontal();
        value = EditorGUILayout.IntField(label, value, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        return value;
    }

    private void Vector3Field(string label, Vector3 variable, float width = 0, float height = 0)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label);
        variable = EditorGUILayout.Vector3Field("", variable, GUILayout.Width(width));
        GUILayout.EndHorizontal();
    }

    private void FloatField(string label, float variable, float width = 0, float height = 0)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(" Grid Height ");
        grid.height = EditorGUILayout.FloatField(grid.height, GUILayout.Width(width));
        GUILayout.EndHorizontal();
    }

    private void LabelField(string label)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
    }

    private void ObjectField(string label, GameObject prefab)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label);
        prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), true, GUILayout.Width(200)) as GameObject;
        GUILayout.EndHorizontal();
    }

}