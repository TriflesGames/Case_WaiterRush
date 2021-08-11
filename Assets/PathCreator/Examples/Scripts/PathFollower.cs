using Game.Actors;
using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : PlayerComponent
    {
        [HideInInspector]
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        float distanceTravelled;

        private float totalDistance;

        public void FillCreator(PathCreator pathCreator, float addStartDistance)
        {
            this.pathCreator = pathCreator;
            this.pathCreator.pathUpdated += OnPathChanged;

            distanceTravelled += addStartDistance;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) + new Vector3(0, 0.1f, 0);

            totalDistance = pathCreator.path.length;

            this.enabled = false;
        }

        private bool isOver;

        void Update()
        {
            if (isOver) return;

            if (playerActor.CanMove)
            {

                distanceTravelled += playerActor._speed * Time.deltaTime;
            }


            if (pathCreator != null)
            {
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) + new Vector3(0, 0.1f, 0);

                transform.rotation = Quaternion.Lerp(transform.rotation, pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction), Time.deltaTime * 5);
                //transform.rotation = pathCreator.path.GetRotationAtDistance(Mathf.Lerp(distanceTravelled-1, distanceTravelled,Time.deltaTime*5), endOfPathInstruction);

                if (distanceTravelled >= totalDistance)
                {
                    isOver = true;

                    FindObjectOfType<GameLevelActor>().FinishPlatform();
                }
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        public void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}