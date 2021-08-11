using System.Collections.Generic;
using UnityEngine;

public class LiquidMeshActor : MonoBehaviour
{
    #region Enums

    public enum ShowSide
    {
        BothSide,
        FrontSide,
        BackSide
    }
    public enum WaveEffect
    {
        Whole,
        TopHalf,
        BottomHalf
    }

    #endregion Enums

    #region Properties
    public bool isWaveActive;

    [SerializeField, Range(.0f, 100.0f)]
    private float maxYScale = 1.0f;
    [SerializeField, Range(.0f, 1.0f)]
    private float gap = .0f;
    [SerializeField]
    private ShowSide showSide;
    [SerializeField]
    private bool calculateEveryFrame = false;

    [Header("Wave Setup")]
    [SerializeField]
    private bool playOnAwake = false;
    [Range(.0f, 10.0f)]
    public float waveHeight = 1.0f;
    [SerializeField, Range(.0f, 10.0f)]
    private float waveSpeed = 1.0f;
    [SerializeField, Range(.0f, .1f)]
    private float waveSmoothness = .05f;
    [SerializeField, Range(.0f, .1f)]
    private float dropWaveHeight = .08f;
    [SerializeField]
    private WaveEffect waveEffect = WaveEffect.TopHalf;
    [SerializeField, Range(.0f, 2.0f)]
    private float fadingSpeed = .5f;

    #endregion Properties

    #region Fields

    private float distance = .0f;
    private Mesh mesh;
    private Vector3[] initVertices;
    private Vector3[] waveVertices;
    private Vector3[] vertices;
    private ShowSide latestShowState;
    private float initWaveHeight;
    private int minYIndex;
    private int maxYIndex;
    private float meshHeight;
    private float initYScale;
    private Material material;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    public float MaxYScale { get { return maxYScale; } }
    public float Height    { get { return this.transform.localScale.y; } }
    public bool  Simulate  { get; private set; } = false;
    public bool  Fading    { get; private set; } = false;
    public bool  IsFilled  { get { return GetOverallYScale() >= maxYScale; } }

    private static List<LiquidMeshActor> layers;

    #endregion Fields

    #region MainBaseMethods

    void Start()
    {
        initYScale = maxYScale / 100.0f;

        if (this.transform.localScale.y < initYScale)
        {
            this.transform.localScale = new Vector3(
                this.transform.localScale.x,
                initYScale,
                this.transform.localScale.z);
        }

        if (layers == null)
        {
            layers = new List<LiquidMeshActor>();
        }

        layers.Add(this);

        initWaveHeight = waveHeight;
        latestShowState = showSide;

        mesh = Instantiate(this.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh);
        waveVertices = mesh.vertices;
        vertices = mesh.vertices;

        if (initVertices == null)
        {
            initVertices = mesh.vertices;
        }

        skinnedMeshRenderer = this.GetComponent<SkinnedMeshRenderer>();
        material = skinnedMeshRenderer.material;

        GetMinMaxYIndex(out minYIndex, out maxYIndex);

        meshHeight = vertices[maxYIndex].y - vertices[minYIndex].y;

        Fit();

        Simulate = playOnAwake;
    }

    void LateUpdate()
    {
        if (!isWaveActive) return;

        if (this.transform.localScale.y <= initYScale && skinnedMeshRenderer.enabled)
        {
            skinnedMeshRenderer.enabled = false;
        }

        if (!skinnedMeshRenderer.enabled)
        {
            if (this.transform.localScale.y > initYScale)
            {
                skinnedMeshRenderer.enabled = true;
            }

            return;
        }

        if (calculateEveryFrame)
        {
            Fit();
        }
        else if (latestShowState != showSide)
        {
            Fit();
            latestShowState = showSide;
        }

        if (Simulate)
        {
            Wave();
        }

        if (Fading)
        {
            waveHeight = Mathf.Lerp(waveHeight, .0f, fadingSpeed * Time.deltaTime);

            if (waveHeight <= .01f)
            {
                Stop();
            }
        }
    }

    #endregion MainBaseMethods

    #region MeshSetup

    public bool FillLiquid(float amount, Texture liquidTexture)
    {
        if (GetOverallYScale() >= maxYScale)
        {
            return true;
        }

        if (!material.GetTexture("_MainTex"))
        {
            material.SetTexture("_MainTex", liquidTexture);
            return true;
        }

        if (layers[layers.Count - 1].material.GetTexture("_MainTex") != liquidTexture)
        {
            InstantiateNewLayer(liquidTexture);
            Drop();
            return false;
        }

        Fill(amount);

        return true;
    }

    public bool FillLiquid(float amount, Color liquidColor)
    {
        if (GetOverallYScale() >= maxYScale)
        {
            return false;
        }

        if (layers[layers.Count - 1].material.GetColor("_TintColor") != liquidColor)
        {
            InstantiateNewLayer(liquidColor);
            Drop();
            return false;
        }

        Fill(amount);

        return true;
    }

    private void Fill(float amount)
    {
        this.transform.localScale += Vector3.up * amount * Time.deltaTime;

        if (!calculateEveryFrame)
        {
            Fit();
        }
    }

    public LiquidMeshActor SetTexture(Texture texture)
    {
        if (!material)
        {
            material = this.GetComponent<SkinnedMeshRenderer>().material;
        }

        material.SetTexture("_MainTex", texture);

        return this;
    }

    public Texture GetTexture()
    {
        if (!material)
        {
            material = this.GetComponent<SkinnedMeshRenderer>().material;
        }

        return material.GetTexture("_MainTex");
    }

    public LiquidMeshActor SetColor(Color liquidColor)
    {
        if (!material)
        {
            material = this.GetComponent<SkinnedMeshRenderer>().material;
        }

        material.SetColor("_TintColor", liquidColor);

        return this;
    }

    public LiquidMeshActor SetHeight(float height)
    {
        this.transform.localScale = new Vector3(
            this.transform.localScale.x,
            height,
            this.transform.localScale.z
        );

        return this;
    }

    private LiquidMeshActor SetInitVertices(Vector3[] initVertices)
    {
        this.initVertices = initVertices;

        return this;
    }

    public LiquidMeshActor SetShowSide(ShowSide showSide)
    {
        for (int i = 0; i < layers.Count; ++i)
        {
            layers[i].showSide = showSide;
        }

        return this;
    }

    public LiquidMeshActor SetWaveEffect(WaveEffect waveEffect)
    {
        this.waveEffect = waveEffect;

        return this;
    }

    private LiquidMeshActor InstantiateLayer()
    {
        Vector3 pos = this.transform.position;
        pos.y = this.transform.TransformPoint(vertices[maxYIndex]).y;

        return Instantiate(this, pos, this.transform.rotation, this.transform.parent)
            .SetHeight(initYScale)
            .SetInitVertices(initVertices);
    }

    private void InstantiateNewLayer(Texture liquidTexture)
    {
        InstantiateLayer().SetTexture(liquidTexture);
    }

    private void InstantiateNewLayer(Color liquidColor)
    {
        InstantiateLayer().SetColor(liquidColor);
    }

    private float GetOverallYScale()
    {
        float y = .0f;

        for (int i = 0; i < layers.Count; ++i)
        {
            y += layers[i].transform.localScale.y;
        }

        return y;
    }

    private void GetMinMaxYIndex(out int minYIndex, out int maxYIndex)
    {
        float min = vertices[0].y;
        float max = vertices[0].y;

        minYIndex = 0;
        maxYIndex = 0;

        for (int i = 0; i < vertices.Length; ++i)
        {
            if (min > vertices[i].y)
            {
                min = vertices[i].y;
                minYIndex = i;
            }

            if (max < vertices[i].y)
            {
                max = vertices[i].y;
                maxYIndex = i;
            }
        }
    }

    private void Fit()
    {
        return;

        distance = .0f;
        RaycastHit hit;

        float teta;
        float dist;
        Vector3 vertex = Vector3.zero;
        Vector3 centerVertex = Vector3.zero;

        for (int i = 0; i < initVertices.Length; i++)
        {
            vertices[i] = initVertices[i];
            centerVertex.y = vertices[i].y;
            distance = .0f;

            if (Physics.Raycast(this.transform.TransformPoint(centerVertex), 
                this.transform.forward * (showSide == ShowSide.BackSide ? -1.0f : 1.0f), 
                out hit, 2.0f, ~LayerMask.GetMask("Water")))
            {
                distance = Vector3.Distance(centerVertex, this.transform.InverseTransformPoint(hit.point));
                distance -= gap;
            }
            else
            {
                vertices[i].x = .0f;
                vertices[i].z = .0f;
                continue;
            }

            dist = Vector3.Distance(centerVertex, vertices[i]);
            teta = Mathf.Acos(vertices[i].x / dist) * Mathf.Rad2Deg;
            teta = vertices[i].z < .0f ? 360.0f - teta : teta;
            vertex.x = Mathf.Cos(teta / Mathf.Rad2Deg) * distance;
            vertex.z = Mathf.Sin(teta / Mathf.Rad2Deg) * distance;

            if ((showSide == ShowSide.FrontSide && teta >= 180.0f) ||
                (showSide == ShowSide.BackSide && teta < 180.0f))
            {
                vertex.z = .0f;
            }

            vertices[i].x = float.IsNaN(vertex.x) ? .0f : vertex.x;
            vertices[i].z = float.IsNaN(vertex.z) ? .0f : vertex.z;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    public Dictionary<Texture, int> GetPercentage()
    {
        Dictionary<Texture, int> percentage = new Dictionary<Texture, int>();
        int percent;

        for (int i = 0; i < layers.Count; ++i)
        {
            percent = Mathf.RoundToInt(100 * (layers[i].Height / MaxYScale));

            if (percentage.ContainsKey(layers[i].GetTexture()))
            {
                percentage[layers[i].GetTexture()] += percent;
            }
            else
            {
                percentage.Add(layers[i].GetTexture(), percent);
            }
        }

        return percentage;
    }

    /* Yeni sipariş geldiği zaman çağrılması gerekiyor, 
     * eğer level resetleniyorsa buna gerek yok
     * UI - ui aracılığı'ile de çağrılabilir */
    public void ResetLayers()
    {
        layers.Clear();
    }

    void OnDestroy()
    {
        layers.Remove(this);
    }

    #endregion MeshSetup

    #region WaveSetup

    private void Wave()
    {
        float teta;
        float dist;
        Vector3 vertex;
        Vector3 centerVertex = Vector3.zero;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (waveEffect == WaveEffect.TopHalf && vertices[i].y < meshHeight / 2.0f ||
                waveEffect == WaveEffect.BottomHalf && vertices[i].y > meshHeight / 2.0f)
            {
                waveVertices[i] = vertices[i];
                continue;
            }

            vertex = vertices[i];
            centerVertex.y = vertices[i].y;

            dist = Vector3.Distance(centerVertex, vertex);
            teta = Mathf.Acos(vertex.x / dist) * Mathf.Rad2Deg;
            teta = vertex.z < .0f ? 360.0f - teta : teta;
            teta = (teta + Time.time * waveSpeed) % 360.0f;
            vertex.y += Mathf.Sin(teta * (1 + waveSmoothness)) * (waveHeight / 1000.0f);
            vertex.y = float.IsNaN(vertex.y) ? vertices[i].y : vertex.y;

            waveVertices[i] = vertex;
        }

        mesh.vertices = waveVertices;
        mesh.RecalculateNormals();

        skinnedMeshRenderer.sharedMesh = mesh;
    }

    public void Play()
    {
        waveHeight = initWaveHeight;
        Fading = false;
        Simulate = true;
    }

    public void Fade()
    {
        Fading = true;
        Simulate = true;
    }

    public void Drop()
    {
        waveHeight = dropWaveHeight;
        Fade();
    }

    public void Pause()
    {
        if (!Simulate)
        {
            return;
        }

        Simulate = false;
        Fading = false;
    }

    public void Stop()
    {
        if (!Simulate)
        {
            return;
        }

        Simulate = false;
        Fading = false;
        waveHeight = .0f;
        Fit();
    }

    #endregion WaveSetup
}