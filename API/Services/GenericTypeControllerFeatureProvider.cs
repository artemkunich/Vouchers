using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Akunich.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Vouchers.API.Controllers;
using Vouchers.Domains.Application.Queries;
using Vouchers.Persistence;

namespace Vouchers.API.Services;

public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public GenericTypeControllerFeatureProvider()
    {
        _controllerTypes = new HashSet<Type>();
    }

    private readonly HashSet<Type> _controllerTypes;

    public GenericTypeControllerFeatureProvider AddPipeline<TRequest, TResponse, TPipeline>(params string[] roles) 
        where TRequest : IRequest<TResponse>
    {
        return this;
    }
    
    public GenericTypeControllerFeatureProvider AddHandler<TRequest, TResponse>(params string[] roles) 
        where TRequest : IRequest<TResponse>
    {
        var genericRequestType = typeof(TRequest);
        var genericResponseType = typeof(TResponse);

        Type controllerType;
        
        if (genericRequestType.Name.EndsWith("Query"))
        {
            if (genericRequestType.IsAssignableTo(typeof(IListQuery)))
            {
                controllerType = typeof(GenericQueryController<,>).MakeGenericType(genericRequestType,genericResponseType).GetTypeInfo();
            } 
            else
            {
                if (genericRequestType.GetProperties().Length != 1)
                    throw new InvalidOperationException();

                var propertyType = genericRequestType.GetProperties().First().PropertyType;

                if (propertyType != typeof(Guid) && propertyType != typeof(Guid?))
                    throw new InvalidOperationException();
                
                controllerType = typeof(GenericRouteController<,>)
                    .MakeGenericType(genericRequestType, genericResponseType).GetTypeInfo();
            }
                
        }
        else if (genericRequestType.Name.EndsWith("Command"))
        {
            if (genericRequestType.GetProperties().Select(p => p.PropertyType).Any(t => t.IsAssignableTo(typeof(IFormFile))))
            {
                controllerType = typeof(GenericCommandFormController<,>).MakeGenericType(genericRequestType,genericResponseType).GetTypeInfo();
            }
            else
            {
                controllerType = typeof(GenericCommandJsonController<,>).MakeGenericType(genericRequestType,genericResponseType).GetTypeInfo();
            }
        }
        else
        {
            throw new InvalidOperationException();
        }

        _controllerTypes.Add(controllerType);
        
        return this;
    }


    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var applicationAssembly = typeof(IRequestHandler<,>).Assembly;
        var persistenceAssembly = typeof(VouchersDbContext).Assembly;
        
        var candidates = applicationAssembly.GetTypes().Union(persistenceAssembly.GetTypes()).Where(t =>
            !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))).ToArray();
        
        foreach (var candidate in candidates)
        {
            var candidateInterface = candidate.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
            var genericArguments = candidateInterface.GetGenericArguments().ToArray();
            
            var genericRequestType = genericArguments[0];
            var genericResponseType = genericArguments[1];
            if (genericRequestType.Name.EndsWith("Query"))
            {
                if (!genericRequestType.IsAssignableTo(typeof(IListQuery)))
                {
                    if (genericRequestType.GetProperties().Length == 1)
                    {
                        var propertyType = genericRequestType.GetProperties().First().PropertyType;

                        if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                            feature.Controllers.Add(
                                typeof(GenericRouteController<,>).MakeGenericType(genericRequestType,genericResponseType).GetTypeInfo()
                            );
                    }
                } else
                {
                    feature.Controllers.Add(
                        typeof(GenericQueryController<,>).MakeGenericType(genericRequestType,genericResponseType).GetTypeInfo()
                    );
                }
                
            }
            else if(genericRequestType.Name.EndsWith("Command"))
            {
                if (genericRequestType.GetProperties().Select(p => p.PropertyType).Any(t => t.IsAssignableTo(typeof(IFormFile))))
                {
                    feature.Controllers.Add(
                        typeof(GenericCommandFormController<,>).MakeGenericType(genericRequestType,genericResponseType).GetTypeInfo()
                    );
                }
                else
                {
                    feature.Controllers.Add(
                        typeof(GenericCommandJsonController<,>).MakeGenericType(genericRequestType,genericResponseType).GetTypeInfo()
                    );
                }
            }
        }
    }
}