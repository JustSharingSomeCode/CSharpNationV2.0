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
            PositionX = x;
            PositionY = y;

            initialSize = iS;
            finalSize = fS;
            frequency = freq;
            ySpeed = yS;
            xSpeed = xS;

            actualSize = initialSize;

            yDir = dir;
        }

        public float PositionX { get; private set; }
        public float PositionY { get; private set; }

        public float HalfSize { get; private set; }

        float initialSize, actualSize, finalSize, frequency, ySpeed, xSpeed;
        int yDir;

        public void Update(float power)
        {
            PositionX -= xSpeed + power;
            PositionY += (ySpeed + power) * yDir;
        }
    }
}
