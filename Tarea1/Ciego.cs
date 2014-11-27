using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Data;

namespace Tarea1
{
    class Ciego
    {

        private Mundo mundoReal; //mundo del ciego
        private Point ubicacion; //ubicacion del ciego

        //constructor vacío
        public Ciego()
        {
        }

        //constructor que recibe un mundo y lo asigna a mundoReal
        public Ciego(Mundo m)
        {
            this.mundoReal = m;
        }

        //recibe un Point y verifica en mundoReal si se puede mover ahí
        public bool PossibleMove(Point p)
        {
            Casilla casilla = (Casilla)mundoReal.tableLayoutPanel1.GetControlFromPosition(p.X, p.Y);
            if (casilla.GetEstadoCasilla() == Casilla.EstadoCasilla.Obstaculo | casilla.GetEstadoCasilla() == Casilla.EstadoCasilla.FantasmaP | casilla.GetEstadoCasilla() == Casilla.EstadoCasilla.FantasmaC)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //recibe un Point y lo asigna a su ubicación
        public void SetUbicacion(Point p)
        {
            this.ubicacion = p;
        }

        //recibe unas coordenadas, pone un fantasmaC en su mundo y regresa el mismo Point
        public Point SetNewPhantom(Point p)
        {
            Casilla casilla = (Casilla)mundoReal.tableLayoutPanel1.GetControlFromPosition(p.X, p.Y); 
            casilla.SetEstadoCasilla(Casilla.EstadoCasilla.FantasmaC);
             return p;   
        }

        public void setMundo(Mundo m)
        {
            this.mundoReal = m;
        }

        public Point GetUbicacion()
        {
            return this.ubicacion;
        }
    }
}
