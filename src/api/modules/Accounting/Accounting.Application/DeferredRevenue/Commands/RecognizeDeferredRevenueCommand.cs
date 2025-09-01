using MediatR;
using System;

namespace Accounting.Application.DeferredRevenue.Commands
{
    public class RecognizeDeferredRevenueCommand : IRequest
    {
        public DefaultIdType Id { get; set; }
        public DateTime RecognizedDate { get; set; }
    }
}

