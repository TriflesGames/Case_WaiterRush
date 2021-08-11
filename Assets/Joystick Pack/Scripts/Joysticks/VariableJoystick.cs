using UnityEngine;
using UnityEngine.EventSystems;

public class VariableJoystick : Joystick
{
    public bool IsMoving { get; set; }
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private JoystickType joystickType = JoystickType.Fixed;


    private RectTransform rectTransform;
    private PlayerActor playerActor;
    private Vector2 fixedPosition = Vector2.zero;

    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if (joystickType == JoystickType.Fixed)
        {
            background.anchoredPosition = fixedPosition;
            background.gameObject.SetActive(true);
        }
        else
            background.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        base.Start();
        fixedPosition = background.anchoredPosition;
        rectTransform = background.GetComponent<RectTransform>();
        SetMode(joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);
    }

    public void Show(bool isShow)
    {
        //IsMoving = false;
        rectTransform.anchoredPosition = new Vector2(0, 400);
        handle.anchoredPosition = Vector2.zero;
        background.gameObject.SetActive(isShow);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)

            rectTransform.anchoredPosition = new Vector2(0, 400);
        background.gameObject.SetActive(false);

        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}

public enum JoystickType { Fixed, Floating, Dynamic }