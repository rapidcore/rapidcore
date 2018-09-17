using System.Collections.Generic;
using System.Linq;

namespace RapidCore.Reflection
{
    /// <summary>
    /// A context provided by the <see cref="InstanceAnalyzer"/> whenever
    /// the <see cref="IInstanceListener"/> is called.
    /// </summary>
    public class InstanceAnalyzerContext : IReadOnlyInstanceAnalyzerContext
    {
        /// <summary>
        /// The instance that the analyzer was called with
        /// </summary>
        public virtual object Instance { get; set; }

        /// <summary>
        /// The breadcrumb leading _up to_ (but not including) the thing you are
        /// being notified about
        /// </summary>
        public virtual IReadOnlyList<string> Breadcrumb
        {
            get
            {
                if (BreadcrumbStack == null)
                {
                    return new List<string>();
                }

                var list = BreadcrumbStack.ToList();
                list.Reverse();
                return list;
            }
        }
        
        /// <summary>
        /// Used internally to track the breadcrumb during analysis
        /// </summary>
        public virtual Stack<string> BreadcrumbStack { get; set; } = new Stack<string>();
        
        /// <summary>
        /// The max depth provided to the analyzer when it was started
        /// </summary>
        public virtual int MaxDepth { get; set; }

        /// <summary>
        /// The current depth
        /// </summary>
        public virtual int CurrentDepth => BreadcrumbStack?.Count ?? 0;

        /// <summary>
        /// Convenience method to check if further
        /// recursion is possible
        /// </summary>
        public virtual bool CanGoDeeper()
        {
            return CurrentDepth < MaxDepth;
        }

        /// <summary>
        /// Convenience method to turn the breadcrumb
        /// into a string
        /// </summary>
        public virtual string BreadcrumbAsString
        {
            get
            {
                if (Breadcrumb == null) return string.Empty;

                return string.Join(".", Breadcrumb);
            }
        }
    }
}