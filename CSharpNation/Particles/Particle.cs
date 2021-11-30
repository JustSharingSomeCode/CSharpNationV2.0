using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNation.Particles
{
    class Particle
    {
        public Particle(float x, float y,float iS, float fS, float freq, float amp, float yS, float xS, int dir, int opacity)
        {
            X = x;
            Y = y;

            initialSize = iS;
            finalSize = fS;
            frequency = freq;
            amplitude = amp;
            ySpeed = yS;
            xSpeed = xS;

            actualSize = initialSize;

            yDir = dir;
            Opacity = opacity;
        }

        public float X { get; private set; }
        public float Y { get; private set; }
        public int Opacity { get; private set; }

        public float HalfSize
        {
            get
            {
                return actualSize / 2;
            }
        }

        float initialSize, actualSize, finalSize, frequency, freqCount, amplitude, ySpeed, xSpeed;
        int yDir;

        public void Update(float power, float width, float height)
        {            
            X -= xSpeed + (xSpeed * power);
            Y += (ySpeed + (ySpeed * power)) * yDir;

            if(xSpeed > ySpeed)
            {
                Y += (float)Math.Sin(freqCount) * amplitude;

                float diff = width / 2 - X;
                float percentaje = diff / (width / 2);
                actualSize = finalSize * percentaje;
            }

            freqCount += frequency + frequency * power;
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
