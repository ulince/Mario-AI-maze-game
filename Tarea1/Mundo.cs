using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tarea1
{
    public partial class Mundo : UserControl
    {

        int x, y; //columnas, fila

        public int getX() //columnas
        {
            return this.x;
        }

        public int getY() //filas
        {
            return this.y;
        }

        public Mundo()
        {
            InitializeComponent();
        }

        //constructor que llena el mundo con las casillas Activas
        public Mundo(int x, int y)
        {
            InitializeComponent();
            this.x = x;
            this.y = y;        
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = x;
            this.tableLayoutPanel1.RowCount = y;
            this.tableLayoutPanel1.RowStyles.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.SuspendLayout();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Casilla casilla = new Casilla(i, j);
                    casilla.Dock = DockStyle.Fill;
                    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, (100 / y)));
                    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (100 / x)));
                    this.tableLayoutPanel1.Controls.Add(casilla, i, j);
                    casilla.CanDrawCasilla += new CanDrawEventHandler(casilla_CanDrawCasilla);
                }
            } this.tableLayoutPanel1.ResumeLayout();
        }

        //evento 
        void casilla_CanDrawCasilla(object sender)
        {
            Casilla c = (Casilla)sender;
            if (!Form1.inicio & !Form1.primerPaso & !Form1.pusoAlCiego & !Form1.pusoLaMeta)
            {
                if (c.GetEstadoCasilla() == Casilla.EstadoCasilla.Activa)
                {
                    c.SetEstadoCasilla(Casilla.EstadoCasilla.Obstaculo);               
                }
            }

            if (Form1.inicio & !Form1.primerPaso & !Form1.pusoAlCiego & !Form1.pusoLaMeta)
            {
                if (c.GetEstadoCasilla() == Casilla.EstadoCasilla.Activa)
                {
                    c.SetEstadoCasilla(Casilla.EstadoCasilla.Ciego);
                    Form1.pusoAlCiego = true;
                    Form1.posCiego = c.GetUbicacion();
                }
            }

            if (Form1.inicio & !Form1.primerPaso & Form1.pusoAlCiego & !Form1.pusoLaMeta)
            {
                if (c.GetEstadoCasilla() == Casilla.EstadoCasilla.Activa)
                {
                    c.SetEstadoCasilla(Casilla.EstadoCasilla.Meta);
                    Form1.pusoLaMeta = true;
                    Form1.posMeta = c.GetUbicacion();
                }
            }

            if (Form1.inicio & Form1.primerPaso & Form1.pusoAlCiego & Form1.pusoLaMeta)
            {
                if (c.GetEstadoCasilla() == Casilla.EstadoCasilla.Migajas || c.GetEstadoCasilla() == Casilla.EstadoCasilla.Activa)
                {
                    c.SetEstadoCasilla(Casilla.EstadoCasilla.FantasmaP);
                }
            }
        }
    }
}
