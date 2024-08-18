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
        private StarSystem selectedSystem;
        private Panel detailsPanel;
        //private ContextMenuStrip contextMenu;
        public GalaxyMapForm()
        {
            this.Text = "Stellar Map";
            this.Width = 800;
            this.Height = 600;
            this.BackColor = Color.Black;

            galaxyMap = new GalaxyMap(10, 800, 600);
            this.MouseClick += GalaxyMapForm_MouseClick;

            detailsPanel = new Panel()
            {
                Width = 200,
                Height = this.Height,
                Left = 800,
                BackColor = Color.Black
            };
            this.Controls.Add(detailsPanel);


        }

        private void GalaxyMapForm_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var system in galaxyMap.StarSystems)
            {
                if (Math.Abs(e.X - system.X) < 10 && Math.Abs(e.Y - system.Y) < 10)
                {
                    selectedSystem = system;
                    //UpdateDetailsPanel(); // Actualizar el panel de detalles
                    this.Invalidate(); // Redibujar la pantalla para reflejar la selección
                    break;
                }
            }
        }

        private void UpdateDetailsPanel()
        {
            detailsPanel.Controls.Clear();

            if (selectedSystem != null)
            {
                
                Label titleLabel = new Label()
                {
                    Text = "System Details",
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Top = 10,
                    Left = 10,
                    AutoSize = true
                };
                detailsPanel.Controls.Add(titleLabel);
                

                Label nameLabel = new Label()
                {
                    Text = $"Name: {selectedSystem.Name}",
                    ForeColor = Color.White,
                    Font = new Font("Arial", 8),
                    Top = 40,
                    Left = 10,
                    AutoSize = true
                };
                detailsPanel.Controls.Add(nameLabel);

                Label positionLabel = new Label()
                {
                    Text = $"Position: ({selectedSystem.X}, {selectedSystem.Y})",
                    ForeColor = Color.White,
                    Font = new Font("Arial", 8),
                    Top = 60,
                    Left = 10,
                    AutoSize = true
                };
                detailsPanel.Controls.Add(positionLabel);

                Label connectionsLabel = new Label()
                {
                    Text = $"Connections: {selectedSystem.ConnectedSystems.Count}",
                    ForeColor = Color.White,
                    Font = new Font("Arial", 8),
                    Top = 80,
                    Left = 10,
                    AutoSize = true
                };
                detailsPanel.Controls.Add(connectionsLabel);
            }
        }

        private void ShowSystemDetails(Graphics g, StarSystem system)
        {
            g.DrawString("Selected System:", new Font("Arial", 10, FontStyle.Bold), Brushes.White, 10, 10);
            g.DrawString($"Name: {system.Name}", new Font("Arial", 8), Brushes.White, 10, 30);
            g.DrawString($"Position: ({system.X}, {system.Y})", new Font("Arial", 8), Brushes.White, 10, 50);
            g.DrawString($"Connections: {system.ConnectedSystems.Count}", new Font("Arial", 8), Brushes.White, 10, 70);
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

            if (selectedSystem != null)
            {
                ShowSystemDetails(g, selectedSystem);
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
