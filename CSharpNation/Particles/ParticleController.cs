using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNation.Textures;
using CSharpNation.Config;

namespace CSharpNation.Particles
{
    class ParticleController
    {
        public ParticleController(float width, float height)
        {
            random = new Random();
            Width = width;
            Height = height;

            particles = new List<Particle>();

            texture = new Texture()
            {
                Path = GlobalConfig.ResourcesDirectoryPath + @"\Particle.png"
            };

            texture.LoadTexture();
        }

        private Random random;

        private float Width, Height;

        private List<Particle> particles;

        private int dir = -1;

        private Texture texture;

        public void UpdateBounds(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public void UpdateParticles(float power)
        {
            float iS = random.Next(5, 10);
            float fs = random.Next(15, 25);
            float freq = (float)random.NextDouble() * 2.0f;
            float ys = (float)random.NextDouble();
            float xs = (float)random.NextDouble();

            if (ys < 0.2)
            {
                ys = 0.2f;
            }

            if (xs < 0.2)
            {
                xs = 0.2f;
            }

            while (particles.Count < 500)
            {
                particles.Add(new Particle(Width / 2, Height / 2, iS, fs, freq, ys, xs, dir));
                dir *= -1;
            }

            for(int i = 0; i < particles.Count; i++)
            {
                particles[i].Update(power);
            }
        }

        public void DrawParticles()
        {
            if(texture.TextureData == -1)
            {
                return;
            }

            for(int i = 0; i < particles.Count; i++)
            {
                Particle p = particles[i];
                float half = p.HalfSize;
                TextureController.DrawTexture(texture.TextureData, p.PositionX - half, p.PositionY - half, p.PositionX + half, p.PositionY + half, 255, 255, 255, 255);
            }
        }

    }
}
