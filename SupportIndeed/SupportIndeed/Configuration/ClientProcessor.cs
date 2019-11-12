using Newtonsoft.Json;
using ProcessorIndeed.CommonData;
using ProcessorIndeed.Models;
using ProcessorIndeed.Models.Emploees;
using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Models.SupportDivision;
using SupportIndeed.JetPipe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SupportIndeed.Configuration
{
    public class ClientProcessor
    {
        private static ClientProcessor mInstance;
        public static ClientProcessor Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new ClientProcessor();
                return mInstance;
            }
        }
        public IStartContent StartContent { get; private set; }
        
        public IStartContent GetStartContent()
        {
            ListEmpl = ConfigurationManager.GetSection(nameof(ListEmploees)) as ListEmploees;
            var ticketProcessingPeriod = ConfigurationManager.AppSettings["TicketProcessingPeriodSecondes"];
            var startManagerOffset = ConfigurationManager.AppSettings["StartManagerOffsetMinutes"];
            var startDirectorOffset = ConfigurationManager.AppSettings["StartDirectorOffsetMinutes"];
            var demonIp = ConfigurationManager.AppSettings["demonIp"];
            var demonPort = ConfigurationManager.AppSettings["demonPort"];
            var demonPipeName = ConfigurationManager.AppSettings["demonPipeName"];
            if (!int.TryParse(demonPort, out int port))
                port = 7777;
            if (!int.TryParse(ticketProcessingPeriod, out int ticketProcessingPeriodSecondes))
                ticketProcessingPeriodSecondes = 60;
            if (!int.TryParse(startManagerOffset, out int startManagerOffsetMinutes))
                startManagerOffsetMinutes = 5;
            if (!int.TryParse(startDirectorOffset, out int startDirectorOffsetMinutes))
                startDirectorOffsetMinutes = 10;
            var poolPositions = new List<Position>();
            Instance.InitPoolPositions(Instance.ListEmpl.Emploees, poolPositions);
            Instance.InitDivision(poolPositions);
            return new StartContent()
            {
                StartDirectorOffsetMinutes = startDirectorOffsetMinutes,
                StartManagerOffsetMinutes = startManagerOffsetMinutes,
                TicketProcessingPeriodSecondes = ticketProcessingPeriodSecondes,
                Supportivision = Instance.SupportDivision,
                PipeAddress = new NetAddress { IPAddress = demonIp, Port = port, PipeName = demonPipeName }
            };
        }

        public static void StartProcessingDemon()
        {
            Task.Factory.StartNew(()=>
            {
                var startContent = Instance.GetStartContent();
                var jsonString = JsonConvert.SerializeObject(startContent);
                var processes = Process.GetProcessesByName(nameof(ProcessingDemon.ProcessingDemon)).Select(x=>x.ProcessName);
                if(!processes.Any())
                {
                    var pathDemon = Path.Combine(HttpRuntime.AppDomainAppPath, $"{LiteralStrings.Bin}\\{nameof(ProcessingDemon.ProcessingDemon)}.{LiteralStrings.exe}");
                    var info = new ProcessStartInfo
                    {
                        Arguments = "\"" + jsonString.Replace("\"", "\\\"") + "\"",
                        FileName = pathDemon
                    };
                    Process.Start(info);
                }
                Instance.StartContent = startContent;
                Instance.InitPipeClient();
            });
        }
        public void InitPipeClient()
        {
            PClient = new PipeClient(StartContent);
        }
        public Division SupportDivision { get; private set; }
        public ListEmploees ListEmpl
        {
            get; private set;
        }
        public PipeClient PClient { get; private set; }

        private void InitDivision(IList<Position> poolPositions)
        {
            var lider = poolPositions.FirstOrDefault(x => x.Level == LevelPositionEnum.Director);
            var div = new Division()
            {
                id = Guid.NewGuid(),
                DivisionName = "Support",
                Lider = lider.Unit,
                Positions = poolPositions
            };
            poolPositions.ToList().ForEach(x =>
            {
                x.Lider = lider.Unit;
                x.SupportDivisionId = div.id;
            });
            SupportDivision = div;
        }

        private void InitPoolPositions(EmploeesCollection emploees, IList<Position> poolPositions)
        {
            foreach (var item in Instance.ListEmpl.Emploees)
            {
                var element = (EmploeesConfigurationElement)item;
                switch (element.level)
                {
                    case nameof(LevelPositionEnum.Director):
                        poolPositions.Add(GetPosition(element, LevelPositionEnum.Director));
                        break;
                    case nameof(LevelPositionEnum.Manager):
                        poolPositions.Add(GetPosition(element, LevelPositionEnum.Manager));
                        break;
                    case nameof(LevelPositionEnum.Operator):
                        poolPositions.Add(GetPosition(element, LevelPositionEnum.Operator));
                        break;
                    default:
                        poolPositions.Add(GetPosition(element, LevelPositionEnum.None));
                        break;
                }
            }
        }

        private Position GetPosition(EmploeesConfigurationElement element, LevelPositionEnum level)
        {
            var unitEmploee = new UnitEmploee()
            {
                id = Guid.NewGuid(),
                Name = element.name,
                SecondName = element.secondName
            };
            var position = new Position()
            {
                id = Guid.NewGuid(),
                Level = level,
                PositionName = level.ToString(),
                Unit = unitEmploee
            };
            return position;
        }
        
    }
}