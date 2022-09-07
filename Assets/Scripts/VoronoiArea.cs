using System.Collections.Generic;
using UnityEngine;

namespace NAMESPACENAME
{
    public struct Line
    {
        public Vector2 dir;
        public Vector2 pointA;
        public Vector2 pointB;
        /// <summary> ///  distance from local [0,0] /// </summary>
        public Vector2 offset; 

        public Line(Vector2 pointA, Vector2 pointB)
        {
            dir = (pointA - pointB).normalized;
            this.pointA = pointA;
            this.pointB = pointB;
            offset = Vector2.Lerp(pointA, pointB, 0.5f);
        }
        public void CalculateOffset()
        {
            offset = Vector2.Lerp(pointA, pointB, 0.5f);
        }
    }
    public class VoronoiArea
    {
        public Vector2 point = Vector2.zero;
        public List<Line> lines = new List<Line>();

        public void CalculateLine(Vector2 otherPoint)
        {
            float lineDis;
            float otherDis;
            float lineOtherDis;

            foreach (Line line in lines)
            {
                //CHECK IF DISTANCE TO OTHER IS CLOSER THAN DISTANCE TO OTHER + LINE-OFF


                //Get distance to the middle of the line & to the other point
                lineDis = Vector2.Distance(point, line.offset);
                otherDis = Vector2.Distance(point, otherPoint);

                //If line is closer than other point, don't calculate it
                if (lineDis < otherDis) return;


            }

            //Set line (calculates pos and dir)
            Line newLine = new Line(point, otherPoint);
            //Use perpendicular of dir, so it's not a line from Point to Other
            newLine.dir = Vector2.Perpendicular(newLine.dir);

            //Add to list
            lines.Add(newLine);
        }
    }
}
