using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Data.Configurations
{
    public class BotUserConfigs : IEntityTypeConfiguration<BotUser>
    {
        public void Configure(EntityTypeBuilder<BotUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Balance)
                .WithOne(x => x.User)
                .HasForeignKey<Balance>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
