using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Command.Interfaces
{
    public interface ICalculateMortgageCommandService
    {
        Task CalculateMortgagesAsync();
    }
}
