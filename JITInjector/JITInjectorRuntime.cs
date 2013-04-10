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
    /// JITInjectorRuntime is an object that generates new types at runtime, based on 
    /// </summary>
    public class JITInjectorRuntime
    {
        public Assembly CompiledAssembly { get { return this.m_CompiledAssembly; } }
        public string CompiledAssemblyName { get { return this.m_CompiledAssemblyName; } }
        public Type[] CompiledAssemblyTypes
        {
            get { return CompiledAssembly.GetTypes(); }
        }

        private string m_CompiledAssemblyName;
        private Assembly m_CompiledAssembly;
        private JITStartupParser m_StartupParser;


        /// <summary>
        /// Generates a new JITInjectorRuntime, and compiles code;
        /// </summary>
        /// <param name="p_RelativeScriptPath">Folder (relative to application executable) where to search for files. e.g. "\\Script\\" </param>
        /// <param name="p_ScriptFileMask">File mask to use for searching files e.g. "*.script" </param>
        /// <param name="p_AssemblyName">Assembly namespace to use in your Application e.g. "MyApplication.Plugins.JIT"</param>
        /// <param name="p_ReferencedAssemblies">A list of Namespaces used for compilation</param>
        /// <param name="p_ProcessHandler">A JITStartupParserProcessingFileHandler delegate used to parse files</param>
        public JITInjectorRuntime(string p_RelativeScriptPath, string p_ScriptFileMask, string p_AssemblyName, List<string> p_ReferencedAssemblies, JITStartupParserProcessingFileHandler p_ProcessHandler)
        {
            this.m_StartupParser = new JITStartupParser(p_RelativeScriptPath, p_ScriptFileMask, p_AssemblyName, p_ReferencedAssemblies, p_ProcessHandler);
            this.m_CompiledAssemblyName = p_AssemblyName;
            this.m_CompiledAssembly = this.m_StartupParser.Run();
        }

        /// <summary>
        /// Spawns an instance of given object name, using given constructor parameters.
        /// </summary>
        /// <param name="p_TypeName">Name of the Type</param>
        /// <param name="p_ConstructorParameters">An objectList containing correct constructor parameters</param>
        /// <returns></returns>
        public object SpawnInstance(string p_TypeName, object[] p_ConstructorParameters)
        {
            return this.CompiledAssembly.CreateInstance(    p_TypeName,
                                                            false,
                                                            System.Reflection.BindingFlags.CreateInstance,
                                                            null,
                                                            p_ConstructorParameters,
                                                            System.Globalization.CultureInfo.CurrentCulture,
                                                            null
                                                       );
        }
        
    }
}
