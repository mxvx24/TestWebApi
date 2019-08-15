namespace TestWebAPI.EventHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using TestWebAPI.Entities;

    /// <summary>
    /// The employee event handler.
    /// </summary>
    public class NameChangeEventHandler : IEventHandler<Employee>
    {
        /// <summary>
        /// The on save.
        /// </summary>
        public readonly Action<IEnumerable<EntityEntry<Employee>>> OnSave = entries =>
            {
                var modifiedEntities = entries.Where(e => e.State == EntityState.Modified).ToList();

                foreach (var entry in modifiedEntities)
                {
                    var entity = entry.Entity;

                    foreach (var originalValuesProperty in entry.OriginalValues.Properties)
                    {
                        var originalValue = entry.OriginalValues[originalValuesProperty].ToString();
                        var currentValue = entry.CurrentValues[originalValuesProperty].ToString();

                        // Only ig change is detected
                        if (originalValue != currentValue)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"Value changed from {originalValue} to {currentValue}");
                            Console.ResetColor();
                        }
                    }
                }
            };
    }
}
