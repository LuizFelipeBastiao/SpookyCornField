using UnityEngine;
using System.Collections.Generic;

public class AutoChunkLoader : MonoBehaviour
{
    public Transform player;
    public float chunkSize = 10f;
    public int radius = 4;              // células ao redor do jogador
    public bool onlyToggleRenderers = true; // true: mantém colliders/NavMesh ativos

    class Chunk
    {
        public GameObject go;
        public Renderer[] renderers;
        public bool visible = true;
    }

    Dictionary<Vector2Int, Chunk> chunks = new();
    HashSet<Vector2Int> active = new();
    Vector2Int lastCell = new(int.MinValue, int.MinValue);

    Vector2Int ToCell(Vector3 p) =>
        new(Mathf.FloorToInt(p.x / chunkSize), Mathf.FloorToInt(p.z / chunkSize));

    void Awake()
    {
        // 1) copia a lista de filhos antes de reparentar
        var pieces = new List<Transform>();
        foreach (Transform t in transform) pieces.Add(t);

        // 2) agrupa cada peça no chunk da sua célula
        var groups = new Dictionary<Vector2Int, GameObject>();
        foreach (var p in pieces)
        {
            var cell = ToCell(p.position);
            if (!groups.TryGetValue(cell, out var parent))
            {
                parent = new GameObject($"Chunk_{cell.x}_{cell.y}");
                parent.transform.SetParent(transform, false);
                groups[cell] = parent;
            }
            p.SetParent(parent.transform, true); // mantém posição no mundo
        }

        // 3) registra os chunks
        foreach (var kv in groups)
        {
            chunks[kv.Key] = new Chunk
            {
                go = kv.Value,
                renderers = kv.Value.GetComponentsInChildren<Renderer>()
            };
        }

        foreach (var c in chunks.Values) SetChunk(c, false);
    }

    void Update()
    {
        var cell = ToCell(player.position);
        if (cell == lastCell) return;
        lastCell = cell;

        var wanted = new HashSet<Vector2Int>();
        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
                wanted.Add(cell + new Vector2Int(x, y));

        foreach (var c in active)
            if (!wanted.Contains(c) && chunks.TryGetValue(c, out var ch)) SetChunk(ch, false);
        foreach (var c in wanted)
            if (chunks.TryGetValue(c, out var ch)) SetChunk(ch, true);

        active = wanted;
    }

    void SetChunk(Chunk c, bool on)
    {
        if (c.visible == on) return;
        c.visible = on;

        if (onlyToggleRenderers)
            foreach (var r in c.renderers) r.enabled = on;
        else
            c.go.SetActive(on);
    }
}