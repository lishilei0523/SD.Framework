using System;
using System.Linq.Expressions;

namespace ShSoft.Framework2015.EntityFramework.Extensions
{
    /// <summary>
    /// 查询条件建造者
    /// </summary>
    public sealed class PredicateBuilder<T> where T : class
    {
        #region # 字段及构造器

        /// <summary>
        /// 表达式
        /// </summary>
        private Expression<Func<T, bool>> _condition;

        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="initialPredicate">初始条件表达式</param>
        public PredicateBuilder(Expression<Func<T, bool>> initialPredicate)
        {
            #region # 验证参数

            if (initialPredicate == null)
            {
                throw new ArgumentNullException("initialPredicate", @"初始条件表达式不可为空！");
            }

            #endregion

            this._condition = initialPredicate;
        }

        #endregion

        #region # 逻辑运算（并） —— void And(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 逻辑运算（并）
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        public void And(Expression<Func<T, bool>> predicate)
        {
            ParameterExpression candidateExpr = Expression.Parameter(typeof(T), "candidate");
            ParameterExpressionVisitor parameterVisitor = new ParameterExpressionVisitor(candidateExpr);

            Expression left = parameterVisitor.Visit(this._condition.Body);
            Expression right = parameterVisitor.Visit(predicate.Body);
            BinaryExpression body = Expression.And(left, right);

            this._condition = Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        #endregion

        #region # 逻辑运算（或） —— void Or(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 逻辑运算（或）
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        public void Or(Expression<Func<T, bool>> predicate)
        {
            ParameterExpression candidateExpr = Expression.Parameter(typeof(T), "candidate");
            ParameterExpressionVisitor parameterVisitor = new ParameterExpressionVisitor(candidateExpr);

            Expression left = parameterVisitor.Visit(this._condition.Body);
            Expression right = parameterVisitor.Visit(predicate.Body);
            BinaryExpression body = Expression.Or(left, right);

            this._condition = Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        #endregion

        #region # 建造完毕 —— Expression<Func<T, bool>> Build()
        /// <summary>
        /// 建造完毕
        /// </summary>
        /// <returns>查询条件</returns>
        public Expression<Func<T, bool>> Build()
        {
            return this._condition;
        }
        #endregion
    }

    /// <summary>
    /// 参数表达式访问者
    /// </summary>
    internal class ParameterExpressionVisitor : ExpressionVisitor
    {
        #region # 字段及构造器

        /// <summary>
        /// 参数表达式
        /// </summary>
        private readonly ParameterExpression _parameterExpression;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="expression">参数表达式</param>
        public ParameterExpressionVisitor(ParameterExpression expression)
        {
            this._parameterExpression = expression;
        }

        #endregion

        #region # 访问 —— override Expression VisitParameter(ParameterExpression expression)
        /// <summary>
        /// 访问
        /// </summary>
        /// <param name="expression">参数表达式</param>
        /// <returns>表达式</returns>
        protected override Expression VisitParameter(ParameterExpression expression)
        {
            return this._parameterExpression;
        }
        #endregion
    }
}
