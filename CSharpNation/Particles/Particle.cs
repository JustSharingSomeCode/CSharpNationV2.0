using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNation.Particles
{
    class Particle
    {
        public Particle(float x, float y,float iS, float fS, float freq, float yS, float xS, int dir)
        {
            X = x;
            Y = y;

            initialSize = iS;
            finalSize = fS;
            frequency = freq;
            ySpeed = yS;
            xSpeed = xS;

            actualSize = initialSize;

            yDir = dir;
        }

        public float X { get; private set; }
        public float Y { get; private set; }

        public float HalfSize
        {
            get
            {
                return actualSize / 2;
            }
        }

        float initialSize, actualSize, finalSize, frequency, ySpeed, xSpeed;
        int yDir;

        public void Update(float power)
        {
            /*
            PositionX -= xSpeed * power;
            PositionY += (ySpeed * power) * yDir;
            */
            X -= xSpeed + (xSpeed * power);
            Y += (ySpeed + (ySpeed * power)) * yDir;
            //X -= xSpeed;
            //Y += ySpeed * yDir;
        }

        public bool IsOutOfBounds(float width, float height)
        {
            if (X < 0 - HalfSize || X > width + HalfSize)
            {
                return true;
            }

            if (Y < 0 - HalfSize || Y > height + HalfSize)
            {
                return true;
            }

            return false;
        }
    }
}
