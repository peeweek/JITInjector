/*=============================================================================================
            DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE
                    Version 2, December 2004

 Copyright (C) 2004 Sam Hocevar <sam@hocevar.net>

 Everyone is permitted to copy and distribute verbatim or modified
 copies of this license document, and changing it is allowed as long
 as the name is changed.

            DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE
   TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION

  0. You just DO WHAT THE FUCK YOU WANT TO.

	NON-WARRANTY CLAUSE:
	This program is free software. It comes without any warranty, to
	the extent permitted by applicable law. You can redistribute it
	and/or modify it under the terms of the Do What The Fuck You Want
	To Public License, Version 2, as published by Sam Hocevar. See
	http://www.wtfpl.net/ for more details. 
 * =============================================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace JITInjector
{
    /// <summary>
    /// A delegate for usage at the enad of a compilation.
    /// </summary>
    public delegate void JITStartupParserEndedHandler();

    /// <summary>
    /// A delegate to use when parsing files.
    /// </summary>
    /// <param name="filename">the filename to parse</param>
    /// <returns>Valid C#/Mono code to compile</returns>
    public delegate string JITStartupParserProcessingFileHandler(string filename);

    /// <summary>
    /// Defines a StartupParser that gathers source files & compiles them at runtime, before injecting new types into an assembly.
    /// </summary>
    public class JITStartupParser
    {
        public event JITStartupParserEndedHandler onFinishParsing;
        public event JITStartupParserProcessingFileHandler onFileProcessing;

        private string m_FileMask;
        private string m_ScriptPath;
        private string m_AssemblyName;
        private List<string> m_ReferencedAssemblies;

        /// <summary>
        /// Creates a new JITStartupParser, given a sub path to search for files.
        /// StartupParser gathers source files & compiles them at runtime, before injecting new types into an assembly.
        /// </summary>
        /// <param name="p_subSearchPath">Folder (relative to application executable) where to search for files. e.g. "\\Script\\" </param>
        /// <param name="p_fileMask">File mask to use for searching files e.g. "*.script" </param>
        /// <param name="p_AssemblyName">Assembly namespace to use in your Application e.g. "MyApplication.Plugins.JIT"</param>
        /// <param name="p_ReferencedAssemblies">A list of Namespaces used for compilation</param>
        /// <param name="p_ProcessHandler">A JITStartupParserProcessingFileHandler delegate used to parse files</param>
        public JITStartupParser(string p_subSearchPath, string p_FileMask, string p_AssemblyName, List<string> p_ReferencedAssemblies, JITStartupParserProcessingFileHandler p_ProcessHandler)
        {
            this.m_ScriptPath = Application.StartupPath + p_subSearchPath;
            this.m_FileMask = p_FileMask;
            this.m_AssemblyName = p_AssemblyName;
            this.m_ReferencedAssemblies = p_ReferencedAssemblies;
            this.onFileProcessing += p_ProcessHandler;
        }


        /// <summary>
        /// Runs the gathering & compilation of the JIT Files
        /// </summary>
        public Assembly Run()
        {

            // Gather all files that requires JIT Compilation
            string[] v_fileNamesToCompile = Directory.GetFiles(this.m_ScriptPath, this.m_FileMask);

            // Prepare a list of source code to build.
            List<string> v_SourcesList = new List<string>();

            // Foreach File
            foreach (string i_fileToCompile in v_fileNamesToCompile)
            {
                if (onFileProcessing != null) v_SourcesList.Add(onFileProcessing(i_fileToCompile));
                else throw new Exception("onFileProcessing event must have to be declared to process files");
            }
            if (v_fileNamesToCompile.Length > 0)
            {
                Assembly v_outputAssembly = this.CompileAll(v_SourcesList.ToArray());
                if (v_outputAssembly == null) throw new Exception("Compilation Failed!");

                if (onFinishParsing != null) onFinishParsing();
                return v_outputAssembly;
            }
            else return null;
        }


        /// <summary>
        /// Compile all source files then outputs compiled assembly.
        /// </summary>
        /// <param name="p_SourceList"></param>
        /// <returns></returns>
        private System.Reflection.Assembly CompileAll(string[] p_SourceList)
        {

            CSharpCodeProvider v_CodeProvider = new CSharpCodeProvider();
            CompilerParameters v_CompilerParameters = new System.CodeDom.Compiler.CompilerParameters();


            v_CompilerParameters.GenerateInMemory = true;
            foreach (string i_assembly in this.m_ReferencedAssemblies)
            {
                v_CompilerParameters.ReferencedAssemblies.Add(i_assembly);
            }
            v_CompilerParameters.OutputAssembly = this.m_AssemblyName;

            CompilerResults v_Results = v_CodeProvider.CompileAssemblyFromSource(v_CompilerParameters, p_SourceList);

            if (v_Results.Errors.Count > 0)
            {
                MessageBox.Show(FormatSourceOutput(v_Results.Output), "Compilation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            else return v_Results.CompiledAssembly;


        }

        /// <summary>
        /// Converts a string collection into a single string
        /// </summary>
        /// <param name="p_Strings"></param>
        /// <returns></returns>
        private string FormatSourceOutput(System.Collections.Specialized.StringCollection p_Strings)
        {
            string v_Out = "";
            foreach (string i_string in p_Strings)
            {
                v_Out += i_string + "\n";
            }
            return v_Out;
        }


    }

}
