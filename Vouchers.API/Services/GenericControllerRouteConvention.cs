using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Swashbuckle.AspNetCore.SwaggerGen;
using Vouchers.Application.Queries;


namespace Vouchers.API.Services;

public class GenericControllerRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerType.IsGenericType)
        {
            var genericRequestType = controller.ControllerType.GenericTypeArguments[0];

            var verb = "";
            var template = "";
            
            if (genericRequestType.Name.EndsWith("Query"))
            {
                verb = "GET";
                template = genericRequestType.Name.Replace("Query", "");
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
                    .Replace("Command", "")
                    .Replace("Create", "")
                    .Replace("Update", "")
                    .Replace("Delete", "");
            }

            //[AspAttributeRouting(HttpVerb="POST")]
            var actionSelector = controller.Actions[0].Selectors[0];
            actionSelector.AttributeRouteModel = new AttributeRouteModel
            {
                Template = template,
            };
            actionSelector.ActionConstraints.Add(new HttpMethodActionConstraint(new List<string> { verb }));
            
            /*controller.Actions[0].Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Template = template,
                },
                ActionConstraints = { new HttpMethodActionConstraint(new List<string> { verb }) }
            });*/
        }
    }
}