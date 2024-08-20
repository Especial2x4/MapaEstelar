using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapaEstelar
{
    internal class GalaxyMap
    {
        public List<StarSystem> StarSystems { get; set; }


        public GalaxyMap(int numSystems, int width, int height)
        {
            StarSystems = new List<StarSystem>();
            Random rand = new Random();

            for (int i = 0; i < numSystems; i++)
            {
                string name = NameGenerator.GenerateStarSystemName();
                int x = rand.Next(0, width);
                int y = rand.Next(0, height);

                var system = new StarSystem(name, x, y);

                // Agregar planetas al sistema estelar
                system.AddPlanet("Planet A", rand.Next(10, 50));
                system.AddPlanet("Planet B", rand.Next(60, 100));
                system.AddPlanet("Planet C", rand.Next(110, 150));

                StarSystems.Add(new StarSystem(name, x, y));
            }

            // Conectar sistemas estelares
            ConnectSystems();
        }

        private void ConnectSystems()
        {
            foreach (var system in StarSystems)
            {
                // Conecta cada sistema a uno o dos sistemas cercanos
                var nearestSystems = StarSystems
                    .Where(s => s != system)
                    .OrderBy(s => GetDistance(system, s))
                    .Take(2);

                foreach (var nearestSystem in nearestSystems)
                {
                    if (!system.ConnectedSystems.Contains(nearestSystem))
                    {
                        system.ConnectTo(nearestSystem);
                    }
                }
            }
        }

        private double GetDistance(StarSystem a, StarSystem b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}
