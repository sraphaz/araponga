namespace Arah.Api.Contracts.Devices;

public sealed record RegisterDeviceRequest(
    string DeviceToken,
    string Platform,
    string? DeviceName
);
