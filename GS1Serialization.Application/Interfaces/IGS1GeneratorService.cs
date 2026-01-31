using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS1Serialization.Application.Interfaces
{
    public interface IGS1GeneratorService
    {
        string GenerateGS1String(string gtin, string lot, DateOnly expireDate, string serialNumber);
        string GenerateSSCC(string extensionDigit, string companyPrefix, string serialReference);
    }
}
