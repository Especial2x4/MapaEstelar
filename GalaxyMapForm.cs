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
            
            // CARACTERÍSTICAS DE LA VENTANA
            
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // para que no se redimensione
            this.StartPosition = FormStartPosition.CenterScreen; // para que aparezca la interfaz en el cento de la pantalla
            this.MaximizeBox = false;
            this.Text = "Stellar Map";
            this.Width = 1000;
            this.Height = 650;
            //BackColor = Color.Black;
            this.BackColor = Color.Blue;
            
            

            galaxyMap = new GalaxyMap(this.numSystems, 800, 600); // se crea el GalaxyMap
            //this.MouseClick += GalaxyMapForm_MouseClick; // creo que esto no hace nada

            

            // Panel que muestra el mapa estelar -------------------------------------------------------------------------------

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

            // Panel que muestra el sistema solar ------------------------------------------------------------------------------

            solarMapPanel = new Panel()
            {
                Width = 800,
                Height = 600,
                BackColor = Color.Azure


            };

            // Suscribirse al evento Paint del Panel
            solarMapPanel.Paint += new PaintEventHandler(estelarMapPanel_Paint); // dibuja el sistema
            // Suscribirse al evento de click
            solarMapPanel.MouseClick += SolarMapForm_MouseClick; // responde a los eventos de click
            //Evento importante para mostrar detalles del sistema solar en el panel
            solarMapPanel.MouseClick += ViewDetails_Click; // muestra los detalles

            Button backButton = new Button()
            {
                Text = "Volver",
                //ForeColor = Color.White,
                //Font = new Font("Arial", 10, FontStyle.Bold),
                Top = 10,
                Left = 10,
                AutoSize = true
            };
            // Asignar el evento Click al botón
            backButton.Click += BackButton_Click;

            solarMapPanel.Controls.Add(backButton);


            // Suscribirse al evento Paint del Panel
            //estelarMapPanel.Paint += new PaintEventHandler(estelarMapPanel_Paint);
            // Suscribirse al evento de click
            //estelarMapPanel.MouseClick += GalaxyMapForm_MouseClick;
            //Evento importante para mostrar detalles del sistema solar en el panel
            //estelarMapPanel.MouseClick += ViewDetails_Click;
            solarMapPanel.Visible = false;


            this.Controls.Add(solarMapPanel);

            // Panel para probar cosas -----------------------------------------------------------------------------------------

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
        
        // EVENTO DE CLICK EN PANEL DE GALAXIA

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

        // EVENTO DE CLICK EN PANEL DE SISTEMA SOLAR

        private void SolarMapForm_MouseClick(object sender, MouseEventArgs e)
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
                        solarMapPanel.Invalidate(); /*Linea importante para que se actualice el puntero*/
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
                        solarMapPanel.Invalidate(); /*Linea importante para que se actualice el puntero*/
                        break;
                    }
                }
            }
        }

        // PANEL DE DETALLES EN PANEL DE GALAXIAS

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


        // EVENTO DE CLICKS EN PANEL DE GALAXIA PARA MENÚ CONTEXTUAL

        private void ViewDetails_Click(object sender, EventArgs e)
        {
            UpdateDetailsPanel();
        }

        private void CreateRoute_Click(object sender, EventArgs e)
        {
            // Aquí podrías implementar la lógica para crear una ruta
            MessageBox.Show($"Creating route to {selectedSystem.Name}");
        }

        
        // EVENTO QUE SE ENCARGA DE DIBUJAR EL MAPA ESTELAR EN PANEL DE GALAXIA

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


                
            }


        }


        // ACÁ IRÍA EL EVENTO PARA DIBUJAR EL SISTEMA SOLAR SELECCIONADO


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

        // Evento para el botón de volver
        private void BackButton_Click(object sender, EventArgs e)
        {
            solarMapPanel.Visible = false; // Ocultar el panel
            estelarMapPanel.Visible= true;
        }


        
    }

}
