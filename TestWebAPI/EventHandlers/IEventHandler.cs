namespace TestWebAPI.EventHandlers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using TestWebAPI.Entities;

    /// <summary>
    /// The base event handler.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IEventHandler<T>
        where T : BaseEntity
    {
        /// <summary>
        /// Gets or sets the on save.
        /// </summary>
        // Action<IEnumerable<EntityEntry<T>>> OnSave { get; set; }
    }
}
