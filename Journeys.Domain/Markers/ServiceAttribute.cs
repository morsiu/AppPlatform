﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journeys.Domain.Markers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ServiceAttribute : Attribute
    {
    }
}