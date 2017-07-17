// <copyright file="ValidationQueryHandlerDecorator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Query
{
    using System.ComponentModel.DataAnnotations;

    public class ValidationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _decorated;

        public ValidationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated) {
            _decorated = decorated;
        }

        public TResult Execute(TQuery query) {
            var validationContext = new ValidationContext(query, null, null);
            Validator.ValidateObject(query, validationContext, validateAllProperties: true);
            return _decorated.Execute(query);
        }
    }
}
