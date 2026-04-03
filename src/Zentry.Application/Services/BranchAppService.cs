using Zentry.Application.Common;
using Zentry.Application.DTOs.Branches;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class BranchAppService : IBranchAppService
{
    private readonly IBranchRepository _branches;

    public BranchAppService(IBranchRepository branches)
    {
        _branches = branches;
    }

    public async Task<ApiResponse<List<BranchDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _branches.ListAsync(cancellationToken);

        return ApiResponse<List<BranchDto>>.Success(items.Select(x => new BranchDto
        {
            Id = x.Id,
            TenantId = x.TenantId,
            Code = x.Code,
            Name = x.Name,
            Phone = x.Phone,
            Email = x.Email,
            Address = x.Address,
            City = x.City,
            State = x.State,
            IsActive = x.IsActive
        }).ToList());
    }

    public async Task<ApiResponse<BranchDto>> CreateAsync(CreateBranchRequest request, CancellationToken cancellationToken = default)
    {
        if (request.TenantId == Guid.Empty)
            return ApiResponse<BranchDto>.Fail("El tenantId es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Code))
            return ApiResponse<BranchDto>.Fail("El código es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Name))
            return ApiResponse<BranchDto>.Fail("El nombre es obligatorio.");

        var existing = await _branches.GetByCodeAsync(request.TenantId, request.Code.Trim(), cancellationToken);
        if (existing is not null)
            return ApiResponse<BranchDto>.Fail("Ya existe una sucursal con ese código.");

        var entity = new Branch
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            Code = request.Code.Trim().ToUpperInvariant(),
            Name = request.Name.Trim(),
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address,
            City = request.City,
            State = request.State,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _branches.AddAsync(entity, cancellationToken);
        await _branches.SaveChangesAsync(cancellationToken);

        return ApiResponse<BranchDto>.Success(new BranchDto
        {
            Id = entity.Id,
            TenantId = entity.TenantId,
            Code = entity.Code,
            Name = entity.Name,
            Phone = entity.Phone,
            Email = entity.Email,
            Address = entity.Address,
            City = entity.City,
            State = entity.State,
            IsActive = entity.IsActive
        }, "Sucursal creada");
    }
}