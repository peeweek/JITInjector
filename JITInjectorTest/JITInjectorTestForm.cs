/*=============================================================================================
 * DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE (http://www.wtfpl.net/about/)
 *                   Version 2, December 2004 
 *
 * Copyright (C) 2013 Thomas ICHE <peeweek@gmail.com> 
 *
 * Everyone is permitted to copy and distribute verbatim or modified 
 * copies of this license document, and changing it is allowed as long 
 * as the name is changed. 
 *
 *          DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE 
 * TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION 
 *
 * 0. You just DO WHAT THE FUCK YOU WANT TO.
 * 
 * =============================================================================================
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using JITInjector;

namespace JITInjectorTest
{
    public partial class JITInjectorTestForm : Form
    {
        JITInjectorRuntime JITRuntime;

        public JITInjectorTestForm()
        {
            InitializeComponent();
            InitializeJITRuntime();
       
        }

        public void InitializeJITRuntime()
        {
            List<string> v_ReferencedAssemblies = new List<string>();
            v_ReferencedAssemblies.Add("System.dll");
            v_ReferencedAssemblies.Add("System.Core.dll");
            v_ReferencedAssemblies.Add("System.Data.dll");
            v_ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
            v_ReferencedAssemblies.Add("System.Deployment.dll");
            v_ReferencedAssemblies.Add("System.Drawing.dll");
            v_ReferencedAssemblies.Add("System.Windows.Forms.dll");
            v_ReferencedAssemblies.Add("System.Xml.dll");
            v_ReferencedAssemblies.Add("System.Xml.Linq.dll");

            // ADD CURRENT ASSEMBLY
            v_ReferencedAssemblies.Add("JITInjectorTest.exe");

            listBoxTasks.Items.Clear();
            listBoxTasks.Items.Add("Building In progress, please wait...");
            listBoxTasks.Refresh();
            buttonExecute.Enabled = false;
            buttonReload.Enabled = false;
            this.JITRuntime = new JITInjectorRuntime("\\",
                                                        "*.jitscript",
                                                        "JITRuntimeTasks",
                                                        v_ReferencedAssemblies,
                                                        new JITStartupParserProcessingFileHandler(ProcessScriptFile)
                                                     );
            if (this.JITRuntime.CompiledAssembly != null)
            {
                listBoxTasks.Items.Clear();
                buttonExecute.Enabled = true;
                buttonReload.Enabled = true;
                foreach (Type t in this.JITRuntime.CompiledAssemblyTypes)
                {
                    listBoxTasks.Items.Add(this.JITRuntime.SpawnInstance(t.ToString(), null) as TaskBase);
                }

            }
            else
            {
                MessageBox.Show("No sources to compile!");
            }
        }

        string ProcessScriptFile(string p_FileName)
        {
            string v_out = "";
            System.IO.StreamReader myFile = new System.IO.StreamReader(p_FileName);
                v_out = myFile.ReadToEnd();
            myFile.Close();
            return v_out;
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            if (listBoxTasks.SelectedIndex != -1)
            {
                ((listBoxTasks.SelectedItem) as TaskBase).Execute();
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            
            this.InitializeJITRuntime();
        }
    }
}
