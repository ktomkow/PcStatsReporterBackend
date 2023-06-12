using LibreHardwareMonitor.Hardware;
using PcStatsReporterBackend.Core;

namespace PcStatsReporterBackend.LibreHardware;

public class CpuCollector: ICollector<CpuSample>
{
    private readonly Computer _computer;

    public CpuCollector()
    {
        _computer = new Computer
        {
            IsCpuEnabled = true
        };

        _computer.Open();
    }

    public CpuSample Collect()
    {
        
        foreach (var hardware in _computer.Hardware)
        {
            hardware.Update();

            // just to be sure
            if (hardware.HardwareType != HardwareType.Cpu)
            {
                continue;
            }

            var sensors = hardware.Sensors.ToList();
            CpuSample cpu = new CpuSample()
            {
                Temperature = GetPackageTemperature(sensors),
                Id = Guid.NewGuid(),
                RegisteredAt = DateTime.UtcNow
            };
            
            return cpu;
        }
        
        throw new Exception("No data found");
    }
    
    private static uint GetPackageTemperature(IEnumerable<ISensor> sensors)
    {
        ISensor? sensor = sensors
            .Where(x => x.SensorType == SensorType.Temperature)
            .Where(x => x.Value.HasValue)
            .FirstOrDefault(x => x.Name.Contains("cpu package", StringComparison.InvariantCultureIgnoreCase));

        if (sensor?.Value is null)
        {
            return default;
        }

        return (uint) sensor.Value;
    }
}