using System;
using System.Windows.Forms;
using LevelEditor.EditorForms;

namespace LevelEditor
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            using (tileditor editor = new tileditor())
            {
                Application.Run(editor);
            }
        }
    }
#endif
}
