using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CasualRacer.Model
{
    internal class Game
    {
        public Track Track
        {
            get; private set;
        }

        public Player Player1
        {
            get; private set;
        }

        public Game()
        {
            Track = Track.Load("./Tracks/Track1.txt");

            Player1 = new Player();
        }

        public void Update(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            //lenkung
            if( Player1.WheelLeft )
                Player1.Direction -= (float)elapsedTime.TotalSeconds * 1000000;
            if( Player1.WheelRight )
                Player1.Direction += (float)elapsedTime.TotalSeconds * 1000000;

            //beschleunigung und bremsen
            float targetSpeed = 0f;
            if( Player1.Acceleration )
                targetSpeed += 100;
            if( Player1.Break )
                targetSpeed -= 50;

            int cellX = (int)(Player1.Position.X / Track.CELLSIZE);
            int cellY = (int)(Player1.Position.Y / Track.CELLSIZE);
            cellX = Math.Min(Track.Tiles.GetLength(0) - 1, Math.Max(0, cellX));
            cellY = Math.Min(Track.Tiles.GetLength(1) - 1, Math.Max(0, cellY));
            TrackTile tile = Track.Tiles[cellX, cellY];
            switch( tile )
            {
                case TrackTile.Dirt:
                    targetSpeed *= 0.2f;
                    break;
                case TrackTile.Sand:
                    targetSpeed *= 0.4f;
                    break;
                case TrackTile.Gras:
                    targetSpeed *= 0.8f;
                    break;
                case TrackTile.Road:
                    targetSpeed *= 1f;
                    break;
                default:
                    break;
            }

            if( targetSpeed > Player1.Velocity )
            {
                Player1.Velocity += 80 * (float)elapsedTime.TotalSeconds*10000;
                Player1.Velocity = Math.Min(targetSpeed, Player1.Velocity);
            }
            if( targetSpeed < Player1.Velocity )
            {
                Player1.Velocity -= 100 * (float)elapsedTime.TotalSeconds*10000;
                Player1.Velocity = Math.Max(targetSpeed, Player1.Velocity);
            }

            //positionsveränderung
            float direction = (float)(Player1.Direction * Math.PI) / 180f;
            Vector velocity = new Vector(
                Math.Sin(direction) * Player1.Velocity * elapsedTime.TotalSeconds*10000,
                -Math.Cos(direction) * Player1.Velocity * elapsedTime.TotalSeconds*10000
                );
            Player1.Position += velocity;

            Debug.WriteLine($"Player1.Position={Player1.Position}");

        }
    }
}
