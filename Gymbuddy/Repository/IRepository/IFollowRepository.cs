﻿using Gymbuddy.Core.Entities;
using GymBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBuddy.Repository.IRepository
{
    public interface IFollowRepository : IRepository<Follow>
    {
        void Update(Follow obj);
    }
}
