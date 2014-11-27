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
    public partial class Casilla : UserControl
    {
        public event CanDrawEventHandler CanDrawCasilla;//evento de click en una casilla
        public enum EstadoCasilla { Activa, Obstaculo, Ciego, FantasmaP, FantasmaC, Migajas, Meta, Estresado, Finish }
        private EstadoCasilla estado;
        private Point coordenadas;

        //constructor 
        public Casilla()
        {
            InitializeComponent();
            this.estado = EstadoCasilla.Activa;
            this.pictureBox1.Click += new EventHandler(pictureBox1_Click);
        }
     
        //constructor que le pone el estado activo y las coordenadas
        public Casilla(int x, int y)
        {
            InitializeComponent();
            this.coordenadas.X = x;
            this.coordenadas.Y = y;
            this.estado = EstadoCasilla.Activa;
            this.pictureBox1.Click += new EventHandler(pictureBox1_Click);
        }

        //cuando le das click a la casilla, pasa esto
        public void pictureBox1_Click(object sender, EventArgs e)
        {
            if (estado == EstadoCasilla.Activa)
            {
                if (CanDrawCasilla != null)
                {          
                    CanDrawCasilla(this);
                }
            }
        }

        //le cambia el estado a la casilla y le pone el picturebox
        public void SetEstadoCasilla(EstadoCasilla edo)  
        {
            this.estado = edo;
            switch(edo){
                case (EstadoCasilla.Activa):
                    this.pictureBox1.Image = null;
                    break;
                case(EstadoCasilla.Ciego):
                    this.pictureBox1.Image = global::Tarea1.Properties.Resources.PaperMario;
                break;
                case(EstadoCasilla.FantasmaP):             
                    this.pictureBox1.Image = global::Tarea1.Properties.Resources.fantasmap;
                break;
                case (EstadoCasilla.FantasmaC):
                    this.pictureBox1.Image = global::Tarea1.Properties.Resources.fantasmac;
                break;
                case (EstadoCasilla.Meta):
                    this.pictureBox1.Image = global::Tarea1.Properties.Resources.meta2;
                break;
                case (EstadoCasilla.Obstaculo):
                    this.pictureBox1.Image = global::Tarea1.Properties.Resources.obstaculo2;
                break;
                case (EstadoCasilla.Migajas):
                    this.pictureBox1.Image = global::Tarea1.Properties.Resources.migajas;
                break;
                case (EstadoCasilla.Estresado):
                    this.pictureBox1.Image = global::Tarea1.Properties.Resources.FlappingMario;
                break;
                case (EstadoCasilla.Finish):
                    this.pictureBox1.Image = global::Tarea1.Properties.Resources.finish;
                break;
            }                      
        } 
          
        public EstadoCasilla GetEstadoCasilla()
        {
            return this.estado;
        }

        public Point GetUbicacion()
        {
            return this.coordenadas;
        }
    }

    public delegate void CanDrawEventHandler(Object sender);
}


