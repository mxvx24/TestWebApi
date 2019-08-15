﻿namespace TestWebAPI.ClassLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.EntityFrameworkCore.Storage;

    using TestWebAPI.Data;

    /// <summary>
    /// The database facade extensions.
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// The raw sql query.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="sqlCommandText">
        /// The sql command text.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<object> RawSqlQuery(this DbContext context, string sqlCommandText)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlCommandText;
                command.CommandType = CommandType.Text;

                await context.Database.OpenConnectionAsync();
                return await command.ExecuteScalarAsync();
            }
        }

        /// <summary>
        /// The raw SQL query.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="map">
        /// The map.
        /// </param>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public static List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map)
        {
            using (var context = new EmployeeDataContext())
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    context.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        var entities = new List<T>();

                        while (result.Read())
                        {
                            entities.Add(map(result));
                        }

                        return entities;
                    }
                }
            }
        }
    }
}

