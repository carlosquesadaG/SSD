using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoDashboard.Models;

namespace ContosoDashboard.Services
{
    public interface IBudgetService
    {
        Task<List<BudgetRequest>> GetProjectBudgetRequestsAsync(int projectId);
        Task<List<BudgetRequest>> GetAllBudgetRequestsAsync();
        Task<BudgetRequest> CreateBudgetRequestAsync(int projectId, string userId, decimal amount, string category, string justification);
        Task<bool> ApproveBudgetRequestAsync(int requestId, string approverUserId, string? comments);
        Task<bool> RejectBudgetRequestAsync(int requestId, string approverUserId, string? comments);
    }
}
