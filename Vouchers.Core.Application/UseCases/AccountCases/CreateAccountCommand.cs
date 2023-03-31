using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Core.Application.UseCases.AccountCases;

public class CreateAccountCommand : IRequest<Unit>
{
    public Guid AccountId { get; set; }
    
    public Guid IdentityId { get; set;}
    
    public Guid DomainId { get; set; }
    
    public DateTime CreatedDateTime { get; set; }
    
    public bool IsIssuer { get; set; }
    
    public bool IsAdmin { get; set; }
    
    public bool IsOwner { get; set; }
    
    public bool IsConfirmed { get; set; }
}