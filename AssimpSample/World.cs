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
using System.Windows.Threading;
using System.Threading;

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

        #region Atributi za animaciju
        private DispatcherTimer timerBall;
        private DispatcherTimer timerGolf_Club;
        private bool startAnimation = false;

        private float[] positionBall = { -13f, -10f, 0.5f };
        private float[] positionGolf_Club = { -14f, -10f, 0.2f };
        private float[] rotationGolf_Club = { 0f, 0f, -60f };
        private bool Golf_Club_Up = false;
        private bool Golf_Club_Down = false;

        #endregion

        #region Restartvanje atributa
        public void RestartAnimation() {
            if (!startAnimation)
            {
                startAnimation = true;
                positionBall = new float[] { -13f, -10f, 0.5f };
            }
            else {
                //Thread.Sleep(2000);
                startAnimation = false;
                positionGolf_Club = new float[] { -14f, -10f, 0.2f };
                rotationGolf_Club = new float[] { 0f, 0f, -60f };
                Golf_Club_Up = false;
                Golf_Club_Down = false;
            }
        }


        #endregion

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
            SetupLighting(gl);
            
            timerBall = new DispatcherTimer();
            timerBall.Interval = TimeSpan.FromMilliseconds(200);
            timerBall.Tick += new EventHandler(BallAnimation);
            timerBall.Start();

            timerGolf_Club = new DispatcherTimer();
            timerGolf_Club.Interval = TimeSpan.FromMilliseconds(200);
            timerGolf_Club.Tick += new EventHandler(Golf_Club_Animation);
            timerGolf_Club.Start();


            m_scene.LoadScene();
            m_scene.Initialize();
        }

        private void BallAnimation(object sender, EventArgs e)
        {
            if (startAnimation) {
                if (positionBall[0] + 0.4 < 0 && Golf_Club_Down)
                {
                    positionBall[0] += 0.4f;
                    positionBall[1] += 0.3f;
                }
                if (positionBall[0] + 0.4 > 0 && Golf_Club_Down)
                {
                    positionBall[2] = -1f;
                    RestartAnimation();
                }
            }

        }

        private void Golf_Club_Animation(object sender, EventArgs e)
        {
            if (startAnimation) {
                if (!Golf_Club_Up)
                {
                    positionGolf_Club[0] -= 3;
                    positionGolf_Club[1] -= 3.5f;
                    positionGolf_Club[2] += 1.4f;
                    rotationGolf_Club[1] += 10;
                    if (positionGolf_Club[0] == -23)
                    {
                        Golf_Club_Up = true;
                    }
                }
                else
                {
                    if (!Golf_Club_Down)
                    {
                        positionGolf_Club[0] += 3;
                        positionGolf_Club[1] += 3.5f;
                        positionGolf_Club[2] -= 1.4f;
                        rotationGolf_Club[1] -= 10;
                    }
                    if (positionGolf_Club[0] == -14)
                    {
                        Golf_Club_Down = true;
                    }
                }
            }
        }

        private void SetupLighting(OpenGL gl)
        {
            //color tracking mehanizam
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            gl.ClearColor(0.0f, 0.5f, 1.0f, 1.0f);
            //definisanje normale 
            gl.Enable(OpenGL.GL_NORMALIZE);
            gl.Enable(OpenGL.GL_AUTO_NORMAL);
            //shade model 
            gl.ShadeModel(OpenGL.GL_SMOOTH);

            //tackasti izvor svetlosti stacionaran na y-osi iznad podloge
            float[] light0pos = new float[] { 0.0f, 5.0f, 0.0f, 1.0f };
            float[] light0ambient = new float[] { 0.5f, 0.5f, 0.5f, 1f };
            float[] light0diffuse = new float[] { 0.3f, 0.3f, 0.3f, 1.0f };

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0pos);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light0ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light0diffuse);

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f);

            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);

        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.FrontFace(OpenGL.GL_CCW);

            gl.PushMatrix();

            /*gl.PushMatrix();
            Grid grid = new Grid();
            gl.Translate(0f, -1f, -10f);
            gl.Rotate(90f, 0f, 0f);
            grid.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Design);
            gl.PopMatrix();*/
            //gl.LoadIdentity();

            //Pitati vezano za LookAt da li se odnosi na to da gleda direktno iznad rupe 
            //gl.LookAt(0, 20, -40, 0, 0, -40, 0, 0, -1);
            gl.Translate(0.0f, -7f, -m_sceneDistance*1.2);
            gl.Scale(0.5, 0.5, 0.5);
            gl.Rotate(-90f,0f,0f);
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
            timerBall.Stop();
            timerGolf_Club.Stop();

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
            gl.Normal(0.0f, -1.0f, 0.0f);
            gl.Vertex(-15.0f, 15.0f);
            gl.Vertex(-50.0f, -25.0f);
            gl.Vertex(50f, -25.0f);
            gl.Vertex(15.0f, 15.0f);
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
            disk.NormalGeneration = Normals.Smooth;
            disk.Loops = 300;
            disk.Slices = 300;
            disk.OuterRadius = 0.7f;
            disk.CreateInContext(gl);
            disk.Render(gl, RenderMode.Render);
            gl.PopMatrix();
        }

        private void Bulk(OpenGL gl) {

            gl.PushMatrix();
            gl.Translate(0.4f, 0f, 0.02f);
            gl.Color(1f, 1f, 1f);
            //gl.Rotate(-30f, 0f, 0f);
            Cylinder cil = new Cylinder();
            cil.NormalGeneration = Normals.Smooth;
            cil.BaseRadius = 0.2f;
            cil.TopRadius = 0.2f;
            cil.Height = 15f;
            cil.Stacks = 300;
            cil.Slices = 300;
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
            gl.Translate(positionBall[0], positionBall[1], positionBall[2]);
            gl.Rotate(90f, 0f, 0f);
            gl.Scale(0.5f, 0.5f, 0.5f);
            Sphere sp = new Sphere();
            sp.NormalGeneration = Normals.Smooth;
            sp.Slices = 300;
            sp.Stacks = 300;
            sp.CreateInContext(gl);
            sp.Render(gl,RenderMode.Render);
            gl.PopMatrix();

        }

        private void Golf_Club(OpenGL gl)
        {
            gl.PushMatrix();

            gl.Translate(positionGolf_Club[0], positionGolf_Club[1], positionGolf_Club[2]);
            gl.Rotate(rotationGolf_Club[0], rotationGolf_Club[1], rotationGolf_Club[2]);
            gl.Scale(0.2f,0.35f,0.2f);


            /*gl.Translate(-17.0f, -13.5f, 1.7f);
            gl.Rotate(0f, 10f, -60f);
            gl.Scale(0.2f, 0.35f, 0.2f);*/

            /*gl.Translate(-20.0f, -17.0f, 3f);
            gl.Rotate(0f, 20f, -60f);
            gl.Scale(0.2f, 0.35f, 0.2f);*/
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
