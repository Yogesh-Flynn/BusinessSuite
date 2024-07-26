using BusinessSuite.Data;
using BusinessSuite.Models;

namespace BusinessSuite.Services
{
    public class MyJobService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MyJobService> _logger;

        public MyJobService(ApplicationDbContext context, ILogger<MyJobService> logger)
        {
            _context = context;
            _logger = logger;
        }

        //public async Task StoreDataAsync(string PhoneNumber,string MessageText,string status, DateTime createdAt)
        //{
        //    var data = new Message { PhoneNumber = PhoneNumber,MessageText=MessageText, ScheduleTime = createdAt,Status="Pending",IsDeleted=false};

        //    _context.Messages.Add(data);
        //    await _context.SaveChangesAsync();

        //    _logger.LogInformation("Data stored successfully at {Time}", DateTime.UtcNow);
        //}
    }
}
