using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapaEstelar
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Eventing.Reader;
    using System.Drawing;
    using System.Windows.Forms;

    public class GalaxyMapForm : Form
    {
        private GalaxyMap galaxyMap;
        private int numSystems = 10; /*Cantidad de sistemas solares que se generan en el mapa, la idea sería hacerlos una constante en un fichero de configuración*/
        private StarSystem selectedSystem;
        
        private Panel detailsPanel;
        private Panel testeoPanel;
        private Panel estelarMapPanel;
        private Panel solarMapPanel;


        private ContextMenuStrip contextMenu;

        private float zoomFactor = 1.0f;
        private int offsetX = 0, offsetY = 0;
        private Point lastMousePosition;
        private bool isPanning;


        public GalaxyMapForm()
        {
            Text = "Stellar Map";
            Width = 1000;
            Height = 650;
            //BackColor = Color.Black;
            BackColor = Color.Blue;
            

            galaxyMap = new GalaxyMap(this.numSystems, 800, 600);
            this.MouseClick += GalaxyMapForm_MouseClick;

            //this.MouseWheel += GalaxyMapForm_MouseWheel;
            //this.MouseDown += GalaxyMapForm_MouseDown;
            //this.MouseUp += GalaxyMapForm_MouseUp;
            //this.MouseMove += GalaxyMapForm_MouseMove;

            // Panel que muestra el mapa estelar

            estelarMapPanel = new Panel()
            {
                
                Width = 800,
                Height = 600,
                BackColor = Color.Black


            };
            // Suscribirse al evento Paint del Panel
            estelarMapPanel.Paint += new PaintEventHandler(estelarMapPanel_Paint);
            // Suscribirse al evento de click
            estelarMapPanel.MouseClick += GalaxyMapForm_MouseClick;
            //Evento importante para mostrar detalles del sistema solar en el panel
            estelarMapPanel.MouseClick += ViewDetails_Click;



            this.Controls.Add(estelarMapPanel);

            // Panel que muestra el sistema solar

            solarMapPanel = new Panel()
            {
                Width = 800,
                Height = 600,
                BackColor = Color.Azure


            };
            // Suscribirse al evento Paint del Panel
            //estelarMapPanel.Paint += new PaintEventHandler(estelarMapPanel_Paint);
            // Suscribirse al evento de click
            //estelarMapPanel.MouseClick += GalaxyMapForm_MouseClick;
            //Evento importante para mostrar detalles del sistema solar en el panel
            //estelarMapPanel.MouseClick += ViewDetails_Click;
            solarMapPanel.Visible = false;


            this.Controls.Add(solarMapPanel);

            // Panel para probar cosas

            testeoPanel = new Panel()
            {
                Width = 200,
                Height = 600,
                Left = 800,
                BackColor = Color.Red
            };
            this.Controls.Add(testeoPanel);

            
            detailsPanel = new Panel()
            {
                Width = 200,
                Height = 150,
                //Left = 800,
                BackColor = Color.Black
            };
            testeoPanel.Controls.Add(detailsPanel);
            

            contextMenu = new ContextMenuStrip();
            //contextMenu.Items.Add("View Details", null, ViewDetails_Click);
            contextMenu.Items.Add("Create Route", null, CreateRoute_Click);
            contextMenu.Items.Add("Mostrar Sistema Solar", null, ShowSolarSystem_Click);


        }
        /*
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
        */

        private void GalaxyMapForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                foreach (var system in galaxyMap.StarSystems)
                {
                    //int scaledX = (int)((system.X + offsetX) * zoomFactor);
                    //int scaledY = (int)((system.Y + offsetY) * zoomFactor);

                    if (Math.Abs(e.X - system.X) < 10 && Math.Abs(e.Y - system.Y) < 10)
                    {
                        selectedSystem = system;
                        contextMenu.Show(this, e.Location);
                        //this.Invalidate();
                        estelarMapPanel.Invalidate(); /*Linea importante para que se actualice el puntero*/
                        break;
                    }
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                foreach (var system in galaxyMap.StarSystems)
                {
                    int scaledX = (int)((system.X + offsetX) * zoomFactor);
                    int scaledY = (int)((system.Y + offsetY) * zoomFactor);

                    if (Math.Abs(e.X - system.X) < 10 && Math.Abs(e.Y - system.Y) < 10)
                    {
                        selectedSystem = system;
                        //contextMenu.Show(this, e.Location);
                        //UpdateDetailsPanel();
                        //this.Invalidate();
                        estelarMapPanel.Invalidate(); /*Linea importante para que se actualice el puntero*/
                        break;
                    }
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



        private void ViewDetails_Click(object sender, EventArgs e)
        {
            UpdateDetailsPanel();
        }

        private void CreateRoute_Click(object sender, EventArgs e)
        {
            // Aquí podrías implementar la lógica para crear una ruta
            MessageBox.Show($"Creating route to {selectedSystem.Name}");
        }

        // Método para mostrar información del sistema clickeado
        //private void ShowSystemDetails(Graphics g, StarSystem system)
        //{
        //    g.DrawString("Selected System:", new Font("Arial", 10, FontStyle.Bold), Brushes.White, 10, 10);
        //    g.DrawString($"Name: {system.Name}", new Font("Arial", 8), Brushes.White, 10, 30);
        //    g.DrawString($"Position: ({system.X}, {system.Y})", new Font("Arial", 8), Brushes.White, 10, 50);
        //    g.DrawString($"Connections: {system.ConnectedSystems.Count}", new Font("Arial", 8), Brushes.White, 10, 70);
        //}

        private void estelarMapPanel_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            foreach (var system in galaxyMap.StarSystems)
            {
                // Dibujar los sistemas estelares
                //int scaledX = (int)((system.X + offsetX) * zoomFactor);
                //int scaledY = (int)((system.Y + offsetY) * zoomFactor);
                //Brush brush = (system == selectedSystem) ? Brushes.Green : Brushes.White;
                g.FillEllipse(Brushes.Yellow, system.X - 5, system.Y - 5, 10, 10);
                g.DrawString(system.Name, new Font("Arial", 8), Brushes.Red, system.X + 5, system.Y + 5);

                // Dibujar las conexiones
                foreach (var connectedSystem in system.ConnectedSystems)
                {
                    int connectedScaledX = (int)((connectedSystem.X + offsetX) * zoomFactor);
                    int connectedScaledY = (int)((connectedSystem.Y + offsetY) * zoomFactor);
                    
                    g.DrawLine(Pens.Cyan, system.X, system.Y, connectedSystem.X, connectedSystem.Y);
                }

            }

            if (selectedSystem != null)
            {
                //ShowSystemDetails(g, selectedSystem);
                //int scaledX = (int)((selectedSystem.X + offsetX) * zoomFactor);
                //int scaledY = (int)((selectedSystem.Y + offsetY) * zoomFactor);
                int scaledX = (int)selectedSystem.X;
                int scaledY = (int)selectedSystem.Y;
                g.DrawEllipse(Pens.Green, scaledX - 10, scaledY - 10, 20, 20);

                //selectedSystem = null;

                //foreach (var planet in selectedSystem.Planets)
                //{
                //    int planetX = scaledX + (int)(planet.DistanceFromStar * zoomFactor);
                //    g.FillEllipse(Brushes.Blue, planetX - 3, scaledY - 3, 6, 6);
                //    g.DrawString(planet.Name, new Font("Arial", 8), Brushes.Blue, planetX + 5, scaledY + 5);
                //}

                Console.WriteLine("evento desencadenado");
            }


        }


        private void ShowSolarSystem_Click(object sender, EventArgs e)
        {
            if (selectedSystem != null && selectedSystem.ConnectedSystems.Count > 0)
            {
                //zoomFactor = 3.0f; // Aumentar el zoom al nivel deseado para mostrar el sistema solar
                //offsetX = -selectedSystem.X + (800 / (int)(2 * zoomFactor));
                //offsetY = -selectedSystem.Y + (600 / (int)(2 * zoomFactor));
                //UpdateDetailsPanel();
                estelarMapPanel.Visible = false;
                solarMapPanel.Visible = true;
                this.Invalidate();
            }
        }









        // Métodos de Zoom y Paneo
        /*
        private void GalaxyMapForm_MouseWheel(object sender, MouseEventArgs e)
        {
            float oldZoomFactor = zoomFactor;
            if (e.Delta > 0)
            {
                zoomFactor *= 1.1f;
            }
            else
            {
                zoomFactor /= 1.1f;
            }

            // Ajustar la posición de desplazamiento para centrarse en el punto de la rueda del mouse
            offsetX = (int)((offsetX - e.X / oldZoomFactor) * (zoomFactor / oldZoomFactor) + e.X / zoomFactor);
            offsetY = (int)((offsetY - e.Y / oldZoomFactor) * (zoomFactor / oldZoomFactor) + e.Y / zoomFactor);

            this.Invalidate(); // Redibujar la pantalla con el nuevo factor de zoom
        }

        private void GalaxyMapForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastMousePosition = e.Location;
                isPanning = true;
            }
        }

        private void GalaxyMapForm_MouseUp(object sender, MouseEventArgs e)
        {
            isPanning = false;
        }

        private void GalaxyMapForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPanning)
            {
                offsetX += (e.X - lastMousePosition.X);
                offsetY += (e.Y - lastMousePosition.Y);
                lastMousePosition = e.Location;
                this.Invalidate(); // Redibujar la pantalla con el nuevo desplazamiento
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
