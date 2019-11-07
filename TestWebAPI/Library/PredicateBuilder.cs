namespace TestWebAPI.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// The predicate builder.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// The true.
        /// </summary>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public static Expression<Func<T, bool>> True<T>() => param => true;

        /// <summary>
        /// The false.
        /// </summary>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public static Expression<Func<T, bool>> False<T>() => param => false;

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) => predicate;

        /// <summary>
        /// The and.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// The second.
        /// </param>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) => first.Compose(second, Expression.AndAlso);

        /// <summary>
        /// The or.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// The second.
        /// </param>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// The not.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// The compose.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// The second.
        /// </param>
        /// <param name="merge">
        /// The merge.
        /// </param>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)    
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first    
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression    
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// The parameter re-binder.
        /// </summary>
        class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// The map.
            /// </summary>
            private readonly Dictionary<ParameterExpression, ParameterExpression> map;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
            /// </summary>
            /// <param name="map">
            /// The map.
            /// </param>
            private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            /// <summary>
            /// The replace parameters.
            /// </summary>
            /// <param name="map">
            /// The map.
            /// </param>
            /// <param name="exp">
            /// The expression.
            /// </param>
            /// <returns>
            /// The <see cref="Expression"/>.
            /// </returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            /// <summary>
            /// The visit parameter.
            /// </summary>
            /// <param name="p">
            /// The parameter expression.
            /// </param>
            /// <returns>
            /// The <see cref="Expression"/>.
            /// </returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                if (this.map.TryGetValue(p, out var replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }
}
