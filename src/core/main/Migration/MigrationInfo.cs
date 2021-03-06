﻿using System;
using System.Collections.Generic;

namespace RapidCore.Migration
{
    /// <summary>
    /// Information about a migration that has run, either
    /// fully or partially
    /// </summary>
    public class MigrationInfo
    {
        public virtual string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the migration.
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the list of steps that have been successfully completed.
        /// </summary>
        public virtual List<string> StepsCompleted { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the migration has
        /// been completed (i.e. all steps have run successfully).
        /// </summary>
        public virtual bool MigrationCompleted { get; set; }

        /// <summary>
        /// Gets or sets the total migration time in milliseconds
        /// </summary>
        public virtual long TotalMigrationTimeInMs { get; set; }

        /// <summary>
        /// Gets or sets the completed at date and time in UTC.
        /// </summary>
        public virtual DateTime CompletedAtUtc { get; set; }

        /// <summary>
        /// Add a completed step
        /// </summary>
        /// <param name="stepName">The name of the completed step</param>
        public virtual void AddCompletedStep(string stepName)
        {
            StepsCompleted.Add(stepName);
        }
    }
}