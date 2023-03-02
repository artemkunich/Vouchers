using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Vouchers.Application;
using Vouchers.Application.Queries;


namespace Vouchers.API.Services;

public class GenericControllerRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (!controller.ControllerType.IsGenericType) return;
        
        var genericRequestType = controller.ControllerType.GenericTypeArguments.First();

        var verb = string.Empty;
        var template = string.Empty;
            
        if (genericRequestType.Name.EndsWith("Query"))
        {
            verb = "GET";
            template = genericRequestType.Name.Replace("Query", string.Empty);
            if (!genericRequestType.IsAssignableTo(typeof(IListQuery)))
            {
                if (genericRequestType.GetProperties().Length == 1)
                {
                    var property = genericRequestType.GetProperties().First();
                    var propertyName = property.Name.ToLower();
                    var propertyType = property.PropertyType;


                    if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    {
                        var propertyTypeName = propertyType == typeof(Guid) ? "guid" : "guid?";
                        template = $"{template}/{{{propertyName}:{propertyTypeName}}}";
                    }
                            
                }
            }
        }
        else if (genericRequestType.Name.EndsWith("Command"))
        {
            if (genericRequestType.Name.StartsWith("Create"))
            {
                verb = "POST";
            }
            else if (genericRequestType.Name.StartsWith("Update"))
            {
                verb = "PUT";
            }
            else if (genericRequestType.Name.StartsWith("Delete"))
            {
                verb = "DELETE";
            }

            template = genericRequestType.Name
                .Replace("Command", string.Empty)
                .Replace("Create", string.Empty)
                .Replace("Update", string.Empty)
                .Replace("Delete", string.Empty);
        }
            
        var authorizeAttribute = controller.Attributes.OfType<AuthorizeAttribute>().FirstOrDefault();
        if (authorizeAttribute is not null)
        {
            var identityRoles = genericRequestType.GetCustomAttributes().OfType<PermissionAttribute>()
                .Select(a => a.Roles)
                .FirstOrDefault(PermissionAttribute.DefaultRoles)
                .ToList();
            
            if(identityRoles.Any())
                authorizeAttribute.Roles = string.Join(",", identityRoles.Select(Enum.GetName));
        }
        
        var actionSelector = controller.Actions.First().Selectors.First();
        actionSelector.AttributeRouteModel = new AttributeRouteModel
        {
            Template = template,
        };
        actionSelector.ActionConstraints.Add(new HttpMethodActionConstraint(new List<string> { verb }));
    }
}