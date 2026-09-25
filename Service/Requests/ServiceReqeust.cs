using System;

namespace Service.Requests;

public abstract class ServiceRequest<T>
{
    public abstract T ToEntity();
}
