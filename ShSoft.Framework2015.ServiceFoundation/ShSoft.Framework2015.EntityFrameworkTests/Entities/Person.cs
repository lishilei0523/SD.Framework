using System;
using ShSoft.Framework2015.EntityFramework.Attributes;
using ShSoft.Framework2015.Infrastructure.IEntity;

namespace ShSoft.Framework2015.EntityFrameworkTests.Entities
{

    [SingleTableMap]
    public abstract class Person : AggregateRootEntity
    {
        public float Age { get; set; }
    }
}
