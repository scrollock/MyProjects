using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Processing.Interfaces;
using System;
using System.Linq;

namespace ProcessorIndeed.Processing
{
    public class ProcessorFactory
    {
        private static IProcessor _processor;
        private static IServiceQueue _serviceQueue;
        private static IAwaitQueue _awaitQueue;
        private static IUnitSupportPool _unitSupportPool;
        private static IProcessingTicket _processingTicket;
        private static IStartContent _startContent;
        internal static IProcessor GetProcessor(IStartContent startContent)
        {
            _unitSupportPool = GetUnitSupportPool();
            _unitSupportPool.Pool = startContent.Supportivision?.Positions?.ToList();
            _startContent = startContent;
            if (_processor == null)
                _processor = new Processor();
            return _processor;
        }
        internal static IServiceQueue GetServiceQueue()
        {
            if(_serviceQueue == null)
                _serviceQueue = new ServiceQueue();
            return _serviceQueue;
        }
        internal static IAwaitQueue GetAwaitQueue()
        {
            if (_awaitQueue == null)
            {
                _awaitQueue = new AwaitQueue();
                _awaitQueue.Td = _startContent.StartDirectorOffsetMinutes;
                _awaitQueue.Tm = _startContent.StartManagerOffsetMinutes;
            }
            return _awaitQueue;
        }
        internal static IUnitSupportPool GetUnitSupportPool()
        {
            if(_unitSupportPool == null)
                _unitSupportPool = new UnitSupportPool();
            return _unitSupportPool;
        }
        internal static IProcessingTicket GetProcessingTicket(IServiceQueue inputQueue, IAwaitQueue awaitQueue)
        {
            if (_processingTicket == null)
            {
                _processingTicket = new ProcessingTicket(inputQueue, awaitQueue);
                _processingTicket.RundomPeriodSecondes = _startContent.TicketProcessingPeriodSecondes;
            }
            return _processingTicket;
        }
    }
}
