using System.Globalization;
using Excursions.Application.Resources;
using Excursions.Domain.Exceptions;
using Microsoft.Extensions.Localization;

namespace Excursions.Api.Gql.Infrastructure;

public class GqlErrorFilter : IErrorFilter
{
    private const string DefaultCategory = "Default";
    
    private readonly ILogger<GqlErrorFilter> _logger;
    private readonly IStringLocalizer<ExcursionResource> _stringLocalizer;

    public GqlErrorFilter(
        ILogger<GqlErrorFilter> logger,
        IStringLocalizer<ExcursionResource> stringLocalizer)
    {
        _logger = logger;
        _stringLocalizer = stringLocalizer;
    }
    
    public IError OnError(IError error)
    {
        if (error.Code is not null)
            return error;

        switch (error.Exception)
        {
            case ValidationException validationException:
                return GetValidationError(error, validationException, _stringLocalizer);
            case DomainException domainException:
                return GetBaseError(error, domainException, _stringLocalizer);
            case ExceptionBase exceptionBase:
                _logger.LogError(exceptionBase, exceptionBase.Message);
                return GetBaseError(error, exceptionBase, _stringLocalizer);
            case null:
                _logger.LogError(error.Message);
                return error.WithCode(DefaultCategory).WithMessage(error.Message);
            default:
                _logger.LogError(error.Exception, error.Exception.Message);
                return error.WithCode(DefaultCategory).WithMessage(error.Exception.Message);
        }
    }

    private static IError GetValidationError(
        IError error,
        ValidationException validationException,
        IStringLocalizer stringLocalizer)
    {
        var errorBuilder = ErrorBuilder.FromError(error.WithExtensions(GetExtensions()));
        errorBuilder.SetCode(validationException.Category);
        errorBuilder.SetMessage(validationException.Message);

        return errorBuilder.Build();

        Dictionary<string, object?> GetExtensions()
        {
            var extensions = new Dictionary<string, object?>();

            var messages = validationException.Messages
                .Select((message, index) => new
                {
                    Message = message,
                    Index = index.ToString(CultureInfo.InvariantCulture)
                });

            foreach (var message in messages)
            {
                var localizedMessage = validationException.IsLocalized
                    ? stringLocalizer.GetString(message.Message.Message, message.Message.MessageParameters)
                    : message.Message.Message;

                var extension = new Dictionary<string, object?>
                {
                    { "location", message.Message.Location },
                    { "message", localizedMessage }
                };
                
                extensions.Add(message.Index, extension);
            }

            return extensions;
        }
    }

    private static IError GetBaseError(
        IError error,
        ExceptionBase exceptionBase,
        IStringLocalizer stringLocalizer)
    {
        var localizedMessage = exceptionBase.IsLocalized
            ? stringLocalizer.GetString(exceptionBase.Message, exceptionBase.MessageParameters)
            : exceptionBase.Message;
        
        return error.WithCode(exceptionBase.Category).WithMessage(localizedMessage);
    }
}