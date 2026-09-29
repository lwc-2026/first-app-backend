using System;

namespace BusinessModel.Http.Requests;

public class RefreshHttpRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
