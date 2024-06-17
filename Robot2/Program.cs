using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleRobotGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    public class Game
    {
        private Robot robot;
        private List<Target> targets;
        private const int width = 40;
        private const int height = 20;
        private Random random = new Random();

        public void Start()
        {
            robot = new Robot(width / 3, height / 2);
            targets = new List<Target>
            {
                new Human(random.Next(1, width), random.Next(1, height)),
                new Animal(random.Next(1, width), random.Next(1, height)),
                new Superhero(random.Next(1, width), random.Next(1, height))
            };

            while (true)
            {
                Draw();
                Update();
                Thread.Sleep(200); // Control the game speed
            }
        }

        private void Draw()
        {
            Console.Clear();

            // Draw robot
            Console.SetCursorPosition(robot.X, robot.Y);
            Console.Write("R");

            // Draw targets
            foreach (var target in targets)
            {
                Console.SetCursorPosition(target.X, target.Y);
                Console.Write(target.Symbol);
            }
        }

        private void Update()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                robot.Move(key, width, height);
            }

            // Check for collisions
            targets.RemoveAll(t => t.X == robot.X && t.Y == robot.Y);

            // If all targets are eliminated, respawn new ones
            if (targets.Count == 0)
            {
                targets.Add(new Human(random.Next(1, width), random.Next(1, height)));
                targets.Add(new Animal(random.Next(1, width), random.Next(1, height)));
                targets.Add(new Superhero(random.Next(1, width), random.Next(1, height)));
            }
        }
    }

    public class Robot
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Robot(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move(ConsoleKey key, int width, int height)
        {
            switch (key)
            {
                case ConsoleKey.W: if (Y > 0) Y--; break;
                case ConsoleKey.S: if (Y < height - 1) Y++; break;
                case ConsoleKey.A: if (X > 0) X--; break;
                case ConsoleKey.D: if (X < width - 1) X++; break;
            }
        }
    }

    public abstract class Target
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public abstract char Symbol { get; }
    }

    public class Human : Target
    {
        public Human(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override char Symbol => 'H';
    }

    public class Animal : Target
    {
        public Animal(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override char Symbol => 'A';
    }

    public class Superhero : Target
    {
        public Superhero(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override char Symbol => 'S';
    }
}