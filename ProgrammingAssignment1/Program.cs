using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingAssignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome! This application will calculate the distance between two points and the angle between those points.");
            Console.WriteLine("Enter Point 1 X value:");
            float point1X = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter Point 1 Y value:");
            float point1Y = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter Point 2 X value:");
            float point2X = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter Point 2 Y value:");
            float point2Y = float.Parse(Console.ReadLine());

            float deltaX = point2X - point1X;
            float deltaY = point2Y - point1Y;

            double distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            double angle = Math.Atan2(deltaX, deltaY) * 180 / Math.PI;

            Console.WriteLine("Distance between points: {0}", distance.ToString("F3"));
            Console.WriteLine("Angle between points: {0} degrees", angle.ToString("F3"));

            Console.ReadKey();
        }
    }
}
