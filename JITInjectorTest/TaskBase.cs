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
using System.Linq;
using System.Text;

namespace JITInjectorTest
{
    /// <summary>
    /// Abstraction level for a Task used in sample application
    /// </summary>
    public abstract class TaskBase
    {
        /// <summary>
        /// Every TaskBase implements Execute (parameterless)
        /// </summary>
        public abstract void Execute();

        public override string ToString()
        {
            return this.GetType().ToString();
        }
    }
}
