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
            CreatePraticles();

            UpdateAndDeleteParticles(power);
        }

        private void CreatePraticles()
        {
            while (particles.Count < 1000)
            {
                float ys = (float)random.NextDouble();
                float xs = (float)random.NextDouble();

                if (ys < 0.2 && xs < 0.2)
                {
                    continue;
                }

                float iS = random.Next(5, 10);
                float fs = random.Next(15, 25);
                float freq = (float)random.NextDouble() * 0.02f;
                float amp = (float)random.NextDouble() * 0.5f;
                int opacity = random.Next(20, 256);

                particles.Add(new Particle(Width / 2, Height / 2, iS, fs, freq, amp, ys, xs, dir, opacity));
                dir *= -1;
            }
        }

        private void UpdateAndDeleteParticles(float power)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i].IsOutOfBounds(Width, Height))
                {
                    _ = particles.Remove(particles[i]);
                    i--;
                }
                else
                {
                    particles[i].Update(power, Width, Height);
                }
            }
        }


        public void DrawParticles()
        {
            if (texture.TextureData == -1)
            {
                return;
            }

            for (int i = 0; i < particles.Count; i++)
            {
                Particle p = particles[i];
                float half = p.HalfSize;
                float diff = Math.Abs(Width / 2 - p.X);
                diff = Width / 2 + diff;
                TextureController.DrawTexture(texture.TextureData, p.X - half, p.Y - half, p.X + half, p.Y + half, p.Opacity, 255, 255, 255);
                TextureController.DrawTexture(texture.TextureData, diff - half, p.Y - half, diff + half, p.Y + half, p.Opacity, 255, 255, 255);
            }
        }

    }
}
