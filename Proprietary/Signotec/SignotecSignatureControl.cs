using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using signotec.STPadLibNet;

namespace OOLaboratories.Proprietary.Signotec
{
    /// <summary>
    /// This <see cref="UserControl"/> provides a simple interface to capture signatures.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl"/>
    public partial class SignotecSignatureControl : UserControl
    {
        /// <summary>
        /// The <see cref="m_ViewportBitmap"/>'s <see cref="Graphics"/> handle to draw lines.
        /// </summary>
        private Graphics m_ViewportGraphics;

        /// <summary>
        /// The viewport bitmap representing the signature image.
        /// </summary>
        private Bitmap m_ViewportBitmap;

        /// <summary>
        /// The signature pad library.
        /// </summary>
        private STPadLib m_SignaturePadLibrary;

        /// <summary>
        /// The last known x position (used to draw connected lines in case of stuttering).
        /// </summary>
        private int m_LastX = 0;

        /// <summary>
        /// The last known y position (used to draw connected lines in case of stuttering).
        /// </summary>
        private int m_LastY = 0;

        /// <summary>
        /// Whether the core components of this class have been initialized.
        /// </summary>
        private bool m_Initialized = false;

        /// <summary>
        /// Whether the signature pad is available (opened correctly).
        /// </summary>
        private bool m_SignaturePadAvailable = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ucSignotecSignature"/> class.
        /// </summary>
        public ucSignotecSignature()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called whenever new signature data is received.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="SignatureDataReceivedEventArgs"/> instance containing the event data.
        /// </param>
        private void SignatureDataReceived(object sender, SignatureDataReceivedEventArgs e)
        {
            // we make use of the background thread to do graphics rendering.
            if (viewport.InvokeRequired)
            {
                int currentX = Convert.ToInt32((e.xPos / 8192.0f) * 320);
                int currentY = Convert.ToInt32((e.yPos / 4096.0f) * 160);

                // new touch:
                if (e.pressure == 0)
                {
                    m_LastX = currentX;
                    m_LastY = currentY;
                }

                // draw line with pressure sensitivity.
                using (Pen pen = new Pen(Brushes.Black, Math.Max(1.0f, e.pressure / 256.0f)))
                    m_ViewportGraphics.DrawLine(pen, m_LastX, m_LastY, currentX, currentY);

                m_LastX = currentX;
                m_LastY = currentY;

                viewport.Invoke((Action)(() => SignatureDataReceived(sender, e)));
            }

            // the main thread simply draws the viewport.
            else
            {
                viewport.Invalidate();
            }
        }

        // PUBLIC API:

        /// <summary>
        /// Initializes the signotec tablet and starts capturing a signature.
        /// </summary>
        /// <exception cref="STPadException">Possibly couldn't open the device.</exception>
        public void StartSignature()
        {
            // initialize some common properties.
            if (!m_Initialized)
            {
                m_ViewportBitmap = new Bitmap(320, 160);
                m_ViewportGraphics = Graphics.FromImage(m_ViewportBitmap);
                m_ViewportGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                viewport.Image = m_ViewportBitmap;
                m_SignaturePadLibrary = new STPadLib();
                m_SignaturePadLibrary.SignatureDataReceived += SignatureDataReceived;

                m_Initialized = true;
            }

            try
            {
                if (!m_SignaturePadAvailable)
                    m_SignaturePadLibrary.DeviceOpen(0, true);
                m_SignaturePadAvailable = true;
                m_SignaturePadLibrary.SignatureStart();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Stops the signature capture and closes the signotec tablet device.
        /// </summary>
        public void StopSignature()
        {
            // if the start signature method wasn't called, do nothing.
            if (!m_Initialized) return;
            // if the tablet hasn't been opened properly, do nothing.
            if (!m_SignaturePadAvailable) return;

            // stop capturing a signature.
            m_SignaturePadLibrary.SignatureConfirm();
            // close the device.
            m_SignaturePadLibrary.DeviceClose(0);
            m_SignaturePadAvailable = false;

            // dispose of other garbage.
            m_ViewportGraphics.Dispose();
            m_SignaturePadLibrary.Dispose();
            m_Initialized = false;

            // we don't dispose of the bitmap in case the developer needs it yet.
        }

        /// <summary>
        /// Retry the signature, clears the screen.
        /// </summary>
        public void RetrySignature()
        {
            // if the start signature method wasn't called, do nothing.
            if (!m_Initialized) return;
            // if the tablet hasn't been opened properly, do nothing.
            if (!m_SignaturePadAvailable) return;

            // clear the screen and retry the capture.
            m_SignaturePadLibrary.SignatureRetry();
            m_ViewportGraphics.Clear(Color.Transparent);
            viewport.Invalidate();
        }

        /// <summary>
        /// Gets the bitmap containing the signature.
        /// </summary>
        /// <value>The bitmap containing the signature.</value>
        public Bitmap SignatureBitmap => (Bitmap)m_ViewportBitmap?.Clone();
    }
}
