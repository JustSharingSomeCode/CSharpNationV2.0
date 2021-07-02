using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNationV2._0.Configuration;
using CSharpNationV2._0.Textures;

namespace CSharpNationV2._0.Particles
{
    public class ParticlesManager
    {
        public ParticlesManager()
        {
            ParticleTexture = TextureManager.LoadTexture(ConfigurationManager.resourcesDirectoryPath + @"\Particle.png");
        }

        private int ParticleTexture = -1;

        private List<Particle> particles = new List<Particle>();

        private Random random = new Random();

        public void UpdateParticles(float power)
        {
            CreateParticles();

            UpdateAndDeleteParticles(power);
        }

        private void CreateParticles()
        {
            while(particles.Count < ConfigurationManager.ParticlesOnScreen / 2)
            {
                float size = random.Next(8, 12);
                Particle p = new Particle(ConfigurationManager.VisualizerWidth / 2, ConfigurationManager.VisualizerHeight / 2, (float)random.NextDouble(), (float)random.NextDouble(), Random_Y_Direction(), size, size * random.Next(2, 8));
                particles.Add(p);
                particles.Add(new Particle(p));
            }
        }

        private void UpdateAndDeleteParticles(float power)
        {
            for(int i = 0; i < particles.Count; i++)
            {
                if(particles[i].IsOutOfBounds())
                {
                    particles.Remove(particles[i]);
                    i--;
                }
                else
                {
                    particles[i].Update(power);
                }
            }
        }

        public void DrawParticles()
        {
            for(int i = 0; i < particles.Count; i++)
            {
                Particle p = particles[i];
                TextureManager.DrawTexture(ParticleTexture, p.X - p.HalfSize, p.Y - p.HalfSize, p.X + p.HalfSize, p.Y + p.HalfSize, 255, 255, 255, 255);
            }
        }        

        private Particle.Y_Direction Random_Y_Direction()
        {
            double r = random.NextDouble();

            if (r < 0.5)
            {
                return Particle.Y_Direction.Bottom;
            }

            return Particle.Y_Direction.Top;
        }
    }
}
