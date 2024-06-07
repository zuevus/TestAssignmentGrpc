using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignmentGrcp.Data;
using Grpc.Core;
using TestAssignmentGrcp;

namespace TestAssignmentGrcp.Services
{
    public class FiboService(ILogger<FiboService> logger, 
        IDbContextFactory<TestAssignmentContext> factory) :Fibo.FiboBase
    {

        public override async Task<SequenceReply> GetFibonacci(PositionsRequest request, ServerCallContext context)
        {
            var seq = await GetFiboAsync(request.Position);
            var sReply = new SequenceReply();
            sReply.Sequence.AddRange(seq);
            return sReply;
        }
        public async Task<List<int>> GetFiboAsync(int count)
        {
            logger.LogInformation("{ServiceName}: Starting counting fibonacci for sequence of {count} positions", 
                nameof(FiboService),
                count);
            using var context = factory.CreateDbContext();
            
          var cache = await context.FiboNumbers.Where(t => t.Position <= count)
                .ToDictionaryAsync(t => t.Position);
            List<int> result = cache.OrderBy(t => t.Key).Select(t => t.Value.Number).ToList();
            if (cache.ContainsKey(count-1))
            {
                logger.LogInformation("{ServiceName}: Sequence for {count} position has already counted. Cash has been returned.", 
                    nameof(FiboService),
                    count);
                return result;
            }
            else
            {
                logger.LogInformation("{ServiceName}: It's a new for us. Waiting for count...", nameof(FiboService));
                if (result.Count == 0) result.AddRange(new int[] { 0, 1, 1 });
                for (int i = result.Count; i < count; i++)
                {
                    result.Add(result[i - 1] + result[i - 2]);
                }
            }
            List<FiboNumber> newCash = new();
            var toDb = result.Skip(cache.Count()).ToArray();
            for (int i = cache.Count(); i < (toDb.Count()+ cache.Count()); i++)
            {
                newCash.Add(
                    new FiboNumber { Number = result[i], 
                    Position = i });
            }
            logger.LogInformation("{ServiceName}: Save cash to database", nameof(FiboService));
            context.FiboNumbers.AddRange(newCash);
            await context.SaveChangesAsync();
            return result;
        }
         
    }
}
