using Game.GlobalVariables;
using System;
using TMPro;
using TriflesGames.Actors;
using TriflesGames.ManagerFramework;
using TriflesGames.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameUiActor : UiActor
{
    [Header("Custom UI")]
    public GameObject obj_TutorialSwipe;

    [Header("Boost")]
    public TextMeshProUGUI txt_nicePrefab;
    public Image img_boostImage;

    protected override void MB_Listen(bool status)
    {
        base.MB_Listen(status);

        if (status)
        {
            CustomLevelManager.Instance.Subscribe(CustomManagerEvents.TutorialCompleted, TutorialCompleted);
            CustomLevelManager.Instance.Subscribe(CustomManagerEvents.CreateBoostUI, CreateBoostUI);
            CustomLevelManager.Instance.Subscribe(CustomManagerEvents.DestroyBoostUI, DestroyBoostUI);
        }
        else
        {
            CustomLevelManager.Instance.Unsubscribe(CustomManagerEvents.TutorialCompleted, TutorialCompleted);
            CustomLevelManager.Instance.Unsubscribe(CustomManagerEvents.CreateBoostUI, CreateBoostUI);
            CustomLevelManager.Instance.Unsubscribe(CustomManagerEvents.DestroyBoostUI, DestroyBoostUI);
        }
    }

    private void CreateBoostUI(object[] args)
    {
        Instantiate(txt_nicePrefab, menu_InGame.transform);

        img_boostImage.DOKill();
        img_boostImage.DOFade(0.03f,0);
        img_boostImage.enabled = true;
        img_boostImage.DOFade(0.05f, 0.9f).SetLoops(-1, LoopType.Yoyo);
    }

    private void DestroyBoostUI(object[] args)
    {
        if (FindObjectOfType<BoostText>() != null)
        {
            Destroy(FindObjectOfType<BoostText>().gameObject);
        }
        img_boostImage.DOKill();
        img_boostImage.enabled = false;
    }

    protected override void InitLevel(object[] args)
    {
        base.InitLevel(args);

        if (CustomLevelManager.Instance.isTutorialLevel)
        {
            btn_Play.gameObject.SetActive(false);
            obj_TutorialSwipe.SetActive(true);
        }
        else
        {
            btn_Play.gameObject.SetActive(true);
            obj_TutorialSwipe.SetActive(false);
        }
    }

    protected override void GameStatus_Start(object[] args)
    {
        base.GameStatus_Start(args);
    }

    protected override void GameStatus_GameOver(object[] args)
    {
        base.GameStatus_GameOver(args);
    }


    protected override void TutorialCompleted(object[] args)
    {
        //base.TutorialCompleted(args);

        obj_Tutorial.SetActive(false);
        Push(ManagerEvents.BtnClick_Play);
    }
}
