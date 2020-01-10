namespace TestWebApi.Domain.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The and specification.
    /// </summary>
    /// <typeparam name="T">
    /// The type.
    /// </typeparam>
    public class AndSpecification<T> : Specification<T>
    {
        /// <summary>
        /// The _left.
        /// </summary>
        private readonly Specification<T> left;

        /// <summary>
        /// The right.
        /// </summary>
        private readonly Specification<T> right;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            this.right = right;
            this.left = left;
        }

        /// <summary>
        /// The to expression.
        /// </summary>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = this.left.ToExpression();
            var rightExpression = this.right.ToExpression();

            ParameterExpression paramExpr = Expression.Parameter(typeof(T));
            BinaryExpression exprBody = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody ?? throw new InvalidOperationException($"{nameof(exprBody)} cannot be null."), paramExpr);

            return finalExpr;
        }
    }
}
