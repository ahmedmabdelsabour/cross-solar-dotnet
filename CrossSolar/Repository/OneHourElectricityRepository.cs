using CrossSolar.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossSolar.Repository
{
    public class OneHourElectricityRepository: GenericRepository<OneHourElectricity>, IOneHourElectricityRepository
    {
        public OneHourElectricityRepository(CrossSolarDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
