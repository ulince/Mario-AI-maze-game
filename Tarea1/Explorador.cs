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
    public class Explorador
    {
        private int columnas, filas; 
        public MyPathNode[,] estadosCasillas;  //matriz de etados de casillas
        private SpatialAStar<MyPathNode, Object> aStar; //propiedad  de tipo A-Star, algoritmo de busqueda
        private LinkedList<Explorador.MyPathNode> path; //lista con el camino a la salida
        private LinkedList<Explorador.MyPathNode>.Enumerator iterador; //iterador de la lista
        
        public class MyPathNode : IPathNode<Object>
        {
            public Point coordenadas { get; set; }
            public Casilla.EstadoCasilla estado;
            public Boolean isWall { get; set; }

            public bool IsWalkable(Object unused) //pregunta si la casilla es un obstaculo
            {
                return !isWall;
            }

            public String ToString(MyPathNode nodo)
            {
                return nodo.coordenadas.ToString();
            }
        }

        //constructor
        public Explorador()
        {
        }

        //otro contructor
        public Explorador(int x, int y)
        {
            columnas = x;
            filas = y;
        }

        public void SetColumnas(int x)
        {
            columnas = x;
        }

        public void SetFilas(int y)
        {
            filas = y;
        }
        
        //crea la matriz de estados de casillas a partir del mundo
        public void CrearMatriz(Mundo m)
        {
            estadosCasillas = new MyPathNode[columnas, filas];
            for (int x = 0; x < columnas; x++)
            {
                for (int y = 0; y < filas; y++)
                {
                    Point p = new Point(x,y);
                    Casilla cas = (Casilla)m.tableLayoutPanel1.GetControlFromPosition(x, y); 
                    Casilla.EstadoCasilla edo = cas.GetEstadoCasilla();
                    estadosCasillas[x, y] = new MyPathNode()
                    {
                        coordenadas = p,
                        estado = edo
                    };
                    if (edo == Casilla.EstadoCasilla.Activa)
                    {
                        estadosCasillas[x, y].isWall = false;
                    }
                    else
                    {
                        estadosCasillas[x, y].isWall = true;
                    }
                }
            }  
        }

        //crea una instancia de SpatialStar, algoritmo de busqueda
        public void SetSolver()
        {
            this.aStar = new SpatialAStar<MyPathNode, Object>(estadosCasillas); 
        }

        //invoca la busqueda del camino mas corto y regresa un lista ligada de tipo MyPathNode, la asigna a path
        public void CaclPath(Point ciego, Point salida) 
        {
            path = aStar.Search(ciego,salida, null);
            if (path != null)
            {       
                iterador = path.GetEnumerator();
                iterador.MoveNext();
            }
            else
            {
                Form1.noHaySalida = true;
            }
        }

        //regresa un Point, con las coordenadas del siguiente elemento de la lista
        public Point nextMove()
        {
            iterador.MoveNext();
            return iterador.Current.coordenadas;
        }

        //recibe un Point,y pone en esasa coordenadas de su matriz un fantasma confirmado
        public void SetNewPhantom(Point p)
        {
            estadosCasillas[p.X, p.Y].estado = Casilla.EstadoCasilla.FantasmaC;
            estadosCasillas[p.X, p.Y].isWall = true;
        }

        public LinkedList<Explorador.MyPathNode> GetPath()
        {
            return path;
        }
    }
}
