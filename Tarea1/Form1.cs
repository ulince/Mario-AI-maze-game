using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tarea1
{
    public partial class Form1 : Form
    {
        private const string regExpr = "([0-9]|[\b])";  //expresión regular para validar los Textbox
        private Mundo m;
        private Ciego ciego;
        private Explorador explorador;

        public static bool inicio = false;
        public static bool primerPaso = false;
        public static bool pusoAlCiego = false;
        public static bool pusoLaMeta = false;
        public static bool noHaySalida = false;

        public static Point posCiego, posMeta, posFantasma; //guarda la las posiciones

        private List<string> nodos = new List<string>(); //lista de strings, tiene lo que se va a mostrar enla listbox

        //constructor
        public Form1()
        {
            InitializeComponent();
            ciego = new Ciego();
            explorador = new Explorador();       
        }

        //Boton Generar. 
        private void button1_Click(object sender, EventArgs e) 
        {
            this.panel1.Controls.Clear();
            inicio = false;
            if (this.textBox1.Text != "" && this.textBox2.Text != "")
            {
                int x = Int32.Parse(this.textBox1.Text);
                int y = Int32.Parse(this.textBox2.Text);
                
                if (x != 0 && y != 0)
                {
                    m = new Mundo(x, y);
                    m.Dock = DockStyle.Fill;
                    this.panel1.Controls.Add(m);

                    primerPaso = false;
                    pusoLaMeta = false;
                    pusoAlCiego = false;
                    noHaySalida = false;

                    explorador.SetColumnas(x);
                    explorador.SetFilas(y);
                    this.listBox1.Items.Clear();
                    this.listBox1.Items.Add("Log:");
                }
            }
        }

       //Botón iniciar
        private void button2_Click(object sender, EventArgs e)
        {
            if (m != null)
            {
                inicio = true;
                ciego.setMundo(m);
                explorador.CrearMatriz(m);
                explorador.SetSolver();
            }
        }

        //Botón Paso a paso.
        private void button3_Click(object sender, EventArgs e)
        {
            Avanzar();
            print();
        }

        //manda a la listbox
        private void print()
        {
            if (nodos.Count > 0)
            {
                this.listBox1.Items.Add(nodos.Last());
            }
        }

        //valida la textbox1
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(e.KeyChar.ToString(), regExpr);
            if (!m.Success)
                e.Handled = true;
        }

        //valida la textbox2
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(e.KeyChar.ToString(), regExpr);
            if (!m.Success)
                e.Handled = true;
        }

        //botón play
        private void button4_Click(object sender, EventArgs e) 
        {
            BackgroundWorker bgWorker;
            bgWorker = new BackgroundWorker { WorkerReportsProgress = true };
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.RunWorkerAsync();
        }

        //reporta el Background worker
        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            print();
        }

        //muestra game OVer cuando termina la búsqueda
        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            nodos.Add("Game Over");
            print();
        }

        //hace el play en el background worker
        void bgWorker_DoWork(object sender, DoWorkEventArgs e) //background 
        {
            BackgroundWorker bgWorker = sender as BackgroundWorker;
            
            while (posCiego != posMeta && inicio == true)
            {
                Avanzar();          
                System.Threading.Thread.Sleep(1300);
                bgWorker.ReportProgress(10);          
            }
        }

        //Para dar un paso
        public void Avanzar()
        {
            if (pusoLaMeta)
            {
                if (!primerPaso)
                {
                    ciego.SetUbicacion(posCiego);
                    explorador.CaclPath(posCiego, posMeta);
                    primerPaso = true;
                }
                if (primerPaso)
                {
                    if (!noHaySalida)
                    {
                        Point nextMove = explorador.nextMove();
                        nodos.Add("Player 1: " + nextMove.ToString());
                        if (ciego.PossibleMove(nextMove))
                        {
                            Casilla c = (Casilla)m.tableLayoutPanel1.GetControlFromPosition(ciego.GetUbicacion().X, ciego.GetUbicacion().Y);
                            c.SetEstadoCasilla(Casilla.EstadoCasilla.Activa);
                            ciego.SetUbicacion(nextMove); 
                            posCiego = nextMove;
                            c = (Casilla)m.tableLayoutPanel1.GetControlFromPosition(ciego.GetUbicacion().X, ciego.GetUbicacion().Y);
                            if (c.GetEstadoCasilla() == Casilla.EstadoCasilla.Meta)
                            {
                                c.SetEstadoCasilla(Casilla.EstadoCasilla.Finish);
                            }
                            else
                            {
                                c.SetEstadoCasilla(Casilla.EstadoCasilla.Ciego);
                            }
                        }
                        else
                        {
                            nodos.Add("Mario se encontró un Boo!");
                            explorador.SetNewPhantom(nextMove);
                            Casilla c = (Casilla)m.tableLayoutPanel1.GetControlFromPosition(nextMove.X, nextMove.Y);
                            c.SetEstadoCasilla(Casilla.EstadoCasilla.FantasmaC);
                            explorador.CaclPath(posCiego, posMeta);
                        }
                    }
                    else
                    {
                        Casilla c = (Casilla)m.tableLayoutPanel1.GetControlFromPosition(ciego.GetUbicacion().X, ciego.GetUbicacion().Y);
                        c.SetEstadoCasilla(Casilla.EstadoCasilla.Estresado); //estresado
                        posCiego = posMeta;
                    }
                }
            }
        }

    }
}
