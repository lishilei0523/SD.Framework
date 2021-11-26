using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Relational database specific extension methods.
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Sets the underlying connection string configured for this Microsoft.EntityFrameworkCore.DbContext.
        /// It may not be possible to change the connection string if existing connection,
        /// if any, is open.
        /// </summary>
        /// <param name="database">The Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade for the context.</param>
        /// <param name="connectionString">The connection string.</param>
        public static void SetConnectionString(this DatabaseFacade database, string connectionString)
        {
            IRelationalDatabaseFacadeDependencies databaseFacadeDependencies = database.GetFacadeDependencies();
            databaseFacadeDependencies.RelationalConnection.DbConnection.ConnectionString = connectionString;
        }

        /// <summary>
        /// Configures the name of the index in the database when targeting a relational
        /// </summary>
        /// <param name="indexBuilder">The builder for the index being configured.</param>
        /// <param name="name">The name of the index.</param>
        /// <returns>A builder to further configure the index.</returns>
        public static IndexBuilder HasDatabaseName(this IndexBuilder indexBuilder, string name)
        {
            indexBuilder.Metadata.SetName(name);
            return indexBuilder;
        }

        /// <summary>
        /// Configures the name of the index in the database when targeting a relational
        /// </summary>
        /// <typeparam name="TEntity">The entity type being configured.</typeparam>
        /// <param name="indexBuilder">The builder for the index being configured.</param>
        /// <param name="name">The name of the index.</param>
        /// <returns>A builder to further configure the index.</returns>
        public static IndexBuilder<TEntity> HasDatabaseName<TEntity>(this IndexBuilder<TEntity> indexBuilder, string name)
        {
            indexBuilder.Metadata.SetName(name);
            return indexBuilder;
        }

        /// <summary>
        /// Configures the precision and scale of the property.
        /// </summary>
        /// <param name="propertyBuilder">The builder for the property being configured.</param>
        /// <param name="precision"> The precision of the property. </param>
        /// <param name="scale"> The scale of the property. </param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained. </returns>
        public static PropertyBuilder<decimal> HasPrecision(this PropertyBuilder<decimal> propertyBuilder, int precision, int scale)
        {
            return propertyBuilder.HasColumnType($"decimal({precision},{scale})");
        }

        /// <summary>
        /// Configures the precision and scale of the property.
        /// </summary>
        /// <param name="propertyBuilder">The builder for the property being configured.</param>
        /// <param name="precision"> The precision of the property. </param>
        /// <param name="scale"> The scale of the property. </param>
        /// <returns> The same builder instance so that multiple configuration calls can be chained. </returns>
        public static PropertyBuilder<decimal?> HasPrecision(this PropertyBuilder<decimal?> propertyBuilder, int precision, int scale)
        {
            return propertyBuilder.HasColumnType($"decimal({precision},{scale})");
        }

        private static IRelationalDatabaseFacadeDependencies GetFacadeDependencies(this IDatabaseFacadeDependenciesAccessor databaseDependenciesAccessor)
        {
            IDatabaseFacadeDependencies dependencies = databaseDependenciesAccessor.Dependencies;
            if (dependencies is IRelationalDatabaseFacadeDependencies relationalDependencies)
            {
                return relationalDependencies;
            }

            throw new InvalidOperationException(RelationalStrings.RelationalNotInUse);
        }
    }
}
