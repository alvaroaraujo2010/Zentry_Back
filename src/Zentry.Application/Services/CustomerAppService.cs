using Zentry.Application.Common;
using Zentry.Application.DTOs.Customers;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class CustomerAppService : ICustomerAppService
{
    private readonly ICustomerRepository _customers;
    private readonly ICurrentUserService _current;
    public CustomerAppService(ICustomerRepository customers, ICurrentUserService current) { _customers = customers; _current = current; }

    public async Task<ApiResponse<List<CustomerDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _customers.ListAsync(cancellationToken);
        return ApiResponse<List<CustomerDto>>.Success(items.Select(x => new CustomerDto { Id = x.Id, FullName = x.FullName, Phone = x.Phone, Email = x.Email }).ToList());
    }

    public async Task<ApiResponse<CustomerDto>> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue) return ApiResponse<CustomerDto>.Fail("Tenant no encontrado.");
        var entity = new Customer { Id = Guid.NewGuid(), TenantId = _current.TenantId.Value, BranchId = request.BranchId ?? _current.BranchId, FullName = request.FullName.Trim(), Phone = request.Phone, Email = request.Email, IsActive = true, CreatedAt = DateTime.UtcNow };
        await _customers.AddAsync(entity, cancellationToken);
        await _customers.SaveChangesAsync(cancellationToken);
        return ApiResponse<CustomerDto>.Success(new CustomerDto { Id = entity.Id, FullName = entity.FullName, Phone = entity.Phone, Email = entity.Email }, "Cliente creado");
    }
}
