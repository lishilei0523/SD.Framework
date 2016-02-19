using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;

namespace ShSoft.Framework2015.EntityFramework.Extensions
{
    /// <summary>
    /// FluentAPI配置扩展
    /// </summary>
    public static class ConfigurationExtension
    {
        #region # FluentAPI配置非聚集索引扩展方法 —— static EntityTypeConfiguration<TEntity> HasIndex<TEntity>(...
        /// <summary>
        /// FluentAPI配置非聚集索引扩展方法
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entityTypeConfiguration">实体类型配置</param>
        /// <param name="indexName">索引名称</param>
        /// <param name="propertySelector">属性选择器</param>
        /// <param name="additionalPropertySelectors">属性选择器集</param>
        /// <returns>实体类型配置</returns>
        public static EntityTypeConfiguration<TEntity> HasIndex<TEntity>(this EntityTypeConfiguration<TEntity> entityTypeConfiguration, string indexName, Func<EntityTypeConfiguration<TEntity>, PrimitivePropertyConfiguration> propertySelector, params Func<EntityTypeConfiguration<TEntity>, PrimitivePropertyConfiguration>[] additionalPropertySelectors) where TEntity : class
        {
            return entityTypeConfiguration.HasIndex(indexName, IndexType.Nonclustered, propertySelector, additionalPropertySelectors);
        }
        #endregion

        #region # FluentAPI配置索引扩展方法 —— static EntityTypeConfiguration<TEntity> HasIndex<TEntity>(...
        /// <summary>
        /// FluentAPI配置索引扩展方法
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entityTypeConfiguration">实体类型配置</param>
        /// <param name="indexName">索引名称</param>
        /// <param name="indexType">索引类型</param>
        /// <param name="propertySelector">属性选择器</param>
        /// <param name="additionalPropertySelectors">属性选择器集</param>
        /// <returns>实体类型配置</returns>
        public static EntityTypeConfiguration<TEntity> HasIndex<TEntity>(this EntityTypeConfiguration<TEntity> entityTypeConfiguration, string indexName, IndexType indexType, Func<EntityTypeConfiguration<TEntity>, PrimitivePropertyConfiguration> propertySelector, params Func<EntityTypeConfiguration<TEntity>, PrimitivePropertyConfiguration>[] additionalPropertySelectors) where TEntity : class
        {
            AddIndexColumn(indexName, indexType, 1, propertySelector(entityTypeConfiguration));
            for (int i = 0; i < additionalPropertySelectors.Length; i++)
            {
                AddIndexColumn(indexName, indexType, i + 2, additionalPropertySelectors[i](entityTypeConfiguration));
            }

            return entityTypeConfiguration;
        }
        #endregion

        //将IndexAttribute添加到IndexAnnotation
        private static void AddIndexColumn(string indexName, IndexType indexOptions, int column, PrimitivePropertyConfiguration propertyConfiguration)
        {
            var indexAttribute = new IndexAttribute(indexName, column)
            {
                IsClustered = indexOptions.HasFlag(IndexType.Clustered),
                IsUnique = indexOptions.HasFlag(IndexType.Unique)
            };

            var annotation = GetIndexAnnotation(propertyConfiguration);
            if (annotation != null)
            {
                var attributes = annotation.Indexes.ToList();
                attributes.Add(indexAttribute);
                annotation = new IndexAnnotation(attributes);
            }
            else
            {
                annotation = new IndexAnnotation(indexAttribute);
            }

            propertyConfiguration.HasColumnAnnotation(IndexAnnotation.AnnotationName, annotation);
        }

        //对属性进行反射得到IndexAnnotation的帮助方法
        private static IndexAnnotation GetIndexAnnotation(PrimitivePropertyConfiguration propertyConfiguration)
        {
            var configuration = typeof(PrimitivePropertyConfiguration)
                .GetProperty("Configuration", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(propertyConfiguration, null);

            var annotations = (IDictionary<string, object>)configuration.GetType()
                .GetProperty("Annotations", BindingFlags.Instance | BindingFlags.Public)
                .GetValue(configuration, null);

            object annotation;
            if (!annotations.TryGetValue(IndexAnnotation.AnnotationName, out annotation))
                return null;

            return annotation as IndexAnnotation;
        }
    }
}
