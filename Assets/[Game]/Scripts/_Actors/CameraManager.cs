using DG.Tweening;
using Cinemachine;
using UnityEngine;
using TriflesGames.ManagerFramework;

public class CameraManager : Manager<CameraManager>
{
    [System.Serializable]
    public class ThemeProperties
    {
        public Material mat;

        [Header("Fog")]
        public Color fogColor;
        public FogMode fogMode = FogMode.Linear;

        public float fogStartDistance=62.8f, fogEndDistance=93.4f;
        public float fogDensity=0.02f;
    }

    public ThemeProperties[] themeMats;

    public delegate void CallBack();

    public Camera currentCamera;

    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public CinemachineBrain cinemachineBrain;

    private CallBack cinemachineCallBack;

    bool xReady;
    bool yReady;
    bool zReady;

    protected override void MB_Start()
    {
        base.MB_Awake();

        Invoke("Init", 0.1f);
    }

    public void Init()
    {

        transform.SetParent(null);
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;

        cinemachineVirtualCamera.m_Lens.FieldOfView = 60;
        GetThemeCount();

        ActiveVirtualMachine();
    }

    void ActiveVirtualMachine()
    {
        cinemachineVirtualCamera.enabled = true;
    }

    public void Shake(float duration = 1f, float strength = 0.2f)
    {
        transform.DOShakeRotation(duration, strength, 90);
    }

    public void SetLookAt(Transform transform)
    {
        cinemachineVirtualCamera.LookAt = transform;  
    }

    public void SetFollow(Transform transform)
    {
        cinemachineVirtualCamera.Follow = transform;
    }

    private void GetThemeCount()
    {
        int themeCount = CustomLevelManager.Instance.customGameData.GetThemeCount();
        GetComponentInChildren<Skybox>().material = themeMats[themeCount].mat;

        RenderSettings.fogMode = themeMats[themeCount].fogMode;
        RenderSettings.fogColor = themeMats[themeCount].fogColor;
        RenderSettings.fogStartDistance = themeMats[themeCount].fogStartDistance;
        RenderSettings.fogEndDistance = themeMats[themeCount].fogEndDistance;
        RenderSettings.fogDensity = themeMats[themeCount].fogDensity;
    }

    public void SetDamping(float newSpeedX, float newSpeedY, float newSpeedZ, float second = 0.5f, CallBack callBack = null)
    {
        xReady = false;
        yReady = false;
        zReady = false;

        float currentSpeedX = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping;
        DOTween.To(() => currentSpeedX, x => currentSpeedX = x, newSpeedX, second).SetEase(Ease.Unset).OnUpdate(() =>
        {
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = currentSpeedX;
        }).OnComplete(() =>
        {
            xReady = true;
            IsAllReady(callBack);
        });

        float currentSpeedY = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping;
        DOTween.To(() => currentSpeedY, x => currentSpeedY = x, newSpeedY, second).SetEase(Ease.Unset).OnUpdate(() =>
        {
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = currentSpeedY;
        }).OnComplete(() =>
        {
            yReady = true;
            IsAllReady(callBack);
        });

        float currentSpeedZ = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping;
        DOTween.To(() => currentSpeedZ, x => currentSpeedZ = x, newSpeedZ, second).SetEase(Ease.Unset).OnUpdate(() =>
        {
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = currentSpeedZ;
        }).OnComplete(() =>
        {

            zReady = true;

            IsAllReady(callBack);
        });
    }

    private void IsAllReady(CallBack callBack)
    {
        if (xReady && yReady && zReady)
        {
            if (callBack != null)
            {

                callBack();
            }
        }
    }

    public void LookAround()
    {
        cinemachineVirtualCamera.enabled = false;
        currentCamera.DOFieldOfView(85,1);
        currentCamera.transform.DOLocalMove(new Vector3(7.22f, 6, FindObjectOfType<BigGlassBottle>().transform.position.z+11.4f), 1);
        currentCamera.transform.DOLocalRotate(new Vector3(0, -80, 0), 1);


      

    }
}
