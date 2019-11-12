using Newtonsoft.Json;
using ProcessorIndeed.Models;
using ProcessorIndeed.Processing;
using System;
using System.Threading;

namespace ProcessingDemon
{
    public class ProcessingDemon
    {
        public static StartContent StartContent { get; private set; }
        [STAThread]
        static void Main(string[] args)
        {
            var mutexIsAvailable = false;
            var mutex = default(Mutex);
            try
            {
                mutex = new Mutex(true, "ProcessingDemon.Singleton");
                mutexIsAvailable = mutex.WaitOne(1, false); // Wait only 1 ms
            }
            catch (AbandonedMutexException)
            {
                mutexIsAvailable = true;
            }
            if (mutexIsAvailable)
            {
                try
                {
                    Console.WriteLine("Start ProcessingDemon.exe");
                    var jsonString = string.Join(" ", args);
                    //var jsonString = "{\"Supportivision\":{\"DivisionName\":\"Support\",\"Parent\":null,\"Positions\":[{\"PositionName\":\"Director\",\"SupportDivisionId\":\"5c8dbef8-89c8-44ff-ac25-54ae4ac1606f\",\"Lider\":{\"Name\":\"directorName\",\"SecondName\":\"directorSecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"661d95ab-9d31-4b6a-8894-a6caaab2c941\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Unit\":{\"Name\":\"directorName\",\"SecondName\":\"directorSecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"661d95ab-9d31-4b6a-8894-a6caaab2c941\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Level\":1,\"IsWorkBusy\":false,\"StartIdle\":null,\"id\":\"20f7a357-6415-4c82-afbd-6ca4b8b5c324\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IPosition, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},{\"PositionName\":\"Manager\",\"SupportDivisionId\":\"5c8dbef8-89c8-44ff-ac25-54ae4ac1606f\",\"Lider\":{\"Name\":\"directorName\",\"SecondName\":\"directorSecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"661d95ab-9d31-4b6a-8894-a6caaab2c941\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Unit\":{\"Name\":\"manager1Name\",\"SecondName\":\"manager1SecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"20c71757-056d-4c47-b513-88bcfbd44ff3\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Level\":2,\"IsWorkBusy\":false,\"StartIdle\":null,\"id\":\"6125f7bf-fd92-4ef5-9fd3-49010c12fe38\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IPosition, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},{\"PositionName\":\"Manager\",\"SupportDivisionId\":\"5c8dbef8-89c8-44ff-ac25-54ae4ac1606f\",\"Lider\":{\"Name\":\"directorName\",\"SecondName\":\"directorSecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"661d95ab-9d31-4b6a-8894-a6caaab2c941\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Unit\":{\"Name\":\"manager2Name\",\"SecondName\":\"manager2SecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"4845bc8c-0761-41c9-b508-b1561ace09bc\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Level\":2,\"IsWorkBusy\":false,\"StartIdle\":null,\"id\":\"fb74b40c-2215-482d-b9bd-8d53797909b3\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IPosition, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},{\"PositionName\":\"Operator\",\"SupportDivisionId\":\"5c8dbef8-89c8-44ff-ac25-54ae4ac1606f\",\"Lider\":{\"Name\":\"directorName\",\"SecondName\":\"directorSecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"661d95ab-9d31-4b6a-8894-a6caaab2c941\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Unit\":{\"Name\":\"operator1Name\",\"SecondName\":\"operator1SecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"653ce69c-e5b4-4aa3-b0dc-6b5377f12142\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Level\":3,\"IsWorkBusy\":false,\"StartIdle\":null,\"id\":\"7d9a2fe7-2d3b-4383-9b02-1f3c1f3cb283\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IPosition, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},{\"PositionName\":\"Operator\",\"SupportDivisionId\":\"5c8dbef8-89c8-44ff-ac25-54ae4ac1606f\",\"Lider\":{\"Name\":\"directorName\",\"SecondName\":\"directorSecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"661d95ab-9d31-4b6a-8894-a6caaab2c941\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Unit\":{\"Name\":\"operator2Name\",\"SecondName\":\"operator2SecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"66fcb560-12d4-4e22-a61e-7bf9d220867d\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Level\":3,\"IsWorkBusy\":false,\"StartIdle\":null,\"id\":\"aa101f84-d724-4a21-b4d2-1dca8b469af5\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IPosition, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},{\"PositionName\":\"Operator\",\"SupportDivisionId\":\"5c8dbef8-89c8-44ff-ac25-54ae4ac1606f\",\"Lider\":{\"Name\":\"directorName\",\"SecondName\":\"directorSecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"661d95ab-9d31-4b6a-8894-a6caaab2c941\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Unit\":{\"Name\":\"operator3Name\",\"SecondName\":\"operator3SecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"57b74592-a2dd-4332-a903-e7e04d9952db\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"Level\":3,\"IsWorkBusy\":false,\"StartIdle\":null,\"id\":\"80f9a17f-ddff-4187-9bf1-c0128e6892cf\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IPosition, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null}],\"Lider\":{\"Name\":\"directorName\",\"SecondName\":\"directorSecondName\",\"Division\":null,\"Contact\":null,\"Login\":null,\"PasswordHash\":null,\"UnitImage\":null,\"id\":\"661d95ab-9d31-4b6a-8894-a6caaab2c941\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IUnitEmploee, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"id\":\"5c8dbef8-89c8-44ff-ac25-54ae4ac1606f\",\"TypeObject\":\"ProcessorIndeed.Models.Interfaces.IDivision, ProcessorIndeed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Deleted\":false,\"PermissionId\":null},\"PipeAddress\":{\"PipeName\":\"DemonPipe\",\"Port\":3333,\"IPAddress\":\"localhost\",\"FullAddress\":\"net.pipe://localhost/DemonPipe:3333\"},\"TicketProcessingPeriodSecondes\":10,\"StartManagerOffsetMinutes\":2,\"StartDirectorOffsetMinutes\":5}";
                    StartContent = JsonConvert.DeserializeObject<StartContent>(jsonString);
                    using (var controller = new PipeServer())
                    {
                        try
                        {
                            controller.InitProcessor(StartContent);
                            controller.InitHost();
                            controller.OpenHost();
                            controller.StartProcessing();
                        }
                        catch (Exception ex)
                        {
                            if (controller != null)
                            {
                                controller.StopProcessing(ex.Message);
                                //controller.CloseHost();
                            }
                            Console.WriteLine(ex.Message);
                            Console.ReadLine();
                        }
                    }
                    Console.WriteLine("Stop ProcessingDemon.exe");
                    Console.ReadLine();
                    
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}

