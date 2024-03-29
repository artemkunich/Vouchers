﻿using Vouchers.Primitives;

namespace Vouchers.Files.Domain;

public sealed class CroppedImage : AggregateRoot<Guid>
{
    public Guid ImageId { get; init; }
    public CropParameters CropParameters { get; set; }

    public static CroppedImage Create(Guid id, Guid imageId, CropParameters cropParameters) => new()
    {
        Id = id,
        ImageId = imageId,
        CropParameters = cropParameters,
    };

}