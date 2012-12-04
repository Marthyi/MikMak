﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DomainModel.Entities;

namespace MikMak.Repository.Interfaces
{
    public interface IBattleRepository
    {
        string CreateBattle(Battle battle);

        Battle Get(string gameId);

        void Update(Battle battle);
    }
}
