using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapaEstelar
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class GalaxyMapForm : Form
    {
        private GalaxyMap galaxyMap;

        public GalaxyMapForm()
        {
            this.Text = "Stellar Map";
            this.Width = 800;
            this.Height = 600;
            this.BackColor = Color.Black;

            galaxyMap = new GalaxyMap(10, 800, 600);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            foreach (var system in galaxyMap.StarSystems)
            {
                // Dibujar los sistemas estelares
                g.FillEllipse(Brushes.Yellow, system.X - 5, system.Y - 5, 10, 10);
                g.DrawString(system.Name, new Font("Arial", 8), Brushes.Red, system.X + 5, system.Y + 5);

                // Dibujar las conexiones
                foreach (var connectedSystem in system.ConnectedSystems)
                {
                    g.DrawLine(Pens.Cyan, system.X, system.Y, connectedSystem.X, connectedSystem.Y);
                }
            }
        }
        /*
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new GalaxyMapForm());
        }
        */
    }

}
