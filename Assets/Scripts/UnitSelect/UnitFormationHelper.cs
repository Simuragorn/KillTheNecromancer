using Assets.Scripts.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UnitSelect
{
    public class UnitFormationHelper
    {
        public static List<Vector3> GetFormationPositions(UnitFormationEnum unitFormation, Vector3 startPosition, int totalSpotsCount)
        {
            switch (unitFormation)
            {
                case UnitFormationEnum.Circle:
                    return GetCircleFormationSpots(startPosition, totalSpotsCount);
                case UnitFormationEnum.Square:
                    return GetSquareFormationPositions(startPosition, totalSpotsCount);
                default:
                    break;
            }
            return new List<Vector3> { startPosition };
        }

        private static List<Vector3> GetSquareFormationPositions(Vector3 startPosition, int totalSpotsCount)
        {
            if (totalSpotsCount == 1)
            {
                return new List<Vector3> { startPosition };
            }

            int spotsLeft = totalSpotsCount;
            var positionsList = new List<Vector3>();

            float squareDistance = 1f;
            int sidePositionsCount = 1;
            while (spotsLeft > 0)
            {
                List<Vector3> positions = GetSquareSpotsAround(startPosition, squareDistance, sidePositionsCount);
                positionsList.AddRange(positions);
                spotsLeft -= positions.Count;
                squareDistance++;
                sidePositionsCount *= 2;
            }
            return positionsList;
        }

        private static List<Vector3> GetSquareSpotsAround(Vector3 startPosition, float distance, int sideSpotsCount)
        {
            var spotsList = new List<Vector3>();
            int spotsLeft = sideSpotsCount;

            while (spotsLeft > 0)
            {
                Vector3 upperLeftPos = new Vector3(startPosition.x - distance / 2, startPosition.y + distance / 2, startPosition.z);

                List<Vector3> levelSpots = new List<Vector3>();
                if (sideSpotsCount > 1)
                    levelSpots.Add(upperLeftPos);
                levelSpots.AddRange(GetSquareSideSpots(upperLeftPos, Vector3.right, 1f, sideSpotsCount));
                levelSpots.AddRange(GetSquareSideSpots(levelSpots.Last(), Vector3.down, 1f, sideSpotsCount));
                levelSpots.AddRange(GetSquareSideSpots(levelSpots.Last(), Vector3.left, 1f, sideSpotsCount));
                levelSpots.AddRange(GetSquareSideSpots(levelSpots.Last(), Vector3.up, 1f, sideSpotsCount));

                spotsList.AddRange(levelSpots);
                spotsLeft -= levelSpots.Count;
                sideSpotsCount *= 2;
                distance++;
            }

            return spotsList;
        }

        private static List<Vector3> GetSquareSideSpots(Vector3 sideStartSpot, Vector3 sideDirection, float spotsDistance, int sideSpotsCount)
        {
            var spots = new List<Vector3>();
            for (int i = 0; i < sideSpotsCount; i++)
            {
                Vector3 lastSpot = spots.Any() ? spots.Last() : sideStartSpot;
                Vector3 newSpot = lastSpot + sideDirection * spotsDistance;
                spots.Add(newSpot);
            }
            return spots;
        }

        private static List<Vector3> GetCircleFormationSpots(Vector3 startSpot, int totalSpotsCount)
        {
            int spotsLeft = totalSpotsCount;
            var spotsList = new List<Vector3>();
            spotsList.Add(startSpot);
            float ringDistance = 1f;
            int spotsCount = 5;
            while (spotsLeft > 0)
            {
                spotsList.AddRange(GetCircleSpotsAround(startSpot, ringDistance, spotsCount));
                spotsLeft -= spotsCount;
                ringDistance++;
                spotsCount *= 2;
            }
            return spotsList;
        }

        private static List<Vector3> GetCircleSpotsAround(Vector3 startPosition, float distance, int spotsCount)
        {
            var spotsList = new List<Vector3>();
            for (int i = 0; i < spotsCount; ++i)
            {
                float angle = i * (360 / spotsCount);
                Vector3 direction = ApplyRotationToVector(new Vector3(1, 0), angle);
                Vector3 position = startPosition + direction * distance;
                spotsList.Add(position);
            }
            return spotsList;
        }

        private static Vector3 ApplyRotationToVector(Vector3 vector, float angle)
        {
            return Quaternion.Euler(0, 0, angle) * vector;
        }
    }
}
