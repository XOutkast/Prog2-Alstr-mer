using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Snake.Core;

namespace Snake.Models;

// Snake: hantera kropp, riktning, växt och rörelser.
public class Snake
{
    public Snake(Point start)
    {
        Body = new List<Point>();
        Reset(start);
    }

    // egenskaper
    public List<Point> Body { get; }
    public Direction Heading { get; private set; }
    public int PendingGrowth { get; private set; }
    public Point Head => Body[0];

    // Återställ snake position, kropp och riktning.
    public void Reset(Point start)
    {
        Body.Clear();
        Body.Add(start);
        Body.Add(new Point(start.X - 1, start.Y));
        Body.Add(new Point(start.X - 2, start.Y));
        Heading = Direction.Right;
        PendingGrowth = 0;
    }

    // rörelse: snake huvud
    public Point NextHead() => new(Head.X + Heading.X, Head.Y + Heading.Y);

    // Ny riktning som appliceras nästa steg om den är säker.
    public void Turn(Direction nextDirection)
    {
        if (nextDirection == Direction.Opposite(Heading))
        {
            return;
        }

        if (nextDirection.X == 0 && nextDirection.Y == 0)
        {
            return;
        }
        Heading = nextDirection;
    }

    public void Grow(int amount)
    {
        PendingGrowth += Math.Max(1, amount);
    }

    public void MoveTo(Point newHead)
    {
        Body.Insert(0, newHead);

        if (PendingGrowth > 0)
        {
            PendingGrowth--;
        }
        else
        {
            Body.RemoveAt(Body.Count - 1);
        }
    }

    public bool Contains(Point position) => Body.Contains(position);

    public bool HitsSelf(Point position) => Body.Skip(1).Contains(position);
}
