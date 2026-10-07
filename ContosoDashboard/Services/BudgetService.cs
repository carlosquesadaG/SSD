using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoDashboard.Data;
using ContosoDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoDashboard.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public BudgetService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<List<BudgetRequest>> GetProjectBudgetRequestsAsync(int projectId)
        {
            return await _context.BudgetRequests
                .Where(b => b.ProjectId == projectId)
                .OrderByDescending(b => b.RequestDate)
                .ToListAsync();
        }

        public async Task<List<BudgetRequest>> GetAllBudgetRequestsAsync()
        {
            return await _context.BudgetRequests
                .Include(b => b.Project)
                .OrderByDescending(b => b.RequestDate)
                .ToListAsync();
        }

        public async Task<BudgetRequest> CreateBudgetRequestAsync(int projectId, string userId, decimal amount, string category, string justification)
        {
            if (amount <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero.");

            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException("Proyecto no encontrado.");

            var request = new BudgetRequest
            {
                ProjectId = projectId,
                UserId = userId,
                Amount = amount,
                Category = category,
                Justification = justification,
                Status = "Pending",
                RequestDate = DateTime.UtcNow
            };

            _context.BudgetRequests.Add(request);
            await _context.SaveChangesAsync();

            // Notify project manager
            if (int.TryParse(project.ProjectManagerId.ToString(), out int pmId))
            {
                await _notificationService.CreateNotificationAsync(new Notification
                {
                    UserId = pmId,
                    Title = "Nueva Solicitud de Gasto",
                    Message = $"Se ha registrado una solicitud de gasto por ${amount:N2} para el proyecto '{project.Name}'.",
                    CreatedDate = DateTime.UtcNow,
                    IsRead = false
                });
            }

            return request;
        }

        public async Task<bool> ApproveBudgetRequestAsync(int requestId, string approverUserId, string? comments)
        {
            var request = await _context.BudgetRequests.Include(b => b.Project).FirstOrDefaultAsync(b => b.Id == requestId);
            if (request == null || request.Status != "Pending")
                return false;

            request.Status = "Approved";
            request.ApproverComments = comments;

            if (request.Project != null)
            {
                request.Project.SpentBudget += request.Amount;
            }

            await _context.SaveChangesAsync();

            if (int.TryParse(request.UserId, out int reqId))
            {
                await _notificationService.CreateNotificationAsync(new Notification
                {
                    UserId = reqId,
                    Title = "Solicitud de Gasto Aprobada",
                    Message = $"Su solicitud de gasto por ${request.Amount:N2} ha sido aprobada.",
                    CreatedDate = DateTime.UtcNow,
                    IsRead = false
                });
            }

            return true;
        }

        public async Task<bool> RejectBudgetRequestAsync(int requestId, string approverUserId, string? comments)
        {
            var request = await _context.BudgetRequests.Include(b => b.Project).FirstOrDefaultAsync(b => b.Id == requestId);
            if (request == null || request.Status != "Pending")
                return false;

            request.Status = "Rejected";
            request.ApproverComments = comments;

            await _context.SaveChangesAsync();

            if (int.TryParse(request.UserId, out int reqId))
            {
                await _notificationService.CreateNotificationAsync(new Notification
                {
                    UserId = reqId,
                    Title = "Solicitud de Gasto Rechazada",
                    Message = $"Su solicitud de gasto por ${request.Amount:N2} ha sido rechazada. Motivo: {comments}",
                    CreatedDate = DateTime.UtcNow,
                    IsRead = false
                });
            }

            return true;
        }
    }
}
