using Microsoft.AspNetCore.Mvc;

namespace Round.Common;

public abstract class BaseController : ControllerBase
{
    protected Guid GetTenantId()
    {
        //TODO: This should come from the auth or the request header.
        return new Guid("930607ed-d7a4-45fd-9f3f-2d669dfdf369");
    }
}