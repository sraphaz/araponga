using Araponga.Domain.Assets;
using Araponga.Domain.Feed;
using Araponga.Domain.Media;
using Araponga.Modules.Assets.Infrastructure.Postgres.Entities;

namespace Araponga.Modules.Assets.Infrastructure.Postgres;

public static class AssetsMappers
{
    public static TerritoryAssetRecord ToRecord(this TerritoryAsset asset)
    {
        return new TerritoryAssetRecord
        {
            Id = asset.Id,
            TerritoryId = asset.TerritoryId,
            Type = asset.Type,
            Name = asset.Name,
            Description = asset.Description,
            Status = asset.Status,
            CreatedByUserId = asset.CreatedByUserId,
            CreatedAtUtc = asset.CreatedAtUtc,
            UpdatedByUserId = asset.UpdatedByUserId,
            UpdatedAtUtc = asset.UpdatedAtUtc,
            ArchivedByUserId = asset.ArchivedByUserId,
            ArchivedAtUtc = asset.ArchivedAtUtc,
            ArchiveReason = asset.ArchiveReason
        };
    }

    public static TerritoryAsset ToDomain(this TerritoryAssetRecord record)
    {
        return new TerritoryAsset(
            record.Id,
            record.TerritoryId,
            record.Type,
            record.Name,
            record.Description,
            record.Status,
            record.CreatedByUserId,
            record.CreatedAtUtc,
            record.UpdatedByUserId,
            record.UpdatedAtUtc,
            record.ArchivedByUserId,
            record.ArchivedAtUtc,
            record.ArchiveReason);
    }

    public static AssetGeoAnchorRecord ToRecord(this AssetGeoAnchor anchor)
    {
        return new AssetGeoAnchorRecord
        {
            Id = anchor.Id,
            AssetId = anchor.AssetId,
            Latitude = anchor.Latitude,
            Longitude = anchor.Longitude,
            CreatedAtUtc = anchor.CreatedAtUtc
        };
    }

    public static AssetGeoAnchor ToDomain(this AssetGeoAnchorRecord record)
    {
        return new AssetGeoAnchor(
            record.Id,
            record.AssetId,
            record.Latitude,
            record.Longitude,
            record.CreatedAtUtc);
    }

    public static AssetValidationRecord ToRecord(this AssetValidation validation)
    {
        return new AssetValidationRecord
        {
            AssetId = validation.AssetId,
            UserId = validation.UserId,
            CreatedAtUtc = validation.CreatedAtUtc
        };
    }

    public static AssetValidation ToDomain(this AssetValidationRecord record)
    {
        return new AssetValidation(
            record.AssetId,
            record.UserId,
            record.CreatedAtUtc);
    }

    public static PostAssetRecord ToRecord(this PostAsset postAsset)
    {
        return new PostAssetRecord
        {
            PostId = postAsset.PostId,
            AssetId = postAsset.AssetId
        };
    }

    public static PostAsset ToDomain(this PostAssetRecord record)
    {
        return new PostAsset(record.PostId, record.AssetId);
    }

    public static MediaAssetRecord ToRecord(this MediaAsset mediaAsset)
    {
        return new MediaAssetRecord
        {
            Id = mediaAsset.Id,
            UploadedByUserId = mediaAsset.UploadedByUserId,
            MediaType = mediaAsset.MediaType,
            MimeType = mediaAsset.MimeType,
            StorageKey = mediaAsset.StorageKey,
            SizeBytes = mediaAsset.SizeBytes,
            WidthPx = mediaAsset.WidthPx,
            HeightPx = mediaAsset.HeightPx,
            Checksum = mediaAsset.Checksum,
            CreatedAtUtc = mediaAsset.CreatedAtUtc,
            DeletedByUserId = mediaAsset.DeletedByUserId,
            DeletedAtUtc = mediaAsset.DeletedAtUtc
        };
    }

    public static MediaAsset ToDomain(this MediaAssetRecord record)
    {
        return new MediaAsset(
            record.Id,
            record.UploadedByUserId,
            record.MediaType,
            record.MimeType,
            record.StorageKey,
            record.SizeBytes,
            record.WidthPx,
            record.HeightPx,
            record.Checksum,
            record.CreatedAtUtc,
            record.DeletedByUserId,
            record.DeletedAtUtc);
    }

    public static MediaAttachmentRecord ToRecord(this MediaAttachment attachment)
    {
        return new MediaAttachmentRecord
        {
            Id = attachment.Id,
            MediaAssetId = attachment.MediaAssetId,
            OwnerType = attachment.OwnerType,
            OwnerId = attachment.OwnerId,
            DisplayOrder = attachment.DisplayOrder,
            CreatedAtUtc = attachment.CreatedAtUtc
        };
    }

    public static MediaAttachment ToDomain(this MediaAttachmentRecord record)
    {
        return new MediaAttachment(
            record.Id,
            record.MediaAssetId,
            record.OwnerType,
            record.OwnerId,
            record.DisplayOrder,
            record.CreatedAtUtc);
    }
}
