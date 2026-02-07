using Microsoft.AspNetCore.Http;

namespace Arah.Api.Contracts.Common;

public sealed record FileUploadRequest(IFormFile File);

