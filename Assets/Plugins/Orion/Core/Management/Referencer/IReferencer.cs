﻿using UnityEngine;

namespace Orion
{
    public interface IReferencer
    {
        Token Token { get; }
        Object Value { get; }

        void SetValue(Object value);
    }
}