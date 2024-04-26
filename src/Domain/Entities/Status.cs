using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManager.Domain.Entities;

public class Status : BaseAuditableEntity
{
    private static Random random = new Random();
    public required string Name { get; set; }
    public required string Theme { get; set; }

    public static string GenerateHexCode()
    {
        // Create a random integer between 0 and 0xFFFFFF (inclusive)
        int randomNumber = random.Next(0x1000000);

        // Convert the number to a hexadecimal string and ensure it is 6 characters long
        return String.Format("#{0:X6}", randomNumber);
    }
}
