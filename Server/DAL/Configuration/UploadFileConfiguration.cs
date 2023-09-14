using ASMC5.Models;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assiment_csad4.Configruration
{
    public class UploadFileConfiguration : IEntityTypeConfiguration<UploadResult>
    {
        public void Configure(EntityTypeBuilder<UploadResult> builder)
        {
            
            builder.ToTable("File");
             
            
        }
    }
}
