﻿using System.Collections.Generic;

namespace RapidCore.Reflection
{
    /// <summary>
    /// The reading part of <see cref="InstanceAnalyzerContext"/>
    /// </summary>
    public interface IReadOnlyInstanceAnalyzerContext
    {
        /// <summary>
        /// The instance that the analyzer was called with
        /// </summary>
        object Instance { get; }
        
        /// <summary>
        /// The breadcrumb leading _up to_ (but not including) the thing you are
        /// being notified about
        /// </summary>
        IReadOnlyList<string> Breadcrumb { get; }
        
        /// <summary>
        /// The max depth provided to the analyzer when it was started
        /// </summary>
        int MaxDepth { get; }

        /// <summary>
        /// The current depth
        /// </summary>
        int CurrentDepth { get; }

        /// <summary>
        /// The breadcrumb as a string
        /// </summary>
        string BreadcrumbAsString { get; }
    }
}