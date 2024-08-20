using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MapaEstelar
{
    internal class StarSystem
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<StarSystem> ConnectedSystems { get; set; }
        public List<Planet> Planets { get; set; } = new List<Planet>();

        public StarSystem(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
            ConnectedSystems = new List<StarSystem>();
        }

        public void AddPlanet(string name, int distance)
        {
            Planets.Add(new Planet { Name = name, DistanceFromStar = distance });
        }

        public void ConnectTo(StarSystem otherSystem)
        {
            ConnectedSystems.Add(otherSystem);
            otherSystem.ConnectedSystems.Add(this);
        }
    }
}
