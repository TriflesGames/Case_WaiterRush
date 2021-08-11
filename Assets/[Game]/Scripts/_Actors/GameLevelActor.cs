using TriflesGames.ManagerFramework;
using TriflesGames.Actors;
using TriflesGames.Managers;
using UnityEngine;
using PathCreation;
using System;

namespace Game.Actors
{
    public class GameLevelActor : LevelActor
    {
        [Header("Other")]

        public PlayerActor playerActorPrefab;
        [HideInInspector] public PlayerActor PlayerActor;
        [HideInInspector] public PathCreator pathCreator;


        public float addStartDistance;

        protected override void MB_Awake()
        {
            base.MB_Awake();

            

            PlayerActor = Instantiate(playerActorPrefab, transform);
            pathCreator = GetComponentInChildren<PathCreator>();
        }

        protected override void MB_Start()
        {
            base.MB_Start();
            CameraManager.Instance.Init();
        }


        public override void InitLevel()
        {
            //pathCreator.GetComponent<PathCreation.Examples.RoadMeshCreator>().CreateMesh();

            PlayerActor.StartPosition(pathCreator, addStartDistance, pathCreator.GetComponent<PathCreation.Examples.RoadMeshCreator>().roadWidth);

            //Camera settings
            CameraManager.Instance.SetFollow(PlayerActor.GetCharacterModel());
            CameraManager.Instance.SetLookAt(PlayerActor.GetCharacterModel());
       
            CustomLevelManager.Instance.isTutorialLevel = GameManager.Instance.gameData.Level == 0 ? true : true; //her level tutorial başlar
        }

        protected override void MB_Listen(bool status)
        {
            base.MB_Listen(status);

            if (status)
            {
                GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Play, BtnClick_Play);
                GameManager.Instance.Subscribe(ManagerEvents.GameStatus_Start, GameStatus_Start);
                GameManager.Instance.Subscribe(ManagerEvents.GameStatus_GameOver, GameStatus_GameOver);        
            }
            else
            {
                GameManager.Instance.Unsubscribe(ManagerEvents.GameStatus_Start, GameStatus_Start);
                GameManager.Instance.Unsubscribe(ManagerEvents.GameStatus_GameOver, GameStatus_GameOver);
            }
        }

        private void BtnClick_Play(object[] arguments)
        {
            Push(ManagerEvents.BtnClick_Play);
        }

        private void GameStatus_Start(object[] args)
        {
            foreach (GameState item in FindObjectsOfType<GameState>())
            {
                item.GameStart();
            }

            PlayerActor.StartGame();
        }

        private void GameStatus_GameOver(object[] args)
        {
            foreach (GameState item in FindObjectsOfType<GameState>())
            {
                item.GameStop();
            }

            PlayerActor.StopGame();

            StopBoostEnvironment();
        }

        public void FinishPlatform()
        {
            StopBoostEnvironment();

            float second = Mathf.Clamp((PlayerActor.GetTotalWater() / 100f)*1.3f, 0.3f, 1);

            FindObjectOfType<BigGlassBottle>().IncrementWater(PlayerActor.GetColorList());
            PlayerActor.Empty(second);
        }

        public void FinishLevel(bool isWin)
        {
            Push(ManagerEvents.FinishLevel, isWin);
        }

        public void StartBoostEnvironment()
        {

            Invoke("StopBoostEnvironment", PlayerActor.boostSecond);
        }

        public void StopBoostEnvironment()
        {
            CancelInvoke("StopBoostEnvironment");
            PlayerActor.BoostDeactive();
        }

    }
}
