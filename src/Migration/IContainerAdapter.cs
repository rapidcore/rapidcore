﻿using System;

namespace RapidCore.Mongo.Migration
{
    /// <summary>
    /// Yet another container adapter
    /// </summary>
    public interface IContainerAdapter
    {
        /// <summary>
        /// Resolve an instance
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        T Resolve<T>();
        
        /// <summary>
        /// Resolve a named instance
        /// </summary>
        /// <param name="name">The name</param>
        /// <typeparam name="T">The type to resolve</typeparam>
        T Resolve<T>(string name);

        /// <summary>
        /// Resolve a type
        /// </summary>
        /// <param name="type">The type to resolve</param>
        object Resolve(Type type);
    }
}