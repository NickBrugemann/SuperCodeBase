using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperCodeBase
{
    static class ToBeNamed
    {
        /// <summary>
        /// Makes a print screen and saves it in the My Images folder.
        /// Returns the full path where the image was saved.
        /// </summary>
        /// <returns>Returns the full path where the image was saved.</returns>
        public static string PrintScreen()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            int fileCount = Directory.GetFiles(folderPath).Length;
            string fileName = "Screenshot "+fileCount+".jpg";
            string fullPath = folderPath + "\\" + fileName;
            Bitmap prtSc = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(prtSc as Image);
            g.CopyFromScreen(0,0,0,0,prtSc.Size);
            prtSc.Save(fullPath);
            return fullPath;
        }
    }
}
