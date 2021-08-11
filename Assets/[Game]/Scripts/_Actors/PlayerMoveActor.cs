using TMPro;
using UnityEngine;

public class PlayerMoveActor : PlayerComponent
{
    public Transform characterModel;

    public float rotMaxAngle = 10;

    public float sliderSpeed;

    private float sliderLeftMax;
    private float sliderRightMax;

    private Vector3 sliderV3;

    private float gravity => Physics.gravity.y;
    private float velocityY = 0f;


    private float newX;
    private Vector3 currentPos;

    public void FillRoadLenght(float lenght)
    {
        sliderRightMax = lenght * 0.9f;
        sliderLeftMax = -lenght * 0.9f;
    }

    private Vector3 lastPoint;
    private void CalculateLiquidWave(float distance)
    {
        lastPoint = characterModel.position;

       // playerActor.NewLiquidWave(Mathf.Clamp(distance * 5f, 3, 10));
    }

    private Vector3 addForce = Vector3.zero;
    private bool isCrash;

    public void AddForce(Vector3 addForce)
    {
        isCrash = true;

        addForce.y = 0;
        this.addForce = addForce * 7;

        Invoke("CrashOver", 0.3f);
    }

    void CrashOver()
    {
        this.addForce = Vector3.zero;

        isCrash = false;
    }

    Vector3 firstPos = Vector3.zero;
    Vector3 secondPos = Vector3.zero;


    public TextMeshProUGUI txt_bigVelocity;
    private float intCurrentVelocity;

    private void Update()
    {
        if (!characterController.isGrounded)
        {
            velocityY += (gravity) * (Time.fixedDeltaTime);
        }
        else
        {
            velocityY = 0.0f;
        }

        Vector3 velocity = velocityY * Vector3.up;

        Quaternion currentRot = characterModel.rotation;
        float newRotY = 0;
        currentRot.y = newRotY;

        if (playerActor.CanMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                secondPos = Input.mousePosition;

                float deltaPos = Mathf.Clamp(((secondPos - firstPos).x * .01f), -0.7f, 0.7f);

                if(deltaPos> intCurrentVelocity)
                {
                    txt_bigVelocity.text = deltaPos.ToString();
                    intCurrentVelocity = deltaPos;
                }

                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    sliderV3 = (new Vector3(/*Input.GetAxis("Mouse X")*/deltaPos, 0, 0) * sliderSpeed);
                }
                else
                {
                    sliderV3 = (new Vector3(/*Input.GetAxis("Mouse X")*/deltaPos, 0, 0) * sliderSpeed * 7);
                }

                #region rotation
                newRotY = Mathf.Clamp(currentRot.y + (sliderV3.x * 0.1f), -rotMaxAngle * 0.003f, rotMaxAngle * 0.003f);
                currentRot.y = newRotY;
                #endregion

                velocity += sliderV3;
                firstPos = Input.mousePosition;
            }

            velocity += (transform.forward * playerActor._speed);

            characterModel.rotation = Quaternion.Lerp(characterModel.rotation, currentRot, Time.deltaTime * 15);
            
            characterController.Move((velocity  /*+addForce*/) * Time.deltaTime);
        }
    }

    public Transform GetCharacterModel()
    {
        return characterModel;
    }
}
