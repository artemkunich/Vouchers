using System;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Events.IdentityEvents;

public class IdentityUpdatedEvent : Event
{
    public string NewFirstName { get; set; }

    public string NewLastName { get; set; }

    public string NewEmail { get; set; }

    public Guid? NewImageId { get; set; }
    
    public Guid? NewCroppedImageId { get; set; }
    
    public CropParametersDto NewCropParameters { get; set; }
    
}