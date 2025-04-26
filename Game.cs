using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace programGraph
{
    internal class Game : GameWindow
    {
        private Escenario escenario;
        private bool isMouseDown = false;
        private Vector2 lastMousePos;
        private float pitch = 0.0f; // Ángulo de rotación alrededor del eje X
        private float yaw = 0.0f;   // Ángulo de rotación alrededor del eje Y
        private float zoom = 5.0f;  // Distancia de la cámara
        private string objetoSeleccionado = null;

        public Game(int width, int height)
              : base(width, height, GraphicsMode.Default, "U 3D")
        {
            VSync = VSyncMode.On; // Activamos la sincronización vertical para evitar desgarros
        }

        // Método que se llama cuando el juego se inicia
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Establecer el color de fondo de la ventana (negro)
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f); // Negro (RGB: 0.1, 0.1, 0.1)
            GL.Enable(EnableCap.DepthTest); // Habilitamos el test de profundidad para la renderización 3D

            // Configuración de la proyección en perspectiva
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                   MathHelper.DegreesToRadians(45.0f), // Ángulo de visión de 45 grados
                   Width / (float)Height,              // Relación de aspecto
                   0.1f,                               // Distancia más cercana de la cámara
                   100.0f                              // Distancia más lejana de la cámara
               );
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            // Creamos los puntos que definirán la "U"
            List<Punto> puntosU = new List<Punto>
            {
                new Punto(-1, 1,  0.5f), new Punto( 1, 1,  0.5f), new Punto( 1, -1, 0.5f), new Punto(-1, -1, 0.5f),
                new Punto(-1, 1, -0.5f), new Punto( 1, 1, -0.5f), new Punto( 1, -1, -0.5f), new Punto(-1, -1, -0.5f),
                new Punto(-0.7f, 1,  0.5f), new Punto( 0.7f, 1,  0.5f), new Punto( 0.7f, -0.7f, 0.5f), new Punto(-0.7f, -0.7f, 0.5f),
                new Punto(-0.7f, 1, -0.5f), new Punto( 0.7f, 1, -0.5f), new Punto( 0.7f, -0.7f, -0.5f), new Punto(-0.7f, -0.7f, -0.5f)
            };

            // Creamos los polígonos que forman la "U" (roja en su totalidad)
            List<Poligono> poligonosU = new List<Poligono>
            {
                new Poligono(new List<Punto> { puntosU[4], puntosU[12], puntosU[15], puntosU[7] }, 1f, 1f, 1f),  // Blanco
                new Poligono(new List<Punto> { puntosU[7], puntosU[15], puntosU[14], puntosU[6] }, 1f, 1f, 1f),  // Blanco
                new Poligono(new List<Punto> { puntosU[13], puntosU[5], puntosU[6], puntosU[14] }, 1f, 1f, 1f), // Blanco
                new Poligono(new List<Punto> { puntosU[0], puntosU[8], puntosU[11], puntosU[3] }, 1f, 1f, 1f),  // Blanco
                new Poligono(new List<Punto> { puntosU[3], puntosU[11], puntosU[10], puntosU[2] }, 1f, 1f, 1f), // Blanco
                new Poligono(new List<Punto> { puntosU[9], puntosU[1], puntosU[2], puntosU[10] }, 1f, 1f, 1f),  // Blanco
                new Poligono(new List<Punto> { puntosU[0], puntosU[4], puntosU[7], puntosU[3] }, 1f, 1f, 1f),   // Blanco
                new Poligono(new List<Punto> { puntosU[8], puntosU[12], puntosU[15], puntosU[11] }, 1f, 1f, 1f), // Blanco
                new Poligono(new List<Punto> { puntosU[1], puntosU[5], puntosU[6], puntosU[2] }, 1f, 1f, 1f),   // Blanco
                new Poligono(new List<Punto> { puntosU[9], puntosU[13], puntosU[14], puntosU[10] }, 1f, 1f, 1f), // Blanco
                new Poligono(new List<Punto> { puntosU[3], puntosU[7], puntosU[6], puntosU[2] }, 1f, 1f, 1f),   // Blanco
                new Poligono(new List<Punto> { puntosU[11], puntosU[15], puntosU[14], puntosU[10] }, 1f, 1f, 1f),// Blanco
                new Poligono(new List<Punto> { puntosU[0], puntosU[8], puntosU[12], puntosU[4] }, 1f, 1f, 1f),  // Blanco
                new Poligono(new List<Punto> { puntosU[9], puntosU[1], puntosU[5], puntosU[13] }, 1f, 1f, 1f)   // Blanco
            };

            // Creamos la parte que contiene los polígonos
            Parte partesU = new Parte();
            foreach (var poligono in poligonosU)
            {
                partesU.AddPoligono(poligono);
            }

            // Creamos el escenario y agregamos las "U"s
            escenario = new Escenario();

            // Creamos 3 objetos "U" con diferentes posiciones
            Objeto u1 = new Objeto(); u1.Addparte(partesU); u1.setCentro(new Punto(3, 0, 0));
            Objeto u2 = new Objeto(); u2.Addparte(partesU); u2.setCentro(new Punto(0, 3, 0));
            Objeto u3 = new Objeto(); u3.Addparte(partesU); u3.setCentro(new Punto(0, 0, 3));

            // Añadimos los objetos al escenario
            //escenario.AddObjeto("U1", u1);
            //escenario.AddObjeto("U2", u2);
            //escenario.AddObjeto("U3", u3);
        }

        // Método para renderizar los objetos en la escena
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // Limpiar el buffer
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0.0f, 0.0f, -zoom); // Mover la cámara
            GL.Rotate(pitch, 1.0f, 0.0f, 0.0f); // Rotar la cámara en el eje X
            GL.Rotate(yaw, 0.0f, 1.0f, 0.0f);   // Rotar la cámara en el eje Y

            DibujarEjes();      // Dibujar los ejes coordenados
            escenario.DibujarEscenario(); // Dibujar el escenario con las "U"s
            SwapBuffers();      // Intercambiar los buffers para mostrar la imagen renderizada
        }

        // Método para dibujar los ejes en la escena
        private void DibujarEjes()
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(1f, 0f, 0f); GL.Vertex3(-10, 0, 0); GL.Vertex3(10, 0, 0); // Eje X en rojo
            GL.Color3(0f, 1f, 0f); GL.Vertex3(0, -10, 0); GL.Vertex3(0, 10, 0); // Eje Y en verde
            GL.Color3(0f, 0f, 1f); GL.Vertex3(0, 0, -10); GL.Vertex3(0, 0, 10); // Eje Z en azul
            GL.End();
        }

        // Métodos para controlar el movimiento de la cámara con el mouse
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                isMouseDown = true;
                lastMousePos = new Vector2(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButton.Left)
                isMouseDown = false;
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (isMouseDown)
            {
                Vector2 delta = new Vector2(e.X, e.Y) - lastMousePos;
                lastMousePos = new Vector2(e.X, e.Y);
                yaw += delta.X * 0.5f;
                pitch += delta.Y * 0.5f;
            }
        }

        // Método para controlar el zoom con la rueda del mouse
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            zoom -= e.DeltaPrecise * 0.5f;
            zoom = zoom < 1f ? 1f : (zoom > 20f ? 20f : zoom); // Limitar el zoom entre 1 y 20
        }

        // Método que se llama cuando el tamaño de la ventana cambia
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height); // Ajustar el viewport
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            // Guardar/Cargar
            if (e.Key == Key.G)
            {
                Serializador.GuardarEscenario(escenario, "escenario1.json");
            }

            if (e.Key == Key.C)
            {
                var cargado = Serializador.CargarEscenario("escenario1.json");
                if (cargado != null)
                {
                    escenario = cargado;
                }
            }

            // Selección de objetos
            if (e.Key == Key.Number1) objetoSeleccionado = "U1";
            if (e.Key == Key.Number2) objetoSeleccionado = "U2";
            if (e.Key == Key.Number3) objetoSeleccionado = "U3";
            if (e.Key == Key.D) objetoSeleccionado = null; // Deseleccionar

            // Transformación de objetos
            if (objetoSeleccionado == null) // Si no hay objeto seleccionado, transformamos todos
            {
                if (e.Key == Key.Up) escenario.trasladarTodo(new Punto(0, 0.2f, 0));
                if (e.Key == Key.Down) escenario.trasladarTodo(new Punto(0, -0.2f, 0));
                if (e.Key == Key.Left) escenario.trasladarTodo(new Punto(-0.2f, 0, 0));
                if (e.Key == Key.Right) escenario.trasladarTodo(new Punto(0.2f, 0, 0));
                if (e.Key == Key.Plus || e.Key == Key.KeypadPlus) escenario.escalarTodo(1.1f);
                if (e.Key == Key.Minus || e.Key == Key.KeypadMinus) escenario.escalarTodo(0.9f);
                if (e.Key == Key.R) escenario.rotarTodo(new Punto(0, 15, 0));
            }
            else if (escenario.GetObjetos().ContainsKey(objetoSeleccionado)) // Si hay un objeto seleccionado
            {
                if (e.Key == Key.Up) escenario.trasladarObjeto(objetoSeleccionado, new Punto(0, 0.2f, 0));
                if (e.Key == Key.Down) escenario.trasladarObjeto(objetoSeleccionado, new Punto(0, -0.2f, 0));
                if (e.Key == Key.Left) escenario.trasladarObjeto(objetoSeleccionado, new Punto(-0.2f, 0, 0));
                if (e.Key == Key.Right) escenario.trasladarObjeto(objetoSeleccionado, new Punto(0.2f, 0, 0));

                if (e.Key == Key.Plus || e.Key == Key.KeypadPlus)
                    escenario.escalarObjeto(objetoSeleccionado, 1.1f);

                if (e.Key == Key.Minus || e.Key == Key.KeypadMinus)
                    escenario.escalarObjeto(objetoSeleccionado, 0.9f);

                if (e.Key == Key.R)
                    escenario.rotarObjeto(objetoSeleccionado, new Punto(0, 15, 0));
            }
        }

    }
}
