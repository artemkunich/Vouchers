﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;

namespace Vouchers.Persistence.Configurations;

internal class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable(nameof(Account));

        builder.HasKey(account => account.Id);

        builder
            .Property(account => account.Supply)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property<byte[]>("RowVersion").IsRowVersion();
    }
}