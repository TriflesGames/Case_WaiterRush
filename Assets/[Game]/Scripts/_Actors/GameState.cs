using UnityEngine;

public class GameState : MonoBehaviour
{
    public delegate void CallBack();
    private CallBack callBackStart;
    private CallBack callBackStop;

    public void FillGameStart(CallBack callBackStart)
    {
        if (callBackStart != null)
        {
            this.callBackStart = callBackStart;
        }
    }

    public void FillGameStop(CallBack callBackStop)
    {
        if (callBackStop != null)
        {
            this.callBackStop = callBackStop;
        }
    }

    public void GameStop()
    {
        if (this.callBackStop != null)
        {
            this.callBackStop();
        }
    }

    public void GameStart()
    {
        if (this.callBackStart != null)
        {
            this.callBackStart();
        }
    }

}
