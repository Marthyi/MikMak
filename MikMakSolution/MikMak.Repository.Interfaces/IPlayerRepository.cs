﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DomainModel.Entities;

namespace MikMak.Repository.Interfaces
{
    public interface IPlayerRepository
    {
        string CreatePlayer(Player player);

        Player Get(string login);
    }
}
