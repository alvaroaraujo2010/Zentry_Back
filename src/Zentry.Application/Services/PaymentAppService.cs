using Zentry.Application.Common;
using Zentry.Application.DTOs.Payments;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;
using Zentry.Domain.Enums;

namespace Zentry.Application.Services;

public class PaymentAppService : IPaymentAppService
{
    private readonly IPaymentRepository _payments;
    private readonly ICurrentUserService _current;
    public PaymentAppService(IPaymentRepository payments, ICurrentUserService current) { _payments = payments; _current = current; }

    public async Task<ApiResponse<List<PaymentDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _payments.ListAsync(cancellationToken);
        return ApiResponse<List<PaymentDto>>.Success(items.Select(x => new PaymentDto { Id = x.Id, Amount = x.Amount, Method = x.PaymentMethod.ToString(), Status = x.Status, ReceivedAt = x.ReceivedAt }).ToList());
    }

    public async Task<ApiResponse<PaymentDto>> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue) return ApiResponse<PaymentDto>.Fail("Tenant no encontrado.");
        var method = Enum.TryParse<PaymentMethod>(request.Method, true, out var parsed) ? parsed : PaymentMethod.CASH;
        var entity = new Payment { Id = Guid.NewGuid(), TenantId = _current.TenantId.Value, BranchId = request.BranchId, InvoiceId = request.InvoiceId, AppointmentId = request.AppointmentId, CustomerId = request.CustomerId, CashSessionId = request.CashSessionId, PaymentMethod = method, ReferenceCode = request.ReferenceCode, Amount = request.Amount, ReceivedAt = DateTime.UtcNow, Status = "PAID", Notes = request.Notes, CreatedByUserId = request.CreatedByUserId, CreatedAt = DateTime.UtcNow };
        await _payments.AddAsync(entity, cancellationToken);
        await _payments.SaveChangesAsync(cancellationToken);
        return ApiResponse<PaymentDto>.Success(new PaymentDto { Id = entity.Id, Amount = entity.Amount, Method = entity.PaymentMethod.ToString(), Status = entity.Status, ReceivedAt = entity.ReceivedAt }, "Pago registrado");
    }
}
