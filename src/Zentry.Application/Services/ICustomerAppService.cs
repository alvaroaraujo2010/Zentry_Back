using Zentry.Application.Common;
using Zentry.Application.DTOs.Customers;

namespace Zentry.Application.Services;
public interface ICustomerAppService { Task<ApiResponse<List<CustomerDto>>> ListAsync(CancellationToken cancellationToken = default); Task<ApiResponse<CustomerDto>> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default); Task<ApiResponse<CustomerDto>> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken = default); }
