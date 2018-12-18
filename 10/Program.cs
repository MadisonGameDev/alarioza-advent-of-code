using System;
using System.Collections.Generic;
using System.Linq;

namespace _10
{
    class Program
    {
        static void Main(string[] args)
        {
            Sky sky = new Sky();

            while (true)
            {
                Console.WriteLine("Enter next light or press enter to continue:");

                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    // done entering changes
                    break;
                }

                Light light;
                if (Light.TryParse(input, out light))
                {
                    sky.AddLight(light);
                }
                else
                {
                    Console.WriteLine("Invalid value");
                }
            }
            
            int step = 0;
            while (!sky.IsAligned())
            {
                sky.PrintSky();
                sky.StepForward();
                step++;
            }
            sky.PrintSky();
        }

        public class Sky
        {
            HashSet<Point> points = new HashSet<Point>();
            List<Light> lights = new List<Light>();

            public void AddLight(Light light)
            {
                lights.Add(light);
                points.Add(light.Position);
            }

            public void StepForward()
            {
                points.Clear();
                for (int i = 0; i < lights.Count; i++)
                {
                    lights[i] = lights[i].AddVelocity();
                    points.Add(lights[i].Position);
                }
            }

            public bool IsAligned()
            {
                foreach (var light in lights)
                {
                    if (!HasNeighbors(light))
                    {
                        return false;
                    }
                }

                return true;
            }

            bool HasNeighbors(Light light)
            {
                foreach (var other in lights)
                {
                    if (other.Position.X == light.Position.X || other.Position.Y == light.Position.Y)
                    {
                        return true;
                    }
                }
                return false;
            }

            public void PrintSky()
            {
                Point min = new Point(
                    lights.Min(l => l.Position.X),
                    lights.Min(l => l.Position.Y)
                );

                Point max = new Point(
                    lights.Max(l => l.Position.X),
                    lights.Max(l => l.Position.Y)
                );

                Point newMin = Point.Empty;
                Point newMax = new Point(10, 10);

                var normalized = points
                    .Select(p => p.Scale(min, max, newMin, newMax));
                var lookup = new HashSet<Point>(normalized);

                for (int y = newMin.Y; y <= newMax.Y; y++)
                {
                    for (int x = newMin.X; x <= newMax.X; x++)
                    {
                        if (lookup.Contains(new Point(x, y)))
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    Console.Write("\n");
                }
            }
        }

        public struct Light
        {
            public static Light Empty = new Light(Point.Empty, Point.Empty);

            public Point Position;
            public Point Velocity;

            public Light(Point position, Point velocity)
            {
                Position = position;
                Velocity = velocity;
            }

            public Light AddVelocity()
            {
                return new Light(Position + Velocity, Velocity);
            }

            public static bool TryParse(string str, out Light light)
            {
                int pStart = str.IndexOf('<') + 1;
                if (pStart == -1)
                {
                    light = Light.Empty;
                    return false;
                }

                int pEnd = str.IndexOf('>');
                if (pEnd == -1)
                {
                    light = Light.Empty;
                    return false;
                }

                Point p;
                var pPair = str.Substring(pStart, pEnd - pStart);
                if (!Point.TryParse(pPair, out p))
                {
                    light = Light.Empty;
                    return false;
                }

                int vStart = str.LastIndexOf('<') + 1;
                if (vStart == -1)
                {
                    light = Light.Empty;
                    return false;
                }

                int vEnd = str.LastIndexOf('>');
                if (vEnd == -1)
                {
                    light = Light.Empty;
                    return false;
                }

                Point v;
                var vPair = str.Substring(vStart, vEnd - vStart);
                if (!Point.TryParse(vPair, out v))
                {
                    light = Light.Empty;
                    return false;
                }

                light = new Light(p, v);
                return true;
            }
        }

        static float ScaleNum(float value, float min1, float max1, float min2, float max2)
        {
            return (((value - min1) * (max2 - min2)) / (max2 - min1)) + min2;
        }

        public struct Point
        {
            public static Point Empty = new Point(0, 0);

            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Point Scale(Point min1, Point max1, Point min2, Point max2)
            {
                return new Point(
                    (int)Math.Floor(ScaleNum(X, min1.X, max1.X, min2.X, max2.X)),
                    (int)Math.Floor(ScaleNum(Y, min1.Y, max1.Y, min2.Y, max2.Y))
                );
            }

            public static Point operator +(Point a, Point b)
            {
                return new Point(a.X + b.X, a.Y + b.Y);
            }

            public static bool TryParse(string str, out Point point)
            {
                var pair = str.Split(',');

                int x;
                if (!int.TryParse(pair[0], out x))
                {
                    point = Point.Empty;
                    return false;
                }

                int y;
                if (!int.TryParse(pair[1], out y))
                {
                    point = Point.Empty;
                    return false;
                }

                point = new Point(x, y);
                return true;
            }
        }
    }
}
