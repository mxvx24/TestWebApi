namespace TestWebApi.Domain.Specifications
{
    using System;
    using System.Linq.Expressions;

    using TestWebApi.Domain.Entities;

    /// <summary>
    /// The new employee specification.
    /// </summary>
    public class NewEmployeeSpecification : Specification<Employee>
    {
        /// <summary>
        /// The to expression.
        /// </summary>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public override Expression<Func<Employee, bool>> ToExpression() => employee => employee.CreatedOn > DateTime.UtcNow.AddDays(-90);
    }
}
