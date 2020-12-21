using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsUtils
    {
        public static float Cos45 = Mathf.Sqrt(2) / 2f; //cos 45 degree

        

        public static Vector3 GetPositionWithBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            //Vector3 result = Vector3.zero;

            //var tt = t * t;
            //var u = (1.0f - t);
            //var uu = u * u;

            //result = uu * p0 + 2 * u * t * p1 + tt * p2;

            //return result;

            return new Vector2(
                 Mathf.Lerp(Mathf.Lerp(p0.x, p1.x, t), p2.x, t)
                , Mathf.Lerp(Mathf.Lerp(p0.y, p1.y, t), p2.y, t)

                );
        }

        public static Vector2 RotateVectorByAngle(Vector2 vt, float angle)
        {
            float radian = Mathf.Deg2Rad*angle;
            return new Vector2(
                    vt.x * Mathf.Cos(radian) - vt.y * Mathf.Sin(radian),
                    vt.x * Mathf.Sin(radian) + vt.y * Mathf.Cos(radian)

                );

        }
        public static float GetDistanceByHypotenuse(float distance)
        {
            return distance * Cos45;
        }
        public static Vector3 GetPointByVectorAndDistance(Vector3 startPoint, Vector3 direction, float distance)
        {
            float distanceVector = distance / direction.magnitude;
            return direction * distanceVector + startPoint;
        }

        public static Vector2 GetPointByVectorAndDistanceV2(Vector2 startPoint, Vector2 direction, float distance)
        {
            float distanceVector = distance / direction.magnitude;
            return direction * distanceVector + startPoint;
        }
        public static Vector3 GetPointByDicrectionAndDistance(Vector3 startPoint, Vector3 endPoint, float maxDistance)
        {
            float distance = Vector3.Distance(startPoint, endPoint);
            if (distance > maxDistance)
            {
                Vector3 direction = (endPoint - startPoint).normalized;
                return GetPointByVectorAndDistance(startPoint, direction, maxDistance);
            }
            return endPoint;
        }
        public static MoveDirection GetDirectionByPosition(Vector2 current, Vector2 target)
        {
            Vector2 direction = target - current;
            float angle = Vector2.Angle(direction, Vector2.right);
            //Debug.Log("angle "+ angle);
            MoveDirection moveDirection;
            if (angle <= 90)
            {
                if (target.y >= current.y)
                {
                    moveDirection = MoveDirection.TopRight;
                }
                else
                {
                    moveDirection = MoveDirection.BottomRight;
                }
            }
            else
            {
                if (target.y >= current.y)
                {
                    moveDirection = MoveDirection.TopLeft;
                }
                else
                {
                    moveDirection = MoveDirection.BottomLeft;
                }
            }
            return moveDirection;
        }

    }
    public enum MoveDirection
    {
        TopLeft = 1 << 0,
        TopRight = 1 << 1,
        BottomLeft = 1 << 2,
        BottomRight = 1 << 3
    }
