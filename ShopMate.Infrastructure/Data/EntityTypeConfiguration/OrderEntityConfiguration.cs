﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMate.Infrastructure.Data.EntityTypeConfiguration
{
    public class OrderEntityConfiguration:IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.UserAddress)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserAddressId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            builder.HasOne(x => x.Coupon)
                .WithOne(x => x.Order)
                .HasForeignKey<Order>(x=>x.CouponId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            builder.HasOne(x => x.Status)
               .WithMany(x => x.Orders)
               .HasForeignKey(x => x.StatusId)
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();

        }
    }
}
