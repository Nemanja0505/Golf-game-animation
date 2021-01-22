﻿using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharpGL.SceneGraph;
using SharpGL;
using Microsoft.Win32;


namespace AssimpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributi

        /// <summary>
        ///	 Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;

        #endregion Atributi

        #region Konstruktori

        public MainWindow()
        {
            // Inicijalizacija komponenti
            InitializeComponent();

            // Kreiranje OpenGL sveta
            try
            {
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\GolfPalica1"), "palica.obj", (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight, openGLControl.OpenGL);
            }
            catch (Exception e)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta. Poruka greške: " + e.Message, "Poruka", MessageBoxButton.OK);
                this.Close();
            }
        }

        #endregion Konstruktori

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            m_world.Draw(args.OpenGL);
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            m_world.Initialize(args.OpenGL);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            m_world.Resize(args.OpenGL, (int)openGLControl.Width, (int)openGLControl.Height);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!m_world.StartAnimation)
            {
                switch (e.Key)
                {
                    case Key.F2: this.Close(); break;
                    case Key.E:
                        if (m_world.RotationX - 5 >= -100)
                        {
                            m_world.RotationX -= 5.0f;
                        }
                        else
                        {
                            m_world.RotationX = m_world.RotationX;
                        }
                        break;
                    case Key.D:
                        if (m_world.RotationX + 5 <= 100)
                        {
                            m_world.RotationX += 5.0f;
                        }
                        else
                        {
                            m_world.RotationX = m_world.RotationX;
                        }
                        break;
                    case Key.F:
                        if (m_world.RotationY - 5 >= -105)
                        {
                            m_world.RotationY -= 5.0f;
                        }
                        else
                        {
                            m_world.RotationY = m_world.RotationY;
                        }
                        break;
                    case Key.S:
                        if (m_world.RotationY + 5 <= 105)
                        {
                            m_world.RotationY += 5.0f;
                        }
                        else
                        {
                            m_world.RotationY = m_world.RotationY;
                        }
                        break;
                    case Key.OemPlus: m_world.SceneDistance -= 5.0f; break;
                    case Key.OemMinus: m_world.SceneDistance += 5.0f; break;
                    case Key.V:
                        m_world.RestartAnimation();
                        cbX.IsEnabled = false;
                        cbY.IsEnabled = false;
                        CbScale.IsEnabled = false;
                        CbDifuse.IsEnabled = false;
                        break;
                    case Key.O: m_world.ChangeShadeModel(openGLControl.OpenGL); break;
                }
            }
        }

        private void X_Change(object sender, SelectionChangedEventArgs e)
        {
            m_world.positionHole[0] = float.Parse(cbX.SelectedItem.ToString().Split(':')[1]);
        }
        private void Y_Change(object sender, SelectionChangedEventArgs e)
        {
            m_world.positionHole[1] = float.Parse(cbY.SelectedItem.ToString().Split(':')[1]);
        }

        public void Enable() {
            cbX.IsEnabled = true;
            cbY.IsEnabled = true;
            CbScale.IsEnabled = true;
            CbDifuse.IsEnabled = true;
        }

        private void Scale_Change(object sender, SelectionChangedEventArgs e)
        {
            m_world.scaleBall = float.Parse(CbScale.SelectedItem.ToString().Split(':')[1]);
        }

        private void Difuse_Change(object sender, SelectionChangedEventArgs e)
        {
            if (CbDifuse.SelectedItem.ToString().Split(':')[1].Equals("crvena"))
            {
                m_world.light0diffuse[0] = 1f;
                m_world.light0diffuse[1] = 0f;
                m_world.light0diffuse[2] = 0f;
            }
            else if (CbDifuse.SelectedItem.ToString().Split(':')[1].Equals("plava"))
            {
                m_world.light0diffuse[0] = 0f;
                m_world.light0diffuse[1] = 0f;
                m_world.light0diffuse[2] = 1f;
            }
            else {
                m_world.light0diffuse[0] = 0f;
                m_world.light0diffuse[1] = 1f;
                m_world.light0diffuse[2] = 0f;
            }
        }
    }
}
