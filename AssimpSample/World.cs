// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using Assimp;
using System.IO;
using System.Reflection;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;
using SharpGL.Enumerations;

namespace AssimpSample
{


    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_scene;

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 30.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        /// <summary>
        ///	 Shade model 
        /// </summary>
        private ShadeModel m_selectedModel;

        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Scene
        {
            get { return m_scene; }
            set { m_scene = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePath, String sceneFileName, int width, int height, OpenGL gl)
        {
            this.m_scene = new AssimpScene(scenePath, sceneFileName, gl);
            this.m_width = width;
            this.m_height = height;
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {

            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.Enable(OpenGL.GL_DEPTH_TEST);

            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            
            gl.ClearColor(0.0f, 0.5f, 1.0f, 1.0f);

            m_scene.LoadScene();
            m_scene.Initialize();
        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.FrontFace(OpenGL.GL_CCW);



            gl.PushMatrix();

            gl.Translate(0.0f, 0.0f, -m_sceneDistance*1.3);
            gl.Scale(0.5, 0.5, 0.5);
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);
            Ground(gl);
            Golf_Club(gl);

            gl.PopMatrix();

            drawText(gl);

            gl.Flush();
        }

        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.Viewport(0,0,m_width,m_height);
            gl.MatrixMode(OpenGL.GL_PROJECTION);      
            gl.LoadIdentity();
            gl.Perspective(45f, (double)width / height, 0.5f, 200f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();              
        }

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_scene.Dispose();
            }
        }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode

        #region scens

        private void Ground(OpenGL gl) {

            gl.PushMatrix();

            gl.Color(0.45f, 0.6f, 0.35f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(-15.0f, 8.0f);
            gl.Vertex(-50.0f, -25.0f);
            gl.Vertex(50f, -25.0f);
            gl.Vertex(15.0f, 8.0f);
            gl.End();

            Hole(gl);
            Bulk(gl);
            Ball(gl);

            gl.PopMatrix();
        }

        private void Hole(OpenGL gl) {

            gl.PushMatrix();
            gl.Color(0f, 0f, 0f);
            gl.Translate(0f, 0.0f, 0.015f);
            Disk disk = new Disk();
            disk.Loops = 50;
            disk.Slices = 50;
            disk.OuterRadius = 0.7f;
            disk.CreateInContext(gl);
            disk.Render(gl, RenderMode.Render);
            gl.PopMatrix();
        }

        private void Bulk(OpenGL gl) {

            gl.PushMatrix();
            gl.Translate(0.4f, 0f, 0.02f);
            gl.Color(1f, 1f, 1f);
            gl.Rotate(-30f, 0f, 0f);
            Cylinder cil = new Cylinder();
            cil.BaseRadius = 0.2f;
            cil.TopRadius = 0.2f;
            cil.Height = 15f;
            cil.CreateInContext(gl);
            cil.Render(gl,RenderMode.Render);

            Flag(gl);
           
            gl.PopMatrix();
        }

        private void Flag(OpenGL gl) {
            
            gl.PushMatrix();
            gl.Color(0.8f, 0.2f, 0.3f);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Vertex(0f, 0.0f, 15f);
            gl.Vertex(0f, 0f, 11f);
            gl.Vertex(2f, -2.0f, 13f);
            gl.End();
            gl.PopMatrix();

        }

        private void Ball(OpenGL gl) {
            
            gl.PushMatrix();
            gl.Color(0.8f, 0.8f, 0.1f);
            gl.Translate(-9f,-7f,0.5f);
            gl.Rotate(90f, 0f, 0f);
            gl.Scale(0.5f, 0.5f, 0.5f);
            Sphere sp = new Sphere();
            sp.CreateInContext(gl);
            sp.Render(gl,RenderMode.Render);
            gl.PopMatrix();

        }

        private void Golf_Club(OpenGL gl)
        {
            gl.PushMatrix();

            gl.Translate(-14.0f, -10.0f,0.2f);
            gl.Rotate(0f, 0f, -60f);
            gl.Scale(0.2f,0.35f,0.2f);
            m_scene.Draw();

            gl.PopMatrix();
        }

        public void drawText(OpenGL gl)
        {

            //gl.Viewport(m_width/2, 0, m_width/2, m_height/2);

            gl.Color(1f, 0.0f, 0.0f);
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Ortho2D(-17f, 17f, -17f, 17f);

            gl.PushMatrix();
            gl.Translate(-16f, 14f, 0.0f);
            gl.DrawText3D("Tahoma", 10f, 0.2f, 0.1f, "Predmet: Racunarska grafika");
            gl.PopMatrix();

            gl.PushMatrix();
            gl.Translate(-16f, 13f, 0.0f);
            gl.DrawText3D("Tahoma", 10f, 0.2f, 0.1f, "Sk.god: 2020/21");
            gl.PopMatrix();

            gl.PushMatrix();
            gl.Translate(-16f, 12f, 0.0f);
            gl.DrawText3D("Tahoma", 10f, 0.2f, 0.1f, "Ime: Nemanja");
            gl.PopMatrix();

            gl.PushMatrix();
            gl.Translate(-16f, 11f, 0.0f);
            gl.DrawText3D("Tahoma", 10f, 0.2f, 0.1f, "Prezime:Jevtic");
            gl.PopMatrix();

            gl.PushMatrix();
            gl.Translate(-16f, 10f, 0.0f);
            gl.DrawText3D("Tahoma", 10f, 0.2f, 0.1f, "Sifra zad: 15.2");
            gl.PopMatrix();


            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(50.0, m_width / (double)m_height, 0.5, 200.0);
            
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
           

        }


        #endregion


        #region Change shade model (Flat or Smooth)
        public void ChangeShadeModel(OpenGL gl)
        {
            if (m_selectedModel == ShadeModel.Flat)
            {
                gl.ShadeModel(ShadeModel.Smooth);
                m_selectedModel = ShadeModel.Smooth;
            }
            else
            {
                gl.ShadeModel(ShadeModel.Flat);
                m_selectedModel = ShadeModel.Flat;
            }
        }
        #endregion


    }
}
