JIT INJECTOR 0.0
================

JIT Injector is a C# JIT Compiler that enables loading of multiple script files,
custom parsing and compilation.

New types are compiled into an assembly at runtime. This assembly can spawn object based
on new compiled types.

At runtime, the assembly can be recompiled to match newly created or updated script
files.

Example : 
=========

	JITInjectorRuntime JITRuntime;

	// ... in constructor body

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

            this.JITRuntime = new JITInjectorRuntime("\\",
                                                        "*.jitscript",
                                                        "JITRuntimeTasks",
                                                        v_ReferencedAssemblies,
                                                        new JITStartupParserProcessingFileHandler(ProcessScriptFile)
                                                     );


	// in class
	// Custom Method based on : 
        string ProcessScriptFile(string p_FileName)
        {
            string v_out = "";
            System.IO.StreamReader myFile = new System.IO.StreamReader(p_FileName);
                v_out = myFile.ReadToEnd();
            myFile.Close();
            return v_out;
        }