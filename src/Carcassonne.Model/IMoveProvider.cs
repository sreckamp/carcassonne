﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model
{
    public interface IMoveProvider
    {
        CarcassonneMove GetMove(Game game);
    }
}
