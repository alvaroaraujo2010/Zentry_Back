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
        return ApiResponse<List<CustomerDto>>.Success(items.Select(x => new CustomerDto { Id = x.Id, BranchId = x.BranchId, FullName = x.FullName, Phone = x.Phone, Email = x.Email }).ToList());
    }

    public async Task<ApiResponse<CustomerDto>> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue) return ApiResponse<CustomerDto>.Fail("Tenant no encontrado.");
        if (string.IsNullOrWhiteSpace(request.FullName)) return ApiResponse<CustomerDto>.Fail("El nombre del cliente es obligatorio.");
        var entity = new Customer { Id = Guid.NewGuid(), TenantId = _current.TenantId.Value, BranchId = request.BranchId ?? _current.BranchId, FullName = request.FullName.Trim(), Phone = string.IsNullOrWhiteSpace(request.Phone) ? null : request.Phone.Trim(), Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(), IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        await _customers.AddAsync(entity, cancellationToken);
        await _customers.SaveChangesAsync(cancellationToken);
        return ApiResponse<CustomerDto>.Success(new CustomerDto { Id = entity.Id, BranchId = entity.BranchId, FullName = entity.FullName, Phone = entity.Phone, Email = entity.Email }, "Cliente creado");
    }

    public async Task<ApiResponse<CustomerDto>> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _customers.GetByIdAsync(id, cancellationToken);
        if (entity is null) return ApiResponse<CustomerDto>.Fail("Cliente no encontrado.");
        if (string.IsNullOrWhiteSpace(request.FullName)) return ApiResponse<CustomerDto>.Fail("El nombre del cliente es obligatorio.");

        entity.BranchId = request.BranchId;
        entity.FullName = request.FullName.Trim();
        entity.Phone = string.IsNullOrWhiteSpace(request.Phone) ? null : request.Phone.Trim();
        entity.Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();
        entity.UpdatedAt = DateTime.UtcNow;

        await _customers.SaveChangesAsync(cancellationToken);

        return ApiResponse<CustomerDto>.Success(new CustomerDto { Id = entity.Id, BranchId = entity.BranchId, FullName = entity.FullName, Phone = entity.Phone, Email = entity.Email }, "Cliente actualizado");
    }
}
