using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNationV2._0.Configuration;

namespace CSharpNationV2._0.Particles
{
    public class Particle
    {
        public enum X_Direction
        {
            Left = -1,
            Right = 1
        }

        public enum Y_Direction
        {
            Top = 1,
            Bottom = -1
        }

        public Particle(float x, float y, float speed, float px, Y_Direction ydir, float initialSize, float finalSize)
        {
            X = x;
            Y = y;

            Speed = speed <= 0 ? 0.1f : speed;

            Px = px;
            Py = 1.0f - px;

            Y_Dir = ydir;

            if (Y_Dir == Y_Direction.Bottom)
            {
                Py *= -1;
            }

            InitialSize = initialSize;
            FinalSize = finalSize;
        }

        public Particle(Particle mirror) : this(mirror.X, mirror.Y, mirror.Speed, mirror.Px, mirror.Y_Dir, mirror.InitialSize, mirror.FinalSize)
        {
            Px *= -1;
        }

        public float X { get; private set; }
        public float Y { get; private set; }

        public Y_Direction Y_Dir { get; private set; }

        public float InitialSize { get; private set; }
        public float ActualSize { get; private set; }
        public float FinalSize { get; private set; }

        public float HalfSize
        {
            get
            {
                return ActualSize / 2;
            }
        }

        public float Speed { get; private set; }

        private float Px;
        private float Py;

        public void Update(float power)
        {
            X += (Speed * Px) + (Speed * Px * power);
            Y += (Speed * Py) + (Speed * Py * power);

            ActualSize = InitialSize + (Math.Abs((ConfigurationManager.VisualizerWidth / 2) - X) * (FinalSize - InitialSize) / (ConfigurationManager.VisualizerWidth / 2));
        }

        public bool IsOutOfBounds()
        {
            if(X < 0 - HalfSize || X > ConfigurationManager.VisualizerWidth + HalfSize)
            {
                return true;
            }

            if (Y < 0 - HalfSize || Y > ConfigurationManager.VisualizerHeight + HalfSize)
            {
                return true;
            }

            return false;
        }
    }
}
