using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IIdentityRepository
    {
        //Создание в AuthService

        public bool IsExists(long identityId);
    }
}
