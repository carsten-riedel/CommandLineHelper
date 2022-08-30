using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CommandLineHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (textBox1.Text != String.Empty)
            {
                var ArgvW = CmdLineToArgvW.SplitArgs(textBox1.Text).ToList();
                for (int i = 0; i < ArgvW.Count; i++)
                {
                    ArgvW[i] = $@"Param {i.ToString().PadLeft(3, '0')}:  {ArgvW[i]}{Environment.NewLine}";
                }
                textBox2.Text = "";
                textBox2.Text = String.Join("", ArgvW.ToArray());
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != String.Empty)
            {
                var ArgvW = CmdLineToArgvW.SplitArgs(textBox1.Text).ToList();
                for (int i = 0; i < ArgvW.Count; i++)
                {
                    ArgvW[i] = $@"Param {i.ToString().PadLeft(3,'0')}:  {ArgvW[i]}{Environment.NewLine}";
                }
                textBox2.Text = "";
                textBox2.Text = String.Join("", ArgvW.ToArray());
            }
        }
    }

    public static class CmdLineToArgvW
    {
        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine,out int pNumArgs);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LocalFree(IntPtr hMem);

        public static string[] SplitArgs(string unsplitArgumentLine)
        {
            int numberOfArgs;
            IntPtr ptrToSplitArgs;
            string[] splitArgs;

            ptrToSplitArgs = CommandLineToArgvW(unsplitArgumentLine, out numberOfArgs);

            if (ptrToSplitArgs == IntPtr.Zero)
            {
                throw new ArgumentException("Unable to split argument.", new Win32Exception());
            }

            try
            {
                splitArgs = new string[numberOfArgs];
                for (int i = 0; i < numberOfArgs; i++)
                {
                    splitArgs[i] = Marshal.PtrToStringUni(Marshal.ReadIntPtr(ptrToSplitArgs, i * IntPtr.Size));
                }
                return splitArgs;
            }
            finally
            {
                LocalFree(ptrToSplitArgs);
            }
        }
    }
}