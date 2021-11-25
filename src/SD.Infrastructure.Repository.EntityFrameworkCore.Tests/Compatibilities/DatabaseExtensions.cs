using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    internal static class DatabaseExtensions
    {
        public static void SetConnectionString(this DatabaseFacade database, string connectionString)
        {
            IRelationalDatabaseFacadeDependencies databaseFacadeDependencies = GetFacadeDependencies(database);
            databaseFacadeDependencies.RelationalConnection.DbConnection.ConnectionString = connectionString;
        }

        private static IRelationalDatabaseFacadeDependencies GetFacadeDependencies(IDatabaseFacadeDependenciesAccessor databaseDependenciesAccessor)
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
