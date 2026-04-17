using Zentry.Application.Common;
using Zentry.Application.DTOs.Invoices;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class InvoiceAppService : IInvoiceAppService
{
    private readonly IInvoiceRepository _invoices;
    private readonly ICurrentUserService _current;

    public InvoiceAppService(IInvoiceRepository invoices, ICurrentUserService current)
    {
        _invoices = invoices;
        _current = current;
    }

    public async Task<ApiResponse<List<InvoiceDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _invoices.ListAsync(cancellationToken);
        return ApiResponse<List<InvoiceDto>>.Success(items.Select(MapDto).ToList());
    }

    public async Task<ApiResponse<InvoiceDto>> CreateAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue && request.TenantId == Guid.Empty)
            return ApiResponse<InvoiceDto>.Fail("Tenant no encontrado.");
        if (request.BranchId == Guid.Empty)
            return ApiResponse<InvoiceDto>.Fail("La sucursal es obligatoria.");
        if (request.CustomerId == Guid.Empty)
            return ApiResponse<InvoiceDto>.Fail("El cliente es obligatorio.");
        if (request.Total <= 0)
            return ApiResponse<InvoiceDto>.Fail("El total debe ser mayor a 0.");

        var subtotal = request.Subtotal ?? request.Total;
        var tax = request.Tax ?? 0;
        var discount = request.Discount ?? 0;
        if (subtotal < 0 || tax < 0 || discount < 0)
            return ApiResponse<InvoiceDto>.Fail("Subtotal, impuesto y descuento no pueden ser negativos.");

        var tenantId = _current.TenantId ?? request.TenantId;
        var issuedAt = DateTime.UtcNow;
        var entity = new Invoice
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            BranchId = request.BranchId,
            AppointmentId = request.AppointmentId,
            CustomerId = request.CustomerId,
            InvoiceNumber = BuildInvoiceNumber(issuedAt),
            Subtotal = subtotal,
            Tax = tax,
            Discount = discount,
            Total = request.Total,
            AmountPaid = 0,
            BalanceDue = request.Total,
            PaymentStatus = "PENDING",
            IssuedAt = issuedAt,
            DueAt = request.DueAt,
            Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
            CreatedByUserId = _current.UserId,
            CreatedAt = issuedAt,
            UpdatedAt = issuedAt
        };

        await _invoices.AddAsync(entity, cancellationToken);
        await _invoices.SaveChangesAsync(cancellationToken);

        return ApiResponse<InvoiceDto>.Success(MapDto(entity), "Factura creada");
    }

    private static InvoiceDto MapDto(Invoice entity)
    {
        return new InvoiceDto
        {
            Id = entity.Id,
            BranchId = entity.BranchId,
            AppointmentId = entity.AppointmentId,
            CustomerId = entity.CustomerId,
            InvoiceNumber = entity.InvoiceNumber,
            Subtotal = entity.Subtotal,
            Tax = entity.Tax,
            Discount = entity.Discount,
            Total = entity.Total,
            AmountPaid = entity.AmountPaid,
            BalanceDue = entity.BalanceDue,
            Status = entity.PaymentStatus,
            IssuedAt = entity.IssuedAt,
            DueAt = entity.DueAt,
            Notes = entity.Notes
        };
    }

    private static string BuildInvoiceNumber(DateTime issuedAt)
    {
        return $"INV-{issuedAt:yyyyMMddHHmmss}";
    }
}
