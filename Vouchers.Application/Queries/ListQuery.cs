﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Queries
{
    public abstract class ListQuery
    {
        public string OrderBy { get; set; }

        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 10;
    }
}