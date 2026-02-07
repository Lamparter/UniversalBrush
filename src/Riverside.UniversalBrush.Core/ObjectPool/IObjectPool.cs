// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Riverside.UniversalBrush.Core.ObjectPool
{
    public interface IObjectPool<T> where T : IPoolableObject
    {
        T GetObject();
        void ReturnObject(T target);
    }
}
